using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum Object3DMeshType
{
    None,
    Mesh,
    SkinnedMesh,
}


[ExecuteAlways]
[DisallowMultipleComponent]
public class Object3D : MonoBehaviour
{
    UniversalManager um;
    Object3DManager om;
    const string pixelationLayerName = "3DPixelation";

    //camera ids
    const string mainCamName = "Main Camera";
    const string pixelCam = "3DPixelCamera";
    const string pixelInfoCam = "3DInfoCamera";
    const string pixelOutlineColorCam = "3DOutlineColorCamera";


    //setting
    public bool pixelate = true;
    public bool isEmptyObject;
    public Object3D parentObject;
    const int defaultID = -1972;
    public int  object3dID = -1972;


    //material
    public Material[] orgMat;
    Material infoMaterial = null;
    Material outlineColorMaterial = null;

    //outline
    public bool outline;
    public Color outlineColor;
    [Range(0, 15)] public float outlineBrightness = 1;

    //mesh renderer
    Object3DMeshType meshType = Object3DMeshType.None;
    MeshRenderer meshRenderer;
    SkinnedMeshRenderer skinnedMeshRenderer;
    private void Awake()
    {
        GetManagers();
        UpdateObjectID();
    }
    public void GetManagers()
    {
        um = FindObjectOfType<UniversalManager>();
        om = um.object3DManager;
    }
    public void UpdateObjectID()
    {
        if (parentObject != null)
        {
            return;
        }



        //get every id in scene
        Object3D[] obj3d = FindObjectsOfType<Object3D>();
        Dictionary<int, Object3D> idList = new Dictionary<int, Object3D>();
        List<Object3D> problemObjects = new List<Object3D>();
        int maxID = 0;
        foreach (Object3D o in obj3d)
        {
            if (o != this)
            {
                int id = o.object3dID;
                if (id != defaultID)
                {
                    if (idList.ContainsKey(id))
                    {
                        bool thatIDImplictWasJustBeingChild = true;
                        if (idList[id].parentObject == o)
                            thatIDImplictWasJustBeingChild = true;
                        else if (o.parentObject == null)
                            thatIDImplictWasJustBeingChild = false;
                        else if(o.parentObject.object3dID != id)
                            thatIDImplictWasJustBeingChild = false;


                        if (!thatIDImplictWasJustBeingChild)
                        {
                            //this object has other object with same id.
                            Debug.LogWarning($"{o.gameObject.name} had the same id with {idList[id].gameObject.name}, but i automatically took care of that lol");
                            problemObjects.Add(o);
                        }
                    }
                    else
                    {
                        idList.Add(id, o);
                        if (id > maxID)
                        {
                            maxID = id;
                        }
                    }
                }
            }
        }


        //first 3d object in scene
        if (idList.Count == 0)
        {
            object3dID = 0;
            om.maxObject3DID = 0;
            return;
        }
        int maxIDBeforeFixingProblemObjects = maxID;
        foreach (Object3D o in problemObjects)
        {
            o.object3dID = maxID + 1;
            maxID++;
        }



        //set max+1 or unused id
        om.maxObject3DID = maxID;
        for (int i = 0; i < maxIDBeforeFixingProblemObjects; i++)
        {
            if (!idList.ContainsKey(i))
            {
                object3dID = i;
                return;
            }
        }
        object3dID = maxID + 1;
        om.maxObject3DID = object3dID;
    }


    public void LateUpdate()
    {
        if (um == null)
        {
            GetManagers();
        }
        if (parentObject != null)
        {
            //copy parent objects property
            pixelate = parentObject.pixelate;
            object3dID = parentObject.object3dID;
            outline = parentObject.outline;
            outlineColor = parentObject.outlineColor;
            outlineBrightness = parentObject.outlineBrightness;
        }

        if (isEmptyObject)
            return;
        SetLayer();

        PassInfoToShader();
    }
    public void SetLayer()
    {
        if (pixelate)
        {
            gameObject.layer = LayerMask.NameToLayer(pixelationLayerName);
        }
        else if (gameObject.layer == LayerMask.NameToLayer(pixelationLayerName))
        {
            gameObject.layer = 0;
        }
    }
    public void PassInfoToShader()
    {
        if (infoMaterial == null) return;

        //info mat
        infoMaterial.SetFloat("_OutlineBrightness", outline ? outlineBrightness : 0);
        infoMaterial.SetFloat("_ObjectID", (float)object3dID / (om.maxObject3DID + 1));

        //outlineColorMat
        outlineColor.a = 1;
        outlineColorMaterial.SetColor("_Color", outline ? outlineColor : new Color(0,0,0,0));
    }





