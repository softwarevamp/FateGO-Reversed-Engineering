using System;
using UnityEngine;

public class UIMasterFaceTexture : UIMasterFaceRender
{
    protected AssetData[] assetDataList;
    [SerializeField]
    protected UITexture bodyTexture;
    protected System.Action callbackFunc;
    protected string[] loadNameList;

    public void Destroy()
    {
        this.OnDestroy();
    }

    protected void EndLoadAsset()
    {
        if (this.loadNameList != null)
        {
            AssetData[] dataArray = AssetManager.getAssetStorage(this.loadNameList);
            if (dataArray != null)
            {
                if (this.assetDataList != null)
                {
                    AssetManager.releaseAsset(this.assetDataList);
                }
                this.assetDataList = dataArray;
                this.loadNameList = null;
                this.bodyTexture.material = new Material(Shader.Find("Custom/SpriteWithMask"));
                this.bodyTexture.mainTexture = base.GetBodyTexture(this.assetDataList);
                this.bodyTexture.material.SetTexture("_MaskTex", base.GetMaskTexture(this.assetDataList));
                this.bodyTexture.uvRect = base.GetBodyUvRect();
                Vector2 bodySize = base.GetBodySize();
                this.bodyTexture.width = (int) bodySize.x;
                this.bodyTexture.height = (int) bodySize.y;
                if (!this.bodyTexture.gameObject.activeSelf)
                {
                    this.bodyTexture.gameObject.SetActive(true);
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
        this.loadNameList = null;
    }

    public void SetActive(bool isActive)
    {
        this.bodyTexture.gameObject.SetActive(isActive);
    }

    public void SetCharacter(UIMasterFaceRender.DispType dispType, int genderType, int equipId, System.Action callbackFunc)
    {
        this.loadNameList = UIMasterFaceRender.GetAssetNameList(dispType, genderType, equipId);
        base.SetCharacter(dispType, genderType, equipId);
        this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callbackFunc);
        AssetManager.loadAssetStorage(this.loadNameList, new System.Action(this.EndLoadAsset));
    }

    public void SetDepth(int d)
    {
        this.bodyTexture.depth = d;
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        this.bodyTexture.color = c;
    }

    public bool IsLoad =>
        (this.loadNameList != null);
}

