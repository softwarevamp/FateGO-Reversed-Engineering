using System;
using System.Collections;
using UnityEngine;

public class UIStandFigureR : MonoBehaviour
{
    protected AssetData assetMain;
    [SerializeField]
    protected UITexture bodyTexture;
    protected System.Action callbackFunc;
    protected Face.Type faceType;
    protected int imageLimitCount;
    protected string imageName;
    protected bool isBusyRender;
    protected bool isRetryRender;
    protected string[] loadNameList;
    protected RenderTexture renderTexture;
    protected int svtId;

    public void Destroy()
    {
        this.OnDestroy();
    }

    protected void EndLoadAsset()
    {
        if (this.loadNameList != null)
        {
            AssetData data = AssetManager.getAssetStorage(this.loadNameList[0]);
            if (data != null)
            {
                if (this.assetMain != null)
                {
                    AssetManager.releaseAsset(this.assetMain);
                }
                this.assetMain = data;
                this.loadNameList = null;
                this.RequestRenderAsset();
            }
        }
    }

    protected void EndRenderAsset(RenderTexture renderTex)
    {
        if (this.renderTexture != null)
        {
            StandFigureManager.Release(this.renderTexture);
        }
        this.renderTexture = renderTex;
        this.bodyTexture.mainTexture = renderTex;
        this.bodyTexture.transform.localPosition = (Vector3) UIStandFigureRender.GetCharacterOffsetMyroom(this.svtId, this.imageLimitCount);
        if (!this.bodyTexture.gameObject.activeSelf)
        {
            this.bodyTexture.gameObject.SetActive(true);
        }
        this.isBusyRender = false;
        if (this.isRetryRender)
        {
            this.isRetryRender = false;
            this.RequestRenderAsset();
        }
        else
        {
            System.Action callbackFunc = this.callbackFunc;
            if (callbackFunc != null)
            {
                this.callbackFunc = null;
                callbackFunc();
            }
        }
    }

    public void MoveAlpha(float duration, float alpha)
    {
        if (duration > 0f)
        {
            TweenAlpha.Begin(this.bodyTexture.gameObject, duration, alpha);
        }
        else
        {
            this.bodyTexture.alpha = alpha;
        }
    }

    public void MoveAlpha(float duration, float alpha, GameObject callbackObject, string callbackFunc)
    {
        if (duration > 0f)
        {
            TweenAlpha alpha2 = TweenAlpha.Begin(this.bodyTexture.gameObject, duration, alpha);
            if ((callbackObject != null) && (alpha2 != null))
            {
                alpha2.eventReceiver = callbackObject;
                alpha2.callWhenFinished = callbackFunc;
            }
        }
        else
        {
            this.bodyTexture.alpha = alpha;
            if (callbackObject != null)
            {
                callbackObject.SendMessage(callbackFunc);
            }
        }
    }

    protected void OnDestroy()
    {
        this.ReleaseCharacter();
    }

    public void ReleaseCharacter()
    {
        if (this.bodyTexture != null)
        {
            this.bodyTexture.gameObject.SetActive(false);
        }
        if (this.bodyTexture != null)
        {
            this.bodyTexture.mainTexture = null;
        }
        if (this.renderTexture != null)
        {
            StandFigureManager.Release(this.renderTexture);
            this.renderTexture = null;
        }
        if (this.assetMain != null)
        {
            AssetManager.releaseAsset(this.assetMain);
            this.assetMain = null;
        }
        if (this.loadNameList != null)
        {
            AssetManager.releaseAssetStorage(this.loadNameList);
            this.loadNameList = null;
        }
    }

    protected void RequestRenderAsset()
    {
        if (this.isBusyRender)
        {
            this.isRetryRender = true;
        }
        else
        {
            this.isBusyRender = true;
            Texture2D textured = this.assetMain.GetObject<Texture2D>(this.imageName + "f");
            Texture2D[] textureList = null;
            if (textured != null)
            {
                textureList = new Texture2D[4];
                textureList[2] = textured;
                textureList[3] = this.assetMain.GetObject<Texture2D>(this.imageName + "fa");
            }
            else
            {
                textureList = new Texture2D[2];
            }
            textureList[0] = this.assetMain.GetObject<Texture2D>(this.imageName);
            textureList[1] = this.assetMain.GetObject<Texture2D>(this.imageName + "a");
            RenderTexture renderTexture = this.renderTexture;
            this.renderTexture = null;
            StandFigureManager.Render(renderTexture, this.svtId, this.imageLimitCount, this.faceType, textureList, new StandFigureRenderWaitStatus.EndHandler(this.EndRenderAsset));
        }
    }

    public void SetActive(bool isActive)
    {
        this.bodyTexture.gameObject.SetActive(isActive);
    }

    public void SetAlpha(float a)
    {
        this.bodyTexture.alpha = a;
    }

    public void SetCharacter(int svtId, int imageLimitCount, Face.Type faceType, System.Action callbackFunc)
    {
        this.loadNameList = UIStandFigureRender.GetAssetNameList(svtId, imageLimitCount);
        this.svtId = svtId;
        this.imageLimitCount = imageLimitCount;
        this.faceType = faceType;
        this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callbackFunc);
        this.imageName = string.Empty + svtId + imageLimitCount;
        AssetManager.loadAssetStorage(this.loadNameList, new System.Action(this.EndLoadAsset));
    }

    public void SetCharacter(int svtId, int limitCount, int lv, Face.Type faceType, System.Action callbackFunc)
    {
        this.SetCharacter(svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount), faceType, callbackFunc);
    }

    public void SetDepth(int d)
    {
        this.bodyTexture.depth = d;
    }

    public void SetFace(Face.Type faceType)
    {
        this.faceType = faceType;
        if (this.loadNameList == null)
        {
            this.RequestRenderAsset();
        }
    }

    public void SetFace(Face.Type faceType, System.Action callbackFunc)
    {
        this.faceType = faceType;
        this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callbackFunc);
        if (this.loadNameList == null)
        {
            this.RequestRenderAsset();
        }
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

    public int ImageLimitCountValue =>
        this.imageLimitCount;

    public bool IsLoad =>
        (this.loadNameList != null);

    public int SvtId =>
        this.svtId;

    public UITexture Texture =>
        this.bodyTexture;
}

