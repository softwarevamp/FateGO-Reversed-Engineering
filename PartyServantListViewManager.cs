using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartyServantListViewManager : ListViewManager
{
    protected int callbackCount;
    [SerializeField]
    protected UILabel explanationLabel;
    [SerializeField]
    protected UILabel infoData2Label;
    [SerializeField]
    protected UILabel infoDataLabel;
    protected InitMode initMode;
    protected static ListViewSort servantSortInfo = new ListViewSort("PartyServant", ListViewSort.SortKind.LEVEL, false);
    protected const string SORT_SAVE_KEY = "PartyServant";

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(PartyListViewItem partyItem, int num)
    {
        base.sort = servantSortInfo;
        base.sort.Load();
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        UserServantEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getOrganizationList();
        this.infoDataLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), entityArray.Length, entity.svtKeep);
        this.infoData2Label.text = string.Format(LocalizationManager.Get("SUM_INFO"), partyItem.Cost, partyItem.MaxCost);
        this.explanationLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_SERVANT_SELECT_EXPLANATION");
        base.CreateList(0);
        for (int i = 0; i < entityArray.Length; i++)
        {
            PartyServantListViewItem item = new PartyServantListViewItem(i, entityArray[i], partyItem, num);
            base.itemList.Add(item);
        }
        base.emptyMessageLabel.text = LocalizationManager.Get("SERVANT_SORT_FILTER_RESULT_EMPTY");
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
        base.sort.Save();
    }

    protected void EndCloseSelectFilterKind()
    {
    }

    protected void EndCloseShowServant()
    {
        this.RequestListObject(PartyServantListViewObject.InitMode.INPUT);
    }

    public void EndSelectFilterKind(bool isDecide)
    {
        if (isDecide)
        {
            base.SortItem(-1, false, -1);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantFilterSelectMenu(new System.Action(this.EndCloseSelectFilterKind));
    }

    protected void EndShowServant(bool isDecide)
    {
        if (isDecide)
        {
            this.ModifyList();
            this.RequestListObject(PartyServantListViewObject.InitMode.MODIFY);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    public long GetAmountSortValue(int svtId)
    {
        int count = base.itemList.Count;
        long num2 = 0L;
        for (int i = 0; i < count; i++)
        {
            PartyServantListViewItem item = base.itemList[i] as PartyServantListViewItem;
            if (item.SvtId == svtId)
            {
                num2 += 1L;
            }
        }
        return num2;
    }

    public PartyServantListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as PartyServantListViewItem);
        }
        return null;
    }

    public void ModifyList()
    {
        UserServantEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getOrganizationList();
        for (int i = 0; i < entityArray.Length; i++)
        {
            long id = entityArray[i].id;
            for (int j = 0; j < base.itemList.Count; j++)
            {
                PartyServantListViewItem item = base.itemList[j] as PartyServantListViewItem;
                if ((item.UserServant != null) && (item.UserServant.id == id))
                {
                    item.Modify(entityArray[i]);
                    break;
                }
            }
        }
    }

    public void OnClickFilterKind()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantFilterSelectMenu(ServantFilterSelectMenu.Kind.SERVANT, base.sort, new ServantFilterSelectMenu.CallbackFunc(this.EndSelectFilterKind));
        }
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.DECIDE, obj.Index);
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
                    base.sort.SetKind(ListViewSort.SortKind.CLASS);
                    break;

                case ListViewSort.SortKind.RARITY:
                    base.sort.SetKind(ListViewSort.SortKind.AMOUNT);
                    break;

                case ListViewSort.SortKind.LEVEL:
                    base.sort.SetKind(ListViewSort.SortKind.HP);
                    break;

                case ListViewSort.SortKind.NP_LEVEL:
                    base.sort.SetKind(ListViewSort.SortKind.COST);
                    break;

                case ListViewSort.SortKind.HP:
                    base.sort.SetKind(ListViewSort.SortKind.ATK);
                    break;

                case ListViewSort.SortKind.ATK:
                    base.sort.SetKind(ListViewSort.SortKind.FRIENDSHIP);
                    break;

                case ListViewSort.SortKind.COST:
                    base.sort.SetKind(ListViewSort.SortKind.RARITY);
                    break;

                case ListViewSort.SortKind.CLASS:
                    base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    break;

                case ListViewSort.SortKind.FRIENDSHIP:
                    base.sort.SetKind(ListViewSort.SortKind.NP_LEVEL);
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
        Debug.Log("Manager ListView OnLongPush " + obj.Index);
        PartyServantListViewItem item = obj.GetItem() as PartyServantListViewItem;
        if (item.UserServant != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            if (item.IsParty)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.NORMAL, item.UserServant, item.GetEquipList(), new ServantStatusDialog.ClickDelegate(this.EndShowServant));
            }
            else
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.NORMAL, item.UserServant, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
            }
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            this.RequestListObject(PartyServantListViewObject.InitMode.INPUT);
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

    protected void RequestListObject(PartyServantListViewObject.InitMode mode)
    {
        List<PartyServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PartyServantListViewObject obj2 in objectList)
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

    protected void RequestListObject(PartyServantListViewObject.InitMode mode, float delay)
    {
        List<PartyServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PartyServantListViewObject obj2 in objectList)
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
        switch (mode)
        {
            case InitMode.VALID:
                this.RequestListObject(PartyServantListViewObject.InitMode.VALID);
                break;

            case InitMode.INPUT:
                this.RequestListObject(PartyServantListViewObject.InitMode.INPUT);
                break;

            case InitMode.ORGANIZATION_GUIDE_FIRST_SELECT:
            {
                List<PartyServantListViewObject> clippingObjectList = this.ClippingObjectList;
                if (clippingObjectList.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                this.callbackCount = clippingObjectList.Count;
                int num = -1;
                long id = 0L;
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    PartyServantListViewItem item = clippingObjectList[i].GetItem();
                    if (((item.UserServant != null) && !item.IsParty) && ((num < 0) || (id < item.UserServant.id)))
                    {
                        num = i;
                        id = item.UserServant.id;
                    }
                }
                for (int j = 0; j < clippingObjectList.Count; j++)
                {
                    PartyServantListViewObject obj3 = clippingObjectList[j];
                    if (j != num)
                    {
                        obj3.Init(PartyServantListViewObject.InitMode.VALID, new System.Action(this.OnMoveEnd));
                    }
                    else
                    {
                        obj3.Init(PartyServantListViewObject.InitMode.TUTORIAL_INPUT, new System.Action(this.OnMoveEnd));
                    }
                }
                break;
            }
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
        PartyServantListViewObject obj2 = obj as PartyServantListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(PartyServantListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(PartyServantListViewObject.InitMode.VALID);
        }
    }

    public List<PartyServantListViewObject> ClippingObjectList
    {
        get
        {
            List<PartyServantListViewObject> list = new List<PartyServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PartyServantListViewObject component = obj2.GetComponent<PartyServantListViewObject>();
                    PartyServantListViewItem item = component.GetItem();
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

    public List<PartyServantListViewObject> ObjectList
    {
        get
        {
            List<PartyServantListViewObject> list = new List<PartyServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PartyServantListViewObject component = obj2.GetComponent<PartyServantListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(PartyServantListViewManager.ResultKind kind, int result);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        ORGANIZATION_GUIDE_FIRST_SELECT
    }

    public enum ResultKind
    {
        NONE,
        CANCEL,
        DECIDE
    }
}

