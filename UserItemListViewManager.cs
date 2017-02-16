using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserItemListViewManager : ListViewManager
{
    [CompilerGenerated]
    private static Comparison<UserItemData> <>f__am$cacheE;
    protected int callbackCount;
    [SerializeField]
    protected UILabel infoLb;
    protected InitMode initMode;
    [SerializeField]
    protected ItemDetailInfoComponent itemDetailInfoComp;
    private int manaNum;
    [SerializeField]
    protected PlayMakerFSM myRoomFsm;
    [SerializeField]
    protected GameObject nonItemNoticeInfo;
    [SerializeField]
    protected UILabel nonItemNoticeLb;
    private UserItemListViewItem selectItem;
    private int stoneNum;
    private UserGameEntity userGameEntity;
    private List<UserItemData> usrItemList;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void closeItemDetail()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.itemDetailInfoComp.Close();
    }

    public void CreateList()
    {
        this.usrItemList = new List<UserItemData>();
        this.infoLb.text = LocalizationManager.Get("HEADER_NOTICE_MSG");
        base.gameObject.SetActive(true);
        base.CreateList(0);
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.stoneNum = this.userGameEntity.stone;
        this.manaNum = this.userGameEntity.mana;
        if (this.stoneNum > 0)
        {
            this.SetUsrItemDataByType(2);
        }
        if (this.manaNum > 0)
        {
            this.SetUsrItemDataByType(5);
        }
        UserItemEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getList();
        if (entityArray != null)
        {
            foreach (UserItemEntity entity in entityArray)
            {
                this.SetUsrItemData(entity);
            }
        }
        if (<>f__am$cacheE == null)
        {
            <>f__am$cacheE = (a, b) => a.dispPriority - b.dispPriority;
        }
        this.usrItemList.Sort(<>f__am$cacheE);
        int count = this.usrItemList.Count;
        if (count > 0)
        {
            this.nonItemNoticeLb.gameObject.SetActive(false);
            for (int i = 0; i < count; i++)
            {
                UserItemListViewItem item = new UserItemListViewItem(this.usrItemList[i]);
                base.itemList.Add(item);
            }
            base.SortItem(-1, false, -1);
        }
        else
        {
            this.nonItemNoticeLb.text = LocalizationManager.Get("NONITEM_NOTICE");
            this.nonItemNoticeLb.gameObject.SetActive(true);
        }
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public UserItemListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as UserItemListViewItem);
        }
        return null;
    }

    protected void OnClickListView(UserItemListViewObject obj)
    {
        this.selectItem = obj.GetItem();
        this.myRoomFsm.SendEvent("SELECT_ITEM");
    }

    protected void OnMoveEnd()
    {
        Debug.Log("OnMoveEnd " + this.callbackCount);
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

    public void openItemDetail()
    {
        if (this.selectItem.ItemType == 2)
        {
            this.itemDetailInfoComp.ShowStoneDetail(this.selectItem.userItemData.name, this.selectItem.userItemData.detail, this.userGameEntity, new ItemDetailInfoComponent.CallbackFunc(this.SelectItemDetail));
        }
        else
        {
            this.itemDetailInfoComp.OpenUserItemInfo(this.selectItem.userItemData, new ItemDetailInfoComponent.CallbackFunc(this.SelectItemDetail));
        }
    }

    protected void RequestListObject(UserItemListViewObject.InitMode mode)
    {
        List<UserItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (UserItemListViewObject obj2 in objectList)
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

    protected void RequestListObject(UserItemListViewObject.InitMode mode, float delay)
    {
        List<UserItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (UserItemListViewObject obj2 in objectList)
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

    private void SelectItemDetail(bool isDecide)
    {
        this.myRoomFsm.SendEvent("CLOSE_DETAIL");
    }

    public void SetMode(InitMode mode)
    {
        this.initMode = mode;
        this.callbackCount = base.objectList.Count;
        base.IsInput = mode == InitMode.INPUT;
        if (mode == InitMode.INPUT)
        {
            this.RequestListObject(UserItemListViewObject.InitMode.INPUT);
        }
        Debug.Log("SetMode " + mode);
    }

    public void SetMode(InitMode mode, System.Action callback)
    {
        this.callbackFunc2 = callback;
        this.SetMode(mode);
    }

    public void SetMode(InitMode mode, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.SetMode(mode);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        UserItemListViewObject obj2 = obj as UserItemListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(UserItemListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(UserItemListViewObject.InitMode.VALID);
        }
    }

    private void SetUsrItemData(UserItemEntity data)
    {
        ItemEntity entity = data.getItemInfo();
        UserItemData item = null;
        if ((((entity != null) && (data.num > 0)) && ((entity.type != 1) && (entity.type != 2))) && (entity.type != 13))
        {
            item = new UserItemData {
                type = entity.type,
                itemId = entity.id,
                dispPriority = entity.priority,
                itemImgId = entity.imageId,
                name = entity.name,
                detail = entity.detail,
                num = data.num
            };
            this.usrItemList.Add(item);
        }
    }

    private void SetUsrItemDataByType(int itemType)
    {
        ItemEntity entityByType = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).GetEntityByType(itemType);
        UserItemData item = null;
        if (entityByType != null)
        {
            item = new UserItemData {
                type = entityByType.type,
                itemId = entityByType.id,
                dispPriority = entityByType.priority,
                itemImgId = entityByType.imageId,
                name = entityByType.name,
                detail = entityByType.detail
            };
            if (itemType == 2)
            {
                item.num = this.stoneNum;
            }
            if (itemType == 5)
            {
                item.num = this.manaNum;
            }
        }
        this.usrItemList.Add(item);
    }

    public List<UserItemListViewObject> ClippingObjectList
    {
        get
        {
            List<UserItemListViewObject> list = new List<UserItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    UserItemListViewObject component = obj2.GetComponent<UserItemListViewObject>();
                    UserItemListViewItem item = component.GetItem();
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

    public List<UserItemListViewObject> ObjectList
    {
        get
        {
            List<UserItemListViewObject> list = new List<UserItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    UserItemListViewObject component = obj2.GetComponent<UserItemListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(int result);

    public enum InitMode
    {
        NONE,
        INPUT
    }

    public enum Type
    {
        MANA = 3,
        SKILLUP = 4,
        STONE = 1,
        TDUP = 5,
        TICKET = 2
    }
}

