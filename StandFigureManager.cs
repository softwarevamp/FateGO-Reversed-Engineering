using System;
using System.Collections.Generic;
using UnityEngine;

public class StandFigureManager : SingletonMonoBehaviour<StandFigureManager>
{
    protected bool isRenderWait;
    protected StandFigureRenderWaitStatus renderInfo;
    protected Queue<StandFigureRenderWaitStatus> renderWaitList = new Queue<StandFigureRenderWaitStatus>();
    [SerializeField]
    protected StandFigureCamera standFigureCamera;
    [SerializeField]
    protected GameObject standFigureMPrefab;
    [SerializeField]
    protected UIStandFigureRender standFigureRender;
    [SerializeField]
    protected GameObject standFigureRPrefab;

    protected void AddRender(StandFigureRenderWaitStatus info)
    {
        this.renderWaitList.Enqueue(info);
    }

    public UIStandFigureM CreateMeshLocal(GameObject parent)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.standFigureMPrefab);
        UIStandFigureM component = obj2.GetComponent<UIStandFigureM>();
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        obj2.name = "StandFigureM";
        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.SetLayer(parent.layer);
        return component;
    }

    public UIStandFigureM CreateMeshLocal(GameObject parent, int svtId, int imageLimitCount, Face.Type faceType, int panelDepth, System.Action callbackFunc)
    {
        UIStandFigureM em = this.CreateMeshLocal(parent);
        object[] objArray1 = new object[] { "StandFigureM(", svtId, imageLimitCount, ")" };
        em.gameObject.name = string.Concat(objArray1);
        em.SetCharacter(svtId, imageLimitCount, faceType, callbackFunc);
        em.SetDepth((float) panelDepth);
        return em;
    }

    public static UIStandFigureM CreateMeshPrefab(GameObject parent) => 
        SingletonMonoBehaviour<StandFigureManager>.Instance.CreateMeshLocal(parent);

    public static UIStandFigureM CreateMeshPrefab(GameObject parent, int svtId, int imageLimitCount, Face.Type faceType, int panelDepth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<StandFigureManager>.Instance.CreateMeshLocal(parent, svtId, imageLimitCount, faceType, panelDepth, callbackFunc);

    public static UIStandFigureM CreateMeshPrefab(GameObject parent, int svtId, int limitCount, int lv, Face.Type faceType, int panelDepth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<StandFigureManager>.Instance.CreateMeshLocal(parent, svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount), faceType, panelDepth, callbackFunc);

    public UIStandFigureR CreateRenderLocal(GameObject parent)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.standFigureRPrefab);
        UIStandFigureR component = obj2.GetComponent<UIStandFigureR>();
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        obj2.name = "StandFigureR";
        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.SetLayer(parent.gameObject.layer);
        return component;
    }

    public UIStandFigureR CreateRenderLocal(GameObject parent, int svtId, int imageLimitCount, Face.Type faceType, int panelDepth, System.Action callbackFunc)
    {
        UIStandFigureR er = this.CreateRenderLocal(parent);
        object[] objArray1 = new object[] { "StandFigureR(", svtId, imageLimitCount, ")" };
        er.gameObject.name = string.Concat(objArray1);
        er.SetCharacter(svtId, imageLimitCount, faceType, callbackFunc);
        er.SetDepth(panelDepth);
        return er;
    }

    public static UIStandFigureR CreateRenderPrefab(GameObject parent) => 
        SingletonMonoBehaviour<StandFigureManager>.Instance.CreateRenderLocal(parent);

    public static UIStandFigureR CreateRenderPrefab(GameObject parent, int svtId, int imageLimitCount, Face.Type faceType, int panelDepth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<StandFigureManager>.Instance.CreateRenderLocal(parent, svtId, imageLimitCount, faceType, panelDepth, callbackFunc);

    public static UIStandFigureR CreateRenderPrefab(GameObject parent, int svtId, int limitCount, int lv, Face.Type faceType, int panelDepth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<StandFigureManager>.Instance.CreateRenderLocal(parent, svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount), faceType, panelDepth, callbackFunc);

    public static void DownloadAsset(int svtId, int imageLimitCount, System.Action callback)
    {
        AssetManager.downloadAssetStorage(GetAssetName(svtId, imageLimitCount), callback);
    }

    public static void DownloadAsset(int svtId, int limitCount, int lv, System.Action callback)
    {
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(svtId, limitCount);
        AssetManager.downloadAssetStorage(GetAssetName(svtId, imageLimitCount), callback);
    }

    public static string[] GetAssetName(int svtId, int imageLimitCount) => 
        UIStandFigureRender.GetAssetNameList(svtId, imageLimitCount);

    public static string[] GetAssetName(int svtId, int limitCount, int lv)
    {
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(svtId, limitCount);
        return UIStandFigureRender.GetAssetNameList(svtId, imageLimitCount);
    }

    protected void LateUpdate()
    {
        if (this.RenderStart())
        {
            this.isRenderWait = true;
        }
        if (this.isRenderWait)
        {
            this.isRenderWait = !this.standFigureCamera.Request(this.renderInfo.GetRenderTexture());
            if (!this.isRenderWait)
            {
                this.standFigureRender.gameObject.SetActive(true);
                this.standFigureRender.SetActive(true);
                this.renderInfo.SetCharacter(this.standFigureRender);
            }
        }
    }

    public static void LoadAsset(int svtId, int imageLimitCount, System.Action callback)
    {
        AssetManager.loadAssetStorage(GetAssetName(svtId, imageLimitCount), callback);
    }

    public static void LoadAsset(int svtId, int limitCount, int lv, System.Action callback)
    {
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(svtId, limitCount);
        AssetManager.loadAssetStorage(GetAssetName(svtId, imageLimitCount), callback);
    }

    public void OnRenderEnd(RenderTexture renderTexture)
    {
        if (this.renderInfo == null)
        {
            Debug.LogError("render info is null");
        }
        else
        {
            this.renderInfo.Callback(renderTexture);
            this.renderInfo = null;
        }
    }

    public void Reboot()
    {
        if (this.renderWaitList.Count > 0)
        {
            this.renderWaitList.Clear();
        }
        this.renderInfo = null;
    }

    public static void Release(RenderTexture renderTex)
    {
        RenderTexture.ReleaseTemporary(renderTex);
    }

    public static void ReleaseAsset(int svtId, int imageLimitCount)
    {
        AssetManager.releaseAssetStorage(GetAssetName(svtId, imageLimitCount));
    }

    public static void ReleaseAsset(int svtId, int limitCount, int lv)
    {
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(svtId, limitCount);
        AssetManager.releaseAssetStorage(GetAssetName(svtId, imageLimitCount));
    }

    public static void Render(int svtId, int imageLimitCount, Face.Type faceType, Texture2D[] textureList, StandFigureRenderWaitStatus.EndHandler callback)
    {
        StandFigureRenderWaitStatus info = new StandFigureRenderWaitStatus(svtId, imageLimitCount, faceType, textureList, callback);
        SingletonMonoBehaviour<StandFigureManager>.Instance.AddRender(info);
    }

    public static void Render(int svtId, int limitCount, int lv, Face.Type faceType, Texture2D[] textureList, StandFigureRenderWaitStatus.EndHandler callback)
    {
        StandFigureRenderWaitStatus info = new StandFigureRenderWaitStatus(svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount), faceType, textureList, callback);
        SingletonMonoBehaviour<StandFigureManager>.Instance.AddRender(info);
    }

    public static void Render(RenderTexture renderTex, int svtId, int imageLimitCount, Face.Type faceType, Texture2D[] textureList, StandFigureRenderWaitStatus.EndHandler callback)
    {
        StandFigureRenderWaitStatus info = new StandFigureRenderWaitStatus(renderTex, svtId, imageLimitCount, faceType, textureList, callback);
        SingletonMonoBehaviour<StandFigureManager>.Instance.AddRender(info);
    }

    public static void Render(RenderTexture renderTex, int svtId, int limitCount, int lv, Face.Type faceType, Texture2D[] textureList, StandFigureRenderWaitStatus.EndHandler callback)
    {
        StandFigureRenderWaitStatus info = new StandFigureRenderWaitStatus(renderTex, svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount), faceType, textureList, callback);
        SingletonMonoBehaviour<StandFigureManager>.Instance.AddRender(info);
    }

    protected bool RenderStart()
    {
        if (this.renderInfo != null)
        {
            return false;
        }
        if (this.renderWaitList.Count <= 0)
        {
            return false;
        }
        this.renderInfo = this.renderWaitList.Dequeue();
        return true;
    }
}

