using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIImageM : UIImageRender
{
    protected AssetData[] assetDataList;
    protected System.Action callbackFunc;
    protected string[] loadNameList;

    public void Destroy()
    {
        this.ReleaseCharacter();
    }

    protected void EndLoadAsset()
    {
        if (this.loadNameList != null)
        {
            AssetData[] dataArray = AssetManager.getAssetStorage(this.loadNameList);
            if (dataArray != null)
            {
                AssetData[] assetDataList = this.assetDataList;
                this.loadNameList = null;
                this.assetDataList = dataArray;
                this.SetTexture();
                if (assetDataList != null)
                {
                    AssetManager.releaseAsset(assetDataList);
                }
                System.Action callbackFunc = this.callbackFunc;
                if (callbackFunc != null)
                {
                    this.callbackFunc = null;
                    callbackFunc();
                }
            }
        }
    }

    protected void OnDestroy()
    {
        this.ReleaseCharacter();
    }

    public void ReleaseCharacter()
    {
        base.ReleaseCharacter();
        if (this.assetDataList != null)
        {
            AssetManager.releaseAsset(this.assetDataList);
            this.assetDataList = null;
        }
        if (this.loadNameList != null)
        {
            AssetManager.releaseAssetStorage(this.loadNameList);
            this.loadNameList = null;
        }
    }

    public void SetCharacter(string imageName, Face.Type faceType, System.Action callbackFunc = null)
    {
        string[] assetNameList = UIImageRender.GetAssetNameList(imageName);
        if (this.loadNameList != null)
        {
            if (AssetManager.compAssetStorageList(this.loadNameList, assetNameList))
            {
                if (callbackFunc != null)
                {
                    this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callbackFunc);
                }
                return;
            }
            AssetManager.releaseAssetStorage(this.loadNameList);
        }
        else if ((this.assetDataList != null) && AssetManager.compAssetStorageList(this.assetDataList, assetNameList))
        {
            this.SetTextureStatus();
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
        this.loadNameList = assetNameList;
        AssetManager.loadAssetStorage(this.loadNameList, new System.Action(this.EndLoadAsset));
    }

    public void SetDispOffset()
    {
        base.bodyFilter.transform.localPosition = (Vector3) base.dispOffset;
    }

    protected void SetTexture()
    {
        AssetData data = this.assetDataList[0];
        string lastName = data.LastName;
        base.textureList = new Texture2D[] { data.GetObject<Texture2D>(lastName), data.GetObject<Texture2D>(lastName + "a") };
        this.SetTextureStatus();
    }

    protected void SetTextureStatus()
    {
        base.SetCharacterRender();
        base.bodyFilter.transform.localPosition = (Vector3) base.dispTop;
    }

    public bool IsLoad =>
        (this.loadNameList != null);
}

