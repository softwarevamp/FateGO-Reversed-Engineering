using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopEventListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(Kind kind)
    {
        int index = 0;
        if (kind == Kind.NORMAL)
        {
            int[] openedEventIdList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).GetOpenedEventIdList();
            base.CreateList(0);
            for (int i = 0; i < openedEventIdList.Length; i++)
            {
                ShopEventListViewItem item = new ShopEventListViewItem(index, openedEventIdList[i]);
                base.itemList.Add(item);
                index++;
            }
        }
        base.emptyMessageLabel.text = LocalizationManager.Get("SHOP_EVENT_LIST_EMPTY");
        base.SortItem(-1, false, 3);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public ShopEventListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ShopEventListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
    }

    protected void OnClickListViewEvent(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickEvent " + obj.Index);
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
        List<ShopEventListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            ShopEventListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(ShopEventListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
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

    protected void RequestInto2()
    {
        base.ClippingItems(true, false);
        base.DragMaskStart();
        List<ShopEventListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            ShopEventListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(ShopEventListViewObject.InitMode.INTO2, new System.Action(this.OnMoveEnd), 0.1f);
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

    protected void RequestListObject(ShopEventListViewObject.InitMode mode)
    {
        List<ShopEventListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ShopEventListViewObject obj2 in objectList)
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

    protected void RequestListObject(ShopEventListViewObject.InitMode mode, float delay)
    {
        List<ShopEventListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ShopEventListViewObject obj2 in objectList)
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

    public ShopEventListViewItem SearchItem(int eventId)
    {
        int count = base.itemList.Count;
        for (int i = 0; i < count; i++)
        {
            ShopEventListViewItem item = base.itemList[i] as ShopEventListViewItem;
            if (item.EventId == eventId)
            {
                return item;
            }
        }
        return null;
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

            case InitMode.INTO2:
                base.DragMaskStart();
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                base.emptyMessageBase.SetActive(false);
                base.Invoke("RequestInto2", 0f);
                break;

            case InitMode.INPUT:
                this.RequestListObject(ShopEventListViewObject.InitMode.INPUT);
                break;

            case InitMode.ENTER:
            {
                base.DragMaskStart();
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                base.emptyMessageBase.SetActive(false);
                List<ShopEventListViewObject> clippingObjectList = this.ClippingObjectList;
                if (clippingObjectList.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0.6f);
                    break;
                }
                this.callbackCount = clippingObjectList.Count;
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    clippingObjectList[i].Init(ShopEventListViewObject.InitMode.ENTER, new System.Action(this.OnMoveEnd), 0.1f);
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
                List<ShopEventListViewObject> list2 = this.ClippingObjectList;
                if (list2.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0.6f);
                    break;
                }
                this.callbackCount = list2.Count;
                for (int j = 0; j < list2.Count; j++)
                {
                    list2[j].Init(ShopEventListViewObject.InitMode.EXIT, new System.Action(this.OnMoveEnd), 0.1f);
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
                List<ShopEventListViewObject> list3 = this.ClippingObjectList;
                this.callbackCount = list3.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int k = 0; k < list3.Count; k++)
                {
                    list3[k].Init(ShopEventListViewObject.InitMode.RETRY, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.ENTERED:
            {
                if (base.scrollBar != null)
                {
                    base.scrollBar.gameObject.SetActive(false);
                }
                List<ShopEventListViewObject> list4 = this.ClippingObjectList;
                this.callbackCount = list4.Count;
                for (int m = 0; m < list4.Count; m++)
                {
                    list4[m].Init(ShopEventListViewObject.InitMode.ENTERED);
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
        ShopEventListViewObject obj2 = obj as ShopEventListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ShopEventListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ShopEventListViewObject.InitMode.VALID);
        }
    }

    public List<ShopEventListViewObject> ClippingObjectList
    {
        get
        {
            List<ShopEventListViewObject> list = new List<ShopEventListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ShopEventListViewObject component = obj2.GetComponent<ShopEventListViewObject>();
                    ShopEventListViewItem item = component.GetItem();
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

    public List<ShopEventListViewObject> ObjectList
    {
        get
        {
            List<ShopEventListViewObject> list = new List<ShopEventListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ShopEventListViewObject component = obj2.GetComponent<ShopEventListViewObject>();
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
        INTO2,
        INPUT,
        ENTER,
        EXIT,
        RETRY,
        ENTERED
    }

    public enum Kind
    {
        NORMAL
    }
}

