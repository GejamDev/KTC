using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rigging : MonoBehaviour
{
    public List<Bone> bones = new List<Bone>();

    public void Update()
    {
        transform.localScale = Vector2.one;
        foreach(Bone b in bones)
        {
            ApplyBoneTransformation(b);
        }
    }
    public void ApplyBoneTransformation(Bone b)
    {
        #region return if null
        if (b.jointStart == null)
            return;
        else if (b.jointEnd == null)
            return;

        Debug.DrawRay(b.jointStart.position, b.jointEnd.position - b.jointStart.position, Color.red);

        if (b.boneObject.obj == null)
        {
            if (transform.Find("BoneObject") != null)
            {
                b.boneObject.obj = transform.Find("BoneObject").gameObject;
            }
            else
            {
                return;
            }
        }
        if (b.boneObject.pivotStart == null)
        {
            if (b.boneObject.obj.transform.Find("PivotStart")!= null)
            {
                b.boneObject.pivotStart = b.boneObject.obj.transform.Find("PivotStart");
            }
            else
            {
                return;
            }
        }
        if (b.boneObject.pivotEnd == null)
        {
            if (b.boneObject.obj.transform.Find("PivotEnd") != null)
            {
                b.boneObject.pivotEnd = b.boneObject.obj.transform.Find("PivotEnd");
            }
            else
            {
                return;
            }
        }
        #endregion



        //get objects
        GameObject g = b.boneObject.obj;
        Transform t1 = b.boneObject.pivotStart;
        Transform t2 = b.boneObject.pivotEnd;




        //set pivot x pos

        t1.localPosition = new Vector2(t1.localPosition.x, t1.localPosition.y);
        t2.localPosition = new Vector2(t1.localPosition.x, t2.localPosition.y);


        Vector2 dir = b.jointEnd.position - b.jointStart.position;
        Vector2 dir_injoint = t1.localPosition - t2.localPosition;


        //scale
        Vector2 targetscale = new Vector2(b.thiccness, dir.magnitude / dir_injoint.magnitude);
        g.transform.localScale = new Vector2(targetscale.x * (g.transform.localScale.x / g.transform.lossyScale.x), targetscale.y * (g.transform.localScale.y / g.transform.lossyScale.y));


        //rotation
        float rot = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);
        float rot_injoint = Mathf.Rad2Deg * Mathf.Atan2(dir_injoint.y * g.transform.lossyScale.y, dir_injoint.x * g.transform.lossyScale.x);
        g.transform.eulerAngles = new Vector3(0, 0, rot - rot_injoint + 180);




        //set poisition
        g.transform.position = (b.jointStart.position + b.jointEnd.position) * 0.5f - ((t1.position + t2.position) * 0.5f - g.transform.position);





        //debug
        Debug.DrawRay(t1.position, t2.position - t1.position, Color.blue);

    }
}
[System.Serializable]
public class Bone
{
    public BoneObject boneObject;
    public Transform jointStart;
    public Transform jointEnd;
    public float thiccness = 1;

}

[System.Serializable]
public class BoneObject
{
    public GameObject obj;
    public Transform pivotStart;
    public Transform pivotEnd;

}
