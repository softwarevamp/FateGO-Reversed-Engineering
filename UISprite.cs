using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Sprite"), ExecuteInEditMode]
public class UISprite : UIBasicSprite
{
    [HideInInspector, SerializeField]
    private UIAtlas mAtlas;
    [HideInInspector, SerializeField]
    private bool mFillCenter = true;
    [NonSerialized]
    protected UISpriteData mSprite;
    [HideInInspector, SerializeField]
    private string mSpriteName;
    [NonSerialized]
    private bool mSpriteSet;

    public UISpriteData GetAtlasSprite()
    {
        if (!this.mSpriteSet)
        {
            this.mSprite = null;
        }
        if ((this.mSprite == null) && (this.mAtlas != null))
        {
            if (!string.IsNullOrEmpty(this.mSpriteName))
            {
                UISpriteData sprite = this.mAtlas.GetSprite(this.mSpriteName);
                if (sprite == null)
                {
                    return null;
                }
                this.SetAtlasSprite(sprite);
            }
            if ((this.mSprite == null) && (this.mAtlas.spriteList.Count > 0))
            {
                UISpriteData sp = this.mAtlas.spriteList[0];
                if (sp == null)
                {
                    return null;
                }
                this.SetAtlasSprite(sp);
                if (this.mSprite == null)
                {
                    Debug.LogError(this.mAtlas.name + " seems to have a null sprite!");
                    return null;
                }
                this.mSpriteName = this.mSprite.name;
            }
        }
        return this.mSprite;
    }

