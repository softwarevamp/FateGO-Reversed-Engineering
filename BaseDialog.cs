using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BaseDialog : BaseMonoBehaviour
{
    protected System.Action baseCallbackFunc;
    [SerializeField]
    protected UIPanel basePanel;
    [SerializeField]
    protected UIPanel[] basePanelList;
    [SerializeField]
    protected GameObject baseWindow;
    protected static readonly float CLOSE_TIME = 0.2666667f;
    protected static readonly float DIALOG_CLOSE_SCALE = 0.95f;
    protected static readonly float DIALOG_INITIAL_SCALE = 0.9f;
    [SerializeField]
    protected UISprite maskSprite;
    protected static readonly float OPEN_TIME = 0.2666667f;

    public void Close(System.Action callback)
    {
        this.baseCallbackFunc = callback;
        bool flag = true;
        if (this.baseWindow != null)
        {
            if ((this.basePanelList != null) && (this.basePanelList.Length > 0))
            {
                for (int i = 0; i < this.basePanelList.Length; i++)
                {
                    UIPanel panel = this.basePanelList[i];
                    if (panel != null)
                    {
                        TweenAlpha.Begin(panel.gameObject, CLOSE_TIME, 0f).method = UITweener.Method.EaseOutQuad;
                    }
                }
            }
            else
            {
                UIPanel targetPanel = this.TargetPanel;
                if (targetPanel != null)
                {
                    TweenAlpha.Begin(targetPanel.gameObject, CLOSE_TIME, 0f).method = UITweener.Method.EaseOutQuad;
                }
            }
            TweenScale scale = TweenScale.Begin(this.baseWindow, CLOSE_TIME, new Vector3(DIALOG_CLOSE_SCALE, DIALOG_CLOSE_SCALE, DIALOG_CLOSE_SCALE));
            if (scale != null)
            {
                scale.method = UITweener.Method.EaseOutQuad;
                scale.eventReceiver = base.gameObject;
                scale.callWhenFinished = "EndCloseBaseDialog";
                flag = false;
            }
            else
            {
                this.baseWindow.transform.localScale = Vector3.zero;
            }
        }
        if (flag)
        {
            base.Invoke("EndCloseBaseDialog", 0.1f);
        }
    }

    protected void EndCloseBaseDialog()
    {
        this.Init();
        if (this.baseCallbackFunc != null)
        {
            System.Action baseCallbackFunc = this.baseCallbackFunc;
            this.baseCallbackFunc = null;
            baseCallbackFunc();
        }
    }

    protected void EndOpenBaseDialog()
    {
        if (this.baseCallbackFunc != null)
        {
            System.Action baseCallbackFunc = this.baseCallbackFunc;
            this.baseCallbackFunc = null;
            baseCallbackFunc();
        }
    }

    public void Init()
    {
        base.gameObject.SetActive(false);
    }

    public void Open(System.Action callback, bool forceMaskClear = true)
    {
        base.gameObject.SetActive(true);
        this.baseCallbackFunc = callback;
        bool flag = true;
        if ((this.maskSprite != null) && forceMaskClear)
        {
            this.maskSprite.spriteName = "clear00";
        }
        if (this.baseWindow != null)
        {
            if ((this.basePanelList != null) && (this.basePanelList.Length > 0))
            {
                for (int i = 0; i < this.basePanelList.Length; i++)
                {
                    UIPanel panel = this.basePanelList[i];
                    if (panel != null)
                    {
                        panel.alpha = 0f;
                        TweenAlpha.Begin(panel.gameObject, OPEN_TIME, 1f).method = UITweener.Method.EaseOutQuad;
                    }
                }
            }
            else
            {
                UIPanel targetPanel = this.TargetPanel;
                if (targetPanel != null)
                {
                    targetPanel.alpha = 0f;
                    TweenAlpha.Begin(targetPanel.gameObject, OPEN_TIME, 1f).method = UITweener.Method.EaseOutQuad;
                }
            }
            this.baseWindow.transform.localScale = new Vector3(DIALOG_INITIAL_SCALE, DIALOG_INITIAL_SCALE, DIALOG_INITIAL_SCALE);
            TweenScale scale = TweenScale.Begin(this.baseWindow, OPEN_TIME, Vector3.one);
            if (scale != null)
            {
                this.baseWindow.transform.localScale = new Vector3(DIALOG_INITIAL_SCALE, DIALOG_INITIAL_SCALE, DIALOG_INITIAL_SCALE);
                scale.method = UITweener.Method.EaseOutQuad;
                scale.eventReceiver = base.gameObject;
                scale.callWhenFinished = "EndOpenBaseDialog";
                flag = false;
            }
            else
            {
                this.baseWindow.transform.localScale = Vector3.one;
            }
        }
        if (flag)
        {
            this.EndOpenBaseDialog();
        }
    }

    public void SetMask(bool forceMaskClear = true)
    {
        base.gameObject.SetActive(true);
        if ((this.maskSprite != null) && forceMaskClear)
        {
            this.maskSprite.spriteName = "clear00";
        }
        if ((this.basePanelList != null) && (this.basePanelList.Length > 0))
        {
            for (int i = 0; i < this.basePanelList.Length; i++)
            {
                UIPanel panel = this.basePanelList[i];
                if (panel != null)
                {
                    panel.alpha = 0.005f;
                }
            }
        }
        else
        {
            UIPanel targetPanel = this.TargetPanel;
            if (targetPanel != null)
            {
                targetPanel.alpha = 0.005f;
            }
        }
        if (this.baseWindow != null)
        {
            this.baseWindow.transform.localScale = Vector3.zero;
        }
    }

    public bool IsBusy =>
        base.gameObject.activeSelf;

    protected UIPanel TargetPanel
    {
        get
        {
            UIPanel basePanel = this.basePanel;
            if (basePanel == null)
            {
                basePanel = base.gameObject.GetComponentInChildren<UIPanel>();
            }
            if (basePanel == null)
            {
                basePanel = base.gameObject.transform.parent.gameObject.GetComponentInChildren<UIPanel>();
            }
            return basePanel;
        }
    }
}

