using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Unity2D Sprite"), ExecuteInEditMode]
public class UI2DSprite : UIBasicSprite
{
    [HideInInspector, SerializeField]
    private Vector4 mBorder = Vector4.zero;
    [HideInInspector, SerializeField]
    private bool mFixedAspect;
    [HideInInspector, SerializeField]
    private Material mMat;
    [HideInInspector, SerializeField]
    private float mPixelSize = 1f;
    [NonSerialized]
    private int mPMA = -1;
    [SerializeField, HideInInspector]
    private Shader mShader;
    [SerializeField, HideInInspector]
    private UnityEngine.Sprite mSprite;
    public UnityEngine.Sprite nextSprite;

    public override void MakePixelPerfect()
    {
        base.MakePixelPerfect();
        if (base.mType != UIBasicSprite.Type.Tiled)
        {
            Texture mainTexture = this.mainTexture;
            if ((mainTexture != null) && ((((base.mType == UIBasicSprite.Type.Simple) || (base.mType == UIBasicSprite.Type.Filled)) || !base.hasBorder) && (mainTexture != null)))
            {
                Rect rect = this.mSprite.rect;
                int num = Mathf.RoundToInt(rect.width);
                int num2 = Mathf.RoundToInt(rect.height);
                if ((num & 1) == 1)
                {
                    num++;
                }
                if ((num2 & 1) == 1)
                {
                    num2++;
                }
                base.width = num;
                base.height = num2;
            }
        }
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture mainTexture = this.mainTexture;
        if (mainTexture != null)
        {
            Rect outer = (this.mSprite == null) ? new Rect(0f, 0f, (float) mainTexture.width, (float) mainTexture.height) : this.mSprite.textureRect;
            Rect inner = outer;
            Vector4 border = this.border;
            inner.xMin += border.x;
            inner.yMin += border.y;
            inner.xMax -= border.z;
            inner.yMax -= border.w;
            float num = 1f / ((float) mainTexture.width);
            float num2 = 1f / ((float) mainTexture.height);
            outer.xMin *= num;
            outer.xMax *= num;
            outer.yMin *= num2;
            outer.yMax *= num2;
            inner.xMin *= num;
            inner.xMax *= num;
            inner.yMin *= num2;
            inner.yMax *= num2;
            int size = verts.size;
            base.Fill(verts, uvs, cols, outer, inner);
            if (base.onPostFill != null)
            {
                base.onPostFill(this, size, verts, uvs, cols);
            }
        }
    }

    protected override void OnUpdate()
    {
        if (this.nextSprite != null)
        {
            if (this.nextSprite != this.mSprite)
            {
                this.sprite2D = this.nextSprite;
            }
            this.nextSprite = null;
        }
        base.OnUpdate();
        if (this.mFixedAspect && (this.mainTexture != null))
        {
            int num = Mathf.RoundToInt(this.mSprite.rect.width);
            int num2 = Mathf.RoundToInt(this.mSprite.rect.height);
            int num3 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
            int num4 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
            int num5 = Mathf.RoundToInt((this.mSprite.rect.width - this.mSprite.textureRect.width) - this.mSprite.textureRectOffset.x);
            int num6 = Mathf.RoundToInt((this.mSprite.rect.height - this.mSprite.textureRect.height) - this.mSprite.textureRectOffset.y);
            num += num3 + num5;
            num2 += num6 + num4;
            float mWidth = base.mWidth;
            float mHeight = base.mHeight;
            float num9 = mWidth / mHeight;
            float num10 = ((float) num) / ((float) num2);
            if (num10 < num9)
            {
                float x = ((mWidth - (mHeight * num10)) / mWidth) * 0.5f;
                base.drawRegion = new Vector4(x, 0f, 1f - x, 1f);
            }
            else
            {
                float y = ((mHeight - (mWidth / num10)) / mHeight) * 0.5f;
                base.drawRegion = new Vector4(0f, y, 1f, 1f - y);
            }
        }
    }

    public override Vector4 border
    {
        get => 
            this.mBorder;
        set
        {
            if (this.mBorder != value)
            {
                this.mBorder = value;
                this.MarkAsChanged();
            }
        }
    }

