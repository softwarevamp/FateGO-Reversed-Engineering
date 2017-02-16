using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class FriendOperationItemListViewManager : ListViewManager
{
    protected int callbackCount;
    private ListViewSort.FilterKind[] classFilter;
    protected string friendCode;
    protected InitMode initMode;
    protected static bool isInitSystem;
    protected FriendStatus.Kind menuKind;
    protected const string SORT_SAVE_KEY = "FriendOperation";
    [SerializeField]
    protected UISprite sortExplanationSprite;
    protected static ListViewSort[] sortStatusList;
    [SerializeField]
    protected PlayMakerFSM targetFSM;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public FriendOperationItemListViewManager()
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

    public void CreateList(FriendStatus.Kind kind, int classPos = 0)
    {
        if (!isInitSystem)
        {
            sortStatusList = new ListViewSort[7];
            for (int i = 0; i < 7; i++)
            {
                sortStatusList[i] = new ListViewSort("FriendOperation" + (i + 1), ListViewSort.SortKind.LEVEL, false);
                base.sort = sortStatusList[i];
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
            }
            isInitSystem = true;
        }
        this.menuKind = kind;
        base.sort = sortStatusList[(int) kind];
        base.sort.Load();
        string key = "FRIEND_EMPTY_OFFERED_MESSAGE";
        if (kind == FriendStatus.Kind.SEARCH)
        {
            OtherUserGameEntity[] friendCodeList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<OtherUserGameMaster>(DataNameKind.Kind.OTHER_USER_GAME).GetFriendCodeList(this.friendCode);
            base.CreateList(0);
            for (int k = 0; k < friendCodeList.Length; k++)
            {
                FriendOperationItemListViewItem item = new FriendOperationItemListViewItem(kind, base.itemList.Count, friendCodeList[k], 0);
                base.itemList.Add(item);
            }
        }
        else
        {
            OtherUserGameEntity[] list = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TblFriendMaser>(DataNameKind.Kind.TBL_FRIEND).GetList(kind);
            base.CreateList(0);
            for (int m = 0; m < list.Length; m++)
            {
                FriendOperationItemListViewItem item2 = new FriendOperationItemListViewItem(kind, base.itemList.Count, list[m], classPos);
                base.itemList.Add(item2);
            }
            if (list.Length <= 0)
            {
                switch (kind)
                {
                    case FriendStatus.Kind.OFFERED:
                        key = "FRIEND_EMPTY_OFFERED_MESSAGE";
                        break;

                    case FriendStatus.Kind.FRIEND:
                        key = "FRIEND_EMPTY_FRIEND_MESSAGE";
                        break;
                }
            }
        }
        base.emptyMessageLabel.text = LocalizationManager.Get(key);
        base.SortItem(-1, false, 3);
    }

    public void CreateListFriendCode(string friendCode)
    {
        this.friendCode = friendCode;
        this.CreateList(FriendStatus.Kind.SEARCH, 0);
    }

    public void DestroyList()
    {
        base.DestroyList();
        base.sort.Save();
    }

    public FriendOperationItemListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as FriendOperationItemListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.NONE, obj.Index);
        }
    }

    protected void OnClickListViewAccept(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickAccept " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.ACCEPT, obj.Index);
        }
    }

    protected void OnClickListViewCancel(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickCancel " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.CANCEL, obj.Index);
        }
    }

    protected void OnClickListViewOffer(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickOffer " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.OFFER, obj.Index);
        }
    }

    protected void OnClickListViewReject(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickReject " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.REJECT, obj.Index);
        }
    }

    protected void OnClickListViewRemove(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickRemove " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.REMOVE, obj.Index);
        }
    }

    protected void OnClickSkill1ListView(ListViewObject obj)
    {
    }

    protected void OnClickSkill2ListView(ListViewObject obj)
    {
    }

    protected void OnClickSkill3ListView(ListViewObject obj)
    {
    }

    public void OnClickSortAscendingOrder()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            base.sort.SetAscendingOrder(!base.sort.IsAscendingOrder);
            base.SortItem(-1, false, 3);
        }
    }

    public void OnClickSortKind()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            ListViewSort.SortKind kind = base.sort.Kind;
            switch (kind)
            {
                case ListViewSort.SortKind.LEVEL:
                    base.sort.SetKind(ListViewSort.SortKind.HP);
                    break;

                case ListViewSort.SortKind.HP:
                    base.sort.SetKind(ListViewSort.SortKind.ATK);
                    break;

                case ListViewSort.SortKind.ATK:
                    base.sort.SetKind(ListViewSort.SortKind.LOGIN_ACCESS);
                    break;

                default:
                    if (kind == ListViewSort.SortKind.LOGIN_ACCESS)
                    {
                        base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    }
                    else
                    {
                        base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    }
                    break;
            }
            base.SortItem(-1, false, 3);
        }
    }

    protected void OnLongPushSkill1ListView(ListViewObject obj)
    {
        if (base.isInput)
        {
            Debug.Log("Manager ListView OnLongPushSkill1 " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SERVANT_SKILL1_STATUS, obj.Index);
            }
        }
    }

    protected void OnLongPushSkill2ListView(ListViewObject obj)
    {
        if (base.isInput)
        {
            Debug.Log("Manager ListView OnLongPushSkill2 " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SERVANT_SKILL2_STATUS, obj.Index);
            }
        }
    }

    protected void OnLongPushSkill3ListView(ListViewObject obj)
    {
        if (base.isInput)
        {
            Debug.Log("Manager ListView OnLongPushSkill3 " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SERVANT_SKILL3_STATUS, obj.Index);
            }
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
                if (this.initMode == InitMode.INTO)
                {
                    base.emptyMessageBase.SetActive(base.itemSortList.Count <= 0);
                }
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

    protected void RequestInto()
    {
        base.ClippingItems(true, false);
        base.DragMaskStart();
        List<FriendOperationItemListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            FriendOperationItemListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(FriendOperationItemListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
            }
            else
            {
                this.callbackCount--;
            }
        }
        if (num == 0)
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0.2f);
        }
    }

    protected void RequestListObject(FriendOperationItemListViewObject.InitMode mode)
    {
        List<FriendOperationItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (FriendOperationItemListViewObject obj2 in objectList)
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

    protected void RequestListObject(FriendOperationItemListViewObject.InitMode mode, float delay)
    {
        List<FriendOperationItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (FriendOperationItemListViewObject obj2 in objectList)
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
            case InitMode.INTO:
                base.emptyMessageBase.SetActive(false);
                base.Invoke("RequestInto", 0f);
                break;

            case InitMode.INPUT:
                this.RequestListObject(FriendOperationItemListViewObject.InitMode.INPUT);
                break;

            case InitMode.ENTER:
            {
                base.DragMaskStart();
                List<FriendOperationItemListViewObject> clippingObjectList = this.ClippingObjectList;
                if (clippingObjectList.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0.2f);
                    break;
                }
                this.callbackCount = clippingObjectList.Count;
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    clippingObjectList[i].Init(FriendOperationItemListViewObject.InitMode.ENTER, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.EXIT:
            {
                base.DragMaskStart();
                List<FriendOperationItemListViewObject> list2 = this.ClippingObjectList;
                if (list2.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0.2f);
                    break;
                }
                this.callbackCount = list2.Count;
                for (int j = 0; j < list2.Count; j++)
                {
                    list2[j].Init(FriendOperationItemListViewObject.InitMode.EXIT, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.MODIFY:
            {
                List<FriendOperationItemListViewObject> list3 = this.ClippingObjectList;
                if (list3.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0.2f);
                    break;
                }
                this.callbackCount = list3.Count;
                for (int k = 0; k < list3.Count; k++)
                {
                    list3[k].Init(FriendOperationItemListViewObject.InitMode.MODIFY, new System.Action(this.OnMoveEnd), 0.1f);
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
        FriendOperationItemListViewObject obj2 = obj as FriendOperationItemListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(FriendOperationItemListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(FriendOperationItemListViewObject.InitMode.VALID);
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
            }
            else
            {
                this.sortExplanationSprite.spriteName = !base.sort.IsAscendingOrder ? "btn_txt_down" : "btn_txt_up";
            }
            base.sortOrderSprite.spriteName = !base.sort.IsAscendingOrder ? "btn_sort_down" : "btn_sort_up";
        }
    }

    public List<FriendOperationItemListViewObject> ClippingObjectList
    {
        get
        {
            List<FriendOperationItemListViewObject> list = new List<FriendOperationItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    FriendOperationItemListViewObject component = obj2.GetComponent<FriendOperationItemListViewObject>();
                    FriendOperationItemListViewItem item = component.GetItem();
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

    public List<FriendOperationItemListViewObject> ObjectList
    {
        get
        {
            List<FriendOperationItemListViewObject> list = new List<FriendOperationItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    FriendOperationItemListViewObject component = obj2.GetComponent<FriendOperationItemListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(FriendOperationItemListViewManager.ResultKind kind, int result);

    public enum InitMode
    {
        NONE,
        INTO,
        INPUT,
        ENTER,
        EXIT,
        MODIFY
    }

    public enum ResultKind
    {
        NONE,
        SERVANT_SKILL1_STATUS,
        SERVANT_SKILL2_STATUS,
        SERVANT_SKILL3_STATUS,
        OFFER,
        ACCEPT,
        REJECT,
        CANCEL,
        REMOVE
    }
}

