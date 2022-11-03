using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelateSprite : MonoBehaviour
{
    public SpriteRenderer sr;
    public Material pixelatingMaterial;

    private void Awake()
    {
        GetRendererData();
    }
    private void Update()
    {
        if (Application.isEditor)
        {
            GetRendererData();
        }
        pixelatingMaterial.SetFloat("_PosX", transform.position.x);
        pixelatingMaterial.SetFloat("_PosY", transform.position.y);
        pixelatingMaterial.SetFloat("_ScaleX", transform.lossyScale.x);
        pixelatingMaterial.SetFloat("_ScaleY", transform.lossyScale.y);

    }
    void GetRendererData()
    {
         sr = GetComponent<SpriteRenderer>();
         pixelatingMaterial = Instantiate(sr.sharedMaterial);
        sr.material = pixelatingMaterial;
    }
}
