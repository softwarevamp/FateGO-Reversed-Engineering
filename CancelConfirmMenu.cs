using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CancelConfirmMenu : BaseDialog
{
    [SerializeField]
    protected UICommonButton cancelButton;
    [SerializeField]
    protected UILabel cancelLabel;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected CancelConfirmItemDraw[] currentSupportMemberObjectList = new CancelConfirmItemDraw[BalanceConfig.SupportDeckMax];
    [SerializeField]
    protected UILabel currentSupportMemberTitleLabel;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected UILabel decideLabel;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected CancelConfirmItemDraw[] oldSupportMemberObjectList = new CancelConfirmItemDraw[BalanceConfig.SupportDeckMax];
    [SerializeField]
    protected UILabel oldSupportMemberTitleLabel;
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
        this.currentSupportMemberTitleLabel.text = string.Empty;
        this.oldSupportMemberTitleLabel.text = string.Empty;
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
            this.Callback(false);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.Callback(true);
        }
    }

    public void Open(Kind kind, SupportServantData supportServantData, CallbackFunc callback)
    {
        if ((this.state == State.INIT) || (this.state == State.CLOSE))
        {
            UserServantEntity entity;
            long num;
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
            long[] equipIdList = new long[1];
            for (int i = 0; i < this.currentSupportMemberObjectList.Length; i++)
            {
                num = supportServantData.getServant(i);
                if (num != 0)
                {
                    entity = master.getEntityFromId<UserServantEntity>(num);
                }
                else
                {
                    entity = null;
                }
                equipIdList[0] = supportServantData.getEquip(i);
                this.currentSupportMemberObjectList[i].SetItem(entity, equipIdList);
            }
            for (int j = 0; j < this.oldSupportMemberObjectList.Length; j++)
            {
                num = supportServantData.getOldServant(j);
                if (num != 0)
                {
                    entity = master.getEntityFromId<UserServantEntity>(num);
                }
                else
                {
                    entity = null;
                }
                equipIdList[0] = supportServantData.getOldEquip(j);
                this.oldSupportMemberObjectList[j].SetItem(entity, equipIdList);
            }
            this.decideButton.gameObject.SetActive(true);
            this.cancelButton.gameObject.SetActive(true);
            this.decideButton.SetState(UICommonButtonColor.State.Normal, true);
            this.cancelButton.SetState(UICommonButtonColor.State.Normal, true);
            this.currentSupportMemberTitleLabel.text = LocalizationManager.Get("SUPPORT_SELECT_CURRENT_STATE");
            this.oldSupportMemberTitleLabel.text = LocalizationManager.Get("SUPPORT_SELECT_OLD_STATE");
            this.titleLabel.text = LocalizationManager.Get("SUPPORT_SELECT_CANCEL_DIALOG_TITLE");
            this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM2_CANCEL_MESSAGE");
            this.decideLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM2_CANCEL_DECIDE");
            this.cancelLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM2_CANCEL_CANCEL");
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public delegate void CallbackFunc(bool result);

    public enum Kind
    {
        CANCEL,
        EMPTY_DECK_MEMBER,
        SHORTAGE_DECK_MEMBER,
        SAME_SAERVANT,
        REMOVE,
        REMOVE_MAIN_DECK,
        REMOVE_MAIN_DECK_LEADER,
        START_SHORTAGE_DECK_MEMBER,
        START_COST_OVER
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

