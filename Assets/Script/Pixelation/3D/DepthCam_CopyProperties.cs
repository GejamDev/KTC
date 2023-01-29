using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DepthCam_CopyProperties : MonoBehaviour
{
    public Camera pixelCam;
    public Camera thisCam;

    private void Update()
    {
        thisCam.orthographicSize = pixelCam.orthographicSize;
        thisCam.farClipPlane = pixelCam.farClipPlane + 0.1f;
        thisCam.nearClipPlane = pixelCam.nearClipPlane;
    }
}
