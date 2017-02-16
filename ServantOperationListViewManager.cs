using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantOperationListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;
    private bool isInConfirm;
    protected int minimumKeep = 1;
    [SerializeField]
    protected ServantOperationManager parentManager;
    [SerializeField]
    protected UILabel selectDoneLabel;
    protected int selectSum;
    protected int sellEnableRestCnt;
    protected static ListViewSort servantEquipSortInfo = new ListViewSort("ServantOperation2", ListViewSort.SortKind.LEVEL, false);
    protected static ListViewSort servantSortInfo = new ListViewSort("ServantOperation1", ListViewSort.SortKind.LEVEL, false);
    protected const string SORT_SAVE_KEY = "ServantOperation";
    [SerializeField]
    protected UISprite sortExplanationSprite;
    protected Kind thisKind;

    protected event CallbackFunc callbackFunc;

    private void changeSellEnableRestCnt(bool isPlus, ServantOperationListViewItem item)
    {
        if (base.sort.IsServantEquip)
        {
            if (!item.IsOrganization)
            {
                if (isPlus)
                {
                    this.sellEnableRestCnt++;
                }
                else
                {
                    this.sellEnableRestCnt--;
                }
            }
        }
        else if (item.IsOrganization && item.IsSellEnableServant)
        {
            if (isPlus)
            {
                this.sellEnableRestCnt++;
            }
            else
            {
                this.sellEnableRestCnt--;
            }
        }
    }

    public void changeSortKindDisp()
    {
        this.SetSortButtonImage();
    }

    public void CreateList(Kind kind)
    {
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        UserServantEntity[] entityArray = null;
        string key = string.Empty;
        base.sort = (kind != Kind.SERVANT_EQUIP) ? servantSortInfo : servantEquipSortInfo;
        base.sort.Load();
        base.sort.SetServantEquip(kind == Kind.SERVANT_EQUIP);
        this.thisKind = kind;
        int index = 0;
        this.sellEnableRestCnt = 0;
        this.selectSum = 0;
        switch (kind)
        {
            case Kind.SERVANT:
                entityArray = master.getKeepServantList();
                key = "SERVANT_ALL_EMPTY";
                break;

            case Kind.SERVANT_EQUIP:
                entityArray = master.getServantEquipList();
                key = "SERVANT_EQUIP_EMPTY";
                break;

            default:
                entityArray = master.getList(entity.userId);
                key = "SERVANT_EQUIP_EMPTY";
                break;
        }
        base.CreateList(0);
        if (entityArray != null)
        {
            long[] numArray;
            long[] numArray2;
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getPartyList(out numArray, out numArray2, NetworkManager.UserId);
            for (int i = 0; i < entityArray.Length; i++)
            {
                long id = entityArray[i].id;
                ServantOperationListViewItem item = new ServantOperationListViewItem(index, entityArray[i], numArray, numArray2, id == entity.favoriteUserSvtId);
                base.itemList.Add(item);
                index++;
                this.changeSellEnableRestCnt(true, item);
            }
        }
        if (index > 0)
        {
            key = "SERVANT_SORT_FILTER_RESULT_EMPTY";
        }
        base.emptyMessageLabel.text = LocalizationManager.Get(key);
        base.SortItem(-1, false, -1);
        this.isInConfirm = false;
    }

    public void CreateList(Kind kind, long[] servantIdList)
    {
        long[] numArray;
        long[] numArray2;
        base.CreateList(0);
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int index = 0;
        this.sellEnableRestCnt = 1;
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getPartyList(out numArray, out numArray2, NetworkManager.UserId);
        foreach (long num2 in servantIdList)
        {
            UserServantEntity userServantEntity = master.getEntityFromId<UserServantEntity>(num2);
            ServantOperationListViewItem item = new ServantOperationListViewItem(index, userServantEntity, numArray, numArray2, num2 == entity.favoriteUserSvtId);
            base.itemList.Add(item);
            index++;
            this.sellEnableRestCnt++;
        }
        base.SortItem(-1, false, -1);
        this.RefrashListDisp();
        this.isInConfirm = true;
    }

    public void decrementNumber(int selectNum)
    {
        foreach (ListViewItem item in base.itemList)
        {
            if (item.SelectNum > selectNum)
            {
                item.SelectNum--;
            }
        }
    }

    public void DestroyList()
    {
        base.DestroyList();
        base.sort.Save();
    }

    protected void EndCloseSelectFilterKind()
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

    public long GetAmountSortValue(int svtId)
    {
        int count = base.itemList.Count;
        long num2 = 0L;
        for (int i = 0; i < count; i++)
        {
            ServantOperationListViewItem item = base.itemList[i] as ServantOperationListViewItem;
            if (item.SvtId == svtId)
            {
                num2 += 1L;
            }
        }
        return num2;
    }

    public void getSelectList(List<long> list)
    {
        foreach (ServantOperationListViewItem item in base.itemList)
        {
            if (item.IsSelect)
            {
                list.Add(item.UserSvtId);
            }
        }
    }

    public static void InitLoad()
    {
        servantSortInfo.InitLoad();
        servantEquipSortInfo.InitLoad();
    }

    public bool IsSelectEnable()
    {
        bool flag = false;
        if (this.parentManager.TotalSum < BalanceConfig.ServantSellSelectMax)
        {
            flag = true;
        }
        if (!base.sort.IsServantEquip && (this.sellEnableRestCnt <= this.minimumKeep))
        {
            flag = false;
        }
        return flag;
    }

    public void ModifyItem(long userSvtId)
    {
        if (base.itemList != null)
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            UserServantEntity entity2 = null;
            if (userSvtId != -1L)
            {
                entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
            }
            foreach (ListViewItem item in base.itemList)
            {
                ServantOperationListViewItem item2 = item as ServantOperationListViewItem;
                if ((entity2 != null) && (item2.UserSvtId == entity2.id))
                {
                    item2.setUserServantEntity(entity2);
                }
                item2.ModifyItem(item2.UserSvtId == entity.favoriteUserSvtId);
                if (item2.ViewObject != null)
                {
                    item2.ViewObject.SetItem(item2);
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

    protected void OnClickSelectListView(ServantOperationListViewItem selectItem)
    {
        if (this.isInConfirm)
        {
            this.OnLongPushListView(selectItem);
        }
        else if (selectItem.IsSelect)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.selectSum--;
            this.parentManager.setServant(false);
            this.selectDoneLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.parentManager.TotalSum, BalanceConfig.ServantSellSelectMax);
            this.changeSellEnableRestCnt(true, selectItem);
            this.parentManager.numberAdjustment(selectItem.SelectNum);
            selectItem.IsSelect = false;
            this.RefrashListDisp();
        }
        else if ((this.parentManager.TotalSum < BalanceConfig.ServantSellSelectMax) && (this.sellEnableRestCnt > 0))
        {
            if (selectItem.IsOrganization && !this.IsSelectEnable())
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                selectItem.SelectNum = this.parentManager.TotalSum;
                this.selectSum++;
                this.parentManager.setServant(true);
                this.selectDoneLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.parentManager.TotalSum, BalanceConfig.ServantSellSelectMax);
                this.changeSellEnableRestCnt(false, selectItem);
                this.RefrashListDisp();
            }
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
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
                    if (this.thisKind != Kind.SERVANT_EQUIP)
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
                    if (this.thisKind != Kind.SERVANT_EQUIP)
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

    protected void OnLongPushListView(ServantOperationListViewItem selectItem)
    {
        Debug.Log("ServantOperationListViewManager::OnLongPushListView" + selectItem.UserSvtId);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(selectItem.UserSvtId);
        }
    }

    protected void OnMoveEnd()
    {
        if (this.callbackCount > 0)
        {
            this.callbackCount--;
            if (this.callbackCount == 0)
            {
                base.DragMaskEnd();
                if (base.scrollView != null)
                {
                    base.scrollView.UpdateScrollbars(true);
                }
            }
        }
    }

    protected void RefrashListDisp()
    {
        this.parentManager.RefrashListDisp();
        List<ServantOperationListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].SetInput(base.isInput);
            }
        }
    }

    public void releaseAll()
    {
        this.sellEnableRestCnt = 0;
        foreach (ServantOperationListViewItem item in base.itemList)
        {
            item.IsSelect = false;
            this.changeSellEnableRestCnt(true, item);
        }
        this.selectSum = 0;
        this.RefrashListDisp();
    }

    protected void RequestInto()
    {
        base.ClippingItems(true, false);
        base.DragMaskStart();
        List<ServantOperationListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            ServantOperationListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(ServantOperationListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
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

    protected void RequestListObject(ServantOperationListViewObject.InitMode mode)
    {
        List<ServantOperationListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantOperationListViewObject obj2 in objectList)
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

    protected void RequestListObject(ServantOperationListViewObject.InitMode mode, float delay)
    {
        List<ServantOperationListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantOperationListViewObject obj2 in objectList)
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
            case InitMode.INPUT:
            {
                bool flag = false;
                foreach (ServantOperationListViewItem item in base.itemList)
                {
                    if (item.IsSelect && item.IsCanNotSelect)
                    {
                        this.selectSum--;
                        this.parentManager.setServant(false);
                        this.selectDoneLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.parentManager.TotalSum, BalanceConfig.ServantSellSelectMax);
                        this.changeSellEnableRestCnt(true, item);
                        this.parentManager.numberAdjustment(item.SelectNum);
                        item.IsSelect = false;
                        flag = true;
                    }
                }
                if (flag)
                {
                    this.RefrashListDisp();
                }
                if (base.IsInput)
                {
                    this.RequestListObject(ServantOperationListViewObject.InitMode.INPUT);
                }
                else
                {
                    this.RequestListObject(ServantOperationListViewObject.InitMode.VALID);
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

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ServantOperationListViewObject obj2 = obj as ServantOperationListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ServantOperationListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ServantOperationListViewObject.InitMode.VALID);
        }
    }

    protected override void SetSortButtonImage()
    {
        if (base.sortKindLabel != null)
        {
            base.sortKindLabel.text = base.sort.GetKindButtonText();
        }
        if (base.sortOrderSprite != null)
        {
            if ((base.sort.Kind == ListViewSort.SortKind.LOGIN_ACCESS) || (base.sort.Kind == ListViewSort.SortKind.CREATE))
            {
                this.sortExplanationSprite.spriteName = !base.sort.IsAscendingOrder ? "btn_txt_new" : "btn_txt_old";
                base.sortOrderSprite.spriteName = !base.sort.IsAscendingOrder ? "btn_sort_up" : "btn_sort_down";
            }
            else
            {
                this.sortExplanationSprite.spriteName = !base.sort.IsAscendingOrder ? "btn_txt_down" : "btn_txt_up";
                base.sortOrderSprite.spriteName = !base.sort.IsAscendingOrder ? "btn_sort_down" : "btn_sort_up";
            }
        }
    }

    public void sumItems(out int qp, out int mana)
    {
        qp = 0;
        mana = 0;
        foreach (ServantOperationListViewItem item in base.itemList)
        {
            if (item.IsSelect)
            {
                qp += item.UserServant.getSellQp();
                mana += item.UserServant.getSellMana();
            }
        }
    }

    public List<ServantOperationListViewObject> ClippingObjectList
    {
        get
        {
            List<ServantOperationListViewObject> list = new List<ServantOperationListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ServantOperationListViewObject component = obj2.GetComponent<ServantOperationListViewObject>();
                    ServantOperationListViewItem item = component.GetItem();
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

    public List<ServantOperationListViewObject> ObjectList
    {
        get
        {
            List<ServantOperationListViewObject> list = new List<ServantOperationListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ServantOperationListViewObject component = obj2.GetComponent<ServantOperationListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(long svtId);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        INTO,
        ENTER,
        EXIT
    }

    public enum Kind
    {
        ALL,
        SERVANT,
        SERVANT_EQUIP,
        SELL
    }

    public enum ResultKind
    {
        NONE,
        SELECT_LIST,
        SERVANT_STATUS
    }
}

