using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
    [CompilerGenerated]
    private static Comparison<UISpriteData> <>f__am$cache8;
    [HideInInspector, SerializeField]
    private Material material;
    [SerializeField, HideInInspector]
    private Coordinates mCoordinates;
    [SerializeField, HideInInspector]
    private float mPixelSize = 1f;
    private int mPMA = -1;
    [SerializeField, HideInInspector]
    private UIAtlas mReplacement;
    private Dictionary<string, int> mSpriteIndices = new Dictionary<string, int>();
    [SerializeField, HideInInspector]
    private List<UISpriteData> mSprites = new List<UISpriteData>();
    [HideInInspector, SerializeField]
    private List<Sprite> sprites = new List<Sprite>();

    public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
    {
        if ((a == null) || (b == null))
        {
            return false;
        }
        return (((a == b) || a.References(b)) || b.References(a));
    }

    public BetterList<string> GetListOfSprites()
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.GetListOfSprites();
        }
        if (this.mSprites.Count == 0)
        {
            this.Upgrade();
        }
        BetterList<string> list = new BetterList<string>();
        int num = 0;
        int count = this.mSprites.Count;
        while (num < count)
        {
            UISpriteData data = this.mSprites[num];
            if ((data != null) && !string.IsNullOrEmpty(data.name))
            {
                list.Add(data.name);
            }
            num++;
        }
        return list;
    }

    public BetterList<string> GetListOfSprites(string match)
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.GetListOfSprites(match);
        }
        if (string.IsNullOrEmpty(match))
        {
            return this.GetListOfSprites();
        }
        if (this.mSprites.Count == 0)
        {
            this.Upgrade();
        }
        BetterList<string> list = new BetterList<string>();
        int num = 0;
        int count = this.mSprites.Count;
        while (num < count)
        {
            UISpriteData data = this.mSprites[num];
            if (((data != null) && !string.IsNullOrEmpty(data.name)) && string.Equals(match, data.name, StringComparison.OrdinalIgnoreCase))
            {
                list.Add(data.name);
                return list;
            }
            num++;
        }
        char[] separator = new char[] { ' ' };
        string[] strArray = match.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strArray.Length; i++)
        {
            strArray[i] = strArray[i].ToLower();
        }
        int num4 = 0;
        int num5 = this.mSprites.Count;
        while (num4 < num5)
        {
            UISpriteData data2 = this.mSprites[num4];
            if ((data2 != null) && !string.IsNullOrEmpty(data2.name))
            {
                string str = data2.name.ToLower();
                int num6 = 0;
                for (int j = 0; j < strArray.Length; j++)
                {
                    if (str.Contains(strArray[j]))
                    {
                        num6++;
                    }
                }
                if (num6 == strArray.Length)
                {
                    list.Add(data2.name);
                }
            }
            num4++;
        }
        return list;
    }

    public string GetRandomSprite(string startsWith)
    {
        if (this.GetSprite(startsWith) != null)
        {
            return startsWith;
        }
        List<UISpriteData> spriteList = this.spriteList;
        List<string> list2 = new List<string>();
        foreach (UISpriteData data in spriteList)
        {
            if (data.name.StartsWith(startsWith))
            {
                list2.Add(data.name);
            }
        }
        return ((list2.Count <= 0) ? null : list2[UnityEngine.Random.Range(0, list2.Count)]);
    }

    public UISpriteData GetSprite(string name)
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.GetSprite(name);
        }
        if (!string.IsNullOrEmpty(name))
        {
            int num;
            if (this.mSprites.Count == 0)
            {
                this.Upgrade();
            }
            if (this.mSprites.Count == 0)
            {
                return null;
            }
            if (this.mSpriteIndices.Count != this.mSprites.Count)
            {
                this.MarkSpriteListAsChanged();
            }
            if (this.mSpriteIndices.TryGetValue(name, out num))
            {
                if ((num > -1) && (num < this.mSprites.Count))
                {
                    return this.mSprites[num];
                }
                this.MarkSpriteListAsChanged();
                return (!this.mSpriteIndices.TryGetValue(name, out num) ? null : this.mSprites[num]);
            }
            int num2 = 0;
            int count = this.mSprites.Count;
            while (num2 < count)
            {
                UISpriteData data = this.mSprites[num2];
                if (!string.IsNullOrEmpty(data.name) && (name == data.name))
                {
                    this.MarkSpriteListAsChanged();
                    return data;
                }
                num2++;
            }
        }
        return null;
    }

    public void MarkAsChanged()
    {
        if (this.mReplacement != null)
        {
            this.mReplacement.MarkAsChanged();
        }
        UISprite[] spriteArray = NGUITools.FindActive<UISprite>();
        int index = 0;
        int length = spriteArray.Length;
        while (index < length)
        {
            UISprite sprite = spriteArray[index];
            if (CheckIfRelated(this, sprite.atlas))
            {
                UIAtlas atlas = sprite.atlas;
                sprite.atlas = null;
                sprite.atlas = atlas;
            }
            index++;
        }
        UIFont[] fontArray = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
        int num3 = 0;
        int num4 = fontArray.Length;
        while (num3 < num4)
        {
            UIFont font = fontArray[num3];
            if (CheckIfRelated(this, font.atlas))
            {
                UIAtlas atlas2 = font.atlas;
                font.atlas = null;
                font.atlas = atlas2;
            }
            num3++;
        }
        UILabel[] labelArray = NGUITools.FindActive<UILabel>();
        int num5 = 0;
        int num6 = labelArray.Length;
        while (num5 < num6)
        {
            UILabel label = labelArray[num5];
            if ((label.bitmapFont != null) && CheckIfRelated(this, label.bitmapFont.atlas))
            {
                UIFont bitmapFont = label.bitmapFont;
                label.bitmapFont = null;
                label.bitmapFont = bitmapFont;
            }
            num5++;
        }
    }

    public void MarkSpriteListAsChanged()
    {
        this.mSpriteIndices.Clear();
        int num = 0;
        int count = this.mSprites.Count;
        while (num < count)
        {
            this.mSpriteIndices[this.mSprites[num].name] = num;
            num++;
        }
    }

    private bool References(UIAtlas atlas)
    {
        if (atlas == null)
        {
            return false;
        }
        return ((atlas == this) || ((this.mReplacement != null) && this.mReplacement.References(atlas)));
    }

    public void SortAlphabetically()
    {
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = (s1, s2) => s1.name.CompareTo(s2.name);
        }
        this.mSprites.Sort(<>f__am$cache8);
    }

    private bool Upgrade()
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.Upgrade();
        }
        if (((this.mSprites.Count != 0) || (this.sprites.Count <= 0)) || (this.material == null))
        {
            return false;
        }
        Texture mainTexture = this.material.mainTexture;
        int width = (mainTexture == null) ? 0x200 : mainTexture.width;
        int height = (mainTexture == null) ? 0x200 : mainTexture.height;
        for (int i = 0; i < this.sprites.Count; i++)
        {
            Sprite sprite = this.sprites[i];
            Rect outer = sprite.outer;
            Rect inner = sprite.inner;
            if (this.mCoordinates == Coordinates.TexCoords)
            {
                NGUIMath.ConvertToPixels(outer, width, height, true);
                NGUIMath.ConvertToPixels(inner, width, height, true);
            }
            UISpriteData item = new UISpriteData {
                name = sprite.name,
                x = Mathf.RoundToInt(outer.xMin),
                y = Mathf.RoundToInt(outer.yMin),
                width = Mathf.RoundToInt(outer.width),
                height = Mathf.RoundToInt(outer.height),
                paddingLeft = Mathf.RoundToInt(sprite.paddingLeft * outer.width),
                paddingRight = Mathf.RoundToInt(sprite.paddingRight * outer.width),
                paddingBottom = Mathf.RoundToInt(sprite.paddingBottom * outer.height),
                paddingTop = Mathf.RoundToInt(sprite.paddingTop * outer.height),
                borderLeft = Mathf.RoundToInt(inner.xMin - outer.xMin),
                borderRight = Mathf.RoundToInt(outer.xMax - inner.xMax),
                borderBottom = Mathf.RoundToInt(outer.yMax - inner.yMax),
                borderTop = Mathf.RoundToInt(inner.yMin - outer.yMin)
            };
            this.mSprites.Add(item);
        }
        this.sprites.Clear();
        return true;
    }

    public float pixelSize
    {
        get => 
            ((this.mReplacement == null) ? this.mPixelSize : this.mReplacement.pixelSize);
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.pixelSize = value;
            }
            else
            {
                float num = Mathf.Clamp(value, 0.25f, 4f);
                if (this.mPixelSize != num)
                {
                    this.mPixelSize = num;
                    this.MarkAsChanged();
                }
            }
        }
    }

    public bool premultipliedAlpha
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.premultipliedAlpha;
            }
            if (this.mPMA == -1)
            {
                Material spriteMaterial = this.spriteMaterial;
                this.mPMA = (((spriteMaterial == null) || (spriteMaterial.shader == null)) || !spriteMaterial.shader.name.Contains("Premultiplied")) ? 0 : 1;
            }
            return (this.mPMA == 1);
        }
    }

    public UIAtlas replacement
    {
        get => 
            this.mReplacement;
        set
        {
            UIAtlas atlas = value;
            if (atlas == this)
            {
                atlas = null;
            }
            if (this.mReplacement != atlas)
            {
                if ((atlas != null) && (atlas.replacement == this))
                {
                    atlas.replacement = null;
                }
                if (this.mReplacement != null)
                {
                    this.MarkAsChanged();
                }
                this.mReplacement = atlas;
                if (atlas != null)
                {
                    this.material = null;
                }
                this.MarkAsChanged();
            }
        }
    }

    public List<UISpriteData> spriteList
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.spriteList;
            }
            if (this.mSprites.Count == 0)
            {
                this.Upgrade();
            }
            return this.mSprites;
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.spriteList = value;
            }
            else
            {
                this.mSprites = value;
            }
        }
    }

    public Material spriteMaterial
    {
        get => 
            ((this.mReplacement == null) ? this.material : this.mReplacement.spriteMaterial);
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.spriteMaterial = value;
            }
            else if (this.material == null)
            {
                this.mPMA = 0;
                this.material = value;
            }
            else
            {
                this.MarkAsChanged();
                this.mPMA = -1;
                this.material = value;
                this.MarkAsChanged();
            }
        }
    }

    public Texture texture =>
        ((this.mReplacement == null) ? this.material?.mainTexture : this.mReplacement.texture);

    private enum Coordinates
    {
        Pixels,
        TexCoords
    }

    [Serializable]
    private class Sprite
    {
        public Rect inner = new Rect(0f, 0f, 1f, 1f);
        public string name = "Unity Bug";
        public Rect outer = new Rect(0f, 0f, 1f, 1f);
        public float paddingBottom;
        public float paddingLeft;
        public float paddingRight;
        public float paddingTop;
        public bool rotated;

        public bool hasPadding =>
            ((((this.paddingLeft != 0f) || (this.paddingRight != 0f)) || (this.paddingTop != 0f)) || (this.paddingBottom != 0f));
    }
}

