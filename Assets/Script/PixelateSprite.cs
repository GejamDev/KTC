using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelateSprite : MonoBehaviour
{
    public SpriteRenderer sr;
    public Material mat;
    public string materialName;

    private void Awake()
    {
        SetMaterial();
    }
    public void SetMaterial()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr.sharedMaterial.name != materialName)
        {
            mat = Instantiate(sr.sharedMaterial);
            mat.name = materialName;
            sr.sharedMaterial = mat;
        }
    }
    private void Update()
    {

    }
}
