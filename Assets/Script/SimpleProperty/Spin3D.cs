using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin3D : MonoBehaviour
{
    public Vector3 direction;
    // Start is c
    private void Update()
    {
        transform.Rotate(direction * Time.deltaTime);
    }
}
