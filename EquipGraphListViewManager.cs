using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EquipGraphListViewManager : ListViewManager
{
    protected int baseCost;
    protected int callbackCount;
    [SerializeField]
    protected UILabel explanationLabel;
    [SerializeField]
    protected UILabel infoData2Label;
    [SerializeField]
    protected UILabel infoDataLabel;
    protected InitMode initMode;
    protected int maxCost;
    [SerializeField]
    protected EquipGraphServantItemDraw servantItemDraw;
    protected EquipGraphServantItem servantItemInfo;
    protected const string SORT_SAVE_KEY = "EquipGraph";
    protected static ListViewSort sortStatus = new ListViewSort("EquipGraph", ListViewSort.SortKind.LEVEL, false);

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(PartyListViewItem partyItem, int member)
    {
        base.sort = sortStatus;
        base.sort.Load();
        PartyOrganizationListViewItem baseItem = partyItem.GetMember(member);
        if (!baseItem.IsFollower)
        {
            this.servantItemInfo = new EquipGraphServantItem(baseItem);
            this.servantItemDraw.SetItem(this.servantItemInfo);
            this.servantItemDraw.SetInput(false);
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getUserIdEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            this.maxCost = partyItem.MaxCost;
            this.baseCost = partyItem.Cost - this.servantItemInfo.EquipCost;
            UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
            long[] equipList = partyItem.GetEquipList();
            UserServantEntity[] entityArray = master.getServantEquipList();
            if (this.infoDataLabel != null)
            {
                this.infoDataLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), entityArray.Length, entity.svtEquipKeep);
            }
            if (this.explanationLabel != null)
            {
                this.explanationLabel.text = LocalizationManager.Get("SERVANT_EQUIP_OPERATION_EXPLANATION");
            }
            base.CreateList(0);
            for (int i = 0; i < entityArray.Length; i++)
            {
                EquipGraphListViewItem item = new EquipGraphListViewItem(i, entityArray[i], this.servantItemInfo.EquipUserSvtId, this.servantItemInfo.OldEquipUserSvtId, equipList);
                base.itemList.Add(item);
            }
            base.emptyMessageLabel.text = LocalizationManager.Get((base.itemList.Count <= 0) ? "SERVANT_EQUIP_EMPTY" : "SERVANT_SORT_FILTER_RESULT_EMPTY");
            this.RefrashListDisp();
            base.SortItem(-1, false, -1);
        }
    }

    public void DestroyList()
    {
        base.DestroyList();
        base.sort.Save();
    }

    public long GetAmountSortValue(int svtId)
    {
        int count = base.itemList.Count;
        long num2 = 0L;
        foreach (ListViewItem item in base.itemList)
        {
            EquipGraphListViewItem item2 = item as EquipGraphListViewItem;
            if (item2.UserServant.svtId == svtId)
            {
                num2 += 1L;
            }
        }
        return num2;
    }

    public EquipGraphListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as EquipGraphListViewItem);
        }
        return null;
    }

    public int GetSelect()
    {
        foreach (ListViewItem item in base.itemList)
        {
            EquipGraphListViewItem item2 = item as EquipGraphListViewItem;
            if (item2.IsBase)
            {
                return item2.Index;
            }
        }
        return -1;
    }

    public EquipGraphListViewItem GetSelectItem()
    {
        foreach (ListViewItem item in base.itemList)
        {
            EquipGraphListViewItem item2 = item as EquipGraphListViewItem;
            if (item2.IsBase)
            {
                return item2;
            }
        }
        return null;
    }

    public EquipGraphServantItem GetServantItemInfo() => 
        this.servantItemInfo;

    public void ModifyList()
    {
        UserServantEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getServantEquipList();
        for (int i = 0; i < entityArray.Length; i++)
        {
            long id = entityArray[i].id;
            for (int j = 0; j < base.itemList.Count; j++)
            {
                EquipGraphListViewItem item = base.itemList[j] as EquipGraphListViewItem;
                if ((item.UserServant != null) && (item.UserServant.id == id))
                {
                    item.Modify(entityArray[i], this.servantItemInfo.EquipUserSvtId, this.servantItemInfo.OldEquipUserSvtId);
                    break;
                }
            }
        }
    }

    public void OnClickDecide()
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            int select = this.GetSelect();
            callbackFunc(ResultKind.DECIDE, select);
        }
    }

    protected void OnClickListView(ListViewObject obj)
    {
    }

    public void OnClickSelectDetail()
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            int select = this.GetSelect();
            callbackFunc(ResultKind.SHOW_STATUS, select);
        }
    }

    protected void OnClickSelectListView(ListViewObject obj)
    {
        EquipGraphListViewItem item = (obj as EquipGraphListViewObject).GetItem();
        if (item.IsBase)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            item.IsBase = false;
            this.servantItemInfo.SetEquipTarget(0L);
            this.servantItemDraw.SetItem(this.servantItemInfo);
            this.RefrashListDisp();
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            foreach (ListViewItem item2 in base.itemList)
            {
                EquipGraphListViewItem item3 = item2 as EquipGraphListViewItem;
                item3.IsBase = false;
            }
            item.IsBase = true;
            this.servantItemInfo.SetEquipTarget(item.UserServant.id);
            this.servantItemDraw.SetItem(this.servantItemInfo);
            this.RefrashListDisp();
        }
    }

    public void OnClickSortAscendingOrder()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            base.sort.SetAscendingOrder(!base.sort.IsAscendingOrder);
            base.SortItem(-1, false, -1);
        }
    }

    public void OnClickSortKind()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            switch (base.sort.Kind)
            {
                case ListViewSort.SortKind.CREATE:
                    base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    break;

                case ListViewSort.SortKind.RARITY:
                    base.sort.SetKind(ListViewSort.SortKind.AMOUNT);
                    break;

                case ListViewSort.SortKind.LEVEL:
                    base.sort.SetKind(ListViewSort.SortKind.HP);
                    break;

                case ListViewSort.SortKind.HP:
                    base.sort.SetKind(ListViewSort.SortKind.ATK);
                    break;

                case ListViewSort.SortKind.ATK:
                    base.sort.SetKind(ListViewSort.SortKind.COST);
                    break;

                case ListViewSort.SortKind.COST:
                    base.sort.SetKind(ListViewSort.SortKind.RARITY);
                    break;

                case ListViewSort.SortKind.AMOUNT:
                    base.sort.SetKind(ListViewSort.SortKind.CREATE);
                    break;

                default:
                    base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    break;
            }
            base.SortItem(-1, false, -1);
        }
    }

    protected void OnLongPushListView(ListViewObject obj)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SHOW_STATUS, obj.Index);
        }
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

    protected void RefrashListDisp()
    {
        List<EquipGraphListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].SetInput(base.isInput);
            }
        }
        if (this.infoData2Label != null)
        {
            int num2 = this.baseCost + this.servantItemInfo.EquipCost;
            this.infoData2Label.text = string.Format(LocalizationManager.Get((num2 <= this.maxCost) ? "SUM_INFO" : "SUM_OVER_INFO"), num2, this.maxCost);
        }
    }

    protected void RequestListObject(EquipGraphListViewObject.InitMode mode)
    {
        List<EquipGraphListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (EquipGraphListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd));
            }
        }
        else
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0f);
        }
    }

    protected void RequestListObject(EquipGraphListViewObject.InitMode mode, float delay)
    {
        List<EquipGraphListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (EquipGraphListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd), delay);
            }
        }
        else
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", delay);
        }
    }

    public void SetMode(InitMode mode)
    {
        this.initMode = mode;
        this.callbackCount = base.ObjectSum;
        base.IsInput = mode == InitMode.INPUT;
        this.servantItemDraw.SetInput(base.IsInput);
        switch (mode)
        {
            case InitMode.VALID:
                this.RequestListObject(EquipGraphListViewObject.InitMode.VALID);
                break;

            case InitMode.INPUT:
                this.RequestListObject(EquipGraphListViewObject.InitMode.INPUT);
                break;

            case InitMode.MODIFY:
                this.RequestListObject(EquipGraphListViewObject.InitMode.MODIFY);
                break;
        }
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
        EquipGraphListViewObject obj2 = obj as EquipGraphListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(EquipGraphListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(EquipGraphListViewObject.InitMode.VALID);
        }
    }

    public List<EquipGraphListViewObject> ClippingObjectList
    {
        get
        {
            List<EquipGraphListViewObject> list = new List<EquipGraphListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    EquipGraphListViewObject component = obj2.GetComponent<EquipGraphListViewObject>();
                    EquipGraphListViewItem item = component.GetItem();
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

    public List<EquipGraphListViewObject> ObjectList
    {
        get
        {
            List<EquipGraphListViewObject> list = new List<EquipGraphListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    EquipGraphListViewObject component = obj2.GetComponent<EquipGraphListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(EquipGraphListViewManager.ResultKind kind, int result);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        MODIFY
    }

    public enum ResultKind
    {
        CANCEL,
        DECIDE,
        SHOW_STATUS
    }
}

