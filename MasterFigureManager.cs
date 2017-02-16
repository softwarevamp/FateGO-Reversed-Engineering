using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterFigureManager : SingletonMonoBehaviour<MasterFigureManager>
{
    protected bool isRenderWait;
    protected StandFigureRenderWaitStatus renderInfo;
    protected Queue<StandFigureRenderWaitStatus> renderWaitList = new Queue<StandFigureRenderWaitStatus>();
    [SerializeField]
    protected StandFigureCamera standFigureCamera;
    [SerializeField]
    protected GameObject standFigureMPrefab;
    [SerializeField]
    protected UIMasterFigureRender standFigureRender;
    [SerializeField]
    protected GameObject standFigureRPrefab;

    protected void AddRender(StandFigureRenderWaitStatus info)
    {
        this.renderWaitList.Enqueue(info);
    }

    public UIMasterFigureM CreateMeshLocal(GameObject parent)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.standFigureMPrefab);
        UIMasterFigureM component = obj2.GetComponent<UIMasterFigureM>();
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        obj2.name = "MasterFigureM";
        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.SetLayer(parent.layer);
        return component;
    }

    public UIMasterFigureM CreateMeshLocal(GameObject parent, int svtId, int imageLimitCount, Face.Type faceType, int panelDepth, System.Action callbackFunc)
    {
        UIMasterFigureM em = this.CreateMeshLocal(parent);
        object[] objArray1 = new object[] { "MasterFigureM(", svtId, imageLimitCount, ")" };
        em.gameObject.name = string.Concat(objArray1);
        em.SetCharacter(svtId, imageLimitCount, faceType, callbackFunc);
        em.SetDepth((float) panelDepth);
        return em;
    }

    public static UIMasterFigureM CreateMeshPrefab(GameObject parent) => 
        SingletonMonoBehaviour<MasterFigureManager>.Instance.CreateMeshLocal(parent);

    public static UIMasterFigureM CreateMeshPrefab(GameObject parent, int svtId, int imageLimitCount, Face.Type faceType, int panelDepth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<MasterFigureManager>.Instance.CreateMeshLocal(parent, svtId, imageLimitCount, faceType, panelDepth, callbackFunc);

    public static UIMasterFigureM CreateMeshPrefab(GameObject parent, int svtId, int limitCount, int lv, Face.Type faceType, int panelDepth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<MasterFigureManager>.Instance.CreateMeshLocal(parent, svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount), faceType, panelDepth, callbackFunc);

    public UIMasterFigureR CreateRenderLocal(GameObject parent)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.standFigureRPrefab);
        UIMasterFigureR component = obj2.GetComponent<UIMasterFigureR>();
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        obj2.name = "MasterFigureR";
        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.SetLayer(parent.gameObject.layer);
        return component;
    }

    public UIMasterFigureR CreateRenderLocal(GameObject parent, int svtId, int imageLimitCount, Face.Type faceType, int panelDepth, System.Action callbackFunc)
    {
        UIMasterFigureR er = this.CreateRenderLocal(parent);
        object[] objArray1 = new object[] { "MasterFigureR(", svtId, imageLimitCount, ")" };
        er.gameObject.name = string.Concat(objArray1);
        er.SetCharacter(svtId, imageLimitCount, faceType, callbackFunc);
        er.SetDepth(panelDepth);
        return er;
    }

    public static UIMasterFigureR CreateRenderPrefab(GameObject parent) => 
        SingletonMonoBehaviour<MasterFigureManager>.Instance.CreateRenderLocal(parent);

    public static UIMasterFigureR CreateRenderPrefab(GameObject parent, int svtId, int imageLimitCount, Face.Type faceType, int panelDepth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<MasterFigureManager>.Instance.CreateRenderLocal(parent, svtId, imageLimitCount, faceType, panelDepth, callbackFunc);

    public static UIMasterFigureR CreateRenderPrefab(GameObject parent, int svtId, int limitCount, int lv, Face.Type faceType, int panelDepth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<MasterFigureManager>.Instance.CreateRenderLocal(parent, svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount), faceType, panelDepth, callbackFunc);

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
        UIMasterFigureRender.GetAssetNameList(svtId, imageLimitCount);

    public static string[] GetAssetName(int svtId, int limitCount, int lv)
    {
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(svtId, limitCount);
        return UIMasterFigureRender.GetAssetNameList(svtId, imageLimitCount);
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
        SingletonMonoBehaviour<MasterFigureManager>.Instance.AddRender(info);
    }

    public static void Render(int svtId, int limitCount, int lv, Face.Type faceType, Texture2D[] textureList, StandFigureRenderWaitStatus.EndHandler callback)
    {
        StandFigureRenderWaitStatus info = new StandFigureRenderWaitStatus(svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount), faceType, textureList, callback);
        SingletonMonoBehaviour<MasterFigureManager>.Instance.AddRender(info);
    }

    public static void Render(RenderTexture renderTex, int svtId, int imageLimitCount, Face.Type faceType, Texture2D[] textureList, StandFigureRenderWaitStatus.EndHandler callback)
    {
        StandFigureRenderWaitStatus info = new StandFigureRenderWaitStatus(renderTex, svtId, imageLimitCount, faceType, textureList, callback);
        SingletonMonoBehaviour<MasterFigureManager>.Instance.AddRender(info);
    }

    public static void Render(RenderTexture renderTex, int svtId, int limitCount, int lv, Face.Type faceType, Texture2D[] textureList, StandFigureRenderWaitStatus.EndHandler callback)
    {
        StandFigureRenderWaitStatus info = new StandFigureRenderWaitStatus(renderTex, svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount), faceType, textureList, callback);
        SingletonMonoBehaviour<MasterFigureManager>.Instance.AddRender(info);
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

