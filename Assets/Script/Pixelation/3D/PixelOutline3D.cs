using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class PixelOutline3D : MonoBehaviour
{
    public const string mainCamName = "Main Camera";
    public const string pixelCam = "3DPixelCamera";
    public const string pixelInfoCam = "3DInfoCamera";
    public MeshRenderer mesh;
    public Material[] originalMat;
    public Material infoMaterial;


    #region make setmaterial called when rendering
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += SetMaterial;
    }
    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= SetMaterial;
    }
    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= SetMaterial;
    }
    #endregion
    void SetMaterial(ScriptableRenderContext context, Camera camera)
    {
        switch (camera.name)
        {
            case mainCamName:
                mesh.materials = originalMat;
                break;
            case pixelCam:
                mesh.materials = originalMat;
                break;
            case pixelInfoCam:
                mesh.materials = new Material[1]{ infoMaterial };
                break;
            default:
                mesh.materials = originalMat;
                break;
        }
    }
}
