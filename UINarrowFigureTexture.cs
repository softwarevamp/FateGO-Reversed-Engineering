using System;
using UnityEngine;

public class UINarrowFigureTexture : UINarrowFigureRender
{
    protected AssetData assetData;
    [SerializeField]
    protected UITexture bodyTexture;
    protected System.Action callbackFunc;
    protected string loadName;

    protected void Awake()
    {
        this.bodyTexture.material = null;
    }

    protected void ClearTexture()
    {
        if (this.bodyTexture.material != null)
        {
            UnityEngine.Object.Destroy(this.bodyTexture.material);
            this.bodyTexture.material = null;
        }
        this.bodyTexture.gameObject.SetActive(false);
        this.bodyTexture.mainTexture = null;
        base.textureImageLimitCount = -1;
    }

    public void Destroy()
    {
        this.ReleaseCharacter();
    }

    protected void EndLoadAsset(AssetData assetData)
    {
        if (((this.loadName != null) && (assetData != null)) && AssetManager.compAssetStorage(assetData, this.loadName))
        {
            AssetData assetInfo = this.assetData;
            this.loadName = null;
            this.assetData = assetData;
            this.SetTexture();
            if (assetInfo != null)
            {
                AssetManager.releaseAsset(assetInfo);
            }
            System.Action callbackFunc = this.callbackFunc;
            if (callbackFunc != null)
            {
                this.callbackFunc = null;
                callbackFunc();
            }
        }
    }

    protected void OnDestroy()
    {
        this.ReleaseCharacter();
    }

    public void ReleaseCharacter()
    {
        this.bodyTexture.gameObject.SetActive(false);
        if (this.bodyTexture.material != null)
        {
            UnityEngine.Object.Destroy(this.bodyTexture.material);
            this.bodyTexture.material = null;
        }
        this.bodyTexture.mainTexture = null;
        if (this.assetData != null)
        {
            AssetManager.releaseAsset(this.assetData);
            this.assetData = null;
        }
        if (this.loadName != null)
        {
            AssetManager.releaseAssetStorage(this.loadName);
            this.loadName = null;
        }
    }

    public void SetActive(bool isActive)
    {
        this.bodyTexture.gameObject.SetActive(isActive);
    }

    public void SetCharacter(int svtId, int imageLimitCount, System.Action callbackFunc)
    {
        string assetName = UINarrowFigureRender.GetAssetName(svtId);
        if (imageLimitCount == BalanceConfig.OtherImageLimitCount)
        {
            imageLimitCount = UINarrowFigureRender.OTHER_IMAGE_LIMIT_COUNT;
        }
        base.SetCharacter(svtId, imageLimitCount);
        if (this.loadName != null)
        {
            if (AssetManager.compAssetStorage(this.loadName, assetName))
            {
                if (callbackFunc != null)
                {
                    this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callbackFunc);
                }
                return;
            }
            AssetManager.releaseAssetStorage(this.loadName);
        }
        else if ((this.assetData != null) && AssetManager.compAssetStorage(this.assetData, assetName))
        {
            if (base.textureImageLimitCount != base.imageLimitCount)
            {
                this.SetTexture();
            }
            if (callbackFunc != null)
            {
                callbackFunc();
            }
            return;
        }
        if (callbackFunc != null)
        {
            this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callbackFunc);
        }
        this.ClearTexture();
        this.loadName = assetName;
        AssetManager.loadAssetStorage(this.loadName, new AssetLoader.LoadEndDataHandler(this.EndLoadAsset));
    }

    public void SetDepth(int d)
    {
        this.bodyTexture.depth = d;
    }

    protected void SetTexture()
    {
        this.bodyTexture.gameObject.SetActive(true);
        if (this.bodyTexture.material == null)
        {
            this.bodyTexture.material = new Material(Shader.Find("Custom/SpriteWithMask"));
        }
        this.bodyTexture.mainTexture = base.GetBodyTexture(this.assetData);
        this.bodyTexture.material.SetTexture("_MaskTex", base.GetBodyAlphaTexture(this.assetData));
        this.bodyTexture.uvRect = base.GetBodyUvRect();
        Vector2 bodySize = base.GetBodySize();
        this.bodyTexture.width = (int) bodySize.x;
        this.bodyTexture.height = (int) bodySize.y;
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        this.bodyTexture.color = c;
    }

    public bool IsLoad =>
        (this.loadName != null);
}

