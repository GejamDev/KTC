using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class InfoCam_CopyProperties : MonoBehaviour
{
    public Camera pixelCam;
    public Camera thisCam;

    private void Update()
    {
        thisCam.orthographicSize = pixelCam.orthographicSize;
        thisCam.farClipPlane = pixelCam.farClipPlane;
        thisCam.nearClipPlane = pixelCam.nearClipPlane;
    }
}
