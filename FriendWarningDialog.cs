using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FriendWarningDialog : BaseDialog
{
    [SerializeField]
    protected UIButton closeButton;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel closeLabel;
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

    public void Open(Kind kind, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            switch (kind)
            {
                case Kind.MAX_FRIEND:
                {
                    UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getUserIdEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
                    OtherUserGameEntity[] list = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TblFriendMaser>(DataNameKind.Kind.TBL_FRIEND).GetList(FriendStatus.Kind.FRIEND);
                    this.messageLabel.text = string.Format(LocalizationManager.Get("FRIEND_MAX_FRIEND_MESSAGE"), list.Length, entity.friendKeep);
                    this.closeLabel.text = LocalizationManager.Get("FRIEND_MAX_FRIEND_CLOSE");
                    break;
                }
                case Kind.NO_SEARCH:
                    this.messageLabel.text = LocalizationManager.Get("FRIEND_NO_SEARCH_MESSAGE");
                    this.closeLabel.text = LocalizationManager.Get("FRIEND_NO_SEARCH_CLOSE");
                    break;

                case Kind.NO_STRING:
                    this.messageLabel.text = LocalizationManager.Get("FRIEND_NO_STRING_MESSAGE");
                    this.closeLabel.text = LocalizationManager.Get("FRIEND_NO_STRING_CLOSE");
                    break;

                case Kind.NO_OFFER:
                    this.messageLabel.text = LocalizationManager.Get("FRIEND_NO_OFFER_MESSAGE");
                    this.closeLabel.text = LocalizationManager.Get("FRIEND_NO_OFFER_CLOSE");
                    break;

                case Kind.NO_OFFERED:
                    this.messageLabel.text = LocalizationManager.Get("FRIEND_NO_OFFERED_MESSAGE");
                    this.closeLabel.text = LocalizationManager.Get("FRIEND_NO_OFFERED_CLOSE");
                    break;

                case Kind.NO_FRIEND:
                    this.messageLabel.text = LocalizationManager.Get("FRIEND_NO_FRIEND_MESSAGE");
                    this.closeLabel.text = LocalizationManager.Get("FRIEND_NO_FRIEND_CLOSE");
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
        MAX_FRIEND,
        NO_SEARCH,
        NO_STRING,
        NO_OFFER,
        NO_OFFERED,
        NO_FRIEND
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

