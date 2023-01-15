using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Pixelation3DManager : MonoBehaviour
{
    public PixelationBundle[] pixelationBundle;
    public Camera mainCam;
    public float pixelatedAmount = 16;

    private void Update()
    {
        Setresolution();
    }
    public void Setresolution()
    {
        //get pixelScale
        pixelatedAmount = PlayerPrefs.GetFloat("UniversalPixelateAmount");
        float pixelSize = pixelatedAmount / 64;

        foreach(PixelationBundle pb in pixelationBundle)
        {
            //get objects
            Camera pCam = pb.pixelCam;
            RectTransform pScreen = pb.pixelatedScreen;
            Material pMat = pb.pixelatedScreenMat;
            RenderTexture pTex = pb.pixelTextureSource;

            //move cam


            ////follows main cam(but jittery pixel)
            pCam.transform.localPosition = new Vector3(0, 0, pCam.transform.localPosition.z);
            pScreen.transform.localPosition = new Vector3(0, 0, pScreen.transform.localPosition.z);

            //don't follow , but has stable pixel
            //pCam.transform.position = new Vector3(0, 0, pCam.transform.position.z);
            //pScreen.transform.position = new Vector3(0, 0, pScreen.transform.position.z);

            //set pixel size and stuff
            float camSize = pCam.orthographicSize;
            pScreen.sizeDelta = new Vector2(32 * camSize / 9, 2 * camSize);
            pb.pixelatedScreenMat.SetFloat("_PixelateAmount", pixelatedAmount);
            pb.pixelatedScreenMat.SetFloat("_Scale", pCam.orthographicSize * 128 / pTex.height);
            pb.pixelatedScreenMat.SetVector("_Position", pScreen.position);

        }

    }
}

[System.Serializable]
public class PixelationBundle
{
    public RectTransform pixelatedScreen;
    public Camera pixelCam;
    public RenderTexture pixelTextureSource;
    public Material pixelatedScreenMat;
}
