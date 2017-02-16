using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScriptSkipDialog : BaseDialog
{
    [SerializeField]
    protected UILabel buttonCancel2Label;
    [SerializeField]
    protected UILabel buttonCancelLabel;
    [SerializeField]
    protected UILabel buttonDecide2Label;
    [SerializeField]
    protected UILabel buttonDecideLabel;
    [SerializeField]
    protected UILabel buttonExitLabel;
    protected ClickDelegate clickFunc;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected GameObject exitBase;
    protected bool isButtonEnable;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected GameObject normalBase;

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
        this.messageLabel.text = string.Empty;
        this.buttonDecideLabel.text = string.Empty;
        this.buttonDecide2Label.text = string.Empty;
        this.buttonCancelLabel.text = string.Empty;
        this.buttonCancel2Label.text = string.Empty;
        this.buttonExitLabel.text = string.Empty;
        this.isButtonEnable = false;
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
                this.clickFunc(ResultKind.CANCEL);
            }
        }
    }

    public void OnClickDecide()
    {
        if (this.isButtonEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.isButtonEnable = false;
            if (this.clickFunc != null)
            {
                this.clickFunc(ResultKind.SKIP);
            }
        }
    }

    public void OnClickExit()
    {
        if (this.isButtonEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.isButtonEnable = false;
            if (this.clickFunc != null)
            {
                this.clickFunc(ResultKind.EXIT);
            }
        }
    }

    public void Open(bool isUseExit, ClickDelegate func)
    {
        this.clickFunc = func;
        this.messageLabel.text = LocalizationManager.Get("SCRIPT_ACTION_SKIP_CONFIRM_DETAIL");
        this.normalBase.SetActive(!isUseExit);
        this.exitBase.SetActive(isUseExit);
        if (isUseExit)
        {
            this.buttonDecide2Label.text = LocalizationManager.Get("COMMON_CONFIRM_YES");
            this.buttonCancel2Label.text = LocalizationManager.Get("COMMON_CONFIRM_NO");
            this.buttonExitLabel.text = LocalizationManager.Get("SCRIPT_ACTION_SKIP_CONFIRM_EXIT");
        }
        else
        {
            this.buttonDecideLabel.text = LocalizationManager.Get("COMMON_CONFIRM_YES");
            this.buttonCancelLabel.text = LocalizationManager.Get("COMMON_CONFIRM_NO");
        }
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public delegate void ClickDelegate(ScriptSkipDialog.ResultKind result);

    public enum ResultKind
    {
        CANCEL,
        SKIP,
        EXIT
    }
}