    public override void MakePixelPerfect()
    {
        if (this.isValid)
        {
            base.MakePixelPerfect();
            if (base.mType != UIBasicSprite.Type.Tiled)
            {
                UISpriteData atlasSprite = this.GetAtlasSprite();
                if (atlasSprite != null)
                {
                    Texture mainTexture = this.mainTexture;
                    if ((mainTexture != null) && ((((base.mType == UIBasicSprite.Type.Simple) || (base.mType == UIBasicSprite.Type.Filled)) || !atlasSprite.hasBorder) && (mainTexture != null)))
                    {
                        int num = Mathf.RoundToInt(this.pixelSize * ((atlasSprite.width + atlasSprite.paddingLeft) + atlasSprite.paddingRight));
                        int num2 = Mathf.RoundToInt(this.pixelSize * ((atlasSprite.height + atlasSprite.paddingTop) + atlasSprite.paddingBottom));
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
        }
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture mainTexture = this.mainTexture;
        if (mainTexture != null)
        {
            if (this.mSprite == null)
            {
                this.mSprite = this.atlas.GetSprite(this.spriteName);
            }
            if (this.mSprite != null)
            {
                Rect rect = new Rect((float) this.mSprite.x, (float) this.mSprite.y, (float) this.mSprite.width, (float) this.mSprite.height);
                Rect rect2 = new Rect((float) (this.mSprite.x + this.mSprite.borderLeft), (float) (this.mSprite.y + this.mSprite.borderTop), (float) ((this.mSprite.width - this.mSprite.borderLeft) - this.mSprite.borderRight), (float) ((this.mSprite.height - this.mSprite.borderBottom) - this.mSprite.borderTop));
                rect = NGUIMath.ConvertToTexCoords(rect, mainTexture.width, mainTexture.height);
                rect2 = NGUIMath.ConvertToTexCoords(rect2, mainTexture.width, mainTexture.height);
                int size = verts.size;
                base.Fill(verts, uvs, cols, rect, rect2);
                if (base.onPostFill != null)
                {
                    base.onPostFill(this, size, verts, uvs, cols);
                }
            }
        }
    }

    protected override void OnInit()
    {
        if (!this.mFillCenter)
        {
            this.mFillCenter = true;
            base.centerType = UIBasicSprite.AdvancedType.Invisible;
        }
        base.OnInit();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (base.mChanged || !this.mSpriteSet)
        {
            this.mSpriteSet = true;
            this.mSprite = null;
            base.mChanged = true;
        }
    }

    protected void SetAtlasSprite(UISpriteData sp)
    {
        base.mChanged = true;
        this.mSpriteSet = true;
        if (sp != null)
        {
            this.mSprite = sp;
            this.mSpriteName = this.mSprite.name;
        }
        else
        {
            this.mSpriteName = (this.mSprite == null) ? string.Empty : this.mSprite.name;
            this.mSprite = sp;
        }
    }

    public UIAtlas atlas
    {
        get => 
            this.mAtlas;
        set
        {
            if (this.mAtlas != value)
            {
                base.RemoveFromPanel();
                this.mAtlas = value;
                this.mSpriteSet = false;
                this.mSprite = null;
                if ((string.IsNullOrEmpty(this.mSpriteName) && (this.mAtlas != null)) && (this.mAtlas.spriteList.Count > 0))
                {
                    this.SetAtlasSprite(this.mAtlas.spriteList[0]);
                    this.mSpriteName = this.mSprite.name;
                }
                if (!string.IsNullOrEmpty(this.mSpriteName))
                {
                    string mSpriteName = this.mSpriteName;
                    this.mSpriteName = string.Empty;
                    this.spriteName = mSpriteName;
                    this.MarkAsChanged();
                }
            }
        }
    }

    public override Vector4 border
    {
        get
        {
            UISpriteData atlasSprite = this.GetAtlasSprite();
            if (atlasSprite == null)
            {
                return base.border;
            }
            return new Vector4((float) atlasSprite.borderLeft, (float) atlasSprite.borderBottom, (float) atlasSprite.borderRight, (float) atlasSprite.borderTop);
        }
    }

    public override Vector4 drawingDimensions
    {
        get
        {
            Vector2 pivotOffset = base.pivotOffset;
            float a = -pivotOffset.x * base.mWidth;
            float num2 = -pivotOffset.y * base.mHeight;
            float b = a + base.mWidth;
            float num4 = num2 + base.mHeight;
            if ((this.GetAtlasSprite() != null) && (base.mType != UIBasicSprite.Type.Tiled))
            {
                int paddingLeft = this.mSprite.paddingLeft;
                int paddingBottom = this.mSprite.paddingBottom;
                int paddingRight = this.mSprite.paddingRight;
                int paddingTop = this.mSprite.paddingTop;
                int num9 = (this.mSprite.width + paddingLeft) + paddingRight;
                int num10 = (this.mSprite.height + paddingBottom) + paddingTop;
                float num11 = 1f;
                float num12 = 1f;
                if (((num9 > 0) && (num10 > 0)) && ((base.mType == UIBasicSprite.Type.Simple) || (base.mType == UIBasicSprite.Type.Filled)))
                {
                    if ((num9 & 1) != 0)
                    {
                        paddingRight++;
                    }
                    if ((num10 & 1) != 0)
                    {
                        paddingTop++;
                    }
                    num11 = (1f / ((float) num9)) * base.mWidth;
                    num12 = (1f / ((float) num10)) * base.mHeight;
                }
                if ((base.mFlip == UIBasicSprite.Flip.Horizontally) || (base.mFlip == UIBasicSprite.Flip.Both))
                {
                    a += paddingRight * num11;
                    b -= paddingLeft * num11;
                }
                else
                {
                    a += paddingLeft * num11;
                    b -= paddingRight * num11;
                }
                if ((base.mFlip == UIBasicSprite.Flip.Vertically) || (base.mFlip == UIBasicSprite.Flip.Both))
                {
                    num2 += paddingTop * num12;
                    num4 -= paddingBottom * num12;
                }
                else
                {
                    num2 += paddingBottom * num12;
                    num4 -= paddingTop * num12;
                }
            }
            Vector4 vector2 = (this.mAtlas == null) ? Vector4.zero : ((Vector4) (this.border * this.pixelSize));
            float num13 = vector2.x + vector2.z;
            float num14 = vector2.y + vector2.w;
            float x = Mathf.Lerp(a, b - num13, this.mDrawRegion.x);
            float y = Mathf.Lerp(num2, num4 - num14, this.mDrawRegion.y);
            float z = Mathf.Lerp(a + num13, b, this.mDrawRegion.z);
            return new Vector4(x, y, z, Mathf.Lerp(num2 + num14, num4, this.mDrawRegion.w));
        }
    }

    [Obsolete("Use 'centerType' instead")]
    public bool fillCenter
    {
        get => 
            (base.centerType != UIBasicSprite.AdvancedType.Invisible);
        set
        {
            if (value != (base.centerType != UIBasicSprite.AdvancedType.Invisible))
            {
                base.centerType = !value ? UIBasicSprite.AdvancedType.Invisible : UIBasicSprite.AdvancedType.Sliced;
                this.MarkAsChanged();
            }
        }
    }

    public bool isValid =>
        (this.GetAtlasSprite() != null);

    public override Material material =>
        this.mAtlas?.spriteMaterial;

    public override int minHeight
    {
        get
        {
            if ((this.type != UIBasicSprite.Type.Sliced) && (this.type != UIBasicSprite.Type.Advanced))
            {
                return base.minHeight;
            }
            Vector4 vector = (Vector4) (this.border * this.pixelSize);
            int num = Mathf.RoundToInt(vector.y + vector.w);
            UISpriteData atlasSprite = this.GetAtlasSprite();
            if (atlasSprite != null)
            {
                num += atlasSprite.paddingTop + atlasSprite.paddingBottom;
            }
            return Mathf.Max(base.minHeight, ((num & 1) != 1) ? num : (num + 1));
        }
    }

    public override int minWidth
    {
        get
        {
            if ((this.type != UIBasicSprite.Type.Sliced) && (this.type != UIBasicSprite.Type.Advanced))
            {
                return base.minWidth;
            }
            Vector4 vector = (Vector4) (this.border * this.pixelSize);
            int num = Mathf.RoundToInt(vector.x + vector.z);
            UISpriteData atlasSprite = this.GetAtlasSprite();
            if (atlasSprite != null)
            {
                num += atlasSprite.paddingLeft + atlasSprite.paddingRight;
            }
            return Mathf.Max(base.minWidth, ((num & 1) != 1) ? num : (num + 1));
        }
    }

    public override float pixelSize =>
        ((this.mAtlas == null) ? 1f : this.mAtlas.pixelSize);

    public override bool premultipliedAlpha =>
        ((this.mAtlas != null) && this.mAtlas.premultipliedAlpha);

    public string spriteName
    {
        get => 
            this.mSpriteName;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(this.mSpriteName))
                {
                    this.mSpriteName = string.Empty;
                    this.mSprite = null;
                    base.mChanged = true;
                    this.mSpriteSet = false;
                }
            }
            else if (this.mSpriteName != value)
            {
                this.mSpriteName = value;
                this.mSprite = null;
                base.mChanged = true;
                this.mSpriteSet = false;
            }
        }
    }
}

