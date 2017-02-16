using System;
using UnityEngine;

public class UIMasterFullFigureTexture : UIMasterFullFigureRender
{
    protected AssetData[] assetDataList;
    [SerializeField]
    protected UITexture bodyTexture;
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
        this.bodyTexture.gameObject.SetActive(false);
        UnityEngine.Object.Destroy(this.bodyTexture.material);
        this.bodyTexture.mainTexture = null;
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

    public void SetActive(bool isActive)
    {
        this.bodyTexture.gameObject.SetActive(isActive);
    }

    public void SetCharacter(UIMasterFullFigureRender.DispType dispType, int genderType, int equipId, System.Action callbackFunc)
    {
        string[] assetNameList = UIMasterFullFigureRender.GetAssetNameList(genderType, equipId);
        base.SetCharacter(dispType, genderType, equipId);
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

    public void SetDepth(int d)
    {
        this.bodyTexture.depth = d;
    }

    protected void SetTexture()
    {
        this.bodyTexture.material = new Material(Shader.Find("Custom/SpriteWithMask"));
        this.bodyTexture.mainTexture = base.GetBodyTexture(this.assetDataList);
        this.bodyTexture.material.SetTexture("_MaskTex", base.GetBodyAlphaTexture(this.assetDataList));
        this.SetTextureStatus();
    }

    protected void SetTextureStatus()
    {
        this.bodyTexture.uvRect = base.GetBodyUvRect();
        Vector2 bodySize = base.GetBodySize();
        this.bodyTexture.width = (int) bodySize.x;
        this.bodyTexture.height = (int) bodySize.y;
        this.bodyTexture.transform.localPosition = (Vector3) base.dispOffset;
        if (!this.bodyTexture.gameObject.activeSelf)
        {
            this.bodyTexture.gameObject.SetActive(true);
        }
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        this.bodyTexture.color = c;
    }

    public bool IsLoad =>
        (this.loadNameList != null);
}

