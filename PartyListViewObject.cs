using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartyListViewObject : ListViewObject
{
    protected PartyListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected PartyListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<PartyListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<PartyListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    protected void EndCloseShowEquip()
    {
    }

    protected void EndCloseShowServant()
    {
    }

    protected void EndShowEquip(bool isDecide)
    {
        base.manager.SendMessage("OnModifyListView", this);
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipStatusDialog(new System.Action(this.EndCloseShowEquip));
    }

    protected void EndShowServant(bool isDecide)
    {
        base.manager.SendMessage("OnModifyListView", this);
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    protected void EventMoveEnd()
    {
        base.isBusy = false;
        this.state = State.IDLE;
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    public PartyListViewItem GetItem() => 
        (base.linkItem as PartyListViewItem);

    public void Init(InitMode initMode)
    {
        this.Init(initMode, null, 0f, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc)
    {
        this.Init(initMode, callbackFunc, 0f, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc, float delay)
    {
        this.Init(initMode, callbackFunc, delay, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc, float delay, Vector3 position)
    {
        PartyListViewItem linkItem = base.linkItem as PartyListViewItem;
        if (linkItem == null)
        {
            this.state = State.INIT;
            this.dispMode = PartyListViewItemDraw.DispMode.INVISIBLE;
            base.SetVisible(false);
            if (this.itemDraw != null)
            {
                this.itemDraw.SetItem(null, this.dispMode);
            }
            this.SetInput(false);
        }
        else
        {
            base.transform.localPosition = base.basePosition;
            base.transform.localScale = base.baseScale;
            this.callbackFunc = callbackFunc;
            switch (initMode)
            {
                case InitMode.INVISIBLE:
                    this.dispMode = PartyListViewItemDraw.DispMode.INVISIBLE;
                    this.state = State.IDLE;
                    break;

                case InitMode.INVALID:
                    this.dispMode = PartyListViewItemDraw.DispMode.INVALID;
                    this.state = State.IDLE;
                    break;

                case InitMode.VALID:
                    this.dispMode = PartyListViewItemDraw.DispMode.VALID;
                    this.state = State.IDLE;
                    break;

                case InitMode.INPUT:
                    this.dispMode = PartyListViewItemDraw.DispMode.VALID;
                    this.state = State.INPUT;
                    break;

                case InitMode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT:
                    this.dispMode = PartyListViewItemDraw.DispMode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT;
                    this.state = State.IDLE;
                    break;
            }
            base.SetVisible(this.dispMode != PartyListViewItemDraw.DispMode.INVISIBLE);
            this.SetInput(this.state == State.INPUT);
            if (this.itemDraw != null)
            {
                this.itemDraw.SetItem(linkItem, this.dispMode);
            }
            if (this.callbackFunc != null)
            {
                System.Action action = this.callbackFunc;
                this.callbackFunc = null;
                action();
            }
        }
    }

    protected void InitItem()
    {
        this.state = State.INIT;
    }

    public void OnClickItem1()
    {
        this.SelectMemberServant(0);
    }

    public void OnClickItem2()
    {
        this.SelectMemberServant(1);
    }

    public void OnClickItem3()
    {
        this.SelectMemberServant(2);
    }

    public void OnClickItem4()
    {
        this.SelectMemberServant(3);
    }

    public void OnClickItem5()
    {
        this.SelectMemberServant(4);
    }

    public void OnClickItem6()
    {
        this.SelectMemberServant(5);
    }

    public void OnClickItemEquip1()
    {
        this.SelectMemberEquip(0);
    }

    public void OnClickItemEquip2()
    {
        this.SelectMemberEquip(1);
    }

    public void OnClickItemEquip3()
    {
        this.SelectMemberEquip(2);
    }

    public void OnClickItemEquip4()
    {
        this.SelectMemberEquip(3);
    }

    public void OnClickItemEquip5()
    {
        this.SelectMemberEquip(4);
    }

    public void OnClickItemEquip6()
    {
        this.SelectMemberEquip(5);
    }

    private void OnDestroy()
    {
        if (this.dragObject != null)
        {
            NGUITools.Destroy(this.dragObject);
            this.dragObject = null;
        }
    }

    public void OnLongPressItem1()
    {
        this.OpenServantDetail(0);
    }

    public void OnLongPressItem2()
    {
        this.OpenServantDetail(1);
    }

    public void OnLongPressItem3()
    {
        this.OpenServantDetail(2);
    }

    public void OnLongPressItem4()
    {
        this.OpenServantDetail(3);
    }

    public void OnLongPressItem5()
    {
        this.OpenServantDetail(4);
    }

    public void OnLongPressItem6()
    {
        this.OpenServantDetail(5);
    }

    public void OnLongPressItemEquip1()
    {
        this.OpenEquipDetail(0);
    }

    public void OnLongPressItemEquip2()
    {
        this.OpenEquipDetail(1);
    }

    public void OnLongPressItemEquip3()
    {
        this.OpenEquipDetail(2);
    }

    public void OnLongPressItemEquip4()
    {
        this.OpenEquipDetail(3);
    }

    public void OnLongPressItemEquip5()
    {
        this.OpenEquipDetail(4);
    }

    public void OnLongPressItemEquip6()
    {
        this.OpenEquipDetail(5);
    }

    protected void OpenEquipDetail(int m)
    {
        if (this.state == State.INPUT)
        {
            PartyListViewItem linkItem = base.linkItem as PartyListViewItem;
            if (linkItem != null)
            {
                PartyOrganizationListViewItem member = linkItem.GetMember(m);
                if (member.EquipUserSvtId > 0L)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(ServantStatusDialog.Kind.SERVANT_EQUIP, member.EquipUserSvtId, true, new ServantStatusDialog.ClickDelegate(this.EndShowEquip));
                }
                else if (((member.FollowerData != null) && (member.FollowerData.type != 3)) && (member.EquipSvtId > 0))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(ServantStatusDialog.Kind.FOLLOWER_SERVANT_EQUIP, member.EquipTarget1, new ServantStatusDialog.ClickDelegate(this.EndShowEquip));
                }
                else
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                }
            }
        }
    }

    protected void OpenServantDetail(int m)
    {
        if (this.state == State.INPUT)
        {
            PartyListViewItem linkItem = base.linkItem as PartyListViewItem;
            if (linkItem != null)
            {
                PartyOrganizationListViewItem member = linkItem.GetMember(m);
                if (member.UserServant != null)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.PARTY, linkItem, m, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                }
                else if ((member.FollowerData != null) && (member.FollowerData.type != 3))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.FOLLOWER, member.ServantLeader, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                }
                else
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                }
            }
        }
    }

    protected void SelectMemberEquip(int m)
    {
        if ((this.state == State.INPUT) && (base.linkItem is PartyListViewItem))
        {
            base.manager.SendMessage("OnClickListViewChangeEquip" + (m + 1), this);
        }
    }

    protected void SelectMemberServant(int m)
    {
        if ((this.state == State.INPUT) && (base.linkItem is PartyListViewItem))
        {
            base.manager.SendMessage("OnClickListViewChangeServant" + (m + 1), this);
        }
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        PartyListViewItem linkItem = base.linkItem as PartyListViewItem;
        if (this.itemDraw != null)
        {
            this.itemDraw.SetInput(linkItem, isInput);
        }
        if (base.mDragDrop != null)
        {
            base.mDragDrop.SetEnable(isInput);
        }
    }

    public override void SetItem(ListViewItem item)
    {
        base.SetItem(item);
        this.InitItem();
    }

    public override void SetItem(ListViewItem item, ListViewItemSeed seed)
    {
        base.SetItem(item, seed);
        this.InitItem();
    }

    public enum InitMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT,
        ORGANIZATION_GUIDE_DECK_EMPTY_SELECT
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

