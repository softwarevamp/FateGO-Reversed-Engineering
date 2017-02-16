using System;
using System.Collections;
using UnityEngine;

public class LogoMain : BaseMonoBehaviour
{
    protected System.Action callbackFunc;
    protected static readonly float FADEIN_TIME = 0.5f;
    protected static readonly float FADEOUT_TIME = 0.5f;
    protected static readonly float FADEWAIT_TIME = 1f;
    protected PlayMakerFSM fsm;
    protected int index;
    protected static bool isPlayLogo = true;
    [SerializeField]
    protected GameObject[] logoList;
    protected static readonly string SAVE_KEY = "LogoPlay";
    protected Status status;

    protected void EndCancelFadeout()
    {
        this.EndLogo();
    }

    protected void EndFadein()
    {
        this.status = Status.WAIT;
        base.Invoke("Fadeout", FADEWAIT_TIME);
    }

    protected void EndFadeout()
    {
        this.logoList[this.index].SetActive(false);
        this.index++;
        this.status = Status.WAIT2;
        base.Invoke("Fadein", 0.2f);
    }

    protected void EndLogo()
    {
        this.status = Status.NONE;
        for (int i = 0; i < this.logoList.Length; i++)
        {
            this.logoList[i].SetActive(false);
        }
        if (this.fsm != null)
        {
            this.fsm.SendEvent("END_LOGO");
        }
        if (this.callbackFunc != null)
        {
            this.callbackFunc();
        }
    }

    protected void Fadein()
    {
        if (this.logoList.Length > this.index)
        {
            this.status = Status.FADEIN;
            this.logoList[this.index].SetActive(true);
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(FADEIN_TIME, new System.Action(this.EndFadein));
        }
        else
        {
            this.EndLogo();
        }
    }

    protected void FadeinFirst()
    {
        this.logoList[this.index].SetActive(true);
        this.EndFadein();
    }

    protected void Fadeout()
    {
        this.status = Status.FADEOUT;
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.WHITE, FADEOUT_TIME, new System.Action(this.EndFadeout));
    }

    public void Init()
    {
        isPlayLogo = false;
        if (!ManagerConfig.UseMock)
        {
            string appVer = ManagerConfig.AppVer;
            if (PlayerPrefs.GetString(SAVE_KEY, "none") != appVer)
            {
                PlayerPrefs.SetString(SAVE_KEY, appVer);
                PlayerPrefs.Save();
            }
        }
        base.gameObject.SetActive(true);
        if (base.transform.parent != null)
        {
            this.SetChildInit(base.transform, base.transform.parent.gameObject.layer);
        }
        if (this.logoList.Length > 0)
        {
            for (int i = 0; i < this.logoList.Length; i++)
            {
                this.logoList[i].SetActive(false);
            }
            this.index = 0;
            this.status = Status.WAIT2;
            base.Invoke("FadeinFirst", 0.1f);
        }
        else
        {
            this.EndLogo();
        }
    }

    public void Init(PlayMakerFSM fsm)
    {
        this.fsm = fsm;
        this.callbackFunc = null;
        this.Init();
    }

    public void Init(System.Action callback)
    {
        this.fsm = null;
        this.callbackFunc = callback;
        this.Init();
    }

    public static bool IsPLayLogo()
    {
        if (!isPlayLogo)
        {
            return false;
        }
        if (ManagerConfig.UseMock)
        {
            return true;
        }
        string appVer = ManagerConfig.AppVer;
        return (PlayerPrefs.GetString(SAVE_KEY, "none") != appVer);
    }

    protected void OnClick()
    {
    }

    public void Quit()
    {
        this.status = Status.NONE;
        base.gameObject.SetActive(false);
    }

    protected void SetChildInit(Transform tf, int layer)
    {
        if (tf.gameObject.layer != layer)
        {
            tf.gameObject.layer = layer;
            IEnumerator enumerator = tf.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    this.SetChildInit(current, layer);
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
    }

    protected enum Status
    {
        NONE,
        FADEIN,
        WAIT,
        FADEOUT,
        WAIT2,
        CANCEL
    }
}

