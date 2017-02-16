using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class NotificationDialog : BaseDialog
{
    protected ClickDelegate clickFunc;
    protected System.Action closeEndFunc;
    protected bool isButtonEnable;
    protected int keepPanelDepth;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected UILabel okBtnLabel;
    [SerializeField]
    protected UILabel titleLabel;

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeEndFunc = callback;
        this.isButtonEnable = false;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        UIPanel targetPanel = base.TargetPanel;
        if (targetPanel != null)
        {
            targetPanel.depth = this.keepPanelDepth;
        }
        this.Init();
        if (this.closeEndFunc != null)
        {
            System.Action closeEndFunc = this.closeEndFunc;
            this.closeEndFunc = null;
            closeEndFunc();
        }
    }

    protected void EndOpen()
    {
        this.isButtonEnable = true;
    }

    public void Init()
    {
        this.titleLabel.text = string.Empty;
        this.messageLabel.text = string.Empty;
        if (this.titleLabel != null)
        {
            this.titleLabel.text = string.Empty;
        }
        if (this.messageLabel != null)
        {
            this.messageLabel.text = string.Empty;
        }
        if (this.okBtnLabel != null)
        {
            this.okBtnLabel.text = string.Empty;
        }
        base.gameObject.SetActive(false);
        base.Init();
    }

    public void OnClickOk()
    {
        if (this.isButtonEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            if (this.clickFunc != null)
            {
                this.clickFunc(true);
            }
        }
    }

    public void Open(string title, string message, ClickDelegate func, int panel_depth = -1)
    {
        this.clickFunc = func;
        if (this.titleLabel != null)
        {
            this.titleLabel.text = (title == null) ? string.Empty : title;
        }
        if (this.messageLabel != null)
        {
            this.messageLabel.text = (message == null) ? string.Empty : message;
        }
        if (this.okBtnLabel != null)
        {
            this.okBtnLabel.text = LocalizationManager.Get("COMMON_CONFIRM_CLOSE");
        }
        UIPanel targetPanel = base.TargetPanel;
        if (targetPanel != null)
        {
            this.keepPanelDepth = targetPanel.depth;
            if (panel_depth >= 0)
            {
                targetPanel.depth = panel_depth;
            }
        }
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public delegate void ClickDelegate(bool isOk);
}

