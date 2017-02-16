using System;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;

public class UINarrowFigureRender : UITweenRenderer
{
    protected static readonly int BODY_SIZE_X = 0x94;
    protected static readonly int BODY_SIZE_Y = 0x177;
    protected static readonly Vector2 bodySize;
    protected static readonly Vector2 bodyTextureSize;
    protected static readonly ReadOnlyCollection<Vector2> bodyTopTable;
    protected int imageLimitCount = -1;
    protected static readonly int MAIN_SIZE_X = 0x200;
    protected static readonly int MAIN_SIZE_Y = 0x200;
    protected static int OTHER_IMAGE_LIMIT_COUNT = 4;
    protected int svtId;
    protected static readonly int TEXTURE_PAGE_SIZE = 3;
    protected int textureImageLimitCount = -1;

    static UINarrowFigureRender()
    {
        Vector2[] array = new Vector2[] { new Vector2(0f, 0f), new Vector2(154f, 0f), new Vector2(308f, 0f) };
        bodyTopTable = Array.AsReadOnly<Vector2>(array);
        bodySize = new Vector2(148f, 375f);
        bodyTextureSize = new Vector2(148f, 375f);
    }

    public static string GetAssetName(int svtId)
    {
        string name = "NarrowFigure/" + svtId;
        if (AssetManager.isExistAssetStorage(name))
        {
            return name;
        }
        return "NarrowFigure/100000";
    }

    public Texture2D GetBodyAlphaTexture(AssetData assetData)
    {
        int num = this.textureImageLimitCount / TEXTURE_PAGE_SIZE;
        string str = (num <= 0) ? assetData.LastName : (assetData.LastName + "_" + (num + 1));
        return assetData.GetObject<Texture2D>(str + "a");
    }

    public Vector2 GetBodySize() => 
        bodySize;

    public Texture2D GetBodyTexture(AssetData assetData)
    {
        this.textureImageLimitCount = this.imageLimitCount;
        int num = this.textureImageLimitCount / TEXTURE_PAGE_SIZE;
        string lastName = assetData.LastName;
        if (num > 0)
        {
            Texture2D textured = assetData.GetObject<Texture2D>(lastName + "_" + (num + 1));
            if (textured != null)
            {
                return textured;
            }
            this.textureImageLimitCount = TEXTURE_PAGE_SIZE - 1;
        }
        return assetData.GetObject<Texture2D>(lastName);
    }

    public Rect GetBodyUvRect()
    {
        int num = this.textureImageLimitCount % TEXTURE_PAGE_SIZE;
        Vector2 vector = bodyTopTable[num];
        Vector2 bodyTextureSize = UINarrowFigureRender.bodyTextureSize;
        vector.x += (BODY_SIZE_X - bodyTextureSize.x) / 2f;
        vector.y += BODY_SIZE_Y - bodyTextureSize.y;
        return new Rect((vector.x / ((float) MAIN_SIZE_X)) + (1f / ((float) MAIN_SIZE_X)), (vector.y / ((float) MAIN_SIZE_Y)) + (((float) (MAIN_SIZE_Y - BODY_SIZE_Y)) / ((float) MAIN_SIZE_Y)), bodyTextureSize.x / ((float) MAIN_SIZE_X), bodyTextureSize.y / ((float) MAIN_SIZE_Y));
    }

    public void MoveAlpha(float duration, float alpha)
    {
        if (duration > 0f)
        {
            Color color = base.color;
            color.a = alpha;
            TweenRendererColor.Begin(base.gameObject, duration, color);
        }
        else
        {
            this.SetAlpha(alpha);
        }
    }

    public void MoveAlpha(float duration, float alpha, GameObject callbackObject, string callbackFunc)
    {
        if (duration > 0f)
        {
            Color color = base.color;
            color.a = alpha;
            TweenRendererColor color2 = TweenRendererColor.Begin(base.gameObject, duration, color);
            if ((callbackObject != null) && (color2 != null))
            {
                color2.eventReceiver = callbackObject;
                color2.callWhenFinished = callbackFunc;
            }
        }
        else
        {
            this.SetAlpha(alpha);
            if (callbackObject != null)
            {
                callbackObject.SendMessage(callbackFunc);
            }
        }
    }

    public void SetAlpha(float alpha)
    {
        Color c = base.color;
        c.a = alpha;
        this.SetTweenColor(c);
    }

    public void SetCharacter(int svtId, int imageLimitCount)
    {
        this.svtId = svtId;
        this.imageLimitCount = imageLimitCount;
    }

    public void SetLayer(int layer)
    {
        if (base.gameObject.layer != layer)
        {
            this.SetLayer(base.transform, layer);
        }
    }

    protected void SetLayer(Transform tf, int layer)
    {
        tf.gameObject.layer = layer;
        IEnumerator enumerator = tf.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                this.SetLayer(current, layer);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    public enum DispType
    {
        FULL,
        USER_SELECT,
        MY_ROOM
    }
}

