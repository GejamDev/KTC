using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PixelOutlineRenderer : MonoBehaviour
{

    SpriteRenderer sr;
    public PixelateSprite targetSprite;
    [ColorUsage(true, true)]
    public Color color;
    [Range(0, 10)]
    public int thickness = 1;
    public bool corner = false;

    [Tooltip("increase this value if outline gets cut. but increasing too much can lead to low resolution")]
    [Range(1, 50)]
    public float scaler = 2;


    const string materialName = "PixelOutline_Clone";

    private void Awake()
    {
        SetMaterial();
    }
    public void SetMaterial()
    {
        sr = GetComponent<SpriteRenderer>();
        Material mat = Instantiate(Resources.Load<Material>("Material/PixelOutline"));
        sr.material = mat;
        mat.name = materialName;
        sr.sharedMaterial = mat;
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            SetOutLine();
        }
    }
    private void FixedUpdate()
    {
        if (Application.isPlaying)
        {
            SetOutLine();
        }
    }
    void SetOutLine()
    {
        if (targetSprite == null)
            return;

        if (thickness == 0)
        {
            sr.color = new Color(1, 1, 1, 0);
            return;
        }
        sr.color = Color.white;
        sr.sprite = targetSprite.sr.sprite;


        transform.position = targetSprite.transform.position;

        transform.localScale = Vector2.one;


        float xthiccness = thickness / targetSprite.sr.sprite.rect.width* targetSprite.pixelateAmount / targetSprite.transform.lossyScale.x * 2;
        float ythiccness = thickness / targetSprite.sr.sprite.rect.height * targetSprite.pixelateAmount / targetSprite.transform.lossyScale.y * 2;
        //transform.localScale = new Vector2(1 + xthiccness, 1 + ythiccness);
        //transform.localScale = Vector2.one * 2;
        transform.localScale = Vector2.one * scaler;

        transform.eulerAngles = new Vector3(0, 0, targetSprite.transform.eulerAngles.z);
        Vector2 position = transform.position;
        Vector2 scale = targetSprite.transform.lossyScale;
        float rotation = transform.eulerAngles.z;


        if (!targetSprite.worldSpace)
        {
            switch (targetSprite.localCenterType)
            {
                case LocalCenterType.Position:
                    //offset transform
                    position -= targetSprite.localCenterPosition;
                    break;
                case LocalCenterType.Transform:
                    if (targetSprite.parent != null)
                    {
                        //set parent size
                        targetSprite.parent.localScale = new Vector2(targetSprite.parent.localScale.x / targetSprite.parent.lossyScale.x, targetSprite.parent.localScale.y / targetSprite.parent.lossyScale.y);



                        //offset transform
                        Vector2 relativePosition = position - (Vector2)targetSprite.parent.position;
                        position = relativePosition;
                        if (!targetSprite.onlyOffsetPos)
                        {
                            //offset rotation by parent's rotation
                            float parentRot = targetSprite.parent.eulerAngles.z;
                            rotation -= parentRot;



                            //rotates position back relative to parent's rotation
                            float position_rot = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
                            float targetRot = position_rot - parentRot;
                            Vector2 rotatedPosition = new Vector2(Mathf.Cos(targetRot * Mathf.Deg2Rad), Mathf.Sin(targetRot * Mathf.Deg2Rad)) * position.magnitude;
                            position = rotatedPosition;
                        }
                    }
                    break;
            }
        }



        //outline
        sr.sharedMaterial.SetColor("_OutlineColor", color);
        sr.sharedMaterial.SetFloat("_Thickness", thickness);
        sr.sharedMaterial.SetFloat("_Scaler", 1/scaler);
        sr.sharedMaterial.SetInt("_Corner", corner ? 1 : 0);

        //pixelation
        sr.sharedMaterial.SetFloat("_Rotation", rotation);
        sr.sharedMaterial.SetFloat("_PosX", position.x);
        sr.sharedMaterial.SetFloat("_PosY", position.y);
        sr.sharedMaterial.SetFloat("_ScaleX", scale.x);
        sr.sharedMaterial.SetFloat("_ScaleY", scale.y);
        sr.sharedMaterial.SetFloat("_Threshold", targetSprite.threshold);
        sr.sharedMaterial.SetFloat("_Pixelate", targetSprite.pixelateAmount);
        sr.sharedMaterial.SetVector("_SpriteAtlasSize", new Vector2(sr.sprite.rect.width, sr.sprite.rect.height));
        sr.sharedMaterial.SetVector("_SpriteAtlasOffset", sr.sprite.rect.position);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PixelOutlineRenderer)), CanEditMultipleObjects]
class PixelOutlineRendererEditor : Editor
{
    SerializedProperty targetSprite;
    SerializedProperty color;
    SerializedProperty thickness;
    SerializedProperty corner;
    SerializedProperty scaler;

    private void OnEnable()
    {
        targetSprite = serializedObject.FindProperty("targetSprite");
        color = serializedObject.FindProperty("color");
        thickness = serializedObject.FindProperty("thickness");
        corner = serializedObject.FindProperty("corner");
        scaler = serializedObject.FindProperty("scaler");
    }
    public override void OnInspectorGUI()
    {

        var por = (PixelOutlineRenderer)target;
        if (por == null)
            return;
        Undo.RecordObject(por, "Change PixelOutlineRenderer");

        GUIStyle fontStyle = new GUIStyle { fontSize = 20, normal = new GUIStyleState { textColor = Color.white } };


        GUILayout.Label("Target", fontStyle);
        EditorGUILayout.PropertyField(targetSprite);
        GUILayout.Space(10);
        GUILayout.Label("Property", fontStyle);
        EditorGUILayout.PropertyField(color);
        EditorGUILayout.PropertyField(thickness);
        EditorGUILayout.PropertyField(corner);
        GUILayout.Space(10);
        GUILayout.Label("Others", fontStyle);
        EditorGUILayout.HelpBox("increase this value if outline gets cut. but increasing too much might lead to low resolution", MessageType.Info);
        EditorGUILayout.PropertyField(scaler);


        serializedObject.ApplyModifiedProperties();
    }
}
#endif