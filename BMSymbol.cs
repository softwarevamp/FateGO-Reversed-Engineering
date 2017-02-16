using System;
using UnityEngine;

[Serializable]
public class BMSymbol
{
    private int mAdvance;
    private int mHeight;
    private bool mIsValid;
    private int mLength;
    private int mOffsetX;
    private int mOffsetY;
    private UISpriteData mSprite;
    private Rect mUV;
    private int mWidth;
    public string sequence;
    public string spriteName;

    public void MarkAsChanged()
    {
        this.mIsValid = false;
    }

    public bool Validate(UIAtlas atlas)
    {
        if (atlas == null)
        {
            return false;
        }
        if (!this.mIsValid)
        {
            if (string.IsNullOrEmpty(this.spriteName))
            {
                return false;
            }
            this.mSprite = atlas?.GetSprite(this.spriteName);
            if (this.mSprite != null)
            {
                Texture texture = atlas.texture;
                if (texture == null)
                {
                    this.mSprite = null;
                }
                else
                {
                    this.mUV = new Rect((float) this.mSprite.x, (float) this.mSprite.y, (float) this.mSprite.width, (float) this.mSprite.height);
                    this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
                    this.mOffsetX = this.mSprite.paddingLeft;
                    this.mOffsetY = this.mSprite.paddingTop;
                    this.mWidth = this.mSprite.width;
                    this.mHeight = this.mSprite.height;
                    this.mAdvance = this.mSprite.width + (this.mSprite.paddingLeft + this.mSprite.paddingRight);
                    this.mIsValid = true;
                }
            }
        }
        return (this.mSprite != null);
    }

    public int advance =>
        this.mAdvance;

    public int height =>
        this.mHeight;

    public int length
    {
        get
        {
            if (this.mLength == 0)
            {
                this.mLength = this.sequence.Length;
            }
            return this.mLength;
        }
    }

    public int offsetX =>
        this.mOffsetX;

    public int offsetY =>
        this.mOffsetY;

    public Rect uvRect =>
        this.mUV;

    public int width =>
        this.mWidth;
}

