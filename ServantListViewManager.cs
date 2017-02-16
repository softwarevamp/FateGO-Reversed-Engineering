using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantListViewManager : ListViewManager
{
    protected int callbackCount;
    protected static ListViewSort equipSortInfo = new ListViewSort("Servant3", ListViewSort.SortKind.LEVEL, false);
    [SerializeField]
    protected UICommonButton filterButton;
    [SerializeField]
    protected UILabel infoDataLabel;
    [SerializeField]
    protected UILabel infoTitleLabel;
    protected InitMode initMode;
    protected Kind kind;
    protected static ListViewSort servantEquipSortInfo = new ListViewSort("Servant2", ListViewSort.SortKind.LEVEL, false);
    protected static ListViewSort servantSortInfo = new ListViewSort("Servant1", ListViewSort.SortKind.LEVEL, false);
    protected const string SORT_SAVE_KEY = "Servant";

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(Kind kind)
    {
        long[] numArray;
        long[] numArray2;
        UserServantEntity[] entityArray;
        this.kind = kind;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getPartyList(out numArray, out numArray2, NetworkManager.UserId);
        base.CreateList(0);
        int svtKeep = 0;
        string str = string.Empty;
        string key = "SERVANT_ALL_EMPTY";
        switch (kind)
        {
            case Kind.SERVANT:
                entityArray = master.getKeepServantList();
                svtKeep = entity.svtKeep;
                str = LocalizationManager.Get("SERVANT_TAKE");
                key = "SERVANT_EQUIP_EMPTY";
                base.sort = servantSortInfo;
                base.sort.Load();
                base.sort.SetServantEquip(false);
                break;

            case Kind.SERVANT_EQUIP:
                entityArray = master.getServantEquipList();
                svtKeep = entity.svtEquipKeep;
                str = LocalizationManager.Get("SERVANT_EQUIP_TAKE");
                key = "SERVANT_EQUIP_EMPTY";
                base.sort = servantEquipSortInfo;
                base.sort.Load();
                base.sort.SetServantEquip(true);
                break;

            default:
                entityArray = master.getOrganizationList();
                svtKeep = entity.svtKeep;
                str = LocalizationManager.Get("SERVANT_TAKE");
                key = "SERVANT_EQUIP_EMPTY";
                base.sort = equipSortInfo;
                base.sort.Load();
                base.sort.SetServantEquip(false);
                break;
        }
        if (this.infoTitleLabel != null)
        {
            this.infoTitleLabel.text = str;
        }
        if (this.infoDataLabel != null)
        {
            this.infoDataLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), entityArray.Length, svtKeep);
        }
        for (int i = 0; i < entityArray.Length; i++)
        {
            ServantListViewItem item = new ServantListViewItem(i, entityArray[i], numArray, numArray2);
            base.itemList.Add(item);
        }
        if (entityArray.Length > 0)
        {
            key = "SERVANT_SORT_FILTER_RESULT_EMPTY";
        }
        if (base.emptyMessageLabel != null)
        {
            base.emptyMessageLabel.text = LocalizationManager.Get(key);
        }
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

    protected void EndCloseSelectSortKind()
    {
    }

    public void EndSelectFilterKind(bool isDecide)
    {
        if (isDecide)
        {
            base.SortItem(-1, false, -1);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantFilterSelectMenu(new System.Action(this.EndCloseSelectFilterKind));
    }

    protected void EndSelectSortKind(bool isDecide)
    {
        if (isDecide)
        {
            base.SortItem(-1, false, -1);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantSortSelectMenu(new System.Action(this.EndCloseSelectSortKind));
    }

    public void filterButtonState(UICommonButtonColor.State state, bool animation)
    {
        this.filterButton.SetState(state, animation);
    }

    public long GetAmountSortValue(int svtId)
    {
        int count = base.itemList.Count;
        long num2 = 0L;
        for (int i = 0; i < count; i++)
        {
            ServantListViewItem item = base.itemList[i] as ServantListViewItem;
            if (item.SvtId == svtId)
            {
                num2 += 1L;
            }
        }
        return num2;
    }

    public ServantListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ServantListViewItem);
        }
        return null;
    }

    public void JumpItemUserId(long userSvtId)
    {
        if (userSvtId > 0L)
        {
            int count = base.itemList.Count;
            for (int i = 0; i < count; i++)
            {
                ServantListViewItem item = base.itemList[i] as ServantListViewItem;
                if ((item.UserServant != null) && (item.UserServant.id == userSvtId))
                {
                    base.JumpItem(item.Index);
                }
            }
        }
    }

    public void ModifyList()
    {
        long[] numArray;
        long[] numArray2;
        UserServantEntity[] entityArray;
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getPartyList(out numArray, out numArray2, NetworkManager.UserId);
        switch (this.kind)
        {
            case Kind.SERVANT:
                entityArray = master.getKeepServantList();
                break;

            case Kind.SERVANT_EQUIP:
                entityArray = master.getServantEquipList();
                break;

            default:
                entityArray = master.getOrganizationList();
                break;
        }
        for (int i = 0; i < entityArray.Length; i++)
        {
            long id = entityArray[i].id;
            for (int j = 0; j < base.itemList.Count; j++)
            {
                ServantListViewItem item = base.itemList[j] as ServantListViewItem;
                if ((item.UserServant != null) && (item.UserServant.id == id))
                {
                    item.Modify(entityArray[i], numArray, numArray2);
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
    }

    protected void OnClickSelectListView(ListViewObject obj)
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
                    if (this.kind != Kind.SERVANT_EQUIP)
                    {
                        base.sort.SetKind(ListViewSort.SortKind.CLASS);
                    }
                    else
                    {
                        base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    }
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
                    if (this.kind != Kind.SERVANT_EQUIP)
                    {
                        base.sort.SetKind(ListViewSort.SortKind.FRIENDSHIP);
                    }
                    else
                    {
                        base.sort.SetKind(ListViewSort.SortKind.COST);
                    }
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

    protected void RequestListObject(ServantListViewObject.InitMode mode)
    {
        List<ServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantListViewObject obj2 in objectList)
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

    protected void RequestListObject(ServantListViewObject.InitMode mode, float delay)
    {
        List<ServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantListViewObject obj2 in objectList)
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
                this.RequestListObject(ServantListViewObject.InitMode.VALID);
                break;

            case InitMode.INPUT:
                this.RequestListObject(ServantListViewObject.InitMode.INPUT);
                break;

            case InitMode.MODIFY:
                this.RequestListObject(ServantListViewObject.InitMode.MODIFY);
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
        ServantListViewObject obj2 = obj as ServantListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ServantListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ServantListViewObject.InitMode.VALID);
        }
    }

    public List<ServantListViewObject> ClippingObjectList
    {
        get
        {
            List<ServantListViewObject> list = new List<ServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ServantListViewObject component = obj2.GetComponent<ServantListViewObject>();
                    ServantListViewItem item = component.GetItem();
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

    public List<ServantListViewObject> ObjectList
    {
        get
        {
            List<ServantListViewObject> list = new List<ServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ServantListViewObject component = obj2.GetComponent<ServantListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(ServantListViewManager.ResultKind kind, int result);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        MODIFY
    }

    public enum Kind
    {
        EQUIP,
        SERVANT,
        SERVANT_EQUIP
    }

    public enum ResultKind
    {
        CANCEL,
        DECIDE,
        SHOW_STATUS
    }
}

