using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteCombiner : MonoBehaviour
{
    public SpriteRenderer sr;
    public Vector2Int size;
    public float pixelperunit;

    private void Update()
    {
        Texture2D texture = new Texture2D(size.x, size.y);
        for(int x = 0; x< size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                texture.SetPixel(x, y, Color.Lerp(Color.white, Color.black, (float)y/size.y));
            }
        }
        texture.Apply();
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        sr.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), pixelperunit);
    }
}
