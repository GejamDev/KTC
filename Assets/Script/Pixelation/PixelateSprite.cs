using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PixelateSprite : MonoBehaviour
{
    SpriteRenderer sr;
    const string materialName = "Own";
    public float threshold = 0.95f;
    public float pixelateAmount = 16;


    private void Awake()
    {
        SetMaterial();
    }
    public void SetMaterial()
    {
        sr = GetComponent<SpriteRenderer>();
        Material mat = Resources.Load<Material>("Material/Pixelate");
        sr.material = mat;
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
        sr.sharedMaterial.SetFloat("_LocalScaleX", transform.localScale.x);
        sr.sharedMaterial.SetFloat("_LocalScaleY", transform.localScale.y);
        sr.sharedMaterial.SetFloat("_Threshold", threshold);
        sr.sharedMaterial.SetFloat("_Pixelate", pixelateAmount);
        sr.sharedMaterial.SetVector("_SpriteAtlasSize", new Vector2(sr.sprite.rect.width, sr.sprite.rect.height));
        sr.sharedMaterial.SetVector("_SpriteAtlasOffset", sr.sprite.rect.position);
    }
}
