using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantEquipFramePurchaseConfirmMenu : BaseDialog
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
    protected UILabel infoLabel1;
    [SerializeField]
    protected UILabel infoLabel2;
    [SerializeField]
    protected UILabel infoLabel3;
    protected Kind kind;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected UILabel numberLabel1;
    [SerializeField]
    protected UILabel numberLabel2;
    [SerializeField]
    protected UILabel numberLabel3;
    [SerializeField]
    protected UISprite spritIconSprite;
    protected State state;
    [SerializeField]
    protected UILabel stoneDataLabel;
    protected StoneShopEntity stoneShopEntity;
    [SerializeField]
    protected UILabel titleLabel;
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
        this.messageLabel.text = string.Empty;
        this.infoLabel1.text = string.Empty;
        this.infoLabel2.text = string.Empty;
        this.infoLabel3.text = string.Empty;
        this.numberLabel1.text = string.Empty;
        this.numberLabel2.text = string.Empty;
        this.numberLabel3.text = string.Empty;
        this.messageLabel.text = string.Empty;
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
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(false);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            if (this.kind == Kind.FRAME)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            }
            this.Callback(true);
        }
    }

    public void Open(Kind kind, StoneShopEntity stoneShopEntity, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.kind = kind;
            this.stoneShopEntity = stoneShopEntity;
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            this.titleLabel.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_TITLE");
            this.stoneDataLabel.text = LocalizationManager.GetNumberFormat(this.userGameEntity.stone);
            switch (kind)
            {
                case Kind.FRAME:
                case Kind.STONE:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_MESSAGE_START"), BalanceConfig.ServantEquipFrameMax);
                    this.infoLabel1.gameObject.SetActive(true);
                    this.infoLabel2.gameObject.SetActive(true);
                    this.infoLabel3.gameObject.SetActive(false);
                    this.infoLabel1.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_INFO1");
                    this.infoLabel2.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_INFO2");
                    this.numberLabel1.gameObject.SetActive(true);
                    this.numberLabel2.gameObject.SetActive(true);
                    this.numberLabel3.gameObject.SetActive(false);
                    this.numberLabel1.text = string.Format(LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_NUMBERO1"), this.userGameEntity.svtEquipKeep);
                    this.numberLabel2.text = string.Format(LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_NUMBERO2"), this.userGameEntity.svtEquipKeep + BalanceConfig.ServantEquipFrameUseStone);
                    this.spritIconSprite.gameObject.SetActive(true);
                    this.decideButton.gameObject.SetActive(true);
                    this.cancelButton.gameObject.SetActive(true);
                    this.closeButton.gameObject.SetActive(false);
                    this.decideButton.enabled = true;
                    this.cancelButton.enabled = true;
                    this.closeButton.enabled = false;
                    break;

                case Kind.PURCHASE:
                case Kind.MAX_FRAME:
                case Kind.NO_INFO:
                    this.infoLabel1.gameObject.SetActive(false);
                    this.infoLabel2.gameObject.SetActive(false);
                    this.infoLabel3.gameObject.SetActive(true);
                    this.infoLabel3.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_INFO1");
                    this.numberLabel1.gameObject.SetActive(false);
                    this.numberLabel2.gameObject.SetActive(false);
                    this.numberLabel3.gameObject.SetActive(true);
                    this.numberLabel3.text = string.Format(LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_NUMBERO2"), this.userGameEntity.svtEquipKeep);
                    this.spritIconSprite.gameObject.SetActive(false);
                    this.closeLabel.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_CLOSE");
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
                case Kind.FRAME:
                    this.warningLabel.text = string.Empty;
                    this.decideLabel.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_CANCEL");
                    break;

                case Kind.STONE:
                    this.warningLabel.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_REQUEST_STONE_MESSAGE");
                    this.decideLabel.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_STONE");
                    this.cancelLabel.text = LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_CANCEL");
                    this.infoLabel1.gameObject.SetActive(false);
                    this.infoLabel2.gameObject.SetActive(false);
                    this.infoLabel3.gameObject.SetActive(false);
                    this.numberLabel1.gameObject.SetActive(false);
                    this.numberLabel2.gameObject.SetActive(false);
                    this.numberLabel3.gameObject.SetActive(false);
                    this.spritIconSprite.gameObject.SetActive(false);
                    break;

                case Kind.PURCHASE:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_MESSAGE_END"), BalanceConfig.ServantEquipFrameMax);
                    break;

                case Kind.MAX_FRAME:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_MESSAGE_MAX_FRAME"), BalanceConfig.ServantEquipFrameMax);
                    break;

                case Kind.NO_INFO:
                    this.messageLabel.text = string.Format(LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE_MESSAGE_INFO_NONE"), BalanceConfig.ServantEquipFrameMax);
                    this.infoLabel3.gameObject.SetActive(false);
                    this.numberLabel3.gameObject.SetActive(false);
                    break;
            }
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public delegate void CallbackFunc(bool result);

    public enum Kind
    {
        FRAME,
        STONE,
        PURCHASE,
        MAX_FRAME,
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

