using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FavoriteChangeListViewManager : ListViewManager
{
    protected int callbackCount;
    [SerializeField]
    protected UILabel infoDataLabel;
    protected InitMode initMode;
    protected static ListViewSort servantSortInfo = new ListViewSort("FavoriteChange1", ListViewSort.SortKind.LEVEL, false);
    protected const string SORT_SAVE_KEY = "FavoriteChange";

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList()
    {
        long[] numArray;
        long[] numArray2;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getPartyList(out numArray, out numArray2, NetworkManager.UserId);
        base.CreateList(0);
        int svtKeep = 0;
        string str = string.Empty;
        string key = "SERVANT_ALL_EMPTY";
        UserServantEntity[] collection = master.getOrganizationList();
        svtKeep = entity.svtKeep;
        str = LocalizationManager.Get("SERVANT_TAKE");
        key = "SERVANT_EQUIP_EMPTY";
        base.sort = servantSortInfo;
        base.sort.Load();
        base.sort.SetServantEquip(false);
        if (this.infoDataLabel != null)
        {
            this.infoDataLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), collection.Length, svtKeep);
        }
        long favoriteUserSvtId = entity.favoriteUserSvtId;
        List<UserServantEntity> list = new List<UserServantEntity>(collection);
        List<UserServantEntity> list2 = new List<UserServantEntity>();
        if (favoriteUserSvtId > 0L)
        {
            for (int j = 0; j < collection.Length; j++)
            {
                UserServantEntity entity2 = collection[j];
                if (entity2.id == favoriteUserSvtId)
                {
                    list2.Add(entity2);
                    list.Remove(entity2);
                }
            }
            list2.AddRange(list);
            collection = list2.ToArray();
        }
        for (int i = 0; i < collection.Length; i++)
        {
            long id = collection[i].id;
            FavoriteChangeListViewItem item = new FavoriteChangeListViewItem(i, collection[i], numArray, id == entity.favoriteUserSvtId);
            base.itemList.Add(item);
        }
        if (collection.Length > 0)
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

    protected void EndCloseSelectSortKind()
    {
    }

    protected void EndSelectSortKind(bool isDecide)
    {
        if (isDecide)
        {
            base.SortItem(-1, false, -1);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantSortSelectMenu(new System.Action(this.EndCloseSelectSortKind));
    }

    public long GetAmountSortValue(int svtId)
    {
        int count = base.itemList.Count;
        long num2 = 0L;
        for (int i = 0; i < count; i++)
        {
            FavoriteChangeListViewItem item = base.itemList[i] as FavoriteChangeListViewItem;
            if (item.SvtId == svtId)
            {
                num2 += 1L;
            }
        }
        return num2;
    }

    public FavoriteChangeListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as FavoriteChangeListViewItem);
        }
        return null;
    }

    public void ModifyItem()
    {
        if (base.itemList != null)
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            foreach (ListViewItem item in base.itemList)
            {
                FavoriteChangeListViewItem item2 = item as FavoriteChangeListViewItem;
                item2.ModifyItem(item2.UserServant.id == entity.favoriteUserSvtId);
                if (item2.ViewObject != null)
                {
                    item2.ViewObject.SetItem(item2);
                }
            }
        }
    }

    protected void OnClickListView(ListViewObject obj)
    {
    }

    protected void OnClickSelectListView(ListViewObject obj)
    {
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
            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantSortSelectMenu(ServantSortSelectMenu.Kind.SERVANT, base.sort, new ServantSortSelectMenu.CallbackFunc(this.EndSelectSortKind));
        }
    }

    protected void OnLongPushListView(ListViewObject obj)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
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

    protected void RequestListObject(FavoriteChangeListViewObject.InitMode mode)
    {
        List<FavoriteChangeListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (FavoriteChangeListViewObject obj2 in objectList)
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

    protected void RequestListObject(FavoriteChangeListViewObject.InitMode mode, float delay)
    {
        List<FavoriteChangeListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (FavoriteChangeListViewObject obj2 in objectList)
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
                this.RequestListObject(FavoriteChangeListViewObject.InitMode.VALID);
                break;

            case InitMode.INPUT:
                this.RequestListObject(FavoriteChangeListViewObject.InitMode.INPUT);
                break;

            case InitMode.MODIFY:
                this.RequestListObject(FavoriteChangeListViewObject.InitMode.MODIFY);
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
        FavoriteChangeListViewObject obj2 = obj as FavoriteChangeListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(FavoriteChangeListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(FavoriteChangeListViewObject.InitMode.VALID);
        }
    }

    public List<FavoriteChangeListViewObject> ClippingObjectList
    {
        get
        {
            List<FavoriteChangeListViewObject> list = new List<FavoriteChangeListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    FavoriteChangeListViewObject component = obj2.GetComponent<FavoriteChangeListViewObject>();
                    FavoriteChangeListViewItem item = component.GetItem();
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

    public List<FavoriteChangeListViewObject> ObjectList
    {
        get
        {
            List<FavoriteChangeListViewObject> list = new List<FavoriteChangeListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    FavoriteChangeListViewObject component = obj2.GetComponent<FavoriteChangeListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(FavoriteChangeListViewManager.ResultKind kind, int result);

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

