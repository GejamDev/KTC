using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryMovement : MonoBehaviour
{
    public Animator an;
    public Rigidbody2D rb;
    public float movespeed;
    public float jumpforce;
    public int lastdir;
    public float rottime;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastdir = 1;
    }

    private void Update()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * movespeed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            lastdir = (int)Input.GetAxisRaw("Horizontal");
        }
        an.transform.localScale = new Vector2(Mathf.Lerp(an.transform.localScale.x, lastdir, Time.deltaTime*rottime), 1);
        an.SetBool("running", Input.GetAxisRaw("Horizontal") != 0);
    }
}
