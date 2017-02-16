using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ContinueDeviceDispMenu : BaseMenu
{
    [SerializeField]
    protected UICommonButton cancelButton;
    [SerializeField]
    protected UILabel cancelLabel;
    protected System.Action closeCallbackFunc;
    protected string code;
    [SerializeField]
    protected UILabel continueCodeLabel;
    [SerializeField]
    protected UILabel copyLabel;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected UILabel decideLabel;
    [SerializeField]
    protected UILabel explanation2Label;
    protected State state;
    [SerializeField]
    protected UILabel titleLabel;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result);
        }
    }

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeCallbackFunc = callback;
        this.state = State.CLOSE;
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

    public void EndCopyDialog(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
    }

    public void Init()
    {
        this.titleLabel.text = string.Empty;
        this.decideLabel.text = string.Empty;
        this.cancelLabel.text = string.Empty;
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(false);
        }
    }

    public void OnClickCopy()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            CommonServicePluginScript.SetClipBoardText(this.code);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(null, LocalizationManager.Get("CONTINUE_DEVICE_DIALOG_MESSAGE1"), new NotificationDialog.ClickDelegate(this.EndCopyDialog), -1);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(true);
        }
    }

    public void Open(string code, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.code = code;
            this.callbackFunc = callback;
            this.titleLabel.text = LocalizationManager.Get("CONTINUE_DEVICE_DISP_TITLE");
            this.explanation2Label.text = LocalizationManager.Get("CONTINUE_DEVICE_DISP_EXPLANATIOIN2") + "\n" + LocalizationManager.Get("CONTINUE_DEVICE_WARNING_MESSAGE");
            this.copyLabel.text = LocalizationManager.Get("CONTINUE_DEVICE_DISP_COPY");
            this.decideLabel.text = LocalizationManager.Get("CONTINUE_DEVICE_DISP_DECIDE");
            this.cancelLabel.text = LocalizationManager.Get("CONTINUE_DEVICE_DISP_CANCEL");
            this.continueCodeLabel.text = this.code;
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if (this.state == State.SELECTED)
        {
            this.callbackFunc = callback;
            this.state = State.INPUT;
        }
    }

    public delegate void CallbackFunc(bool result);

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

