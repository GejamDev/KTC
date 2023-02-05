using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum LocalCenterType
{
    Transform,Position
}

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public class PixelateSprite : MonoBehaviour
{
    [HideInInspector]public SpriteRenderer sr;


    //const value
    const string materialName = "PixelateSprite_Clone";
    const string layerName = "PixelatedSprite";
    const string depthCameraName = "SpriteDepthCamera";


    //settings
    public bool worldSpace = true;
    public LocalCenterType localCenterType;
    public Transform parent;
    public bool onlyOffsetPos;
    public Vector2 orgScale = Vector2.one;
    public Vector2 localCenterPosition;
    public float threshold = 0.95f;
    public bool followUniversalPixelateAmount = true;
    public float pixelateAmount = 16;
    [ColorUsage(true, true)]
    public Color glowColor;


    public Material pixelMaterial;
    public Material depthMaterial;

    public bool outline;
    public PixelOutlineRenderer outlineRenderer;

    private void Awake()
    {
        SetMaterial();
    }
    public void SetMaterial()
    {
        sr = GetComponent<SpriteRenderer>();
        pixelMaterial = Instantiate(Resources.Load<Material>("Material/Pixelate"));
        sr.material = pixelMaterial;
        pixelMaterial.name = materialName;
        sr.sharedMaterial = pixelMaterial;
        if (parent == null)
            parent = transform;
    }

    private void LateUpdate()
    {
        PassInfoToShader();
        SetLayer();
        if (pixelMaterial == null)
        {
            SetMaterial();
        }
    }
    public void SetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
    void PassInfoToShader()
    {
        if (followUniversalPixelateAmount)
        {
            pixelateAmount = PlayerPrefs.GetFloat("UniversalPixelateAmount");
        }
        Vector2 position = transform.position;
        Vector2 scale = transform.lossyScale;
        float rotation = transform.eulerAngles.z;


        if (!worldSpace)
        {
            switch (localCenterType)
            {
                case LocalCenterType.Position:
                    //offset transform
                    position -= localCenterPosition;
                    break;
                case LocalCenterType.Transform:
                    if (parent != null)
                    {
                        //set parent size
                        if (parent != transform)
                        {
                            parent.localScale = new Vector3(parent.localScale.x / parent.lossyScale.x, parent.localScale.y / parent.lossyScale.y, 1);
                        }
                        else
                        {

                        }

                        //offset transform
                        Vector2 relativePosition = position - (Vector2)parent.position;
                        position = relativePosition;
                        if (!onlyOffsetPos)
                        {
                            //offset rotation by parent's rotation
                            float parentRot = parent.eulerAngles.z;
                            rotation -= parentRot;



                            //rotates position back relative to parent's rotation
                            float position_rot = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
                            float targetRot = position_rot - parentRot;
                            Vector2 rotatedPosition = new Vector2(Mathf.Cos(targetRot * Mathf.Deg2Rad), Mathf.Sin(targetRot * Mathf.Deg2Rad))*position.magnitude;
                            position = rotatedPosition;

                            if (parent == transform)
                            {
                                //org scale
                                scale = orgScale;
                            }
                        }
                    }
                    break;
            }
        }

        void PassInfoTo(Material mat)
        {
            if (mat == null)
                return;

            mat.SetFloat("_Rotation", rotation);
            mat.SetFloat("_PosX", position.x);
            mat.SetFloat("_PosY", position.y);
            mat.SetFloat("_ScaleX", scale.x);
            mat.SetFloat("_ScaleY", scale.y);
            mat.SetFloat("_Threshold", threshold);
            mat.SetFloat("_Pixelate", pixelateAmount);
            mat.SetVector("_SpriteAtlasSize", new Vector2(sr.sprite.rect.width, sr.sprite.rect.height));
            mat.SetVector("_SpriteAtlasOffset", sr.sprite.rect.position);
            mat.SetColor("_GlowColor", glowColor);
        }

        PassInfoTo(pixelMaterial);
        PassInfoTo(depthMaterial);
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

        //get info material
        if (depthMaterial == null)
        {
            depthMaterial = Instantiate(Resources.Load<Material>("Material/SpriteDepth"));
        }
        bool hasOutline = outlineRenderer != null;
        switch (camera.name)
        {
            case depthCameraName:
                sr.material = depthMaterial;
                if (hasOutline)
                {
                    outlineRenderer.sr.material = outlineRenderer.depthMaterial;
                }
                break;
            default:
                sr.material = pixelMaterial;
                if (hasOutline)
                {
                    outlineRenderer.sr.material = outlineRenderer.outlineMaterial;
                }
                break;
        }

    }



}
#if UNITY_EDITOR
[CustomEditor(typeof(PixelateSprite)), CanEditMultipleObjects]
class PixelateSpriteEditor : Editor
{
    SerializedProperty worldSpace;
    SerializedProperty localCenterType;
    SerializedProperty parent;
    SerializedProperty localCenterPosition;
    SerializedProperty threshold;
    SerializedProperty pixelateAmount;
    SerializedProperty followUniversalPixelateAmount;
    SerializedProperty onlyOffsetPosition;
    SerializedProperty glowColor;
    SerializedProperty outline;
    SerializedProperty orgScale;

