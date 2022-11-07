using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PixelateSprite : MonoBehaviour
{
    SpriteRenderer sr;
    const string materialName = "Own";
    public float threshold;
    public float pixelateAmount;


    private void Awake()
    {
        SetMaterial();
    }
    public void SetMaterial()
    {
        sr = GetComponent<SpriteRenderer>();
        Material mat;
        mat = Instantiate(sr.sharedMaterial);
        mat.name = materialName;
        sr.sharedMaterial = mat;
    }
    private void Update()
    {
        sr.sharedMaterial.SetFloat("_Rotation", transform.eulerAngles.z);
        sr.sharedMaterial.SetFloat("_PosX", transform.position.x);
        sr.sharedMaterial.SetFloat("_PosY", transform.position.y);
        sr.sharedMaterial.SetFloat("_ScaleX", transform.lossyScale.x);
        sr.sharedMaterial.SetFloat("_ScaleY", transform.lossyScale.y);
        sr.sharedMaterial.SetFloat("_Threshold", threshold);
        sr.sharedMaterial.SetFloat("_Pixelate", pixelateAmount);
    }
}
