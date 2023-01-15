using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PixelLineRenderer : MonoBehaviour
{

    SpriteRenderer sr;
    const string materialName = "PixelLine_Clone";

    public Transform[] vectors;
    public Transform a;
    public Transform b;
    public float pixelateAmount = 16;
    [ColorUsage(true, true)]
    public Color color;


    private void Awake()
    {
        SetMaterial();
    }
    public void SetMaterial()
    {
        sr = GetComponent<SpriteRenderer>();
        Material mat = Instantiate(Resources.Load<Material>("Material/PixelLine"));
        sr.material = mat;
        mat.name = materialName;
        sr.sharedMaterial = mat;
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            SetLine();
        }
    }
    private void FixedUpdate()
    {
        if (Application.isPlaying)
        {
            SetLine();
        }
    }
    void SetLine()
    {
        //set transform
        float maxx = float.MinValue;
        float minx = float.MaxValue;
        float maxy = float.MinValue;
        float miny = float.MaxValue;

        foreach (Transform t in vectors)
        {
            if (t.position.x > maxx)
                maxx = t.position.x;
            if (t.position.x < minx)
                minx = t.position.x;
            if (t.position.y > maxy)
                maxy = t.position.y;
            if (t.position.y < miny)
                miny = t.position.y;
        }
        transform.position = (new Vector2(maxx, maxy) + new Vector2(minx, miny)) * 0.5f;
        transform.localScale = new Vector3(maxx - minx + 1, maxy - miny + 1, 1);





        Debug.DrawRay(a.transform.position, b.transform.position - a.transform.position, Color.red);
        //pass info
        sr.sharedMaterial.SetFloat("_PixelateAmount", pixelateAmount);
        sr.sharedMaterial.SetColor("_LineColor", color);
        sr.sharedMaterial.SetVector("_StartPos", a.transform.position - transform.position);
        sr.sharedMaterial.SetVector("_Scale", transform.lossyScale);
        sr.sharedMaterial.SetVector("_Position", transform.position);
        sr.sharedMaterial.SetVector("_EndPos", b.transform.position - transform.position);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PixelLineRenderer)), CanEditMultipleObjects]
class PixelLineRendererEditor : Editor
{

    private void OnEnable()
    {
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        //var por = (PixelOutlineRenderer)target;
        //if (por == null)
        //    return;
        //Undo.RecordObject(por, "Change PixelOutlineRenderer");

        //GUIStyle fontStyle = new GUIStyle { fontSize = 20, normal = new GUIStyleState { textColor = Color.white } };


        //GUILayout.Label("Target", fontStyle);
        //EditorGUILayout.PropertyField(targetSprite);
        //GUILayout.Space(10);
        //GUILayout.Label("Property", fontStyle);
        //EditorGUILayout.PropertyField(color);
        //EditorGUILayout.PropertyField(thickness);
        //EditorGUILayout.PropertyField(corner);
        //GUILayout.Space(10);
        //GUILayout.Label("Others", fontStyle);
        //EditorGUILayout.HelpBox("increase this value if outline gets cut. but increasing too much might lead to low resolution", MessageType.Info);
        //EditorGUILayout.PropertyField(scaler);


        //serializedObject.ApplyModifiedProperties();
    }
}
#endif