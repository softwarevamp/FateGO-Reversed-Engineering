using System;
using System.Runtime.CompilerServices;

public class PartyOrganizationChangeObject : BaseMonoBehaviour
{
    protected PartyOrganizationListViewItemDraw itemDraw;
    protected int member;
    protected PartyListViewItem partyItem;
    protected State state;

    protected event CallbackFunc callbackFunc;

    protected void Awake()
    {
        this.itemDraw = base.GetComponent<PartyOrganizationListViewItemDraw>();
    }

    public void ClearItem()
    {
        this.partyItem = null;
        this.member = -1;
        this.callbackFunc = null;
        this.state = State.INIT;
        if (this.itemDraw != null)
        {
            this.itemDraw.ClearItem();
        }
    }

    protected void EndCloseShow()
    {
    }

    protected void EndCloseShowEquip()
    {
    }

    protected void EndShowEquip(bool isDecide)
    {
        if (isDecide)
        {
            this.ModifyItem();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipStatusDialog(new System.Action(this.EndCloseShowEquip));
    }

    protected void EndShowServant(bool isDecide)
    {
        if (isDecide)
        {
            this.ModifyItem();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShow));
    }

    public void ModifyItem()
    {
        PartyOrganizationListViewItem member = this.partyItem.GetMember(this.member);
        member.Modify();
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(member, PartyOrganizationListViewItemDraw.DispMode.VALID);
        }
    }

    public void OnClickItem()
    {
        if (((this.state == State.INPUT) && (this.callbackFunc != null)) && (this.partyItem != null))
        {
            PartyOrganizationListViewItem member = this.partyItem.GetMember(this.member);
            if ((member != null) && !member.IsFollower)
            {
                this.callbackFunc(ResultKind.SELECT_SERVANT, member.Index);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
        }
    }

    public void OnClickItemEquip()
    {
        if (((this.state == State.INPUT) && (this.callbackFunc != null)) && (this.partyItem != null))
        {
            PartyOrganizationListViewItem member = this.partyItem.GetMember(this.member);
            if ((member != null) && !member.IsFollower)
            {
                this.callbackFunc(ResultKind.SELECT_EQUIP, member.Index);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
        }
    }

    public void OnLongPressItem()
    {
        if ((this.state == State.INPUT) && (this.callbackFunc != null))
        {
            this.OpenServantDetail();
        }
    }

    public void OnLongPressItemEquip()
    {
        if ((this.state == State.INPUT) && (this.callbackFunc != null))
        {
            this.OpenEquipDetail();
        }
    }

    protected void OpenEquipDetail()
    {
        if (this.partyItem != null)
        {
            PartyOrganizationListViewItem member = this.partyItem.GetMember(this.member);
            if (member != null)
            {
                if (member.EquipUserSvtId > 0L)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(ServantStatusDialog.Kind.SERVANT_EQUIP, member.EquipUserSvtId, true, new ServantStatusDialog.ClickDelegate(this.EndShowEquip));
                    return;
                }
                if (((member.FollowerData != null) && (member.FollowerData.type != 3)) && (member.EquipSvtId > 0))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(ServantStatusDialog.Kind.FOLLOWER_SERVANT_EQUIP, member.EquipTarget1, new ServantStatusDialog.ClickDelegate(this.EndShowEquip));
                    return;
                }
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    protected void OpenServantDetail()
    {
        if (this.partyItem != null)
        {
            PartyOrganizationListViewItem member = this.partyItem.GetMember(this.member);
            if (member != null)
            {
                if (member.UserServant != null)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.PARTY, this.partyItem, this.member, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                    return;
                }
                if (((member.FollowerData != null) && (member.FollowerData.type != 3)) && ((member.ServantLeader != null) && (member.ServantLeader.svtId > 0)))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.FOLLOWER, member.ServantLeader, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                    return;
                }
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    public void SetInput(bool isInput)
    {
        this.state = !isInput ? State.IDLE : State.INPUT;
        if (this.itemDraw != null)
        {
            this.itemDraw.SetInput(this.partyItem.GetMember(this.member), isInput);
        }
    }

    public void SetItem(PartyListViewItem partyItem, int member, CallbackFunc callback)
    {
        this.partyItem = partyItem;
        this.member = member;
        PartyOrganizationListViewItem item = this.partyItem.GetMember(this.member);
        this.callbackFunc = callback;
        if (item == null)
        {
            this.state = State.IDLE;
        }
        else if (this.state != State.INPUT)
        {
            this.state = State.IDLE;
        }
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(item, PartyOrganizationListViewItemDraw.DispMode.VALID);
        }
    }

    public void SetVisible(bool isVisible)
    {
        if (this.itemDraw != null)
        {
            this.itemDraw.gameObject.SetActive(isVisible);
        }
    }

    public delegate void CallbackFunc(PartyOrganizationChangeObject.ResultKind result, int n);

    public enum ResultKind
    {
        NONE,
        SELECT_SERVANT,
        SELECT_EQUIP,
        MODIFY_STATUS
    }

    protected enum State
    {
        INIT,
        IDLE,
        INPUT
    }
}

