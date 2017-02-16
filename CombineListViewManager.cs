using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CombineListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;
    protected static CombineListItemInfo[] itemInfo = new CombineListItemInfo[] { new CombineListItemInfo(1, 1, "img_myroom_01", "MENU_OPERATION_FRIEND"), new CombineListItemInfo(2, 1, "img_myroom_02", "MENU_OPERATION_OFFER"), new CombineListItemInfo(3, 1, "img_myroom_03", "MENU_OPERATION_OFFERED") };
    protected static int[] normalKindList = new int[] { 1, 2, 3 };

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(Kind kind)
    {
        int[] normalKindList = null;
        if (kind == Kind.NORMAL)
        {
            normalKindList = CombineListViewManager.normalKindList;
        }
        int sum = (normalKindList == null) ? 0 : normalKindList.Length;
        base.CreateList(sum);
        int index = 0;
        for (int i = 0; i < sum; i++)
        {
            int num4 = normalKindList[i];
            CombineListItemInfo info = null;
            foreach (CombineListItemInfo info2 in itemInfo)
            {
                if (info2.kind == num4)
                {
                    info = info2;
                    break;
                }
            }
            if (info != null)
            {
                CombineListViewItem item = new CombineListViewItem(index, info) {
                    BasePosition = base.seed.GetLocalPosition(index)
                };
                base.itemList.Add(item);
                index++;
            }
        }
        if (index > 0)
        {
            CombineListViewItem item2 = base.itemList[0] as CombineListViewItem;
            item2.IsTermination = true;
            item2 = base.itemList[index - 1] as CombineListViewItem;
            item2.IsTermination = true;
        }
        base.ClippingItems(true, false);
        if (base.scrollView != null)
        {
            base.scrollView.ResetPosition();
        }
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public CombineListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as CombineListViewItem);
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
            CombineListViewObject obj2 = obj as CombineListViewObject;
            callbackFunc(obj2.GetItem().EventData);
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
                if ((this.initMode == InitMode.RETRY) && (base.scrollView != null))
                {
                    base.scrollView.Press(false);
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

    protected void RequestListObject(CombineListViewObject.InitMode mode)
    {
        List<CombineListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (CombineListViewObject obj2 in objectList)
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

    protected void RequestListObject(CombineListViewObject.InitMode mode, float delay)
    {
        List<CombineListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (CombineListViewObject obj2 in objectList)
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
            case InitMode.INPUT:
                this.RequestListObject(CombineListViewObject.InitMode.INPUT);
                break;

            case InitMode.INTO:
            {
                List<CombineListViewObject> clippingObjectList = this.ClippingObjectList;
                this.callbackCount = clippingObjectList.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    clippingObjectList[i].Init(CombineListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f * (i + 1));
                }
                break;
            }
            case InitMode.ENTER:
            {
                List<CombineListViewObject> list2 = this.ClippingObjectList;
                this.callbackCount = list2.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int j = 0; j < list2.Count; j++)
                {
                    list2[j].Init(CombineListViewObject.InitMode.ENTER, new System.Action(this.OnMoveEnd), 0.1f * (j + 1));
                }
                break;
            }
            case InitMode.RETRY:
            {
                List<CombineListViewObject> list3 = this.ClippingObjectList;
                this.callbackCount = list3.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int k = 0; k < list3.Count; k++)
                {
                    list3[k].Init(CombineListViewObject.InitMode.RETRY, new System.Action(this.OnMoveEnd), 0.1f);
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
        CombineListViewObject obj2 = obj as CombineListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(CombineListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(CombineListViewObject.InitMode.VALID);
        }
    }

    public List<CombineListViewObject> ClippingObjectList
    {
        get
        {
            List<CombineListViewObject> list = new List<CombineListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    CombineListViewObject component = obj2.GetComponent<CombineListViewObject>();
                    CombineListViewItem item = component.GetItem();
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

    public List<CombineListViewObject> ObjectList
    {
        get
        {
            List<CombineListViewObject> list = new List<CombineListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    CombineListViewObject component = obj2.GetComponent<CombineListViewObject>();
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
        RETRY
    }

    public enum Kind
    {
        NORMAL
    }
}

