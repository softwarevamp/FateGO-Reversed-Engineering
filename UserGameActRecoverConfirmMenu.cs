using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserGameActRecoverConfirmMenu : BaseDialog
{
    [SerializeField]
    protected UIButton cancelButton;
    [SerializeField]
    protected UILabel cancelLabel;
    [SerializeField]
    protected UIButton closeButton;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel closeLabel;
    [SerializeField]
    protected UIButton decideButton;
    [SerializeField]
    protected UILabel decideLabel;
    [SerializeField]
    protected UILabel infoLabel;
    [SerializeField]
    protected UILabel messageLabel;
    protected State state;
    [SerializeField]
    protected UILabel stoneDataLabel;
    protected StoneShopEntity stoneShopEntity;
    [SerializeField]
    protected UILabel stoneTitleLabel;
    protected UserGameEntity userGameEntity;
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
        this.stoneTitleLabel.text = string.Empty;
        this.stoneDataLabel.text = string.Empty;
        this.messageLabel.text = string.Empty;
        this.infoLabel.text = string.Empty;
        this.warningLabel.text = string.Empty;
        this.decideLabel.text = string.Empty;
        this.cancelLabel.text = string.Empty;
        this.closeLabel.text = string.Empty;
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

    public void OnClickClose()
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
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(true);
        }
    }

    public void Open(Kind kind, StoneShopEntity stoneShopEntity, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.stoneShopEntity = stoneShopEntity;
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            this.stoneTitleLabel.text = LocalizationManager.Get("STONE_TAKE");
            this.stoneDataLabel.text = string.Format(LocalizationManager.Get("STONE_UNIT"), this.userGameEntity.stone);
            string str = string.Format(LocalizationManager.Get("USER_GAME_ACT_RECOVER_INFO"), this.userGameEntity.getAct(), this.userGameEntity.actMax);
            switch (kind)
            {
                case Kind.USER_GAME_ACT:
                case Kind.STONE:
                    this.messageLabel.text = LocalizationManager.Get("USER_GAME_ACT_RECOVER_MESSAGE_START");
                    this.decideButton.gameObject.SetActive(true);
                    this.cancelButton.gameObject.SetActive(true);
                    this.closeButton.gameObject.SetActive(false);
                    this.decideButton.enabled = true;
                    this.cancelButton.enabled = true;
                    this.closeButton.enabled = false;
                    break;

                case Kind.RECOVER:
                case Kind.MAX_ACT:
                case Kind.NO_INFO:
                    this.closeLabel.text = LocalizationManager.Get("USER_GAME_ACT_RECOVER_CLOSE");
                    this.warningLabel.text = string.Empty;
                    this.decideButton.gameObject.SetActive(false);
                    this.cancelButton.gameObject.SetActive(false);
                    this.closeButton.gameObject.SetActive(true);
                    this.decideButton.enabled = false;
                    this.cancelButton.enabled = false;
                    this.closeButton.enabled = true;
                    break;
            }
            switch (kind)
            {
                case Kind.USER_GAME_ACT:
                {
                    this.warningLabel.text = string.Empty;
                    this.decideLabel.text = LocalizationManager.Get("USER_GAME_ACT_RECOVER_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("USER_GAME_ACT_RECOVER_CANCEL");
                    string str2 = string.Format(LocalizationManager.Get("USER_GAME_ACT_RECOVER_INFO"), this.userGameEntity.actMax, this.userGameEntity.actMax);
                    str = string.Format(LocalizationManager.Get("USER_GAME_ACT_RECOVER_INFO_BEFORE_AFTER"), str, str2);
                    break;
                }
                case Kind.STONE:
                    this.warningLabel.text = LocalizationManager.Get("USER_GAME_ACT_RECOVER_REQUEST_STONE_MESSAGE");
                    this.decideLabel.text = LocalizationManager.Get("USER_GAME_ACT_RECOVER_STONE");
                    this.cancelLabel.text = LocalizationManager.Get("USER_GAME_ACT_RECOVER_CANCEL");
                    break;

                case Kind.RECOVER:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("USER_GAME_ACT_RECOVER_MESSAGE_END"), BalanceConfig.ServantFrameMax);
                    break;

                case Kind.MAX_ACT:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("USER_GAME_ACT_RECOVER_MESSAGE_MAX_ACT"), BalanceConfig.ServantFrameMax);
                    break;

                case Kind.NO_INFO:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("USER_GAME_ACT_RECOVER_MESSAGE_INFO_NONE"), BalanceConfig.ServantFrameMax);
                    break;
            }
            this.infoLabel.text = str;
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public delegate void CallbackFunc(bool result);

    public enum Kind
    {
        USER_GAME_ACT,
        STONE,
        RECOVER,
        MAX_ACT,
        NO_INFO
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

