using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public enum SortingObjectType
{
    Object2D,
    Object3D,
    NonPixelated3D,
    MainCamera,
    PixScreen,
    PixCam

}
[ExecuteAlways]
public class ObjectSorter : MonoBehaviour
{
    [Range(0, 6)]
    public int layer;
    public SortingObjectType objectType = SortingObjectType.NonPixelated3D;
    public const float gap = 50;
    public const float Obj2DNonPix3DSpace = 100;
    public const float Obj3DSpace = 100;
    public const int spriteOrderGap = 100;
    public const string pixelation3DLayerName = /*N*/ "_3DPixelation";
    SpriteRenderer sr;
    Canvas cv;



    void Awake()
    {
    }
    private void Update()
    {
        if (!Application.isPlaying)
        {
            if (LayerMask.LayerToName(gameObject.layer).Contains("3DPixelation"))
            {
                objectType = SortingObjectType.Object3D;
            }
            else if(TryGetComponent(out sr))
            {
                objectType = SortingObjectType.Object2D;
            }
        }
        switch (objectType)
        {
            case SortingObjectType.Object2D:
                {
                    //get spriterenderer
                    if (sr == null)
                    {
                        if (!TryGetComponent(out sr))
                        {
                            Debug.LogError("ObjectSorter: SpriteRenderer Not found!!! : " + gameObject.name);
                        }
                    }
                    //Update layer to correct one
                    layer = FindCorrectLayer(new int[4] { 0, 2, 4, 6 });

                    //clamp Z pos

                    float startpoint0 = -(gap * layer * 1.5f + Obj3DSpace * layer * 0.5f + Obj2DNonPix3DSpace * layer * 0.5f);
                    ClampZPos(startpoint0 - Obj2DNonPix3DSpace, startpoint0);
                    //floor z pos
                    FloorZPos(startpoint0, Obj2DNonPix3DSpace / spriteOrderGap);

                    //set sprite order
                    sr.sortingOrder = Mathf.FloorToInt((startpoint0 - transform.position.z) * spriteOrderGap / Obj2DNonPix3DSpace) + (int)(layer * 0.5f * (1 + spriteOrderGap)) + 1 * (int)(layer * 0.5f + 1);
                }
                break;
            case SortingObjectType.NonPixelated3D:
                {
                    //Update layer to correct one
                    layer = FindCorrectLayer(new int[4] { 0, 2, 4, 6 });

                    //clamp Z pos
                    float startpoint1 = -(gap * layer + Obj3DSpace * layer / 2);
                    ClampZPos(startpoint1 - Obj2DNonPix3DSpace, startpoint1);
                }
                break;
            case SortingObjectType.Object3D:
                //Update layer to correct one
                layer = FindCorrectLayer(new int[3] { 1,3,5 });

                //clamp Z pos
                float startpoint = -(gap * (1.5f * layer - 0.5f) + Obj2DNonPix3DSpace * (layer * 0.5f + 0.5f) + Obj3DSpace * (layer*0.5f -0.5f));
                ClampZPos(startpoint - Obj3DSpace, startpoint);

                //set layer
                gameObject.layer = LayerMask.NameToLayer((layer * 0.5f - 0.5f).ToString() + "_3DPixelation");
                break;
            case SortingObjectType.PixScreen:

                //get canvas
                if (cv == null)
                {
                    if (!TryGetComponent(out cv))
                    {
                        Debug.LogError("ObjectSorter: Canvas Not found!!! : " + gameObject.name);
                    }
                }


                //Update layer to correct one
                layer = FindCorrectLayer(new int[3] { 1, 3, 5 });

                //set z pos
                float pixelScreenZ = -(gap * (1.5f * layer + 0.5f) + Obj2DNonPix3DSpace * (layer * 0.5f + 0.5f) + Obj3DSpace * (layer * 0.5f + 0.5f));
                SetZPos(pixelScreenZ);

                //set orderin layer
                cv.sortingOrder = (spriteOrderGap + 2)*(int)(layer*0.5f + 0.5f);
                break;
            case SortingObjectType.PixCam:



                //Update layer to correct one
                layer = FindCorrectLayer(new int[3] { 1, 3, 5 });

                //set z pos
                float pixelCamZ = -(gap * (1.5f * layer + 0.5f) + Obj2DNonPix3DSpace * (layer * 0.5f + 0.5f) + Obj3DSpace * (layer * 0.5f + 0.5f) + gap);
                SetZPos(pixelCamZ);

                break;
            case SortingObjectType.MainCamera:
                float camZ = -(gap * 10 + Obj2DNonPix3DSpace * 4 + Obj3DSpace * 3);
                SetZPos(camZ);
                break;




            default:
                Debug.LogError("Object Sorting Type Not Found wtf!!!!!!!!!!!! nidayce" + objectType.ToString());
                break;
        }

    }
    public void ClampZPos(float min, float max)
    {
        float z = transform.position.z;
        if (z < min)
        {
            SetZPos(min);
        }
        else if (z > max)
        {
            SetZPos(max);
        }
    }
    public void FloorZPos(float startPoint, float step)
    {
        float z = transform.position.z;
        z -= startPoint;
        z = step * Mathf.Floor(z / step) + startPoint;
        SetZPos(z);
    }
    public void SetZPos(float value)
    {
        //if (Application.isEditor)
        //{
        //    if (Selection.transforms.Contains(transform))
        //    {
        //        Debug.Log("value:" + value);
        //        return;
        //    }
        //}
        transform.position = new Vector3(transform.position.x, transform.position.y, value);
    }
    int FindCorrectLayer(int[] array)
    {
        int minValue = int.MinValue;
        bool found = false;

        foreach (int value in array)
        {
            if (value <= layer && value > minValue)
            {
                minValue = value;
                found = true;
            }
        }
        if (!found)
            return array.Min();
        else
            return minValue;
    }

}
