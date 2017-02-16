using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattlePerformanceBg : MonoBehaviour
{
    private AssetData BgAssetData;
    protected List<BgInfo> bgInfoList = new List<BgInfo>();
    public GameObject bgobject;
    public Transform bgRoot;
    public Transform bgRootCamera;
    private bool changeDirect;
    public string[] debuglist;
    private System.Action EndCallback;
    private FADE fade;
    protected static readonly string Frontfile = "front";
    public GameObject frontobject;
    protected bool IsLoading;
    private int loadbgno;
    private int loadbgType;
    public int newBgId;
    public int nowBgId;
    private bool parentCamera;
    private Vector3 pos;
    private Vector3 rot;
    private Color tmpcolor;

    protected void AddChangeBgInfo(int No, int tp, Vector3 pos, Vector3 rot, bool changeDirect = false, bool parentCamera = false, System.Action callback = null)
    {
        BgInfo item = new BgInfo(No, tp, pos, rot, changeDirect, parentCamera, callback);
        this.bgInfoList.Add(item);
    }

    public void changeBg(int No, int tp, Vector3 pos, Vector3 rot, bool changeDirect = false, bool parentCamera = false, System.Action callback = null)
    {
        if (this.IsLoading)
        {
            this.AddChangeBgInfo(No, tp, pos, rot, changeDirect, parentCamera, callback);
        }
        else
        {
            this.IsLoading = true;
            this.EndCallback = callback;
            Debug.Log(string.Concat(new object[] { "changeBg:", No, ":", changeDirect, ":", parentCamera }));
            if ((this.fade != FADE.NONE) && !changeDirect)
            {
                if (this.EndCallback != null)
                {
                    this.EndCallback();
                }
                this.OnEndBgLoad();
            }
            else
            {
                this.loadbgno = No;
                this.loadbgType = tp;
                this.changeDirect = changeDirect;
                this.parentCamera = parentCamera;
                this.pos = pos;
                this.rot = rot;
                this.fade = FADE.IN;
                if (!changeDirect)
                {
                    this.tmpcolor = RenderSettings.ambientLight;
                    object[] args = new object[] { "from", this.tmpcolor, "to", Color.black, "time", 0.5f, "onupdate", "UpdateColor", "oncomplete", "endChangeColor" };
                    iTween.ValueTo(base.gameObject, iTween.Hash(args));
                }
                else
                {
                    this.endChangeColor();
                }
            }
        }
    }

    public void DebugPrint(AssetData data)
    {
        Debug.Log("DebugPrint>>");
        foreach (string str in data.GetObjectNameList())
        {
            Debug.Log("name:" + str);
        }
    }

    public void endChangeBg(AssetData data)
    {
        Debug.Log("endChangeBg");
        this.BgAssetData = data;
        GameObject obj2 = this.getBgObject(data, this.loadbgType);
        if (this.parentCamera)
        {
            obj2.transform.parent = this.bgRootCamera;
            obj2.transform.localEulerAngles = this.rot;
        }
        else
        {
            obj2.transform.parent = this.bgRoot;
            obj2.transform.eulerAngles = Vector3.up;
        }
        obj2.transform.localPosition = this.pos;
        obj2.transform.localScale = Vector3.one;
        this.bgobject = obj2;
        GameObject obj3 = this.getFrontObject(data, this.loadbgType);
        if (obj3 != null)
        {
            obj3.transform.parent = this.bgRootCamera;
            obj3.transform.localEulerAngles = this.rot;
            obj3.transform.localPosition = this.pos;
            obj3.transform.localScale = Vector3.one;
            this.frontobject = obj3;
        }
        if (!this.changeDirect)
        {
            this.fade = FADE.OUT;
            object[] args = new object[] { "from", Color.black, "to", this.tmpcolor, "time", 0.5f, "onupdate", "UpdateColor", "oncomplete", "endChangeColor" };
            iTween.ValueTo(base.gameObject, iTween.Hash(args));
        }
        else
        {
            this.fade = FADE.NONE;
            this.UpdateColor(Color.white);
            if (this.EndCallback != null)
            {
                this.EndCallback();
            }
            this.OnEndBgLoad();
        }
    }

    public void endChangeColor()
    {
        Debug.Log("endChangeColor:" + this.fade);
        if (this.fade == FADE.IN)
        {
            RenderSettings.ambientLight = Color.black;
            this.ReleaseBg();
            string name = "Bg/" + this.loadbgno;
            Debug.Log("path:" + name);
            AssetManager.loadAssetStorage(name, new AssetLoader.LoadEndDataHandler(this.endChangeBg));
        }
        else if (this.fade == FADE.OUT)
        {
            this.fade = FADE.NONE;
            if (this.EndCallback != null)
            {
                this.EndCallback();
            }
            this.OnEndBgLoad();
        }
    }

    public void endloadBg(AssetData data)
    {
        this.IsLoading = false;
        if ((this.bgobject != null) || (this.frontobject != null))
        {
            if (this.bgobject != null)
            {
                UnityEngine.Object.Destroy(this.bgobject);
            }
            if (this.frontobject != null)
            {
                UnityEngine.Object.Destroy(this.frontobject);
            }
            this.ReleaseBg(this.nowBgId);
        }
        Debug.Log(string.Concat(new object[] { "endloadBg:", data.Name, ":", this.parentCamera }));
        this.BgAssetData = data;
        GameObject obj2 = this.getBgObject(data, this.loadbgType);
        if (this.parentCamera)
        {
            obj2.transform.parent = this.bgRootCamera;
            obj2.transform.localEulerAngles = this.rot;
        }
        else
        {
            obj2.transform.parent = this.bgRoot;
            obj2.transform.eulerAngles = Vector3.up;
        }
        obj2.transform.localPosition = this.pos;
        obj2.transform.localScale = Vector3.one;
        this.nowBgId = this.newBgId;
        this.bgobject = obj2;
        GameObject obj3 = this.getFrontObject(data, this.loadbgType);
        if (obj3 != null)
        {
            obj3.transform.parent = this.bgRootCamera;
            obj3.transform.localEulerAngles = this.rot;
            obj3.transform.localPosition = this.pos;
            obj3.transform.localScale = Vector3.one;
            this.frontobject = obj3;
        }
    }

    protected bool ExistsLoadBgInfo() => 
        (this.bgInfoList.Count > 0);

    protected BgInfo FetchBgInfo()
    {
        BgInfo info = null;
        if (this.ExistsLoadBgInfo())
        {
            info = this.bgInfoList[0];
            this.bgInfoList.RemoveAt(0);
        }
        return info;
    }

    private GameObject getBgObject(AssetData data, int tp)
    {
        if (tp > 0)
        {
            GameObject obj3 = data.GetObject<GameObject>("bg" + tp);
            if (obj3 != null)
            {
                return UnityEngine.Object.Instantiate<GameObject>(obj3);
            }
            return UnityEngine.Object.Instantiate<GameObject>(data.GetObject<GameObject>("bg"));
        }
        GameObject original = data.GetObject<GameObject>("bg");
        if (original != null)
        {
            return UnityEngine.Object.Instantiate<GameObject>(original);
        }
        return UnityEngine.Object.Instantiate<GameObject>(data.GetObject<GameObject>("bg0"));
    }

    public string[] getChangeBgList() => 
        this.debuglist;

    private GameObject getFrontObject(AssetData data, int tp)
    {
        GameObject obj2 = null;
        string name = (tp <= 0) ? Frontfile : (Frontfile + tp);
        GameObject original = data.GetObject<GameObject>(name);
        if (original != null)
        {
            return UnityEngine.Object.Instantiate<GameObject>(original);
        }
        GameObject obj4 = data.GetObject<GameObject>(Frontfile);
        if (obj4 != null)
        {
            obj2 = UnityEngine.Object.Instantiate<GameObject>(obj4);
        }
        return obj2;
    }

    public Texture2D GetShadowTexture(int shadowId)
    {
        Texture2D textured = null;
        if (this.BgAssetData != null)
        {
            textured = this.BgAssetData.GetObject<Texture2D>("shadow_" + shadowId);
        }
        return textured;
    }

    public void loadBg(int no, int tp = 0)
    {
        this.IsLoading = true;
        Debug.Log(string.Concat(new object[] { "loadBg:", no, "：", tp }));
        if (no == 0)
        {
            this.IsLoading = false;
        }
        else
        {
            this.loadbgno = no;
            this.loadbgType = tp;
            this.newBgId = no;
            if (!AssetManager.loadAssetStorage("Bg/" + no, new AssetLoader.LoadEndDataHandler(this.endloadBg)))
            {
                this.IsLoading = false;
            }
        }
    }

    protected void OnEndBgLoad()
    {
        this.IsLoading = false;
        if (this.ExistsLoadBgInfo())
        {
            BgInfo info = this.FetchBgInfo();
            if (info != null)
            {
                this.changeBg(info.bgNo, info.tp, info.pos, info.rot, info.changeDirect, info.parentCamera, info.callback);
            }
        }
    }

    public void ReleaseBg()
    {
        if ((this.bgobject != null) || (this.frontobject != null))
        {
            if (this.bgobject != null)
            {
                UnityEngine.Object.Destroy(this.bgobject);
                this.bgobject = null;
            }
            if (this.frontobject != null)
            {
                UnityEngine.Object.Destroy(this.frontobject);
                this.frontobject = null;
            }
            this.ReleaseBg(this.nowBgId);
        }
    }

    protected void ReleaseBg(int no)
    {
        string name = "Bg/" + no;
        this.BgAssetData = null;
        AssetManager.releaseAssetStorage(name);
    }

    public void UpdateColor(Color color)
    {
        RenderSettings.ambientLight = color;
    }

    public bool IsBusy =>
        this.IsLoading;

    protected class BgInfo
    {
        public int bgNo;
        public System.Action callback;
        public bool changeDirect;
        public bool parentCamera;
        public Vector3 pos;
        public Vector3 rot;
        public int tp;

        public BgInfo(int No, int tp, Vector3 pos, Vector3 rot, bool changeDirect = false, bool parentCamera = false, System.Action callback = null)
        {
            this.bgNo = No;
            this.tp = tp;
            this.pos = pos;
            this.rot = rot;
            this.changeDirect = changeDirect;
            this.parentCamera = parentCamera;
            this.callback = callback;
        }
    }

    private enum FADE
    {
        NONE,
        IN,
        OUT
    }
}

