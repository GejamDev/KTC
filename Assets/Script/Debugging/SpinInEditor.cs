using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpinInEditor : MonoBehaviour
{
    public Vector3 speed;
    private void Update()
    {
        transform.eulerAngles += speed * Time.deltaTime;
    }
}