    public override Vector4 drawingDimensions
    {
        get
        {
            float num13;
            float num14;
            Vector2 pivotOffset = base.pivotOffset;
            float a = -pivotOffset.x * base.mWidth;
            float num2 = -pivotOffset.y * base.mHeight;
            float b = a + base.mWidth;
            float num4 = num2 + base.mHeight;
            if ((this.mSprite != null) && (base.mType != UIBasicSprite.Type.Tiled))
            {
                int num5 = Mathf.RoundToInt(this.mSprite.rect.width);
                int num6 = Mathf.RoundToInt(this.mSprite.rect.height);
                int num7 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
                int num8 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
                int num9 = Mathf.RoundToInt((this.mSprite.rect.width - this.mSprite.textureRect.width) - this.mSprite.textureRectOffset.x);
                int num10 = Mathf.RoundToInt((this.mSprite.rect.height - this.mSprite.textureRect.height) - this.mSprite.textureRectOffset.y);
                float num11 = 1f;
                float num12 = 1f;
                if (((num5 > 0) && (num6 > 0)) && ((base.mType == UIBasicSprite.Type.Simple) || (base.mType == UIBasicSprite.Type.Filled)))
                {
                    if ((num5 & 1) != 0)
                    {
                        num9++;
                    }
                    if ((num6 & 1) != 0)
                    {
                        num10++;
                    }
                    num11 = (1f / ((float) num5)) * base.mWidth;
                    num12 = (1f / ((float) num6)) * base.mHeight;
                }
                if ((base.mFlip == UIBasicSprite.Flip.Horizontally) || (base.mFlip == UIBasicSprite.Flip.Both))
                {
                    a += num9 * num11;
                    b -= num7 * num11;
                }
                else
                {
                    a += num7 * num11;
                    b -= num9 * num11;
                }
                if ((base.mFlip == UIBasicSprite.Flip.Vertically) || (base.mFlip == UIBasicSprite.Flip.Both))
                {
                    num2 += num10 * num12;
                    num4 -= num8 * num12;
                }
                else
                {
                    num2 += num8 * num12;
                    num4 -= num10 * num12;
                }
            }
            if (this.mFixedAspect)
            {
                num13 = 0f;
                num14 = 0f;
            }
            else
            {
                Vector4 vector2 = (Vector4) (this.border * this.pixelSize);
                num13 = vector2.x + vector2.z;
                num14 = vector2.y + vector2.w;
            }
            float x = Mathf.Lerp(a, b - num13, this.mDrawRegion.x);
            float y = Mathf.Lerp(num2, num4 - num14, this.mDrawRegion.y);
            float z = Mathf.Lerp(a + num13, b, this.mDrawRegion.z);
            return new Vector4(x, y, z, Mathf.Lerp(num2 + num14, num4, this.mDrawRegion.w));
        }
    }

    public override Texture mainTexture
    {
        get
        {
            if (this.mSprite != null)
            {
                return this.mSprite.texture;
            }
            if (this.mMat != null)
            {
                return this.mMat.mainTexture;
            }
            return null;
        }
    }

    public override Material material
    {
        get => 
            this.mMat;
        set
        {
            if (this.mMat != value)
            {
                base.RemoveFromPanel();
                this.mMat = value;
                this.mPMA = -1;
                this.MarkAsChanged();
            }
        }
    }

    public override float pixelSize =>
        this.mPixelSize;

    public override bool premultipliedAlpha
    {
        get
        {
            if (this.mPMA == -1)
            {
                Shader shader = this.shader;
                this.mPMA = ((shader == null) || !shader.name.Contains("Premultiplied")) ? 0 : 1;
            }
            return (this.mPMA == 1);
        }
    }

    public override Shader shader
    {
        get
        {
            if (this.mMat != null)
            {
                return this.mMat.shader;
            }
            if (this.mShader == null)
            {
                this.mShader = Shader.Find("Unlit/Transparent Colored");
            }
            return this.mShader;
        }
        set
        {
            if (this.mShader != value)
            {
                base.RemoveFromPanel();
                this.mShader = value;
                if (this.mMat == null)
                {
                    this.mPMA = -1;
                    this.MarkAsChanged();
                }
            }
        }
    }

    public UnityEngine.Sprite sprite2D
    {
        get => 
            this.mSprite;
        set
        {
            if (this.mSprite != value)
            {
                base.RemoveFromPanel();
                this.mSprite = value;
                this.nextSprite = null;
                base.CreatePanel();
            }
        }
    }
}

