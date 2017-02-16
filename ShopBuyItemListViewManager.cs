using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopBuyItemListViewManager : ListViewManager
{
    private const int AddHeight = 0x3a;
    protected int callbackCount;
    private const int DefaultCount = 4;
    protected int eventId;
    private int eventItemCount;
    protected InitMode initMode;
    protected Kind kind;
    private const int MaxCount = 10;
    [SerializeField]
    protected EventItemComponent shopEventItemDraw;
    [SerializeField]
    protected EventItemComponent[] shopEventItemDrawList;
    [SerializeField]
    protected UISprite shopEventItemWindow;
    private const int WindowBaseSize = 150;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(Kind kind)
    {
        ShopEntity[] enableEntitiyList;
        List<ShopEntity> list;
        this.kind = kind;
        this.eventId = 0;
        int index = 0;
        switch (kind)
        {
            case Kind.QP:
            case Kind.MANA:
            {
                ShopMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP);
                enableEntitiyList = null;
                list = new List<ShopEntity>();
                switch (kind)
                {
                    case Kind.QP:
                        enableEntitiyList = master.GetEnableEntitiyList(Purchase.Type.ALL, PayType.Type.QP);
                        goto Label_007B;

                    case Kind.MANA:
                        enableEntitiyList = master.GetEnableEntitiyList(Purchase.Type.ALL, PayType.Type.MANA);
                        goto Label_007B;
                }
                break;
            }
            case Kind.STONE:
            {
                ShopEntity[] entityArray3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).GetEnableEntitiyList(Purchase.Type.ALL, PayType.Type.STONE);
                base.CreateList(0);
                for (int i = 0; i < entityArray3.Length; i++)
                {
                    ShopBuyItemListViewItem item4 = new ShopBuyItemListViewItem(index, entityArray3[i]);
                    base.itemList.Add(item4);
                    index++;
                }
                goto Label_0327;
            }
            case Kind.BANK:
            {
                BankShopEntity[] entityArray4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BankShopMaster>(DataNameKind.Kind.BANK_SHOP).GetEnableEntitiyList();
                string[] productList = SingletonMonoBehaviour<AccountingManager>.Instance.GetProductList();
                base.CreateList(0);
                for (int j = 0; j < entityArray4.Length; j++)
                {
                    bool flag;
                    if (productList != null)
                    {
                        flag = false;
                        for (int k = 0; k < productList.Length; k++)
                        {
                            if (productList[k].Equals(entityArray4[j].googleShopId))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        ShopBuyItemListViewItem item5 = new ShopBuyItemListViewItem(index, entityArray4[j]);
                        base.itemList.Add(item5);
                        index++;
                    }
                }
                goto Label_0327;
            }
            case Kind.ACCOUNT:
            {
                StoneShopEntity[] entityArray5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<StoneShopMaster>(DataNameKind.Kind.STONE_SHOP).GetEnableEntitiyList();
                base.CreateList(0);
                for (int m = 0; m < entityArray5.Length; m++)
                {
                    ShopBuyItemListViewItem item6 = new ShopBuyItemListViewItem(index, entityArray5[m]);
                    base.itemList.Add(item6);
                    index++;
                }
                goto Label_0327;
            }
            default:
                goto Label_0327;
        }
    Label_007B:
        base.CreateList(0);
        foreach (ShopEntity entity in enableEntitiyList)
        {
            int purchaseShop = entity.GetPurchaseShop();
            if (entity.IsSoldOut())
            {
                list.Add(entity);
                continue;
            }
            if (purchaseShop > 0)
            {
                for (int n = 0; n < base.itemList.Count; n++)
                {
                    ShopBuyItemListViewItem item = base.itemList[n] as ShopBuyItemListViewItem;
                    if ((item.Shop != null) && (item.Shop.id == purchaseShop))
                    {
                        item.Modify(entity);
                        purchaseShop = 0;
                        break;
                    }
                }
            }
            else
            {
                purchaseShop = entity.id;
            }
            if (purchaseShop > 0)
            {
                ShopBuyItemListViewItem item2 = new ShopBuyItemListViewItem(index, entity);
                base.itemList.Add(item2);
                index++;
            }
        }
        foreach (ShopEntity entity2 in list)
        {
            ShopBuyItemListViewItem item3 = new ShopBuyItemListViewItem(index, entity2);
            base.itemList.Add(item3);
            index++;
        }
    Label_0327:
        base.emptyMessageLabel.text = LocalizationManager.Get("SHOP_LIST_EMPTY");
        base.SortItem(-1, false, 3);
    }

    public void CreateList(int eventId)
    {
        this.kind = Kind.EVENT;
        this.eventId = eventId;
        List<ShopEntity> list = new List<ShopEntity>();
        int index = 0;
        ShopMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP);
        ShopEntity[] enableEventEntitiyList = master.GetEnableEventEntitiyList(eventId);
        int[] eventItemList = master.GetEventItemList(eventId);
        this.eventItemCount = eventItemList.Length;
        this.RefreshEventItemInfo(eventItemList);
        base.CreateList(0);
        foreach (ShopEntity entity in enableEventEntitiyList)
        {
            int purchaseShop = entity.GetPurchaseShop();
            if (entity.IsSoldOut())
            {
                list.Add(entity);
                continue;
            }
            if (purchaseShop > 0)
            {
                for (int i = 0; i < base.itemList.Count; i++)
                {
                    ShopBuyItemListViewItem item = base.itemList[i] as ShopBuyItemListViewItem;
                    if ((item.Shop != null) && (item.Shop.id == purchaseShop))
                    {
                        item.Modify(entity);
                        purchaseShop = 0;
                        break;
                    }
                }
            }
            else
            {
                purchaseShop = entity.id;
            }
            if (purchaseShop > 0)
            {
                ShopBuyItemListViewItem item2 = new ShopBuyItemListViewItem(index, entity);
                base.itemList.Add(item2);
                index++;
            }
        }
        foreach (ShopEntity entity2 in list)
        {
            ShopBuyItemListViewItem item3 = new ShopBuyItemListViewItem(index, entity2);
            base.itemList.Add(item3);
            index++;
        }
        base.emptyMessageLabel.text = LocalizationManager.Get("SHOP_LIST_EMPTY");
        base.SortItem(-1, false, 3);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public ShopBuyItemListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ShopBuyItemListViewItem);
        }
        return null;
    }

    public bool ModifyList()
    {
        ShopMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP);
        ShopEntity[] enableEntitiyList = null;
        switch (this.kind)
        {
            case Kind.QP:
                enableEntitiyList = master.GetEnableEntitiyList(Purchase.Type.ALL, PayType.Type.QP);
                break;

            case Kind.MANA:
                enableEntitiyList = master.GetEnableEntitiyList(Purchase.Type.ALL, PayType.Type.MANA);
                break;

            case Kind.STONE:
                enableEntitiyList = master.GetEnableEntitiyList(Purchase.Type.ALL, PayType.Type.STONE);
                break;

            case Kind.EVENT:
            {
                enableEntitiyList = master.GetEnableEventEntitiyList(this.eventId);
                int[] eventItemList = master.GetEventItemList(this.eventId);
                this.RefreshEventItemInfo(eventItemList);
                break;
            }
        }
        if (enableEntitiyList != null)
        {
            for (int i = 0; i < enableEntitiyList.Length; i++)
            {
                ShopEntity shop = enableEntitiyList[i];
                int id = shop.id;
                if (id > 0)
                {
                    for (int j = 0; j < base.itemList.Count; j++)
                    {
                        ShopBuyItemListViewItem item = base.itemList[j] as ShopBuyItemListViewItem;
                        if ((item.Shop != null) && (item.Shop.id == id))
                        {
                            item.Modify(shop);
                            id = 0;
                            break;
                        }
                    }
                    if (shop.IsSoldOut())
                    {
                        id = 0;
                    }
                }
                if (id > 0)
                {
                    id = shop.GetPurchaseShop();
                    if (id > 0)
                    {
                        for (int k = 0; k < base.itemList.Count; k++)
                        {
                            ShopBuyItemListViewItem item2 = base.itemList[k] as ShopBuyItemListViewItem;
                            if ((item2.Shop != null) && (item2.Shop.id == id))
                            {
                                item2.Modify(shop);
                                id = 0;
                                break;
                            }
                        }
                    }
                    else
                    {
                        id = shop.id;
                    }
                }
                if (id > 0)
                {
                    this.CreateList(this.kind);
                    return false;
                }
            }
        }
        return true;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(obj.Index);
        }
    }

    protected void OnMoveEnd()
    {
        Debug.Log("OnMoveEnd " + this.callbackCount);
        if (this.callbackCount > 0)
        {
            this.callbackCount--;
            if (this.callbackCount == 0)
            {
                base.DragMaskEnd();
                if ((this.initMode == InitMode.INTO) && (base.scrollBar != null))
                {
                    base.scrollBar.gameObject.SetActive(true);
                }
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

    public void RefreshEventItemInfo(int[] eventItemList)
    {
        int length = eventItemList.Length;
        int num2 = this.shopEventItemDrawList.Length;
        if (length == 1)
        {
            this.shopEventItemWindow.height = 150;
            this.shopEventItemDraw.Set(eventItemList[0]);
            for (int i = 0; i < num2; i++)
            {
                this.shopEventItemDrawList[i].Clear();
            }
        }
        else
        {
            int num6;
            int num4 = length - 4;
            int num5 = 0;
            if ((length % 2) > 0)
            {
                num5++;
            }
            this.shopEventItemWindow.height = 150;
            if (length > 4)
            {
                this.shopEventItemWindow.height += ((num4 / 2) + num5) * 0x3a;
            }
            this.shopEventItemDraw.Clear();
            if (length == 2)
            {
                num6 = 6;
            }
            else
            {
                num6 = (10 - length) - num5;
            }
            for (int j = 0; j < num2; j++)
            {
                if ((j >= num6) && (j < (length + num6)))
                {
                    this.shopEventItemDrawList[j].Set(eventItemList[j - num6]);
                }
                else
                {
                    this.shopEventItemDrawList[j].Clear();
                }
            }
        }
    }

    protected void RequestInto()
    {
        base.ClippingItems(true, false);
        base.DragMaskStart();
        List<ShopBuyItemListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            ShopBuyItemListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(ShopBuyItemListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
            }
            else
            {
                this.callbackCount--;
            }
        }
        if (num == 0)
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0.6f);
        }
    }

    protected void RequestListObject(ShopBuyItemListViewObject.InitMode mode)
    {
        List<ShopBuyItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ShopBuyItemListViewObject obj2 in objectList)
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

    protected void RequestListObject(ShopBuyItemListViewObject.InitMode mode, float delay)
    {
        List<ShopBuyItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ShopBuyItemListViewObject obj2 in objectList)
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
                base.DragMaskStart();
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                base.emptyMessageBase.SetActive(false);
                base.Invoke("RequestInto", 0f);
                break;

            case InitMode.INPUT:
                this.RequestListObject(ShopBuyItemListViewObject.InitMode.INPUT);
                break;

            case InitMode.ENTER:
            {
                base.DragMaskStart();
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                base.emptyMessageBase.SetActive(false);
                List<ShopBuyItemListViewObject> clippingObjectList = this.ClippingObjectList;
                if (clippingObjectList.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0.6f);
                    break;
                }
                this.callbackCount = clippingObjectList.Count;
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    clippingObjectList[i].Init(ShopBuyItemListViewObject.InitMode.ENTER, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.EXIT:
            {
                base.DragMaskStart();
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                base.emptyMessageBase.SetActive(false);
                List<ShopBuyItemListViewObject> list2 = this.ClippingObjectList;
                if (list2.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0.6f);
                    break;
                }
                this.callbackCount = list2.Count;
                for (int j = 0; j < list2.Count; j++)
                {
                    list2[j].Init(ShopBuyItemListViewObject.InitMode.EXIT, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.MODIFY:
                this.RequestListObject(ShopBuyItemListViewObject.InitMode.MODIFY);
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
        ShopBuyItemListViewObject obj2 = obj as ShopBuyItemListViewObject;
        switch (this.initMode)
        {
            case InitMode.INPUT:
                obj2.Init(ShopBuyItemListViewObject.InitMode.INPUT);
                return;

            case InitMode.MODIFY:
                obj2.Init(ShopBuyItemListViewObject.InitMode.MODIFY);
                return;
        }
        obj2.Init(ShopBuyItemListViewObject.InitMode.VALID);
    }

    public List<ShopBuyItemListViewObject> ClippingObjectList
    {
        get
        {
            List<ShopBuyItemListViewObject> list = new List<ShopBuyItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ShopBuyItemListViewObject component = obj2.GetComponent<ShopBuyItemListViewObject>();
                    ShopBuyItemListViewItem item = component.GetItem();
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

    public int EventItemCount =>
        this.eventItemCount;

    public List<ShopBuyItemListViewObject> ObjectList
    {
        get
        {
            List<ShopBuyItemListViewObject> list = new List<ShopBuyItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ShopBuyItemListViewObject component = obj2.GetComponent<ShopBuyItemListViewObject>();
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
        INTO,
        INPUT,
        ENTER,
        EXIT,
        MODIFY
    }

    public enum Kind
    {
        QP,
        MANA,
        STONE,
        BANK,
        ACCOUNT,
        EVENT
    }
}

