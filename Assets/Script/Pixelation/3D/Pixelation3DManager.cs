using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pixelation3DManager : MonoBehaviour
{
    public RectTransform pixelatedScreen;
    public Camera mainCam;
    public Camera pixelCam;
    public RenderTexture pixelTextureSource;
    public float pixelatedAmount = 16;

    private void Update()
    {
        Setresolution();
    }
    Vector2 prePixelTexRes;
    public void Setresolution()
    {
        pixelCam.orthographicSize = mainCam.orthographicSize;
        float camSize = mainCam.orthographicSize;
        pixelatedScreen.sizeDelta = new Vector2(32 * camSize / 9, 2 * camSize);



        Vector2Int size = new Vector2Int((int)(1920/pixelatedAmount * camSize), (int)(1080 / pixelatedAmount * camSize));
        if (size != prePixelTexRes)
        {
            //set res
            pixelTextureSource.Release();
            pixelTextureSource.width = size.x;
            pixelTextureSource.height = size.y;
        }
        prePixelTexRes = size;
    }
}
