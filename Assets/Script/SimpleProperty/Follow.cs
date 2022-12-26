using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float speed;

    private void Update()
    {
        Vector2 pos = Vector2.Lerp(transform.position, target.position, speed * Time.deltaTime);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
