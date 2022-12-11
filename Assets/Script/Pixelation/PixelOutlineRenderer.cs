using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PixelOutlineRenderer : MonoBehaviour
{
    public SpriteRenderer[] targetSprites;
    public float thickness;
    public Vector2 offset;
    public SpriteRenderer resultSpriteRenderer;

    private void Update()
    {
        if (resultSpriteRenderer == null)
            resultSpriteRenderer = GetComponent<SpriteRenderer>();

        //set pos/scale to ideal value where all target sprites can be renderered outline

        //pass target sprite's info to result sprite shader

        //and that shader will render outline fuck
    }
}
