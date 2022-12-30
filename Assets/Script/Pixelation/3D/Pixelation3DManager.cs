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
    public Material pixelatedScreenMat;

    private void Update()
    {
        Setresolution();
    }
    public void Setresolution()
    {
        //pixelCam.orthographicSize = mainCam.orthographicSize;
        float pixelSize = pixelatedAmount / 64;
        //pixelCam.transform.position = Vector2.zero;//new Vector2(mainCam.transform.position.x - mainCam.transform.position.x % pixelSize, mainCam.transform.position.y - mainCam.transform.position.y % pixelSize);
        //pixelatedScreen.transform.position = pixelCam.transform.position;
        //pixelatedScreen.transform.parent.position = pixelCam.transform.localPosition;
        //pixelCam.transform.position = new Vector2(pixelCam.transform.position.x - pixelCam.transform.position.x % pixelSize, pixelCam.transform.position.y - pixelCam.transform.position.y % pixelSize);
        float camSize = pixelCam.orthographicSize;
        pixelatedScreen.sizeDelta = new Vector2(32 * camSize / 9, 2 * camSize);
        pixelatedScreenMat.SetFloat("_PixelateAmount", pixelatedAmount);
        pixelatedScreenMat.SetFloat("_Scale", pixelCam.orthographicSize * 128 /pixelTextureSource.height);
        pixelatedScreenMat.SetVector("_Position", pixelatedScreen.transform.position);


    }
}
