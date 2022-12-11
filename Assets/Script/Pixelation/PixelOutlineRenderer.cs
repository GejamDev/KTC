using UnityEngine;
[DisallowMultipleComponent]
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PixelOutlineRenderer : MonoBehaviour
{
    public SpriteRenderer[] targetSprites;
    public float thickness = 1;
    [ColorUsage(true, true)]
    public Color color = Color.white;
    public Vector2 offset;
    public SpriteRenderer sr;

    const string matName = "PixelOutline_Clone";


    public void SetMaterial()
    {
        sr = GetComponent<SpriteRenderer>();
        Material mat = Instantiate(Resources.Load<Material>("Material/PixelOutline"));
        sr.material = mat;
        mat.name = matName;
        sr.sharedMaterial = mat;
    }

    private void Update()
    {
        //set sprite renderer
        if (sr == null)
        {
            SetMaterial();
        }
        else if (sr.sharedMaterial.name != matName)
        {
            SetMaterial();
        }


        SetColor();
        SetOutline();
    }
    public void SetColor()
    {
        sr.sharedMaterial.SetColor("_Color", color);
    }
    public void SetOutline()
    {

        //set pos/scale to ideal value where all target sprites can be renderered outline
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        foreach (SpriteRenderer sr in targetSprites)
        {
            //get position
            Vector2 pos = sr.transform.position;


            //get scale relative to angle
            Vector2 scale = new Vector2(Mathf.Abs(sr.sprite.rect.width * sr.transform.lossyScale.x / sr.sprite.pixelsPerUnit), Mathf.Abs(sr.sprite.rect.height * sr.transform.lossyScale.y / sr.sprite.pixelsPerUnit));
            float objAngle = sr.transform.eulerAngles.z * Mathf.Deg2Rad;
            if (objAngle != 0)
            {
                float orgAngle0 = Mathf.Atan2(scale.y, scale.x);
                Vector2 corner0 = new Vector2(Mathf.Cos(orgAngle0 + objAngle), Mathf.Sin(orgAngle0 + objAngle)) * scale.magnitude;
                float orgAngle1 = Mathf.Atan2(-scale.y, scale.x);
                Vector2 corner1 = new Vector2(Mathf.Cos(orgAngle1 + objAngle), Mathf.Sin(orgAngle1 + objAngle)) * scale.magnitude;


                scale = Vector2.zero;

                scale += Vector2.right * (Mathf.Abs(corner0.x) > Mathf.Abs(corner1.x) ? Mathf.Abs(corner0.x) : Mathf.Abs(corner1.x));
                scale += Vector2.up * (Mathf.Abs(corner0.y) > Mathf.Abs(corner1.y) ? Mathf.Abs(corner0.y) : Mathf.Abs(corner1.y));
            }




            //find corners
            float _minx = sr.transform.position.x - scale.x * 0.5f;
            float _maxx = sr.transform.position.x + scale.x * 0.5f;
            float _miny = sr.transform.position.y - scale.y * 0.5f;
            float _maxy = sr.transform.position.y + scale.y * 0.5f;
            if (_minx < minX)
                minX = _minx;
            if (_maxx > maxX)
                maxX = _maxx;
            if (_miny < minY)
                minY = _miny;
            if (_maxy > maxY)
                maxY = _maxy;
        }
        minX -= thickness;
        maxX += thickness;
        minY -= thickness;
        maxY += thickness;
        transform.position = new Vector2(minX + maxX, minY + maxY) * 0.5f;
        transform.localScale = new Vector2(maxX - minX, maxY - minY);




        //pass target sprite's info to result sprite shader

        //turn transform data into texture
        Texture2D objInfoTex = new Texture2D(2, targetSprites.Length);



        //collect data
        for(int i =0; i < targetSprites.Length; i++)
        {
            SpriteRenderer tsr = targetSprites[i];
            objInfoTex.SetPixel(0, i, new Color(tsr.transform.position.x - transform.position.x, tsr.transform.position.y - transform.position.y, tsr.transform.position.z - transform.position.z, tsr.transform.eulerAngles.z));
            objInfoTex.SetPixel(1, i, new Color(tsr.transform.lossyScale.x, tsr.transform.lossyScale.y, tsr.transform.lossyScale.z, 0));
        }


        //make texture
        objInfoTex.Apply();
        objInfoTex.filterMode = FilterMode.Point;

        //pass data
        sr.sharedMaterial.SetTexture("_ObjectsInformation", objInfoTex);

        //and that shader will render outline fuck
    }
}
