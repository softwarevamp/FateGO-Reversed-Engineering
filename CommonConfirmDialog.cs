using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CommonConfirmDialog : BaseDialog
{
    [SerializeField]
    protected UILabel buttonCancelLabel;
    [SerializeField]
    protected UILabel buttonDecideLabel;
    protected ClickDelegate clickFunc;
    protected System.Action closeCallbackFunc;
    protected bool isButtonEnable;
    protected bool isDecideBtnSe;
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
        this.closeCallbackFunc = callback;
        this.isButtonEnable = false;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        this.Init();
        base.gameObject.SetActive(false);
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndOpen()
    {
        this.isButtonEnable = true;
    }

    public void Init()
    {
        if (this.titleLabel != null)
        {
            this.titleLabel.text = string.Empty;
        }
        if (this.messageLabel != null)
        {
            this.messageLabel.text = string.Empty;
        }
        if (this.buttonDecideLabel != null)
        {
            this.buttonDecideLabel.text = string.Empty;
        }
        if (this.buttonCancelLabel != null)
        {
            this.buttonCancelLabel.text = string.Empty;
        }
        this.isButtonEnable = false;
        this.isDecideBtnSe = false;
        base.gameObject.SetActive(false);
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.isButtonEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.isButtonEnable = false;
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
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.isButtonEnable = false;
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
            if (this.isDecideBtnSe)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            }
            this.isButtonEnable = false;
            if (this.clickFunc != null)
            {
                this.clickFunc(true);
            }
        }
    }

    public void OnClickDecide2()
    {
        if (this.isButtonEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.isButtonEnable = false;
            if (this.clickFunc != null)
            {
                this.clickFunc(true);
            }
        }
    }

    public void Open(string title, string message, ClickDelegate func)
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
        if (this.buttonDecideLabel != null)
        {
            this.buttonDecideLabel.text = LocalizationManager.Get("COMMON_CONFIRM_YES");
        }
        if (this.buttonCancelLabel != null)
        {
            this.buttonCancelLabel.text = LocalizationManager.Get("COMMON_CONFIRM_NO");
        }
        this.isButtonEnable = false;
        this.isDecideBtnSe = false;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public void Open(string title, string message, string decideTxt, string cancleTxt, ClickDelegate func)
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
        if (this.buttonDecideLabel != null)
        {
            this.buttonDecideLabel.text = decideTxt;
        }
        if (this.buttonCancelLabel != null)
        {
            this.buttonCancelLabel.text = cancleTxt;
        }
        this.isButtonEnable = false;
        this.isDecideBtnSe = false;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public void OpenDecideDlg(string title, string message, string decideTxt, string cancleTxt, ClickDelegate func)
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
        if (this.buttonDecideLabel != null)
        {
            this.buttonDecideLabel.text = decideTxt;
        }
        if (this.buttonCancelLabel != null)
        {
            this.buttonCancelLabel.text = cancleTxt;
        }
        this.isButtonEnable = false;
        this.isDecideBtnSe = true;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public UILabel ButtonCancelLabel =>
        this.buttonCancelLabel;

    public UILabel ButtonDecideLabel =>
        this.buttonDecideLabel;

    public delegate void ClickDelegate(bool isDecide);
}

