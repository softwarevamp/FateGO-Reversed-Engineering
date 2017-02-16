using System;
using UnityEngine;

public class StonePurchaseNotificationMenu : BaseDialog
{
    protected System.Action callbackFunc;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel closeLabel;
    [SerializeField]
    protected UILabel infoLabel;
    [SerializeField]
    protected UILabel messageLabel;
    protected State state;
    [SerializeField]
    protected UILabel stoneDataLabel;
    protected UserGameEntity userGameEntity;

    protected void Callback()
    {
        System.Action callbackFunc = this.callbackFunc;
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
        this.stoneDataLabel.text = string.Empty;
        this.messageLabel.text = string.Empty;
        this.infoLabel.text = string.Empty;
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
            this.Callback();
        }
    }

    public void Open(Kind kind, System.Action callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.stoneDataLabel.text = string.Empty;
            try
            {
                this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                return;
            }
            base.gameObject.SetActive(true);
            this.stoneDataLabel.text = LocalizationManager.GetNumberFormat(this.userGameEntity.stone);
            this.infoLabel.text = string.Empty;
            this.closeLabel.text = LocalizationManager.Get("STONE_PURCHASE_RESULT_CLOSE");
            switch (kind)
            {
                case Kind.CANCEL:
                    this.messageLabel.text = LocalizationManager.Get("STONE_PURCHASE_RESULT_CANCEL");
                    break;

                case Kind.SUCCESS:
                    this.messageLabel.text = LocalizationManager.Get("STONE_PURCHASE_RESULT_SUCCESS");
                    break;

                case Kind.WAIT:
                    this.messageLabel.text = LocalizationManager.Get("STONE_PURCHASE_RESULT_WAIT");
                    break;

                case Kind.FAIL:
                    this.messageLabel.text = LocalizationManager.Get("STONE_PURCHASE_RESULT_FAIL");
                    break;

                case Kind.SUSPEND:
                    this.messageLabel.text = LocalizationManager.Get("STONE_PURCHASE_RESULT_SUSPEND");
                    break;
            }
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public enum Kind
    {
        CANCEL,
        SUCCESS,
        WAIT,
        FAIL,
        SUSPEND
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

