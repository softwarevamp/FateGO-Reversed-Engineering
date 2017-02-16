using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartyOrganizationCommandCardMenu : BaseDialog
{
    [SerializeField]
    protected UICommonButton closeButton;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel closeLabel;
    [SerializeField]
    protected UILabel[] commandLabelList;
    [SerializeField]
    protected PartyOrganizationConfirmItemDraw[] memberObjectList = new PartyOrganizationConfirmItemDraw[3];
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
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(false);
        }
    }

    public void Open(Kind kind, PartyListViewItem partyItem, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            int[] commandCardList = partyItem.GetCommandCardList(this.commandLabelList.Length);
            for (int i = 0; i < this.memberObjectList.Length; i++)
            {
                PartyOrganizationListViewItem member = partyItem.GetMember(i);
                this.memberObjectList[i].SetItem(member, PartyOrganizationConfirmItemDraw.DispMode.VALID);
            }
            for (int j = 0; j < this.commandLabelList.Length; j++)
            {
                this.commandLabelList[j].text = string.Empty + commandCardList[j];
            }
            this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_COMMAND_CARD_TITLE");
            this.messageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_COMMAND_CARD_MESSAGE");
            this.closeLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_COMMAND_CARD_CLOSE");
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public delegate void CallbackFunc(bool result);

    public enum Kind
    {
        NORMAL
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

