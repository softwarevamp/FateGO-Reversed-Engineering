using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TermsOfUseMenu : BaseDialog
{
    [SerializeField]
    protected UILabel cancelLabel;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel decideLabel;
    [SerializeField]
    protected UILabel messageLabel;
    protected static readonly string SAVE_DEFAULT_STR = "none";
    protected static readonly string SAVE_KEY = "UsePolicyConsent";
    [SerializeField]
    protected UILabel showLabel;
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
        this.decideLabel.text = string.Empty;
        this.cancelLabel.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

    public bool IsConcent()
    {
        if (ManagerConfig.UseMock)
        {
            return true;
        }
        string usePolicyVersion = BalanceConfig.UsePolicyVersion;
        return ((usePolicyVersion == null) || (PlayerPrefs.GetString(SAVE_KEY, SAVE_DEFAULT_STR) != usePolicyVersion));
    }

    public bool IsConcentFirst() => 
        (ManagerConfig.UseMock || (PlayerPrefs.GetString(SAVE_KEY, SAVE_DEFAULT_STR) == SAVE_DEFAULT_STR));

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(false);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            Save();
            this.Callback(true);
        }
    }

    public void OnClickShow()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            string title = LocalizationManager.Get("WEB_VIEW_TITLE_TERMS_OF_USE");
            string path = string.Empty;
            WebViewManager.OpenView(title, path, new System.Action(this.OnEndShowWebView));
            NoticeInfoComponent.TermID = "2";
        }
    }

    protected void OnEndShowWebView()
    {
    }

    public void Open(CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            this.titleLabel.text = LocalizationManager.Get("TERMS_OF_USE_TITLE");
            this.messageLabel.text = LocalizationManager.Get(!this.IsConcentFirst() ? "TERMS_OF_USE_MESSAGE2" : "TERMS_OF_USE_MESSAGE1");
            this.showLabel.text = LocalizationManager.Get("TERMS_OF_USE_SHOW");
            this.cancelLabel.text = LocalizationManager.Get("TERMS_OF_USE_CANCEL");
            this.decideLabel.text = LocalizationManager.Get("TERMS_OF_USE_DECIDE");
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public static void Save()
    {
        string usePolicyVersion = BalanceConfig.UsePolicyVersion;
        PlayerPrefs.SetString(SAVE_KEY, usePolicyVersion);
        PlayerPrefs.Save();
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

