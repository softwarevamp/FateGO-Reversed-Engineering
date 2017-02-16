using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartyOrganizationConfirmDeck2Menu : BaseDialog
{
    [SerializeField]
    protected PartyOrganizationConfirmItemDraw[] baseObjectList = new PartyOrganizationConfirmItemDraw[6];
    [SerializeField]
    protected UILabel baseTitleLabel;
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
    protected PartyOrganizationConfirmItemDraw[] memberObjectList = new PartyOrganizationConfirmItemDraw[6];
    [SerializeField]
    protected UILabel memberTitleLabel;
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
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(true);
        }
    }

    public void Open(Kind kind, PartyListViewItem basePartyItem, PartyListViewItem partyItem, CallbackFunc callback)
    {
        if ((this.state == State.INIT) || (this.state == State.CLOSE))
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            for (int i = 0; i < this.memberObjectList.Length; i++)
            {
                PartyOrganizationListViewItem member = partyItem.GetMember(i);
                this.memberObjectList[i].SetItem(member, PartyOrganizationConfirmItemDraw.DispMode.VALID);
            }
            for (int j = 0; j < this.baseObjectList.Length; j++)
            {
                PartyOrganizationListViewItem item = basePartyItem.GetMember(j);
                this.baseObjectList[j].SetItem(item, PartyOrganizationConfirmItemDraw.DispMode.VALID);
            }
            switch (kind)
            {
                case Kind.CANCEL:
                case Kind.EMPTY_DECK_MEMBER:
                case Kind.SHORTAGE_DECK_MEMBER:
                case Kind.REMOVE:
                case Kind.REMOVE_MAIN_DECK:
                    this.decideButton.gameObject.SetActive(true);
                    this.cancelButton.gameObject.SetActive(true);
                    this.closeButton.gameObject.SetActive(false);
                    this.decideButton.SetState(UICommonButtonColor.State.Normal, true);
                    this.cancelButton.SetState(UICommonButtonColor.State.Normal, true);
                    break;

                case Kind.REMOVE_MAIN_DECK_LEADER:
                case Kind.START_SHORTAGE_DECK_MEMBER:
                case Kind.START_COST_OVER:
                    this.decideButton.gameObject.SetActive(false);
                    this.cancelButton.gameObject.SetActive(false);
                    this.closeButton.gameObject.SetActive(true);
                    this.closeButton.SetState(UICommonButtonColor.State.Normal, true);
                    break;
            }
            this.memberTitleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM2_NOW_PARTY_TITLE");
            this.baseTitleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM2_BASE_PARTY_TITLE");
            switch (kind)
            {
                case Kind.CANCEL:
                    this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM2_CANCEL_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM2_CANCEL_MESSAGE");
                    this.decideLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM2_CANCEL_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM2_CANCEL_CANCEL");
                    break;

                case Kind.EMPTY_DECK_MEMBER:
                    this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_EMPTY_DECK_MEMBER_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_EMPTY_DECK_MEMBER_MESSAGE");
                    this.decideLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_EMPTY_DECK_MEMBER_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_EMPTY_DECK_MEMBER_CANCEL");
                    break;

                case Kind.SHORTAGE_DECK_MEMBER:
                    this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_SHORTAGE_DECK_MEMBER_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_SHORTAGE_DECK_MEMBER_MESSAGE");
                    this.decideLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_SHORTAGE_DECK_MEMBER_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_SHORTAGE_DECK_MEMBER_CANCEL");
                    break;

                case Kind.REMOVE:
                    this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_MESSAGE");
                    this.decideLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_CANCEL");
                    break;

                case Kind.REMOVE_MAIN_DECK:
                    this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_MAIN_DECK_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_MAIN_DECK_MESSAGE");
                    this.decideLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_MAIN_DECK_DECIDE");
                    this.cancelLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_MAIN_DECK_CANCEL");
                    break;

                case Kind.REMOVE_MAIN_DECK_LEADER:
                    this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_MAIN_DECK_LEADER_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_MAIN_DECK_LEADER_MESSAGE");
                    this.closeLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_REMOVE_MAIN_DECK_LEADER_CLOSE");
                    break;

                case Kind.START_SHORTAGE_DECK_MEMBER:
                    this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_START_SHORTAGE_DECK_MEMBER_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_START_SHORTAGE_DECK_MEMBER_MESSAGE");
                    this.closeLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_START_SHORTAGE_DECK_MEMBER_CLOSE");
                    break;

                case Kind.START_COST_OVER:
                    this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_START_COST_OVER_TITLE");
                    this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_START_COST_OVER_MESSAGE");
                    this.closeLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_CONFIRM_START_COST_OVER_CLOSE");
                    break;
            }
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

