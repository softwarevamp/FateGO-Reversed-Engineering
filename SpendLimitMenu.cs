using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpendLimitMenu : BaseDialog
{
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel closeLabel;
    [SerializeField]
    protected UILabel messageLabel;
    protected State state;
    [SerializeField]
    protected UILabel titleLabel;
    protected static string[] typeTextList = new string[] { "AGE_VEIFICATION_NONE", "AGE_VEIFICATION_TYPE1", "AGE_VEIFICATION_TYPE2", "AGE_VEIFICATION_TYPE3" };

    protected event CallbackFunc callbackFunc;

    protected void Callback()
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc();
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
        this.closeLabel.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

    public bool IsLimitOver(int value)
    {
        if (ManagerConfig.UseMock)
        {
            return true;
        }
        switch (AgeVerificationMenu.GetAgeType())
        {
            case 0:
                return true;

            case 1:
                return (value > 0x1388);

            case 2:
                return (value > 0x7530);
        }
        return false;
    }

    public void OnClickClose()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback();
        }
    }

    public void Open(int value, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            int ageType = AgeVerificationMenu.GetAgeType();
            this.titleLabel.text = LocalizationManager.Get("SPEND_LIMIT_TITLE");
            this.messageLabel.text = string.Format(LocalizationManager.Get("SPEND_LIMIT_MESSAGE"), LocalizationManager.Get(typeTextList[ageType]), LocalizationManager.GetPriceInfo(value));
            this.closeLabel.text = LocalizationManager.Get("SPEND_LIMIT_CLOSE");
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
        else if (this.state == State.SELECTED)
        {
            this.callbackFunc = callback;
            this.state = State.INIT;
        }
    }

    public delegate void CallbackFunc();

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

