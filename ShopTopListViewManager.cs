using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShopTopListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;
    protected static ShopTopItemInfo[] itemInfo = new ShopTopItemInfo[] { new ShopTopItemInfo(1, "SHOP_BUY_BANK_ITEM", "img_shop_1", "MENU_BUY_BANK_ITEM"), new ShopTopItemInfo(2, "SHOP_BUY_QP_ITEM", string.Empty, "MENU_BUY_QP_ITEM"), new ShopTopItemInfo(3, "SHOP_BUY_MANA_ITEM", "img_shop_3", "MENU_BUY_MANA_ITEM"), new ShopTopItemInfo(4, "SHOP_BUY_STONE_ITEM", "img_shop_7", "MENU_BUY_STONE_ITEM"), new ShopTopItemInfo(5, "SHOP_BUY_EVENT_ITEM", "img_shop_8", "MENU_EVENT"), new ShopTopItemInfo(6, "SHOP_BUY_SERVANT_FRAME", "img_shop_2", "MENU_BUY_SERVANT_FRAME"), new ShopTopItemInfo(7, "SHOP_BUY_SERVANT_EQUIP_FRAME", "img_shop_6", "MENU_BUY_SERVANT_EQUIP_FRAME"), new ShopTopItemInfo(8, "SHOP_SELL_SERVANT", "img_shop_4", "MENU_SELL_SERVANT"), new ShopTopItemInfo(9, "SHOP_RECOVER_USER_GAME_ACT", "img_shop_5", "MENU_RECOVER_USER_GAME_ACT"), new ShopTopItemInfo(10, "SHOP_NOAH", string.Empty, "MENU_NOAH"), new ShopTopItemInfo(11, "SHOP_BUY_ACCOUNT_ITEM", string.Empty, "MENU_BUY_ACCOUNT_ITEM"), new ShopTopItemInfo(12, string.Empty, "mask00", null) };
    protected static int[] normalKindList = new int[] { 5, 8, 3, 1, 4, 9, 12 };

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(Kind kind)
    {
        if ((NetworkManager.PlatformManagement[7] == 0) && (NetworkManager.PlatformManagement[9] == 0))
        {
            ShopTopListViewManager.normalKindList = new int[] { 5, 8, 3, 9, 12 };
        }
        else if (NetworkManager.PlatformManagement[9] == 0)
        {
            ShopTopListViewManager.normalKindList = new int[] { 5, 8, 3, 1, 9, 12 };
        }
        else if (NetworkManager.PlatformManagement[7] == 0)
        {
            ShopTopListViewManager.normalKindList = new int[] { 5, 8, 3, 4, 9, 12 };
        }
        int[] normalKindList = null;
        if (kind == Kind.NORMAL)
        {
            normalKindList = ShopTopListViewManager.normalKindList;
        }
        int sum = (normalKindList == null) ? 0 : normalKindList.Length;
        base.CreateList(sum);
        for (int i = 0; i < sum; i++)
        {
            int num3 = normalKindList[i];
            ShopTopItemInfo info = null;
            foreach (ShopTopItemInfo info2 in itemInfo)
            {
                if (info2.kind == num3)
                {
                    info = info2;
                    break;
                }
            }
            if (info != null)
            {
                ShopEntity[] entityArray = null;
                bool isUse = true;
                switch (((ItemKind) num3))
                {
                    case ItemKind.SHOP_RECOVER_USER_GAME_ACT:
                        if (BalanceConfig.UserGameActRecoverMenuDispFlg <= 0)
                        {
                            info = null;
                        }
                        break;
                }
                if ((entityArray != null) && (entityArray.Length <= 0))
                {
                    info = null;
                }
                if (info != null)
                {
                    ShopTopListViewItem item = new ShopTopListViewItem(base.itemList.Count, info, isUse);
                    base.itemList.Add(item);
                }
            }
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public ShopTopListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ShopTopListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        ShopTopListViewObject obj2 = obj as ShopTopListViewObject;
        if ((obj2.GetItem().EventData != null) && (this.callbackFunc != null))
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc(obj2.GetItem().EventData);
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
                if ((this.initMode == InitMode.INTO) || (this.initMode == InitMode.RETRY))
                {
                    if (base.scrollBar != null)
                    {
                        base.scrollBar.gameObject.SetActive(true);
                    }
                    if (base.scrollView != null)
                    {
                        base.scrollView.Press(false);
                    }
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

    protected void RequestListObject(ShopTopListViewObject.InitMode mode)
    {
        List<ShopTopListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ShopTopListViewObject obj2 in objectList)
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

    protected void RequestListObject(ShopTopListViewObject.InitMode mode, float delay)
    {
        List<ShopTopListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ShopTopListViewObject obj2 in objectList)
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

    public void SetMode(InitMode mode, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.SetMode(mode, 0);
    }

    public void SetMode(InitMode mode, System.Action callback)
    {
        this.callbackFunc2 = callback;
        this.SetMode(mode, 0);
    }

    public void SetMode(InitMode mode, int index = 0)
    {
        this.initMode = mode;
        this.callbackCount = base.ObjectSum;
        base.IsInput = mode == InitMode.INPUT;
        switch (mode)
        {
            case InitMode.INPUT:
                this.RequestListObject(ShopTopListViewObject.InitMode.INPUT);
                break;

            case InitMode.INTO:
            {
                base.DragMaskStart();
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                List<ShopTopListViewObject> clippingObjectList = this.ClippingObjectList;
                this.callbackCount = clippingObjectList.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    clippingObjectList[i].Init(ShopTopListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.ENTER:
            {
                base.DragMaskStart();
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                List<ShopTopListViewObject> list2 = this.ClippingObjectList;
                this.callbackCount = list2.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int j = 0; j < list2.Count; j++)
                {
                    list2[j].Init(ShopTopListViewObject.InitMode.ENTER, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.RETRY:
            {
                base.DragMaskStart();
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                List<ShopTopListViewObject> list3 = this.ClippingObjectList;
                this.callbackCount = list3.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int k = 0; k < list3.Count; k++)
                {
                    list3[k].Init(ShopTopListViewObject.InitMode.RETRY, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.ENTERED:
            {
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                List<ShopTopListViewObject> list4 = this.ClippingObjectList;
                this.callbackCount = list4.Count;
                for (int m = 0; m < list4.Count; m++)
                {
                    list4[m].Init(ShopTopListViewObject.InitMode.ENTERED);
                }
                break;
            }
        }
        Debug.Log("SetMode " + mode);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ShopTopListViewObject obj2 = obj as ShopTopListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ShopTopListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ShopTopListViewObject.InitMode.VALID);
        }
    }

    public List<ShopTopListViewObject> ClippingObjectList
    {
        get
        {
            List<ShopTopListViewObject> list = new List<ShopTopListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ShopTopListViewObject component = obj2.GetComponent<ShopTopListViewObject>();
                    ShopTopListViewItem item = component.GetItem();
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

    public List<ShopTopListViewObject> ObjectList
    {
        get
        {
            List<ShopTopListViewObject> list = new List<ShopTopListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ShopTopListViewObject component = obj2.GetComponent<ShopTopListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(string result);

    public enum InitMode
    {
        NONE,
        INPUT,
        INTO,
        ENTER,
        EXIT,
        RETRY,
        ENTERED
    }

    public enum ItemKind
    {
        SHOP_BLANK = 12,
        SHOP_BUY_ACCOUNT_ITEM = 11,
        SHOP_BUY_BANK_ITEM = 1,
        SHOP_BUY_EVENT_ITEM = 5,
        SHOP_BUY_MANA_ITEM = 3,
        SHOP_BUY_QP_ITEM = 2,
        SHOP_BUY_SERVANT_EQUIP_FRAME = 7,
        SHOP_BUY_SERVANT_FRAME = 6,
        SHOP_BUY_STONE_ITEM = 4,
        SHOP_NOAH = 10,
        SHOP_RECOVER_USER_GAME_ACT = 9,
        SHOP_SELL_SERVANT = 8
    }

    public enum Kind
    {
        NORMAL
    }
}

