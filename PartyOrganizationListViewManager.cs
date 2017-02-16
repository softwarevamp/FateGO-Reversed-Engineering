using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartyOrganizationListViewManager : ListViewManager
{
    protected PartyListViewItem basePartyItem;
    protected int callbackCount;
    [SerializeField]
    protected GameObject explanationBase;
    [SerializeField]
    protected UILabel explanationLabel;
    protected InitMode initMode;
    protected PartyListViewItem partyItem;
    protected PartyListViewItem.SetupKind setupKind;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(PartyListViewItem.SetupKind setupKind, PartyListViewItem partyItem, PartyListViewItem basePartyItem)
    {
        this.setupKind = setupKind;
        this.basePartyItem = basePartyItem;
        this.partyItem = partyItem;
        if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
        {
        }
        base.CreateList(0);
        for (int i = 0; i < base.dropObjectList.Count; i++)
        {
            PartyOrganizationListViewDropObject obj2 = base.dropObjectList[i] as PartyOrganizationListViewDropObject;
            obj2.SetItem(this.partyItem.GetMember(i));
            obj2.SetManager(this);
            obj2.SetDragPrefab(base.dropDragPrefab);
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        if (base.dropObjectList != null)
        {
            foreach (ListViewDropObject obj2 in base.dropObjectList)
            {
                PartyOrganizationListViewDropObject obj3 = obj2 as PartyOrganizationListViewDropObject;
                if (obj3 != null)
                {
                    obj3.ReleaseItem();
                }
            }
        }
        this.basePartyItem = null;
        this.partyItem = null;
        base.DestroyList();
    }

    protected void EndCloseShowEquip()
    {
        this.initMode = InitMode.INPUT;
    }

    protected void EndCloseShowServant()
    {
        this.initMode = InitMode.INPUT;
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
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    public PartyOrganizationListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as PartyOrganizationListViewItem);
        }
        return null;
    }

    public PartyListViewItem GetPartyItem() => 
        this.partyItem;

    public bool IsCanDrag() => 
        (this.initMode == InitMode.INPUT);

    public bool IsDropDropSurface(ListViewDropInfo info)
    {
        if (info.DropSurfaceObject != null)
        {
            PartyOrganizationListViewDropObject component = info.ListViewObject.GetComponent<PartyOrganizationListViewDropObject>();
            PartyOrganizationUIDragDropListViewSurface surface = info.DropSurfaceObject.GetComponent<PartyOrganizationUIDragDropListViewSurface>();
            if (((component != null) && (surface != null)) && (component.GetItem() != null))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsItemDropSurface(ListViewDropInfo info)
    {
        if (info.DropSurfaceObject != null)
        {
            PartyOrganizationListViewObject component = info.ListViewObject.GetComponent<PartyOrganizationListViewObject>();
            PartyOrganizationUIDragDropListViewSurface surface = info.DropSurfaceObject.GetComponent<PartyOrganizationUIDragDropListViewSurface>();
            if (((component != null) && (surface != null)) && (component.GetItem() != null))
            {
                return true;
            }
        }
        return false;
    }

    public override void ItemDragEnd()
    {
        if (this.initMode == InitMode.DRAG)
        {
            this.initMode = InitMode.INPUT;
            base.ItemDragEnd();
        }
        else
        {
            Debug.LogError("ItemDragEnd DragModeError " + this.initMode);
        }
    }

    public override void ItemDragStart()
    {
        if (this.initMode == InitMode.INPUT)
        {
            this.initMode = InitMode.DRAG;
            base.ItemDragStart();
        }
        else
        {
            Debug.LogError("ItemDragStart DragModeError " + this.initMode);
        }
    }

    public void ModifyItem()
    {
        this.basePartyItem.Modify();
        this.partyItem.Modify();
        this.RequestListObject(PartyOrganizationListViewObject.InitMode.MODIFY);
        this.RequestDropObject(PartyOrganizationListViewDropObject.InitMode.MODIFY);
    }

    protected void OnClickListDropEquip(ListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            Debug.Log("Manager ListDrop OnClickEquip " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SELECT_EQUIP, obj.Index);
            }
        }
    }

    protected void OnClickListDropEquipDetail(ListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            Debug.Log("Manager ListDrop OnClickEquipDetail " + obj.Index);
            PartyOrganizationListViewItem item = obj.GetItem() as PartyOrganizationListViewItem;
            if (item != null)
            {
                if (item.EquipUserSvtId > 0L)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.initMode = InitMode.DETAIL_EQUIP;
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(ServantStatusDialog.Kind.SERVANT_EQUIP, item.EquipUserSvtId, true, new ServantStatusDialog.ClickDelegate(this.EndShowEquip));
                    return;
                }
                if (((item.FollowerData != null) && (item.FollowerData.type != 3)) && (item.EquipSvtId > 0))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.initMode = InitMode.DETAIL_EQUIP;
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(ServantStatusDialog.Kind.FOLLOWER_SERVANT_EQUIP, item.EquipTarget1, new ServantStatusDialog.ClickDelegate(this.EndShowEquip));
                    return;
                }
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    protected void OnClickListDropServant(ListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            Debug.Log("Manager ListDrop OnClickServant " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SELECT_SERVANT, obj.Index);
            }
        }
    }

    protected void OnClickListDropServantDetail(ListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            Debug.Log("Manager ListDrop OnClickServantDetail " + obj.Index);
            PartyOrganizationListViewItem item = obj.GetItem() as PartyOrganizationListViewItem;
            if (item != null)
            {
                if (item.UserServant != null)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.initMode = InitMode.DETAIL_SERVANT;
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.PARTY, this.GetPartyItem(), item.Index, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                    return;
                }
                if (((item.FollowerData != null) && (item.FollowerData.type != 3)) && (item.SvtId > 0))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.initMode = InitMode.DETAIL_SERVANT;
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.FOLLOWER, item.ServantLeader, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                    return;
                }
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    protected void OnClickListView(ListViewObject obj)
    {
    }

    protected void OnMoveEnd()
    {
        if (this.callbackCount > 0)
        {
            this.callbackCount--;
            if (this.callbackCount == 0)
            {
                if (base.scrollView != null)
                {
                    base.scrollView.UpdateScrollbars(true);
                }
                System.Action action = this.callbackFunc2;
                this.callbackFunc2 = null;
                if (action != null)
                {
                    action();
                }
            }
        }
    }

    protected void RequestDropObject(PartyOrganizationListViewDropObject.InitMode mode)
    {
        if (base.dropObjectList != null)
        {
            foreach (PartyOrganizationListViewDropObject obj2 in base.dropObjectList)
            {
                if (obj2 != null)
                {
                    obj2.Init(mode, new System.Action(this.OnMoveEnd));
                }
            }
        }
    }

    protected void RequestDropObject(PartyOrganizationListViewDropObject.InitMode mode, float delay)
    {
        if (base.dropObjectList != null)
        {
            foreach (PartyOrganizationListViewDropObject obj2 in base.dropObjectList)
            {
                if (obj2 != null)
                {
                    obj2.Init(mode, new System.Action(this.OnMoveEnd), delay);
                }
            }
        }
    }

    protected void RequestInto()
    {
        base.ClippingItems(true, false);
        List<PartyOrganizationListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            PartyOrganizationListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(PartyOrganizationListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
            }
            else
            {
                this.callbackCount--;
            }
        }
        if (num == 0)
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0f);
        }
    }

    protected void RequestListObject(PartyOrganizationListViewObject.InitMode mode)
    {
        List<PartyOrganizationListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PartyOrganizationListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd));
            }
        }
    }

    protected void RequestListObject(PartyOrganizationListViewObject.InitMode mode, float delay)
    {
        List<PartyOrganizationListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PartyOrganizationListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd), delay);
            }
        }
    }

    public void SetMode(InitMode mode)
    {
        if (mode == InitMode.MODIFY)
        {
            this.ModifyItem();
            return;
        }
        this.initMode = mode;
        this.callbackCount = base.dropObjectList.Count;
        base.IsInput = mode == InitMode.INPUT;
        switch (mode)
        {
            case InitMode.VALID:
                this.RequestListObject(PartyOrganizationListViewObject.InitMode.VALID);
                this.RequestDropObject(PartyOrganizationListViewDropObject.InitMode.VALID);
                goto Label_029C;

            case InitMode.INPUT:
                this.RequestListObject(PartyOrganizationListViewObject.InitMode.INPUT);
                this.RequestDropObject(PartyOrganizationListViewDropObject.InitMode.INPUT);
                goto Label_029C;

            case InitMode.INTO:
            {
                List<PartyOrganizationListViewObject> clippingObjectList = this.ClippingObjectList;
                if (clippingObjectList.Count > 0)
                {
                    this.callbackCount += clippingObjectList.Count;
                    for (int j = 0; j < clippingObjectList.Count; j++)
                    {
                        clippingObjectList[j].Init(PartyOrganizationListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
                    }
                }
                break;
            }
            case InitMode.ENTER:
            {
                List<PartyOrganizationListViewObject> list2 = this.ClippingObjectList;
                if (list2.Count > 0)
                {
                    this.callbackCount += list2.Count;
                    for (int m = 0; m < list2.Count; m++)
                    {
                        list2[m].Init(PartyOrganizationListViewObject.InitMode.ENTER, new System.Action(this.OnMoveEnd), 0.1f);
                    }
                }
                for (int k = 0; k < base.dropObjectList.Count; k++)
                {
                    (base.dropObjectList[k] as PartyOrganizationListViewDropObject).Init(PartyOrganizationListViewDropObject.InitMode.VALID, new System.Action(this.OnMoveEnd), 0.1f);
                }
                goto Label_029C;
            }
            case InitMode.EXIT:
            {
                List<PartyOrganizationListViewObject> list3 = this.ClippingObjectList;
                if (list3.Count > 0)
                {
                    this.callbackCount += list3.Count;
                    for (int num5 = 0; num5 < list3.Count; num5++)
                    {
                        list3[num5].Init(PartyOrganizationListViewObject.InitMode.EXIT, new System.Action(this.OnMoveEnd), 0.1f);
                    }
                }
                for (int n = 0; n < base.dropObjectList.Count; n++)
                {
                    (base.dropObjectList[n] as PartyOrganizationListViewDropObject).Init(PartyOrganizationListViewDropObject.InitMode.VALID, new System.Action(this.OnMoveEnd), 0.1f);
                }
                goto Label_029C;
            }
            default:
                goto Label_029C;
        }
        for (int i = 0; i < base.dropObjectList.Count; i++)
        {
            (base.dropObjectList[i] as PartyOrganizationListViewDropObject).Init(PartyOrganizationListViewDropObject.InitMode.VALID, new System.Action(this.OnMoveEnd), 0.1f);
        }
    Label_029C:
        this.explanationBase.SetActive(true);
        this.explanationLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_SERVANT_SWAP_EXPLANATION");
        Debug.Log("SetMode " + mode);
    }

    public void SetMode(InitMode mode, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.SetMode(mode);
    }

    public void SetMode(InitMode mode, System.Action callback)
    {
        this.callbackFunc2 = callback;
        this.SetMode(mode);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        PartyOrganizationListViewObject obj2 = obj as PartyOrganizationListViewObject;
        if (obj2 != null)
        {
            if (this.initMode == InitMode.INPUT)
            {
                obj2.Init(PartyOrganizationListViewObject.InitMode.INPUT);
            }
            else
            {
                obj2.Init(PartyOrganizationListViewObject.InitMode.VALID);
            }
        }
    }

    public List<PartyOrganizationListViewObject> ClippingObjectList
    {
        get
        {
            List<PartyOrganizationListViewObject> list = new List<PartyOrganizationListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PartyOrganizationListViewObject component = obj2.GetComponent<PartyOrganizationListViewObject>();
                    PartyOrganizationListViewItem item = component.GetItem();
                    if (item.IsTermination)
                    {
                        if (base.ClippingItem(item))
                        {
                            list.Add(component);
                        }
                    }
                    else
                    {
                        list.Add(component);
                    }
                }
            }
            return list;
        }
    }

    public List<PartyOrganizationListViewObject> ObjectList
    {
        get
        {
            List<PartyOrganizationListViewObject> list = new List<PartyOrganizationListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PartyOrganizationListViewObject component = obj2.GetComponent<PartyOrganizationListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(PartyOrganizationListViewManager.ResultKind kind, int result);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        INTO,
        ENTER,
        EXIT,
        DETAIL_SERVANT,
        DETAIL_EQUIP,
        DRAG,
        MODIFY
    }

    public enum ResultKind
    {
        LIST,
        SELECT_SERVANT,
        SELECT_EQUIP
    }
}

