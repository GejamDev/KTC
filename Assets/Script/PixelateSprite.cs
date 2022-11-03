using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelateSprite : MonoBehaviour
{
    public SpriteRenderer sr;
    public Material mat;
    const string materialName = "Own";

    private void Awake()
    {
        SetMaterial();
    }
    public void SetMaterial()
    {
        sr = GetComponent<SpriteRenderer>();

        mat = Instantiate(sr.sharedMaterial);
        mat.name = materialName;
        sr.sharedMaterial = mat;
    }
    private void Update()
    {

    }
}
