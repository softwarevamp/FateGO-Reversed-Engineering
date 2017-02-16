using System;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI ExSpriteRenderer"), ExecuteInEditMode]
public class ExUISpriteRenderer : UITweenRenderer
{
    protected AssetData assetData;
    protected string assetLabel;
    protected Vector3 baseScale;
    protected System.Action callbackFunc;
    [SerializeField]
    public Texture2D defaultTexture;
    protected bool isInit;
    protected string loadAssetName;
    protected int oldHeight = 1;
    protected int oldWidth = 1;
    [SerializeField]
    public SpriteRenderer spriteRenderer;

    public void ClearImage()
    {
        this.spriteRenderer.sprite = null;
        if (this.assetData != null)
        {
            AssetManager.releaseAsset(this.assetData);
            this.assetData = null;
        }
        if (this.loadAssetName != null)
        {
            AssetManager.releaseAssetStorage(this.loadAssetName);
            this.loadAssetName = null;
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc();
            }
        }
    }

    protected void EndLoad(AssetData data)
    {
        if (((data != null) && (this.loadAssetName != null)) && AssetManager.compAssetStorage(data, this.loadAssetName))
        {
            AssetData assetData = this.assetData;
            this.loadAssetName = null;
            this.assetData = data;
            this.SetTexture(data.GetObject<Texture2D>(this.assetLabel));
            if (assetData != null)
            {
                AssetManager.releaseAsset(assetData);
            }
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc();
            }
        }
    }

    public override Color GetTweenColor() => 
        base.color;

    protected override void OnInit()
    {
        base.OnInit();
        if (!this.isInit)
        {
            this.isInit = true;
            this.baseScale = base.transform.localScale;
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (this.spriteRenderer == null)
        {
            this.spriteRenderer = base.GetComponent<SpriteRenderer>();
        }
        if (this.defaultTexture != null)
        {
            this.SetImage(this.defaultTexture);
        }
        if (!this.isInit)
        {
            this.isInit = true;
            this.baseScale = base.transform.localScale;
        }
        this.ReScaleUpdate();
    }

    protected void ReScaleUpdate()
    {
        if (this.isInit && ((this.oldWidth != base.width) || (this.oldHeight != base.height)))
        {
            if (this.spriteRenderer.sprite != null)
            {
                Texture2D texture = this.spriteRenderer.sprite.texture;
                Vector3 vector = new Vector3((this.baseScale.x * base.width) / ((float) texture.width), (this.baseScale.y * base.height) / ((float) texture.height), this.baseScale.y);
                base.transform.localScale = vector;
            }
            this.oldWidth = base.width;
            this.oldHeight = base.height;
        }
    }

    public void SetAssetImage(string assetName, System.Action callback = null)
    {
        this.SetAssetImage(assetName, null, callback);
    }

    public void SetAssetImage(string assetName, string assetLabel, System.Action callback = null)
    {
        this.assetLabel = assetLabel;
        if (this.loadAssetName != null)
        {
            if (AssetManager.compAssetStorage(this.loadAssetName, assetName))
            {
                if (this.callbackFunc != null)
                {
                    this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, this.callbackFunc);
                }
                return;
            }
            AssetManager.releaseAssetStorage(this.loadAssetName);
        }
        else if ((this.assetData != null) && AssetManager.compAssetStorage(this.assetData, assetName))
        {
            this.SetTexture(this.assetData.GetObject<Texture2D>(assetLabel));
            if (this.callbackFunc != null)
            {
                this.callbackFunc();
            }
            return;
        }
        this.loadAssetName = assetName;
        if (callback != null)
        {
            this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callback);
        }
        AssetManager.loadAssetStorage(assetName, new AssetLoader.LoadEndDataHandler(this.EndLoad));
    }

    public void SetImage(Texture2D tex)
    {
        this.SetTexture(tex);
        if (this.assetData != null)
        {
            AssetManager.releaseAsset(this.assetData);
            this.assetData = null;
        }
        if (this.loadAssetName != null)
        {
            AssetManager.releaseAssetStorage(this.loadAssetName);
            this.loadAssetName = null;
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc();
            }
        }
    }

    protected void SetTexture(Texture2D tex)
    {
        Rect rect = new Rect(0f, 0f, (float) tex.width, (float) tex.height);
        this.spriteRenderer.sprite = UnityEngine.Sprite.Create(tex, rect, base.pivotOffset, 1f);
        Vector3 vector = new Vector3((this.baseScale.x * base.width) / ((float) tex.width), (this.baseScale.y * base.height) / ((float) tex.height), this.baseScale.y);
        base.transform.localScale = vector;
        this.oldWidth = base.width;
        this.oldHeight = base.height;
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        this.spriteRenderer.enabled = c.a > 0f;
        this.spriteRenderer.color = c;
    }
}

