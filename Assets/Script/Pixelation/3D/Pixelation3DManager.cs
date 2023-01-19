using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Pixelation3DManager : MonoBehaviour
{
    public PixelationBundle[] pixelationBundle;
    public Camera mainCam;
    public float pixelatedAmount = 16;
    public bool followUniversalPixelateAmount = true;


    private void Update()
    {
        Setresolution();
    }
    public void Setresolution()
    {
        //get pixelScale
        if (followUniversalPixelateAmount)
        {
            pixelatedAmount = PlayerPrefs.GetFloat("UniversalPixelateAmount");
            float pixelSize = pixelatedAmount / 64;
        }
        foreach (PixelationBundle pb in pixelationBundle)
        {
            //get objects
            Camera pCam = pb.pixelCam;
            RectTransform pScreen = pb.pixelatedScreen;
            Material pMat = pb.pixelatedScreenMat;
            RenderTexture pTex = pb.pixelTextureSource;

            #region old jittery cam move code
            //move cam


            ////follows main cam(but jittery pixel)
            //pCam.transform.localPosition = new Vector3(0, 0, pCam.transform.localPosition.z);
            //pScreen.transform.localPosition = new Vector3(0, 0, pScreen.transform.localPosition.z);
            #endregion

            pb.pivotGrid = new Vector2((pCam.orthographicSize-mainCam.orthographicSize) * 1.7f, (pCam.orthographicSize - mainCam.orthographicSize) * 0.9f);
            Vector2 pixelCamPivotGrid = pb.pivotGrid;
            Vector2 PivotResetDistance = pixelCamPivotGrid*0.5f;


            //check if pivot should be resetted
            if (mainCam.transform.position.x> pb.pivot.x + PivotResetDistance.x)
            {
                pb.pivot = new Vector2(Mathf.Floor(mainCam.transform.position.x/ pixelCamPivotGrid.x)* pixelCamPivotGrid.x, pb.pivot.y);
            }
            else if(mainCam.transform.position.x < pb.pivot.x - PivotResetDistance.x)
            {
                pb.pivot = new Vector2(Mathf.Ceil(mainCam.transform.position.x / pixelCamPivotGrid.x) * pixelCamPivotGrid.x, pb.pivot.y);
            }


            if (mainCam.transform.position.y > pb.pivot.y + PivotResetDistance.y)
            {
                pb.pivot = new Vector2(pb.pivot.x, Mathf.Floor(mainCam.transform.position.y / pixelCamPivotGrid.y) * pixelCamPivotGrid.y);
            }
            else if (mainCam.transform.position.y < pb.pivot.y - PivotResetDistance.y)
            {
                pb.pivot = new Vector2(pb.pivot.x, Mathf.Ceil(mainCam.transform.position.y / pixelCamPivotGrid.y) * pixelCamPivotGrid.y);
            }

            //move cam/screen to cam pivot pos
            pCam.transform.position = new Vector3(pb.pivot.x, pb.pivot.y, pCam.transform.position.z);
            pScreen.transform.position = new Vector3(pb.pivot.x, pb.pivot.y, pScreen.transform.position.z);

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
    public Vector2 pivot;
    public Vector2 pivotGrid;
}
