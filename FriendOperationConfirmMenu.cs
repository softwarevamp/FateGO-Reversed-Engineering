using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FriendOperationConfirmMenu : BaseDialog
{
    [SerializeField]
    protected UIButton cancelButton;
    [SerializeField]
    protected UILabel cancelLabel;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UIButton decideButton;
    [SerializeField]
    protected UILabel decideLabel;
    [SerializeField]
    protected UILabel messageLabel;
    protected State state;

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
        this.messageLabel.text = string.Empty;
        this.decideLabel.text = string.Empty;
        this.cancelLabel.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

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
            this.Callback(true);
        }
    }

    public void Open(Kind kind, OtherUserGameEntity entity, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            switch (kind)
            {
                case Kind.OFFER:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("FRIEND_OFFER_MESSAGE"), entity.userName);
                    this.decideLabel.text = LocalizationManager.Get("FRIEND_OFFER_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("FRIEND_OFFER_CANCEL");
                    break;

                case Kind.ACCEPT:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("FRIEND_ACCEPT_MESSAGE"), entity.userName);
                    this.decideLabel.text = LocalizationManager.Get("FRIEND_ACCEPT_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("FRIEND_ACCEPT_CANCEL");
                    break;

                case Kind.REJECT:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("FRIEND_REJECT_MESSAGE"), entity.userName);
                    this.decideLabel.text = LocalizationManager.Get("FRIEND_REJECT_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("FRIEND_REJECT_CANCEL");
                    break;

                case Kind.CANCEL:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("FRIEND_CANCEL_MESSAGE"), entity.userName);
                    this.decideLabel.text = LocalizationManager.Get("FRIEND_CANCEL_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("FRIEND_CANCEL_CANCEL");
                    break;

                case Kind.REMOVE:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("FRIEND_REMOVE_MESSAGE"), entity.userName);
                    this.decideLabel.text = LocalizationManager.Get("FRIEND_REMOVE_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("FRIEND_REMOVE_CANCEL");
                    break;
            }
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public delegate void CallbackFunc(bool result);

    public enum Kind
    {
        NONE,
        OFFER,
        ACCEPT,
        REJECT,
        CANCEL,
        REMOVE
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

