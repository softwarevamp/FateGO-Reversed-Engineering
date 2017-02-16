using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class PartyOrganizationMenu : BaseMenu
{
    protected PartyListViewItem basePartyItem;
    protected System.Action closeCallbackFunc;
    protected PartyListViewItem.SetupKind kind;
    [SerializeField]
    protected GameObject mainDeckBase;
    protected System.Action openCallbackFunc;
    [SerializeField]
    protected UICommonButton partyCancelButton;
    [SerializeField]
    protected UICommonButton partyDecideButton;
    protected PartyListViewItem partyItem;
    [SerializeField]
    protected PartyListViewIndicator partyListViewIndicator;
    [SerializeField]
    protected PartyOrganizationListViewManager partyOrganizationListViewManager;
    [SerializeField]
    protected UICommonButton partyPointEventButton;
    [SerializeField]
    protected UICommonButton partyRemoveButton;
    protected EventUpValSetupInfo setupInfo;
    protected State state;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultKind result, int n = -1)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, n);
        }
    }

    protected void ClearItem()
    {
        this.mainDeckBase.SetActive(false);
        this.partyOrganizationListViewManager.DestroyList();
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
        if (this.callbackFunc != null)
        {
            this.state = State.INPUT;
            this.SetInput(true);
        }
        else
        {
            this.state = State.SELECTED;
        }
        if (this.openCallbackFunc != null)
        {
            System.Action openCallbackFunc = this.openCallbackFunc;
            this.openCallbackFunc = null;
            openCallbackFunc();
        }
    }

    public PartyListViewItem GetBaseItem() => 
        this.basePartyItem;

    public PartyListViewItem GetItem() => 
        this.partyItem;

    public void Init()
    {
        this.ClearItem();
        this.basePartyItem = null;
        this.partyItem = null;
        this.state = State.INIT;
        base.Init();
    }

    public void ModifyItem()
    {
        this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.MODIFY);
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            Debug.Log("PartyOrganizationMenu:OnClickCancel");
            this.state = State.SELECTED;
            this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.VALID);
            this.Callback(ResultKind.CANCEL, -1);
        }
    }

    public void OnClickClassCompatibility()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.VALID);
            this.Callback(ResultKind.CLASS_COMPATIBILITY, -1);
        }
    }

    public void OnClickCommandCard()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.VALID);
            this.Callback(ResultKind.COMMAND_CARD, -1);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.VALID);
            this.Callback(ResultKind.DECIDE, -1);
        }
    }

    protected void OnClickItem(PartyOrganizationListViewManager.ResultKind result, int n)
    {
        if (this.state == State.INPUT)
        {
            Debug.Log("PartyOrganizationChangeMenu : OnClickItem " + n);
            PartyOrganizationListViewItem member = this.partyItem.GetMember(n);
            if (result == PartyOrganizationListViewManager.ResultKind.SELECT_SERVANT)
            {
                if (!member.IsFollower)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.state = State.SELECTED;
                    this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.VALID);
                    this.Callback(ResultKind.SELECT_SERVANT, n);
                    return;
                }
            }
            else if ((result == PartyOrganizationListViewManager.ResultKind.SELECT_EQUIP) && !member.IsFollower)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.state = State.SELECTED;
                this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.VALID);
                this.Callback(ResultKind.SELECT_EQUIP, n);
                return;
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.INPUT, new PartyOrganizationListViewManager.CallbackFunc(this.OnClickItem));
        }
    }

    public void OnClickPointEvent()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.VALID);
            this.Callback(ResultKind.EVENT_POINT, -1);
        }
    }

    public void OnClickRemove()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.VALID);
            this.Callback(ResultKind.REMOVE, -1);
        }
    }

    public void Open(PartyListViewItem.SetupKind kind, PartyListViewItem basePartyItem, PartyListViewItem partyItem, EventUpValSetupInfo setupInfo, CallbackFunc callback, System.Action openCallback)
    {
        Debug.Log("PartyOrganizationMenu:Open " + this.state);
        if (this.state == State.INIT)
        {
            this.kind = kind;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            this.basePartyItem = basePartyItem;
            this.partyItem = partyItem;
            this.setupInfo = setupInfo;
            base.gameObject.SetActive(true);
            this.SetItem();
            this.SetInput(false);
            this.SetupButton(false);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if (this.state == State.ENTER)
        {
            this.kind = kind;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            this.basePartyItem = basePartyItem;
            this.partyItem = partyItem;
            this.setupInfo = setupInfo;
            base.gameObject.SetActive(true);
            this.SetItem();
            this.SetInput(false);
            this.SetupButton(true);
            this.EndOpen();
        }
        else if (this.state == State.SELECTED)
        {
            this.kind = kind;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            this.basePartyItem = basePartyItem;
            this.partyItem = partyItem;
            this.setupInfo = setupInfo;
            this.SetItem();
            this.SetInput(false);
            this.SetupButton(true);
            this.EndOpen();
        }
    }

    protected void SetInput(bool isInput)
    {
        if (isInput)
        {
            this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.INPUT, new PartyOrganizationListViewManager.CallbackFunc(this.OnClickItem));
        }
        else
        {
            this.partyOrganizationListViewManager.SetMode(PartyOrganizationListViewManager.InitMode.VALID);
        }
    }

    protected void SetItem()
    {
        this.partyListViewIndicator.DrawPartyInfo(this.partyItem);
        this.partyOrganizationListViewManager.CreateList(PartyListViewItem.SetupKind.PARTY_ORGANIZATION, this.partyItem, this.basePartyItem);
    }

    protected void SetupButton(bool isMove)
    {
        this.partyPointEventButton.gameObject.SetActive(this.setupInfo != null);
        this.partyPointEventButton.SetState(UICommonButtonColor.State.Normal, isMove);
    }

    public delegate void CallbackFunc(PartyOrganizationMenu.ResultKind result, int n);

    public enum ResultKind
    {
        CANCEL,
        DECIDE,
        REMOVE,
        CLASS_COMPATIBILITY,
        COMMAND_CARD,
        EVENT_POINT,
        SELECT_SERVANT,
        SELECT_EQUIP
    }

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE,
        ENTER
    }
}

