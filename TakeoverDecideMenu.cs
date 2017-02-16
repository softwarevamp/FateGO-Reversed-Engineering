using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TakeoverDecideMenu : BaseDialog
{
    [SerializeField]
    protected UIButton closeButton;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel closeLabel;
    [SerializeField]
    protected UILabel info1Label;
    [SerializeField]
    protected UILabel info2Label;
    [SerializeField]
    protected UILabel messageLabel;
    protected State state;
    [SerializeField]
    protected UILabel titleLabel;
    [SerializeField]
    protected UILabel warningLabel;

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
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
    }

    public void Init()
    {
        this.titleLabel.text = string.Empty;
        this.messageLabel.text = string.Empty;
        this.info1Label.text = string.Empty;
        this.info2Label.text = string.Empty;
        this.warningLabel.text = string.Empty;
        this.closeLabel.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickClose()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(false);
        }
    }

    public void Open(string userName, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            this.titleLabel.text = string.Empty;
            this.closeButton.gameObject.SetActive(true);
            this.closeButton.enabled = true;
            this.closeLabel.text = LocalizationManager.Get("COMMON_CONFIRM_CLOSE");
            this.messageLabel.text = string.Format(LocalizationManager.Get("CONTINUE_DEVICE_TAKEOVER_DECIDE_MESSAGE"), userName);
            this.warningLabel.text = string.Empty;
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
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

