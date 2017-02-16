using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CommonEffectLoadComponent : BaseMonoBehaviour
{
    [SerializeField]
    protected string effectName;
    protected bool isEnable;
    protected bool isInit;
    protected bool isSkip;
    protected object param;

    protected event LoadEndHandler loadCallback;

    protected void EndLoad(AssetData data)
    {
        if (this.isEnable)
        {
            try
            {
                Debug.Log("EndLoadEffect " + this.effectName);
                Transform parent = base.transform?.parent;
                if (parent != null)
                {
                    GameObject original = data.GetObject<GameObject>();
                    GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(original);
                    CommonEffectComponent effect = obj3.GetComponent<CommonEffectComponent>();
                    Transform transform = obj3.transform;
                    Vector3 localScale = original.transform.localScale;
                    transform.parent = parent.transform;
                    transform.localPosition = base.transform.localPosition;
                    transform.localRotation = Quaternion.identity;
                    localScale.x *= base.transform.localScale.x;
                    localScale.y *= base.transform.localScale.y;
                    localScale.z *= base.transform.localScale.z;
                    transform.localScale = localScale;
                    effect.Init(data, this.isSkip, false);
                    if (this.param != null)
                    {
                        effect.SetParam(this.param);
                    }
                    this.isEnable = false;
                    if (this.loadCallback != null)
                    {
                        this.loadCallback(effect);
                    }
                }
                UnityEngine.Object.Destroy(base.gameObject);
            }
            catch (MissingReferenceException exception)
            {
                Debug.LogWarning(exception.Message);
                try
                {
                    if (this.isEnable)
                    {
                        this.isEnable = false;
                        AssetManager.releaseAssetStorage(this.effectName);
                    }
                }
                catch
                {
                }
            }
        }
    }

    public void Init(string filename)
    {
        if (!this.isInit)
        {
            this.isInit = true;
            this.isEnable = true;
            this.effectName = filename;
            base.gameObject.name = "EffectLoad(" + filename + ")";
            if (!AssetManager.loadAssetStorage(filename, new AssetLoader.LoadEndDataHandler(this.EndLoad)))
            {
                Debug.Log("CommonEffectLoadComponent: load error [" + filename + "]");
                this.isInit = false;
                this.isEnable = false;
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
    }

    public void Init(string filename, LoadEndHandler callback, bool isSkip = false)
    {
        this.Init(filename, null, callback, isSkip);
    }

    public void Init(string filename, object param, LoadEndHandler callback, bool isSkip = false)
    {
        this.param = param;
        this.isSkip = isSkip;
        if (callback != null)
        {
            this.loadCallback = (LoadEndHandler) Delegate.Combine(this.loadCallback, callback);
        }
        if (this.isInit)
        {
            if (this.isEnable)
            {
                return;
            }
        }
        else if (!this.isEnable)
        {
            this.Init(filename);
            if (this.isInit)
            {
                return;
            }
        }
        if (callback != null)
        {
            callback(null);
        }
    }

    protected void onDestroy()
    {
        if (this.isEnable)
        {
            this.isEnable = false;
            AssetManager.releaseAssetStorage(this.effectName);
        }
    }

    public void Resume(bool isSkip)
    {
        this.isSkip = isSkip;
    }

    protected void Start()
    {
        if (!string.IsNullOrEmpty(this.effectName))
        {
            this.Init(this.effectName);
        }
    }

    public bool Stop()
    {
        if (!this.isEnable)
        {
            return false;
        }
        this.isEnable = false;
        AssetManager.releaseAssetStorage(this.effectName);
        if (this.loadCallback != null)
        {
            this.loadCallback(null);
        }
        UnityEngine.Object.Destroy(base.gameObject);
        return true;
    }

    public string EffectName =>
        this.effectName;

    public delegate void LoadEndHandler(CommonEffectComponent effect);
}

