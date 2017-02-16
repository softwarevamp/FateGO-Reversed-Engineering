using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/NGUI Rot45 Sprite")]
public class UIRotSprite : UISprite
{
    [NonSerialized]
    private Rect mOuterUV = new Rect();

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture mainTexture = this.mainTexture;
        if (mainTexture != null)
        {
            if (base.mSprite == null)
            {
                base.mSprite = base.atlas.GetSprite(base.spriteName);
            }
            if (base.mSprite != null)
            {
                Rect rect = new Rect((float) base.mSprite.x, (float) base.mSprite.y, (float) base.mSprite.width, (float) base.mSprite.height);
                Rect rect2 = new Rect((float) (base.mSprite.x + base.mSprite.borderLeft), (float) (base.mSprite.y + base.mSprite.borderTop), (float) ((base.mSprite.width - base.mSprite.borderLeft) - base.mSprite.borderRight), (float) ((base.mSprite.height - base.mSprite.borderBottom) - base.mSprite.borderTop));
                rect = NGUIMath.ConvertToTexCoords(rect, mainTexture.width, mainTexture.height);
                rect2 = NGUIMath.ConvertToTexCoords(rect2, mainTexture.width, mainTexture.height);
                int size = verts.size;
                this.RotFill(verts, uvs, cols, rect, rect2);
                if (base.onPostFill != null)
                {
                    base.onPostFill(this, size, verts, uvs, cols);
                }
            }
        }
    }

    private void RotFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, Rect outer, Rect inner)
    {
        this.mOuterUV = outer;
        Vector4 drawingDimensions = this.drawingDimensions;
        Vector4 drawingUVs = this.drawingUVs;
        Color32 drawingColor = this.drawingColor;
        verts.Add(new Vector3((drawingDimensions.z + drawingDimensions.x) / 2f, drawingDimensions.y));
        verts.Add(new Vector3(drawingDimensions.x, (drawingDimensions.y + drawingDimensions.w) / 2f));
        verts.Add(new Vector3((drawingDimensions.z + drawingDimensions.x) / 2f, drawingDimensions.w));
        verts.Add(new Vector3(drawingDimensions.z, (drawingDimensions.y + drawingDimensions.w) / 2f));
        uvs.Add(new Vector2((drawingUVs.x + drawingUVs.z) / 2f, drawingUVs.y));
        uvs.Add(new Vector2(drawingUVs.x, (drawingUVs.y + drawingUVs.w) / 2f));
        uvs.Add(new Vector2((drawingUVs.x + drawingUVs.z) / 2f, drawingUVs.w));
        uvs.Add(new Vector2(drawingUVs.z, (drawingUVs.y + drawingUVs.w) / 2f));
        cols.Add(drawingColor);
        cols.Add(drawingColor);
        cols.Add(drawingColor);
        cols.Add(drawingColor);
    }

    private Color32 drawingColor
    {
        get
        {
            Color c = base.color;
            c.a = base.finalAlpha;
            if (this.premultipliedAlpha)
            {
                c = NGUITools.ApplyPMA(c);
            }
            if (QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                c.r = Mathf.Pow(c.r, 2.2f);
                c.g = Mathf.Pow(c.g, 2.2f);
                c.b = Mathf.Pow(c.b, 2.2f);
            }
            return c;
        }
    }

    private Vector4 drawingUVs
    {
        get
        {
            switch (base.mFlip)
            {
                case UIBasicSprite.Flip.Horizontally:
                    return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMin, this.mOuterUV.xMin, this.mOuterUV.yMax);

                case UIBasicSprite.Flip.Vertically:
                    return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMax, this.mOuterUV.xMax, this.mOuterUV.yMin);

                case UIBasicSprite.Flip.Both:
                    return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMax, this.mOuterUV.xMin, this.mOuterUV.yMin);
            }
            return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMin, this.mOuterUV.xMax, this.mOuterUV.yMax);
        }
    }
}

