using System;
using UnityEngine;

public class StandFigureBack : BaseMonoBehaviour
{
    protected System.Action baseCallbackFunc;
    [SerializeField]
    protected UIPanel basePanel;
    protected Vector3 basePosition;
    [SerializeField]
    protected GameObject baseStandFigure;
    [SerializeField]
    protected GameObject baseWindow;
    protected static readonly float CLOSE_TIME = 0.3f;
    [SerializeField]
    protected Transform closeTransform;
    private bool isBasePosition;
    protected bool isDisp;
    protected bool isDispRequest;
    protected bool isLoad;
    protected static readonly float OPEN_TIME = 0.3f;
    protected UIStandFigureR standFigure;

    protected void EndClose()
    {
        base.gameObject.SetActive(false);
        if (this.baseCallbackFunc != null)
        {
            System.Action baseCallbackFunc = this.baseCallbackFunc;
            this.baseCallbackFunc = null;
            baseCallbackFunc();
        }
    }

    protected void EndFadein()
    {
        if (this.baseCallbackFunc != null)
        {
            System.Action baseCallbackFunc = this.baseCallbackFunc;
            this.baseCallbackFunc = null;
            baseCallbackFunc();
        }
    }

    protected void EndFadeout()
    {
        if (this.baseCallbackFunc != null)
        {
            System.Action baseCallbackFunc = this.baseCallbackFunc;
            this.baseCallbackFunc = null;
            baseCallbackFunc();
        }
    }

    protected void EndLoad()
    {
        if (this.isLoad)
        {
            this.isLoad = false;
        }
        if (this.isDispRequest)
        {
            this.Fadein(this.baseCallbackFunc);
        }
    }

    public void Fadein(System.Action callback)
    {
        this.baseCallbackFunc = callback;
        this.isDispRequest = true;
        if (!this.isLoad)
        {
            if (!this.isDisp)
            {
                this.isDisp = true;
                base.gameObject.SetActive(true);
                if ((this.baseWindow != null) && (this.closeTransform != null))
                {
                    TweenPosition position = TweenPosition.Begin(this.baseWindow, OPEN_TIME, this.basePosition);
                    if (position != null)
                    {
                        position.method = UITweener.Method.EaseInOut;
                        position.eventReceiver = base.gameObject;
                        position.callWhenFinished = "EndFadein";
                        return;
                    }
                    this.baseWindow.transform.localPosition = this.basePosition;
                }
            }
            this.EndFadein();
        }
    }

    public void Fadeout(System.Action callback)
    {
        this.baseCallbackFunc = callback;
        this.isDispRequest = false;
        if (this.isDisp)
        {
            this.isDisp = false;
            base.gameObject.SetActive(true);
            if ((this.baseWindow != null) && (this.closeTransform != null))
            {
                Vector3 pos = this.baseWindow.transform.parent.InverseTransformPoint(this.closeTransform.position);
                TweenPosition position = TweenPosition.Begin(this.baseWindow, OPEN_TIME, pos);
                if (position != null)
                {
                    position.method = UITweener.Method.EaseInOut;
                    position.eventReceiver = base.gameObject;
                    position.callWhenFinished = "EndFadeout";
                    return;
                }
                this.baseWindow.transform.localPosition = this.basePosition;
            }
        }
        this.EndClose();
    }

    public UIStandFigureR getSvtStandFigure() => 
        this.standFigure;

    public void Init()
    {
        if ((this.baseWindow != null) && !this.isBasePosition)
        {
            this.isBasePosition = true;
            this.basePosition = this.baseWindow.transform.localPosition;
        }
        if (this.standFigure != null)
        {
            this.standFigure.Destroy();
            this.standFigure = null;
        }
        base.gameObject.SetActive(false);
        this.isLoad = false;
        this.isDisp = false;
        this.isDispRequest = false;
        if ((this.baseWindow != null) && (this.closeTransform != null))
        {
            this.baseWindow.transform.position = this.closeTransform.position;
        }
    }

    public void Set(Face.Type faceType)
    {
        if (this.standFigure != null)
        {
            this.standFigure.SetFace(faceType);
        }
    }

    public void Set(int svtId, int imageLimitCount, Face.Type faceType, System.Action callback)
    {
        base.gameObject.SetActive(true);
        this.baseCallbackFunc = callback;
        this.isDispRequest = true;
        if ((this.baseWindow != null) && !this.isBasePosition)
        {
            this.isBasePosition = true;
            this.basePosition = this.baseWindow.transform.localPosition;
            if (this.closeTransform != null)
            {
                this.baseWindow.transform.position = this.closeTransform.position;
            }
        }
        if (this.standFigure == null)
        {
            GameObject parent = (this.baseStandFigure == null) ? this.baseWindow : this.baseStandFigure;
            this.standFigure = StandFigureManager.CreateRenderPrefab(parent);
            this.isLoad = true;
        }
        this.standFigure.SetCharacter(svtId, imageLimitCount, faceType, new System.Action(this.EndLoad));
    }

    public void Set(int svtId, int limitCount, int lv, Face.Type faceType, System.Action callback)
    {
        base.gameObject.SetActive(true);
        this.baseCallbackFunc = callback;
        this.isDispRequest = true;
        if ((this.baseWindow != null) && !this.isBasePosition)
        {
            this.isBasePosition = true;
            this.basePosition = this.baseWindow.transform.localPosition;
            if (this.closeTransform != null)
            {
                this.baseWindow.transform.position = this.closeTransform.position;
            }
        }
        if (this.standFigure == null)
        {
            GameObject parent = (this.baseStandFigure == null) ? this.baseWindow : this.baseStandFigure;
            this.standFigure = StandFigureManager.CreateRenderPrefab(parent, svtId, limitCount, lv, Face.Type.NORMAL, 10, new System.Action(this.EndLoad));
            this.isLoad = true;
        }
        else
        {
            int imageLimitCount = ImageLimitCount.GetImageLimitCount(svtId, limitCount);
            this.standFigure.SetCharacter(svtId, imageLimitCount, faceType, this.baseCallbackFunc);
        }
    }

    public bool IsBusy =>
        base.gameObject.activeSelf;

    public bool IsDisp =>
        this.isDispRequest;
}