    private void OnEnable()
    {
        worldSpace = serializedObject.FindProperty("worldSpace");
        localCenterType = serializedObject.FindProperty("localCenterType");
        parent = serializedObject.FindProperty("parent");
        localCenterPosition = serializedObject.FindProperty("localCenterPosition");
        threshold = serializedObject.FindProperty("threshold");
        pixelateAmount = serializedObject.FindProperty("pixelateAmount");
        followUniversalPixelateAmount = serializedObject.FindProperty("followUniversalPixelateAmount");
        onlyOffsetPosition = serializedObject.FindProperty("onlyOffsetPos");
        glowColor = serializedObject.FindProperty("glowColor");
        outline = serializedObject.FindProperty("outline");
        orgScale = serializedObject.FindProperty("orgScale");
    }
    public override void OnInspectorGUI()
    {
        var ps = (PixelateSprite)target;
        if (ps == null)
            return;
        Undo.RecordObject(ps, "Change PixelateSprite");

        GUIStyle fontStyle = new GUIStyle { fontSize = 20, normal = new GUIStyleState { textColor = Color.white } };


        GUILayout.Label("Positioning", fontStyle);
        //world space
        serializedObject.Update();
        EditorGUILayout.PropertyField(worldSpace);
        if (!ps.worldSpace)
        {
            EditorGUILayout.PropertyField(localCenterType);
            switch (ps.localCenterType)
            {
                case LocalCenterType.Transform:
                    EditorGUILayout.PropertyField(parent);
                    EditorGUILayout.PropertyField(onlyOffsetPosition);
                    EditorGUILayout.HelpBox("warning:parent's size must be stayed at 1x1 !!!!(if it's other object)", MessageType.Warning);
                    if (ps.parent == ps.transform)
                    {
                        EditorGUILayout.PropertyField(orgScale);
                    }
                    break;
                case LocalCenterType.Position:
                    EditorGUILayout.PropertyField(localCenterPosition);
                    break;
            }
        }
        GUILayout.Space(20);
        GUILayout.Label("Property", fontStyle);
        EditorGUILayout.PropertyField(threshold);
        EditorGUILayout.PropertyField(followUniversalPixelateAmount);
        EditorGUILayout.PropertyField(pixelateAmount);
        EditorGUILayout.PropertyField(glowColor);

        GUILayout.Space(20);
        GUILayout.Label("Outline", fontStyle);
        EditorGUILayout.PropertyField(outline);
        if (ps.outline)
        {
            GUILayout.Label("Outline", fontStyle);
            if (ps.outlineRenderer == null)
            {
                Selection.activeTransform = ps.transform;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif