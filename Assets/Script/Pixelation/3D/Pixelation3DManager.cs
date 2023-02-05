using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Pixelation3DManager : MonoBehaviour
{
    public PixelationBundle pixelationBundle;
    public Camera mainCam;
    public float pixelatedAmount = 16;
    public bool followUniversalPixelateAmount = true;

    private void LateUpdate()
    {
        Setresolution();
        SetGlobalShaderVariable();
    }

    public void Setresolution()
    {
        //get pixelScale
        if (followUniversalPixelateAmount)
        {
            pixelatedAmount = PlayerPrefs.GetFloat("UniversalPixelateAmount");
            float pixelSize = pixelatedAmount / 64;
        }




        PixelationBundle pb = pixelationBundle;

        //get objects
        Camera pCam = pb.pixelCam;
        RectTransform pScreen = pb.pixelatedScreen;
        Material pMat = pb.pixelatedScreenMat;
        RenderTexture pTex = pb.pixelTextureSource;


        //set pivot grid
        pb.pivotGrid = new Vector2((pCam.orthographicSize - mainCam.orthographicSize) * 1.7f, (pCam.orthographicSize - mainCam.orthographicSize) * 0.9f);
        Vector2 pixelCamPivotGrid = pb.pivotGrid;
        Vector2 PivotResetDistance = pixelCamPivotGrid * 0.5f;


        //set pixelcam distance to cam distance;
        pCam.farClipPlane = mainCam.farClipPlane;
        pCam.nearClipPlane = mainCam.nearClipPlane;


        //check if pivot should be resetted
        if (mainCam.transform.position.x > pb.pivot.x + PivotResetDistance.x)
        {
            pb.pivot = new Vector2(Mathf.Floor(mainCam.transform.position.x / pixelCamPivotGrid.x) * pixelCamPivotGrid.x, pb.pivot.y);
        }
        else if (mainCam.transform.position.x < pb.pivot.x - PivotResetDistance.x)
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
        //pb.pixelatedScreenMat.SetFloat("_PixelateAmount", pixelatedAmount);
        Shader.SetGlobalFloat("_3DPixelatedScreenPixelateAmount", pixelatedAmount);
        //pb.pixelatedScreenMat.SetFloat("_Scale", pCam.orthographicSize * 128 / pTex.height);
        Shader.SetGlobalFloat("_PixelatedScreenScale", pCam.orthographicSize * 128 / pTex.height);
        pb.pixelatedScreenMat.SetVector("_Position", pScreen.position);


    }
    public void SetGlobalShaderVariable()
    {
        Vector3 pCamPos = pixelationBundle.pixelCam.transform.position;
        Shader.SetGlobalVector("_PixelCamPosition", new Vector4(pCamPos.x, pCamPos.y, pCamPos.z, 0));
        Shader.SetGlobalFloat("_PixelCamDistance", pixelationBundle.pixelCam.farClipPlane);
        Shader.SetGlobalFloat("_CamSize", mainCam.orthographicSize);
        Shader.SetGlobalFloat("_PixelCamSize", pixelationBundle.pixelCam.orthographicSize);
    }
    private void OnDrawGizmos()
    {
        float verticalHeightSeen = mainCam.orthographicSize * 2.0f;
        float far = mainCam.farClipPlane;
        float near = mainCam.nearClipPlane;
        float zdist = far - near;

        //box
        Gizmos.color = new Color(1, 1, 1, 0.5f);
        Gizmos.DrawWireCube(transform.position + (zdist + near * 2) * Vector3.forward * 0.5f, new Vector3((verticalHeightSeen * Camera.main.aspect), verticalHeightSeen, zdist));

        //point
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + near * Vector3.forward * 0.5f, new Vector3((verticalHeightSeen * Camera.main.aspect), verticalHeightSeen, 0));
    }
}

[System.Serializable]
public class PixelationBundle
{
    public RectTransform pixelatedScreen;
    public Transform canvas;
    public Camera pixelCam;
    public Camera infoCam;
    public RenderTexture pixelTextureSource;
    public Material pixelatedScreenMat;
    public Vector2 pivot;
    public Vector2 pivotGrid;
}
