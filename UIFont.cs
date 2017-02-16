using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/NGUI Font")]
public class UIFont : MonoBehaviour
{
    [HideInInspector, SerializeField]
    private UIAtlas mAtlas;
    [HideInInspector, SerializeField]
    private Font mDynamicFont;
    [HideInInspector, SerializeField]
    private int mDynamicFontSize = 0x10;
    [HideInInspector, SerializeField]
    private FontStyle mDynamicFontStyle;
    [HideInInspector, SerializeField]
    private BMFont mFont = new BMFont();
    [SerializeField, HideInInspector]
    private Material mMat;
    private int mPacked = -1;
    private int mPMA = -1;
    [HideInInspector, SerializeField]
    private UIFont mReplacement;
    [NonSerialized]
    private UISpriteData mSprite;
    [SerializeField, HideInInspector]
    private List<BMSymbol> mSymbols = new List<BMSymbol>();
    [SerializeField, HideInInspector]
    private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

    public void AddSymbol(string sequence, string spriteName)
    {
        this.GetSymbol(sequence, true).spriteName = spriteName;
        this.MarkAsChanged();
    }

    public static bool CheckIfRelated(UIFont a, UIFont b)
    {
        if ((a == null) || (b == null))
        {
            return false;
        }
        return (((a.isDynamic && b.isDynamic) && (a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0])) || (((a == b) || a.References(b)) || b.References(a)));
    }

    private BMSymbol GetSymbol(string sequence, bool createIfMissing)
    {
        int num = 0;
        int count = this.mSymbols.Count;
        while (num < count)
        {
            BMSymbol symbol = this.mSymbols[num];
            if (symbol.sequence == sequence)
            {
                return symbol;
            }
            num++;
        }
        if (createIfMissing)
        {
            BMSymbol item = new BMSymbol {
                sequence = sequence
            };
            this.mSymbols.Add(item);
            return item;
        }
        return null;
    }

    public void MarkAsChanged()
    {
        if (this.mReplacement != null)
        {
            this.mReplacement.MarkAsChanged();
        }
        this.mSprite = null;
        UILabel[] labelArray = NGUITools.FindActive<UILabel>();
        int index = 0;
        int length = labelArray.Length;
        while (index < length)
        {
            UILabel label = labelArray[index];
            if ((label.enabled && NGUITools.GetActive(label.gameObject)) && CheckIfRelated(this, label.bitmapFont))
            {
                UIFont bitmapFont = label.bitmapFont;
                label.bitmapFont = null;
                label.bitmapFont = bitmapFont;
            }
            index++;
        }
        int num3 = 0;
        int count = this.symbols.Count;
        while (num3 < count)
        {
            this.symbols[num3].MarkAsChanged();
            num3++;
        }
    }

    public BMSymbol MatchSymbol(string text, int offset, int textLength)
    {
        int count = this.mSymbols.Count;
        if (count != 0)
        {
            textLength -= offset;
            for (int i = 0; i < count; i++)
            {
                BMSymbol symbol = this.mSymbols[i];
                int length = symbol.length;
                if ((length != 0) && (textLength >= length))
                {
                    bool flag = true;
                    for (int j = 0; j < length; j++)
                    {
                        if (text[offset + j] != symbol.sequence[j])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag && symbol.Validate(this.atlas))
                    {
                        return symbol;
                    }
                }
            }
        }
        return null;
    }

    private bool References(UIFont font)
    {
        if (font == null)
        {
            return false;
        }
        return ((font == this) || ((this.mReplacement != null) && this.mReplacement.References(font)));
    }

    public void RemoveSymbol(string sequence)
    {
        BMSymbol item = this.GetSymbol(sequence, false);
        if (item != null)
        {
            this.symbols.Remove(item);
        }
        this.MarkAsChanged();
    }

    public void RenameSymbol(string before, string after)
    {
        BMSymbol symbol = this.GetSymbol(before, false);
        if (symbol != null)
        {
            symbol.sequence = after;
        }
        this.MarkAsChanged();
    }

    private void Trim()
    {
        if ((this.mAtlas.texture != null) && (this.mSprite != null))
        {
            Rect rect = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
            Rect rect2 = new Rect((float) this.mSprite.x, (float) this.mSprite.y, (float) this.mSprite.width, (float) this.mSprite.height);
            int xMin = Mathf.RoundToInt(rect2.xMin - rect.xMin);
            int yMin = Mathf.RoundToInt(rect2.yMin - rect.yMin);
            int xMax = Mathf.RoundToInt(rect2.xMax - rect.xMin);
            int yMax = Mathf.RoundToInt(rect2.yMax - rect.yMin);
            this.mFont.Trim(xMin, yMin, xMax, yMax);
        }
    }

    public void UpdateUVRect()
    {
        if (this.mAtlas != null)
        {
            Texture texture = this.mAtlas.texture;
            if (texture != null)
            {
                this.mUVRect = new Rect((float) (this.mSprite.x - this.mSprite.paddingLeft), (float) (this.mSprite.y - this.mSprite.paddingTop), (float) ((this.mSprite.width + this.mSprite.paddingLeft) + this.mSprite.paddingRight), (float) ((this.mSprite.height + this.mSprite.paddingTop) + this.mSprite.paddingBottom));
                this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
                if (this.mSprite.hasPadding)
                {
                    this.Trim();
                }
            }
        }
    }

    public bool UsesSprite(string s)
    {
        if (!string.IsNullOrEmpty(s))
        {
            if (s.Equals(this.spriteName))
            {
                return true;
            }
            int num = 0;
            int count = this.symbols.Count;
            while (num < count)
            {
                BMSymbol symbol = this.symbols[num];
                if (s.Equals(symbol.spriteName))
                {
                    return true;
                }
                num++;
            }
        }
        return false;
    }

    public UIAtlas atlas
    {
        get => 
            ((this.mReplacement == null) ? this.mAtlas : this.mReplacement.atlas);
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.atlas = value;
            }
            else if (this.mAtlas != value)
            {
                this.mPMA = -1;
                this.mAtlas = value;
                if (this.mAtlas != null)
                {
                    this.mMat = this.mAtlas.spriteMaterial;
                    if (this.sprite != null)
                    {
                        this.mUVRect = this.uvRect;
                    }
                }
                this.MarkAsChanged();
            }
        }
    }

    public BMFont bmFont
    {
        get => 
            ((this.mReplacement == null) ? this.mFont : this.mReplacement.bmFont);
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.bmFont = value;
            }
            else
            {
                this.mFont = value;
            }
        }
    }

    public int defaultSize
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.defaultSize;
            }
            if (!this.isDynamic && (this.mFont != null))
            {
                return this.mFont.charSize;
            }
            return this.mDynamicFontSize;
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.defaultSize = value;
            }
            else
            {
                this.mDynamicFontSize = value;
            }
        }
    }

    public Font dynamicFont
    {
        get => 
            ((this.mReplacement == null) ? this.mDynamicFont : this.mReplacement.dynamicFont);
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.dynamicFont = value;
            }
            else if (this.mDynamicFont != value)
            {
                if (this.mDynamicFont != null)
                {
                    this.material = null;
                }
                this.mDynamicFont = value;
                this.MarkAsChanged();
            }
        }
    }

    public FontStyle dynamicFontStyle
    {
        get => 
            ((this.mReplacement == null) ? this.mDynamicFontStyle : this.mReplacement.dynamicFontStyle);
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.dynamicFontStyle = value;
            }
            else if (this.mDynamicFontStyle != value)
            {
                this.mDynamicFontStyle = value;
                this.MarkAsChanged();
            }
        }
    }

    private Texture dynamicTexture
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.dynamicTexture;
            }
            if (this.isDynamic)
            {
                return this.mDynamicFont.material.mainTexture;
            }
            return null;
        }
    }

    public bool hasSymbols =>
        ((this.mReplacement == null) ? ((this.mSymbols != null) && (this.mSymbols.Count != 0)) : this.mReplacement.hasSymbols);

    public bool isDynamic =>
        ((this.mReplacement == null) ? (this.mDynamicFont != null) : this.mReplacement.isDynamic);

    public bool isValid =>
        ((this.mDynamicFont != null) || this.mFont.isValid);

    public Material material
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.material;
            }
            if (this.mAtlas != null)
            {
                return this.mAtlas.spriteMaterial;
            }
            if (this.mMat != null)
            {
                if ((this.mDynamicFont != null) && (this.mMat != this.mDynamicFont.material))
                {
                    this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
                }
                return this.mMat;
            }
            if (this.mDynamicFont != null)
            {
                return this.mDynamicFont.material;
            }
            return null;
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.material = value;
            }
            else if (this.mMat != value)
            {
                this.mPMA = -1;
                this.mMat = value;
                this.MarkAsChanged();
            }
        }
    }

    public bool packedFontShader
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.packedFontShader;
            }
            if (this.mAtlas != null)
            {
                return false;
            }
            if (this.mPacked == -1)
            {
                Material material = this.material;
                this.mPacked = (((material == null) || (material.shader == null)) || !material.shader.name.Contains("Packed")) ? 0 : 1;
            }
            return (this.mPacked == 1);
        }
    }

    [Obsolete("Use UIFont.premultipliedAlphaShader instead")]
    public bool premultipliedAlpha =>
        this.premultipliedAlphaShader;

    public bool premultipliedAlphaShader
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.premultipliedAlphaShader;
            }
            if (this.mAtlas != null)
            {
                return this.mAtlas.premultipliedAlpha;
            }
            if (this.mPMA == -1)
            {
                Material material = this.material;
                this.mPMA = (((material == null) || (material.shader == null)) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1;
            }
            return (this.mPMA == 1);
        }
    }

    public UIFont replacement
    {
        get => 
            this.mReplacement;
        set
        {
            UIFont font = value;
            if (font == this)
            {
                font = null;
            }
            if (this.mReplacement != font)
            {
                if ((font != null) && (font.replacement == this))
                {
                    font.replacement = null;
                }
                if (this.mReplacement != null)
                {
                    this.MarkAsChanged();
                }
                this.mReplacement = font;
                if (font != null)
                {
                    this.mPMA = -1;
                    this.mMat = null;
                    this.mFont = null;
                    this.mDynamicFont = null;
                }
                this.MarkAsChanged();
            }
        }
    }

    [Obsolete("Use UIFont.defaultSize instead")]
    public int size
    {
        get => 
            this.defaultSize;
        set
        {
            this.defaultSize = value;
        }
    }

    public UISpriteData sprite
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.sprite;
            }
            if (((this.mSprite == null) && (this.mAtlas != null)) && !string.IsNullOrEmpty(this.mFont.spriteName))
            {
                this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
                if (this.mSprite == null)
                {
                    this.mSprite = this.mAtlas.GetSprite(base.name);
                }
                if (this.mSprite == null)
                {
                    this.mFont.spriteName = null;
                }
                else
                {
                    this.UpdateUVRect();
                }
                int num = 0;
                int count = this.mSymbols.Count;
                while (num < count)
                {
                    this.symbols[num].MarkAsChanged();
                    num++;
                }
            }
            return this.mSprite;
        }
    }

    public string spriteName
    {
        get => 
            ((this.mReplacement == null) ? this.mFont.spriteName : this.mReplacement.spriteName);
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.spriteName = value;
            }
            else if (this.mFont.spriteName != value)
            {
                this.mFont.spriteName = value;
                this.MarkAsChanged();
            }
        }
    }

    public List<BMSymbol> symbols =>
        ((this.mReplacement == null) ? this.mSymbols : this.mReplacement.symbols);

    public int texHeight
    {
        get => 
            ((this.mReplacement == null) ? ((this.mFont == null) ? 1 : this.mFont.texHeight) : this.mReplacement.texHeight);
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.texHeight = value;
            }
            else if (this.mFont != null)
            {
                this.mFont.texHeight = value;
            }
        }
    }

    public Texture2D texture
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.texture;
            }
            Material material = this.material;
            return ((material == null) ? null : (material.mainTexture as Texture2D));
        }
    }

    public int texWidth
    {
        get => 
            ((this.mReplacement == null) ? ((this.mFont == null) ? 1 : this.mFont.texWidth) : this.mReplacement.texWidth);
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.texWidth = value;
            }
            else if (this.mFont != null)
            {
                this.mFont.texWidth = value;
            }
        }
    }

    public Rect uvRect
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.uvRect;
            }
            return (((this.mAtlas == null) || (this.sprite == null)) ? new Rect(0f, 0f, 1f, 1f) : this.mUVRect);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.uvRect = value;
            }
            else if ((this.sprite == null) && (this.mUVRect != value))
            {
                this.mUVRect = value;
                this.MarkAsChanged();
            }
        }
    }
}

