using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin2D : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.eulerAngles += Vector3.forward * speed * Time.deltaTime;
    }
}
