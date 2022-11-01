using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelateCam : MonoBehaviour
{
    [Range(1, 100)] public int pixelate = 1;
    //public Vector2 pin;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Vector2 offset = (Vector2)transform.position - pin;
        //Vector2 offset_resized = new Vector2(offset.x % source.width / pixelate, offset.y % source.width / pixelate);
        source.filterMode = FilterMode.Point;
        RenderTexture resultTexture = RenderTexture.GetTemporary(source.width / pixelate, source.height / pixelate, 0, source.format);
        resultTexture.filterMode = FilterMode.Point;
        Graphics.Blit(source, resultTexture);
        Graphics.Blit(resultTexture, destination);
        RenderTexture.ReleaseTemporary(resultTexture);



        //Vector2 offset = (Vector2)transform.position - pin;
        //Vector2 offset_resized = new Vector2(offset.x % source.width / pixelate, offset.y % source.width / pixelate);
        //source.filterMode = FilterMode.Point;
        //RenderTexture resultTexture = RenderTexture.GetTemporary(source.width / pixelate, source.height/pixelate, 0, source.format);
        //resultTexture.filterMode = FilterMode.Point;
        //Graphics.Blit(source, resultTexture);
        //Graphics.Blit(resultTexture, destination, Vector2.one , Vector2.zero);
        //RenderTexture.ReleaseTemporary(resultTexture);
    }
}
