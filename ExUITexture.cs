using System;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/NGUI ExTexture")]
public class ExUITexture : UITexture
{
    protected AssetData assetData;
    protected string assetLabel;
    protected System.Action callbackFunc;
    protected string loadAssetName;

    public void ClearImage()
    {
        this.mainTexture = null;
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
            this.assetData = data;
            this.mainTexture = data.GetObject<Texture2D>(this.assetLabel);
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
            this.mainTexture = this.assetData.GetObject<Texture2D>(this.assetLabel);
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

    public void SetImage(Texture tex)
    {
        this.mainTexture = tex;
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
}