    #region make setmaterial called when rendering
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += UpdateMaterial;
    }
    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= UpdateMaterial;
    }
    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= UpdateMaterial;
    }
    #endregion


    public void UpdateMaterial(ScriptableRenderContext context, Camera camera)
    {
        //set object id
        if (object3dID == defaultID)
        {
            UpdateObjectID();
        }

        if (isEmptyObject)
            return;
        if (!pixelate)
            return;

        //get info material
        if (infoMaterial == null)
        {
            infoMaterial = Instantiate(Resources.Load<Material>("Material/3dObjectInfo"));
            outlineColorMaterial = Instantiate(Resources.Load<Material>("Material/3dObjectOutlineColor"));
        }

        switch (meshType)
        {

            case Object3DMeshType.None:

                //check mesh type
                if(TryGetComponent(out meshRenderer))
                {
                    meshType = Object3DMeshType.Mesh;
                    orgMat = meshRenderer.sharedMaterials;
                    SetMRMaterial(camera);
                }
                else if (TryGetComponent(out skinnedMeshRenderer))
                {
                    meshType = Object3DMeshType.SkinnedMesh;
                    orgMat = skinnedMeshRenderer.sharedMaterials;
                    SetSMRMaterial(camera);
                }
                break;
            case Object3DMeshType.Mesh:
                SetMRMaterial(camera);
                break;
            case Object3DMeshType.SkinnedMesh:
                SetSMRMaterial(camera);
                break;
        }
    }
    public void SetMRMaterial(Camera camera)
    {
        //return if org mat is null
        if (orgMat == null)
            return;

        //switch material according to camera
        switch (camera.name)
        {
            #region org mats
            case mainCamName:
                SetMeshRendererMaterial(orgMat);
                break;
            case pixelCam:
                SetMeshRendererMaterial(orgMat);
                break;
            default:
                SetMeshRendererMaterial(orgMat);
                break;
            #endregion

            #region pixel mat
            case pixelInfoCam:
                //change material
                SetMeshRendererMaterial(infoMaterial);

                break;
                #endregion

            #region outline color mat
            case pixelOutlineColorCam:
                SetMeshRendererMaterial(outlineColorMaterial);
                break;
            #endregion
        }

    }
    public void SetSMRMaterial(Camera camera)
    {

        //return if org mat is null
        if (orgMat == null)
            return;

        //switch material according to camera
        switch (camera.name)
        {
            #region org mats
            case mainCamName:
                SetSkinnedMeshRendererMaterial(orgMat);
                break;
            case pixelCam:
                SetSkinnedMeshRendererMaterial(orgMat);
                break;
            default:
                SetSkinnedMeshRendererMaterial(orgMat);
                break;
            #endregion

            #region pixel mat
            case pixelInfoCam:
                //change material
                SetSkinnedMeshRendererMaterial(infoMaterial);

                break;
            #endregion

            #region outline color mat
            case pixelOutlineColorCam:
                SetSkinnedMeshRendererMaterial(outlineColorMaterial);
                break;
                #endregion
        }

    }
    void SetMeshRendererMaterial(Material[] mat)
    {
        meshRenderer.materials = mat;
    }
    void SetMeshRendererMaterial(Material mat)
    {
        meshRenderer.materials = new Material [1]{mat};
    }
    void SetSkinnedMeshRendererMaterial(Material[] mat)
    {
        skinnedMeshRenderer.materials = mat;
    }
    void SetSkinnedMeshRendererMaterial(Material mat)
    {
        skinnedMeshRenderer.materials = new Material[1] { mat };
    }

}

//#if UNITY_EDITOR
//[CustomEditor(typeof(Object3D)), CanEditMultipleObjects]
//class PixelateSpriteEditor : Editor
//{
//    SerializedProperty worldSpace;

//    private void OnEnable()
//    {
//        worldSpace = serializedObject.FindProperty("worldSpace");
//    }
//    public override void OnInspectorGUI()
//    {
//        var ps = (PixelateSprite)target;
//        if (ps == null)
//            return;
//        Undo.RecordObject(ps, "Change PixelateSprite");

//        GUIStyle fontStyle = new GUIStyle { fontSize = 20, normal = new GUIStyleState { textColor = Color.white } };


//        GUILayout.Label("Positioning", fontStyle);
//        //world space
//        serializedObject.Update();
//        EditorGUILayout.PropertyField(worldSpace);
//        if (!ps.worldSpace)
//        {
//            EditorGUILayout.PropertyField(localCenterType);
//            switch (ps.localCenterType)
//            {
//                case LocalCenterType.Transform:
//                    EditorGUILayout.PropertyField(parent);
//                    EditorGUILayout.PropertyField(onlyOffsetPosition);
//                    EditorGUILayout.HelpBox("warning:parent's size must be stayed at 1x1 !!!!", MessageType.Warning);
//                    break;
//                case LocalCenterType.Position:
//                    EditorGUILayout.PropertyField(localCenterPosition);
//                    break;
//            }
//        }
//        GUILayout.Space(20);
//        GUILayout.Label("Property", fontStyle);
//        EditorGUILayout.PropertyField(threshold);
//        EditorGUILayout.PropertyField(followUniversalPixelateAmount);
//        EditorGUILayout.PropertyField(pixelateAmount);
//        EditorGUILayout.PropertyField(glowColor);


//        serializedObject.ApplyModifiedProperties();
//    }
//}
//#endif