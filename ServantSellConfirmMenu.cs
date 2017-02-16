using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantSellConfirmMenu : BaseDialog
{
    private const int BuyCancel = 0;
    private const int BuyDecide = 1;
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
    protected UILabel info1Label;
    [SerializeField]
    protected UILabel info2Label;
    [SerializeField]
    protected UILabel info3Label;
    protected Kind kind;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected ServantOperationListViewManager servantOperationListViewManager;
    private long servantStatusId;
    protected State state;
    [SerializeField]
    protected UILabel titleLabel;
    protected UserGameEntity userGameEntity;
    [SerializeField]
    protected UILabel warningLabel;

    protected event CallbackFunc callbackFunc;

    protected void Callback(int result)
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

    public void DestroyList()
    {
        if (this.servantOperationListViewManager != null)
        {
            this.servantOperationListViewManager.DestroyList();
        }
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

    protected void EndCloseShowServant()
    {
        this.state = State.INPUT;
        this.servantOperationListViewManager.SetMode(ServantOperationListViewManager.InitMode.INPUT, new ServantOperationListViewManager.CallbackFunc(this.OnSelectServantDetail));
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
        switch (this.kind)
        {
            case Kind.SELET_SERVANT:
            case Kind.SELET_SERVANT_EQUIP:
            case Kind.SELET_SERVANT_EXCEEDED:
                this.servantOperationListViewManager.SetMode(ServantOperationListViewManager.InitMode.INPUT, new ServantOperationListViewManager.CallbackFunc(this.OnSelectServantDetail));
                break;
        }
    }

    protected void EndShowServant(bool isDecide)
    {
        long userSvtId = -1L;
        if (isDecide)
        {
            userSvtId = this.servantStatusId;
            this.ModifyItem(userSvtId);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    public void Init()
    {
        this.titleLabel.text = string.Empty;
        this.messageLabel.text = string.Empty;
        this.info1Label.text = string.Empty;
        this.info2Label.text = string.Empty;
        this.warningLabel.text = string.Empty;
        this.decideLabel.text = string.Empty;
        this.cancelLabel.text = string.Empty;
        this.closeLabel.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

    public void ModifyItem(long userSvtId)
    {
        this.servantOperationListViewManager.ModifyItem(userSvtId);
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(0);
        }
    }

    public void OnClickClose()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(0);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            if (((this.kind == Kind.SELET_SERVANT) || (this.kind == Kind.SELET_SERVANT_EQUIP)) || (this.kind == Kind.SELET_SERVANT_EXCEEDED))
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            }
            this.Callback(1);
        }
    }

    protected void OnSelectServant(long svtId)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
    }

    protected void OnSelectServantDetail(long svtId)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.servantStatusId = svtId;
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.NORMAL, this.servantStatusId, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
    }

    public void Open(Kind kind, long[] servantIdList, CallbackFunc callback)
    {
        int num3;
        if (this.state != State.INIT)
        {
            return;
        }
        Vector3 zero = Vector3.zero;
        this.kind = kind;
        this.callbackFunc = callback;
        base.gameObject.SetActive(true);
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.titleLabel.text = LocalizationManager.Get("SERVANT_SELL_TITLE");
        this.servantOperationListViewManager.gameObject.SetActive(false);
        switch (kind)
        {
            case Kind.SELET_SERVANT:
            case Kind.SELET_SERVANT_EQUIP:
            case Kind.SELET_SERVANT_EXCEEDED:
                this.decideButton.gameObject.SetActive(true);
                this.cancelButton.gameObject.SetActive(true);
                this.closeButton.gameObject.SetActive(false);
                this.decideButton.SetState(UICommonButtonColor.State.Normal, false);
                this.cancelButton.SetState(UICommonButtonColor.State.Normal, false);
                break;

            case Kind.SELL_SERVANT:
            case Kind.SELL_SERVANT_EQUIP:
            case Kind.SELL_ERROR_SERVANT:
            case Kind.SELL_ERROR_SERVANT_EQUIP:
            case Kind.NO_SELECT_SERVANT:
            case Kind.NO_SELECT_SERVANT_EQUIP:
                this.closeLabel.text = LocalizationManager.Get("SERVANT_SELL_CLOSE");
                this.warningLabel.text = string.Empty;
                this.decideButton.gameObject.SetActive(false);
                this.cancelButton.gameObject.SetActive(false);
                this.closeButton.gameObject.SetActive(true);
                this.closeButton.SetState(UICommonButtonColor.State.Normal, false);
                break;
        }
        switch (kind)
        {
            case Kind.SELET_SERVANT:
            case Kind.SELET_SERVANT_EQUIP:
            case Kind.SELET_SERVANT_EXCEEDED:
            {
                this.decideLabel.text = LocalizationManager.Get("SERVANT_SELL_DECIDE");
                this.cancelLabel.text = LocalizationManager.Get("SERVANT_SELL_CANCEL");
                UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
                int data = 0;
                int num2 = 0;
                num3 = 0;
                int num4 = 0;
                bool flag = false;
                foreach (long num5 in servantIdList)
                {
                    UserServantEntity entity = master.getEntityFromId<UserServantEntity>(num5);
                    ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity.svtId);
                    data += entity.getSellQp();
                    num2 += entity.getSellMana();
                    flag = entity.getRarity() >= 3;
                    if (entity2.IsKeepServantEquip)
                    {
                        num4 |= 2;
                        if (flag)
                        {
                            num3 |= 2;
                        }
                    }
                    else
                    {
                        num4 |= 1;
                        if (flag)
                        {
                            num3 |= 1;
                        }
                    }
                    if (kind == Kind.SELET_SERVANT_EXCEEDED)
                    {
                        num3 |= 4;
                    }
                }
                this.info1Label.text = string.Format(LocalizationManager.Get("SERVANT_SELL_INFO0"), LocalizationManager.GetNumberFormat(data));
                this.info2Label.text = string.Format(LocalizationManager.Get("SERVANT_SELL_INFO1"), LocalizationManager.GetNumberFormat(num2));
                switch (num4)
                {
                    case 1:
                        this.messageLabel.text = LocalizationManager.Get("SERVANT_SELL_MESSAGE_START");
                        goto Label_0317;

                    case 2:
                        this.messageLabel.text = LocalizationManager.Get("SERVANT_EQUIP_SELL_MESSAGE_START");
                        goto Label_0317;

                    case 3:
                        this.messageLabel.text = LocalizationManager.Get("SERVANT_ALL_SELL_MESSAGE_START");
                        goto Label_0317;
                }
                break;
            }
            case Kind.SELL_SERVANT:
            case Kind.SELL_SERVANT_EQUIP:
                this.messageLabel.text = LocalizationManager.Get("SERVANT_ALL_SELL_MESSAGE_END");
                this.info1Label.text = string.Format(LocalizationManager.Get("SERVANT_SELL_INFO2"), LocalizationManager.GetNumberFormat(this.userGameEntity.qp));
                this.info2Label.text = string.Format(LocalizationManager.Get("SERVANT_SELL_INFO3"), LocalizationManager.GetNumberFormat(this.userGameEntity.mana));
                zero = this.messageLabel.transform.localPosition;
                zero.y = 100f;
                this.messageLabel.transform.localPosition = zero;
                zero = this.info1Label.transform.localPosition;
                zero.y = 0f;
                this.info1Label.transform.localPosition = zero;
                zero = this.info2Label.transform.localPosition;
                zero.y = -80f;
                this.info2Label.transform.localPosition = zero;
                goto Label_0706;

            case Kind.SELL_ERROR_SERVANT:
            case Kind.SELL_ERROR_SERVANT_EQUIP:
                this.messageLabel.text = LocalizationManager.Get((kind != Kind.SELL_ERROR_SERVANT) ? "SERVANT_EQUIP_SELL_MESSAGE_ERROR" : "SERVANT_SELL_MESSAGE_ERROR");
                this.info1Label.text = string.Empty;
                this.info2Label.text = string.Empty;
                zero = this.messageLabel.transform.localPosition;
                zero.y = 5f;
                this.messageLabel.transform.localPosition = zero;
                zero = this.info1Label.transform.localPosition;
                zero.y = -60f;
                this.info1Label.transform.localPosition = zero;
                zero = this.info2Label.transform.localPosition;
                zero.y = -95f;
                this.info2Label.transform.localPosition = zero;
                goto Label_0706;

            case Kind.NO_SELECT_SERVANT:
            case Kind.NO_SELECT_SERVANT_EQUIP:
                this.messageLabel.text = LocalizationManager.Get((kind != Kind.NO_SELECT_SERVANT) ? "SERVANT_EQUIP_SELL_MESSAGE_NO_SELECT" : "SERVANT_SELL_MESSAGE_NO_SELECT");
                this.info1Label.text = string.Empty;
                this.info2Label.text = string.Empty;
                zero = this.messageLabel.transform.localPosition;
                zero.y = 5f;
                this.messageLabel.transform.localPosition = zero;
                zero = this.info1Label.transform.localPosition;
                zero.y = -60f;
                this.info1Label.transform.localPosition = zero;
                zero = this.info2Label.transform.localPosition;
                zero.y = -95f;
                this.info2Label.transform.localPosition = zero;
                goto Label_0706;

            default:
                goto Label_0706;
        }
    Label_0317:
        if (num3 == 0)
        {
            this.warningLabel.text = string.Empty;
        }
        else if ((num3 & 4) != 0)
        {
            this.warningLabel.text = LocalizationManager.Get("SELL_EXCEEDED_MATERIAL_INFO_MSG");
        }
        else if (num3 == 1)
        {
            this.warningLabel.text = LocalizationManager.Get("SERVANT_SELL_REQUEST_RARITY_MESSAGE");
        }
        else if (num3 == 2)
        {
            this.warningLabel.text = LocalizationManager.Get("SERVANT_EQUIP_SELL_REQUEST_RARITY_MESSAGE");
        }
        else if (num3 == 3)
        {
            this.warningLabel.text = LocalizationManager.Get("SERVANT_ALL_SELL_REQUEST_RARITY_MESSAGE");
        }
        zero = this.messageLabel.transform.localPosition;
        zero.y = 5f;
        this.messageLabel.transform.localPosition = zero;
        zero = this.info1Label.transform.localPosition;
        zero.y = -60f;
        this.info1Label.transform.localPosition = zero;
        zero = this.info2Label.transform.localPosition;
        zero.y = -95f;
        this.info2Label.transform.localPosition = zero;
        this.servantOperationListViewManager.gameObject.SetActive(true);
        this.servantOperationListViewManager.CreateList(ServantOperationListViewManager.Kind.SELL, servantIdList);
    Label_0706:
        this.state = State.OPEN;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public void SetMode(ServantOperationListViewManager.InitMode mode)
    {
        Debug.Log("ServantOperationManager::SetMode " + mode);
        this.servantOperationListViewManager.SetMode(mode, new ServantOperationListViewManager.CallbackFunc(this.OnSelectServant));
    }

    public delegate void CallbackFunc(int result);

    public enum Kind
    {
        SELET_SERVANT,
        SELET_SERVANT_EQUIP,
        SELET_SERVANT_EXCEEDED,
        SELL_SERVANT,
        SELL_SERVANT_EQUIP,
        SELL_ERROR_SERVANT,
        SELL_ERROR_SERVANT_EQUIP,
        NO_SELECT_SERVANT,
        NO_SELECT_SERVANT_EQUIP
    }

    protected enum ServantType
    {
        NONE,
        SERVANT,
        SERVANT_EQUIP,
        BOTH,
        EXCEEDED
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

