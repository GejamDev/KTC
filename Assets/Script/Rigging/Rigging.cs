using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rigging : MonoBehaviour
{
    public List<Bone> bones;

    public void Update()
    {
        foreach(Bone b in bones)
        {
            ApplyBoneTransformation(b);
        }
    }
    public void ApplyBoneTransformation(Bone b)
    {
        if (b.boneObject == null)
            return;
        if (b.jointStart == null)
            return;
        if (b.jointEnd == null)
            return;
        GameObject g = b.boneObject;
        g.transform.position = (b.jointStart.position + b.jointEnd.position) * 0.5f;
        Vector2 dir = b.jointEnd.position - b.jointStart.position;
        float rot = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);
        g.transform.eulerAngles = new Vector3(0, 0, rot + 90);
        g.transform.localScale = new Vector2(b.thiccness, dir.magnitude);
    }
}
[System.Serializable]
public class Bone
{
    public GameObject boneObject;
    public Transform jointStart;
    public Transform jointEnd;
    public float thiccness = 1;
    [Tooltip("where original sprite is pointing at")]
    public Vector2 orgDirection;

}
