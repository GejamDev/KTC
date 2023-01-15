using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum LocalCenterType
{
    Transform,Position
}

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class PixelateSprite : MonoBehaviour
{
    [HideInInspector]public SpriteRenderer sr;
    const string materialName = "PixelateSprite_Clone";
    public bool worldSpace = true;
    [HideInInspector] public LocalCenterType localCenterType;
    public Transform parent;
    public bool onlyOffsetPos;
    public Vector2 localCenterPosition;
    public float threshold = 0.95f;
    public bool followUniversalPixelateAmount = true;
    public float pixelateAmount = 16;
    [ColorUsage(true, true)]
    public Color glowColor;

    private void Awake()
    {
        SetMaterial();
    }
    public void SetMaterial()
    {
        sr = GetComponent<SpriteRenderer>();
        Material mat = Instantiate(Resources.Load<Material>("Material/Pixelate"));
        sr.material = mat;
        mat.name = materialName;
        sr.sharedMaterial = mat;
        if (parent == null)
            parent = transform;
    }

    private void FixedUpdate()
    {

    }
    private void Update()
    {
        PassInfoToShader();
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
                        parent.localScale = new Vector3(parent.localScale.x/ parent.lossyScale.x, parent.localScale.y / parent.lossyScale.y, 1);


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
                        }
                    }
                    break;
            }
        }





        sr.sharedMaterial.SetFloat("_Rotation", rotation);
        sr.sharedMaterial.SetFloat("_PosX", position.x);
        sr.sharedMaterial.SetFloat("_PosY", position.y);
        sr.sharedMaterial.SetFloat("_ScaleX", scale.x);
        sr.sharedMaterial.SetFloat("_ScaleY", scale.y);
        sr.sharedMaterial.SetFloat("_Threshold", threshold);
        sr.sharedMaterial.SetFloat("_Pixelate", pixelateAmount);
        sr.sharedMaterial.SetVector("_SpriteAtlasSize", new Vector2(sr.sprite.rect.width, sr.sprite.rect.height));
        sr.sharedMaterial.SetVector("_SpriteAtlasOffset", sr.sprite.rect.position);
        sr.sharedMaterial.SetColor("_GlowColor", glowColor);
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
                    EditorGUILayout.HelpBox("warning:parent's size must be stayed at 1x1 !!!!", MessageType.Warning);
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


        serializedObject.ApplyModifiedProperties();
    }
}
#endif