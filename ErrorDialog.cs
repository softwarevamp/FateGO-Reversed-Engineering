using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ErrorDialog : BaseDialog
{
    [SerializeField]
    protected UILabel cancelLabel;
    [SerializeField]
    protected string cancelTextCode;
    protected ClickDelegate clickFunc;
    protected System.Action closeEndFunc;
    [SerializeField]
    protected UILabel decideLabel;
    [SerializeField]
    protected string decideTextCode;
    protected bool isButtonEnable;
    [SerializeField]
    protected UILabel messageLabel;
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
        this.Init();
        base.gameObject.SetActive(false);
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
        if (this.decideLabel != null)
        {
            this.decideLabel.text = string.Empty;
        }
        if (this.cancelLabel != null)
        {
            this.cancelLabel.text = string.Empty;
        }
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.isButtonEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            if (this.clickFunc != null)
            {
                this.clickFunc(false);
            }
        }
    }

    public void OnClickClose()
    {
        if (this.isButtonEnable)
        {
            if ((string.Compare(this.cancelLabel.text, "Exit") != 0) && ManagementManager.IsNetorking)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            }
            if (this.clickFunc != null)
            {
                this.clickFunc(false);
            }
        }
    }

    public void OnClickDecide()
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

    public void Open()
    {
        this.Open(null, null, null);
    }

    public void Open(string message)
    {
        this.Open(null, message, null);
    }

    public void Open(string title, string message)
    {
        this.Open(title, message, null);
    }

    public void Open(string title, string message, ClickDelegate func)
    {
        this.clickFunc = func;
        this.titleLabel.text = (title == null) ? string.Empty : title;
        this.messageLabel.text = (message == null) ? string.Empty : message;
        if ((this.decideLabel != null) && !string.IsNullOrEmpty(this.decideTextCode))
        {
            this.decideLabel.text = LocalizationManager.Get(this.decideTextCode);
        }
        if ((this.cancelLabel != null) && !string.IsNullOrEmpty(this.cancelTextCode))
        {
            if (string.Compare(LocalizationManager.Get(this.cancelTextCode), this.cancelTextCode) != 0)
            {
                this.cancelLabel.text = LocalizationManager.Get(this.cancelTextCode);
            }
            else
            {
                this.cancelLabel.text = "退出";
            }
        }
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public void Open(string title, string message, string decideTxt, string cancleTxt)
    {
        this.Open(title, message, decideTxt, cancleTxt, null);
    }

    public void Open(string title, string message, string decideTxt, string cancleTxt, ClickDelegate func)
    {
        this.clickFunc = func;
        this.titleLabel.text = (title == null) ? string.Empty : title;
        this.messageLabel.text = (message == null) ? string.Empty : message;
        if (this.decideLabel != null)
        {
            this.decideLabel.text = decideTxt;
        }
        if (this.cancelLabel != null)
        {
            this.cancelLabel.text = cancleTxt;
        }
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public delegate void ClickDelegate(bool isDecide);
}

