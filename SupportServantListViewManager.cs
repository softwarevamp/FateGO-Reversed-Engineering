using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SupportServantListViewManager : ListViewManager
{
    protected int callbackCount;
    private ListViewSort.FilterKind[] classFilter;
    private int classPos;
    [SerializeField]
    protected UILabel explanationLabel;
    protected InitMode initMode;
    protected static ListViewSort servantSortInfo = new ListViewSort("SupportServantSelect", ListViewSort.SortKind.LEVEL, false);
    protected const string SORT_SAVE_KEY = "SupportServantSelect";
    protected SupportServantData supportServantData;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public SupportServantListViewManager()
    {
        ListViewSort.FilterKind[] kindArray1 = new ListViewSort.FilterKind[8];
        kindArray1[0] = ListViewSort.FilterKind.CLASS_ETC;
        kindArray1[2] = ListViewSort.FilterKind.CLASS_2_14;
        kindArray1[3] = ListViewSort.FilterKind.CLASS_3_15;
        kindArray1[4] = ListViewSort.FilterKind.CLASS_4_16;
        kindArray1[5] = ListViewSort.FilterKind.CLASS_5_17;
        kindArray1[6] = ListViewSort.FilterKind.CLASS_6_18;
        kindArray1[7] = ListViewSort.FilterKind.CLASS_7_19;
        this.classFilter = kindArray1;
    }

    public void CreateList(SupportServantData supportServantData, int classPos)
    {
        this.supportServantData = supportServantData;
        this.classPos = classPos;
        base.sort = servantSortInfo;
        base.sort.Load();
        if ((this.classPos != 0) && (base.sort.Kind == ListViewSort.SortKind.CLASS))
        {
            base.sort.SetKind(ListViewSort.SortKind.LEVEL);
        }
        base.sort.ClassFilterOFF();
        if (classPos == 0)
        {
            for (int j = 0; j < this.classFilter.Length; j++)
            {
                base.sort.SetFilter(this.classFilter[j], true);
            }
        }
        else
        {
            base.sort.SetFilter(this.classFilter[classPos], true);
        }
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        UserServantEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getOrganizationList();
        this.explanationLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_SERVANT_SELECT_EXPLANATION");
        base.CreateList(0);
        for (int i = 0; i < entityArray.Length; i++)
        {
            SupportServantListViewItem item = new SupportServantListViewItem(i, entityArray[i], supportServantData, classPos);
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

    protected void EndCloseSelectSortKind()
    {
    }

    protected void EndCloseShowServant()
    {
        this.RequestListObject(SupportServantListViewObject.InitMode.INPUT);
    }

    protected void EndSelectSortKind(bool isDecide)
    {
        if (isDecide)
        {
            base.SortItem(-1, false, -1);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantSortSelectMenu(new System.Action(this.EndCloseSelectSortKind));
    }

    protected void EndShowServant(bool isDecide)
    {
        if (isDecide)
        {
            this.ModifyList();
            this.RequestListObject(SupportServantListViewObject.InitMode.MODIFY);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    public long GetAmountSortValue(int svtId)
    {
        int count = base.itemList.Count;
        long num2 = 0L;
        for (int i = 0; i < count; i++)
        {
            SupportServantListViewItem item = base.itemList[i] as SupportServantListViewItem;
            if (item.SvtId == svtId)
            {
                num2 += 1L;
            }
        }
        return num2;
    }

    public SupportServantListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as SupportServantListViewItem);
        }
        return null;
    }

    public void ModifyItem()
    {
        this.RequestListObject(SupportServantListViewObject.InitMode.MODIFY);
    }

    public void ModifyList()
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        UserServantEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getOrganizationList();
        for (int i = 0; i < entityArray.Length; i++)
        {
            long id = entityArray[i].id;
            for (int j = 0; j < base.itemList.Count; j++)
            {
                SupportServantListViewItem item = base.itemList[j] as SupportServantListViewItem;
                if ((item.UserServant != null) && (item.UserServant.id == id))
                {
                    item.Modify(entityArray[i]);
                    break;
                }
            }
        }
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("SupportServantListViewManager::OnClickListView " + obj.Index);
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
                    if (this.classPos != 0)
                    {
                        base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    }
                    else
                    {
                        base.sort.SetKind(ListViewSort.SortKind.CLASS);
                    }
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

                case ListViewSort.SortKind.CLASS:
                    base.sort.SetKind(ListViewSort.SortKind.LEVEL);
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
        Debug.Log("SupportServantListViewManager::OnLongPush : " + obj.Index);
        SupportServantListViewItem item = obj.GetItem() as SupportServantListViewItem;
        UserServantEntity userServant = item.UserServant;
        if (userServant != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.PARTY, userServant, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            this.RequestListObject(SupportServantListViewObject.InitMode.INPUT);
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

    protected void RequestListObject(SupportServantListViewObject.InitMode mode)
    {
        List<SupportServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (SupportServantListViewObject obj2 in objectList)
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

    protected void RequestListObject(SupportServantListViewObject.InitMode mode, float delay)
    {
        List<SupportServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (SupportServantListViewObject obj2 in objectList)
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
        if (mode == InitMode.MODIFY)
        {
            this.ModifyItem();
        }
        else
        {
            this.initMode = mode;
            this.callbackCount = base.ObjectSum;
            base.IsInput = mode == InitMode.INPUT;
            switch (mode)
            {
                case InitMode.VALID:
                    this.RequestListObject(SupportServantListViewObject.InitMode.VALID);
                    break;

                case InitMode.INPUT:
                    this.RequestListObject(SupportServantListViewObject.InitMode.INPUT);
                    break;
            }
            Debug.Log("SupportServantListViewManager::SetMode " + mode);
        }
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
        SupportServantListViewObject obj2 = obj as SupportServantListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(SupportServantListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(SupportServantListViewObject.InitMode.VALID);
        }
    }

    public List<SupportServantListViewObject> ClippingObjectList
    {
        get
        {
            List<SupportServantListViewObject> list = new List<SupportServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    SupportServantListViewObject component = obj2.GetComponent<SupportServantListViewObject>();
                    SupportServantListViewItem item = component.GetItem();
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

    public List<SupportServantListViewObject> ObjectList
    {
        get
        {
            List<SupportServantListViewObject> list = new List<SupportServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    SupportServantListViewObject component = obj2.GetComponent<SupportServantListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(SupportServantListViewManager.ResultKind kind, int result);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        MODIFY
    }

    public enum ResultKind
    {
        NONE,
        CANCEL,
        DECIDE
    }
}

