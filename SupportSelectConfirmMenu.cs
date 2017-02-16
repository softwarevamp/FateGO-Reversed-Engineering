using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SupportSelectConfirmMenu : BaseDialog
{
    [SerializeField]
    protected UICommonButton cancelButton;
    [SerializeField]
    protected UILabel cancelLabel;
    [SerializeField]
    protected UICommonButton closeButton;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel closeLabel;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected UILabel decideLabel;
    [SerializeField]
    protected UILabel messageLabel;
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
        this.closeLabel.text = string.Empty;
        this.cancelLabel.text = string.Empty;
        this.decideLabel.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

    public void OnCancelClose()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(false);
        }
    }

    public void OnClickClose()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(true);
        }
    }

    public void OnDecideClose()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(true);
        }
    }

    public void Open(Kind kind, CallbackFunc callback)
    {
        if ((this.state == State.INIT) || (this.state == State.CLOSE))
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            switch (kind)
            {
                case Kind.NO_SERVANT:
                    this.titleLabel.text = LocalizationManager.Get("SUPPORT_SELECT_NO_SERVANT_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("SUPPORT_SELECT_NO_SERVANT_WARNING");
                    this.closeButton.gameObject.SetActive(true);
                    this.cancelButton.gameObject.SetActive(false);
                    this.decideButton.gameObject.SetActive(false);
                    break;

                case Kind.SERVANT_CLEAR:
                    this.titleLabel.text = LocalizationManager.Get("SUPPORT_SELECT_SERVANT_CLEAR_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("SUPPORT_SELECT_SERVANT_CLEAR_WARNING");
                    this.closeButton.gameObject.SetActive(false);
                    this.cancelButton.gameObject.SetActive(true);
                    this.decideButton.gameObject.SetActive(true);
                    break;

                case Kind.EQUIP_CLEAR:
                    this.titleLabel.text = LocalizationManager.Get("SUPPORT_SELECT_EQUIP_CLEAR_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("SUPPORT_SELECT_EQUIP_CLEAR_WARNING");
                    this.closeButton.gameObject.SetActive(false);
                    this.cancelButton.gameObject.SetActive(true);
                    this.decideButton.gameObject.SetActive(true);
                    break;

                case Kind.ALL_CLEAR:
                    this.titleLabel.text = LocalizationManager.Get("SUPPORT_SELECT_ALL_CLEAR_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("SUPPORT_SELECT_ALL_CLEAR_WARNING");
                    this.closeButton.gameObject.SetActive(false);
                    this.cancelButton.gameObject.SetActive(true);
                    this.decideButton.gameObject.SetActive(true);
                    break;
            }
            this.closeLabel.text = LocalizationManager.Get("SUPPORT_SELECT_WARNING_DIALOG_CLOSE");
            this.cancelLabel.text = LocalizationManager.Get("SUPPORT_SELECT_WARNING_DIALOG_CANCEL");
            this.decideLabel.text = LocalizationManager.Get("SUPPORT_SELECT_WARNING_DIALOG_DECIDE");
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public delegate void CallbackFunc(bool result);

    public enum Kind
    {
        NONE,
        NO_SERVANT,
        SERVANT_CLEAR,
        EQUIP_CLEAR,
        ALL_CLEAR
    }

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

