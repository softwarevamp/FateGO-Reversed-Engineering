using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ReceiptListViewManager")]
public class ReceiptListViewManager : ListViewManager
{
    protected int callbackCount;
    protected int callbackIndex;
    protected InitMode initMode;

    protected event System.Action callbackFunc;

    public void CreateList()
    {
        string[] paymentHistoryList = SingletonMonoBehaviour<AccountingManager>.Instance.GetPaymentHistoryList();
        int length = paymentHistoryList.Length;
        base.CreateList(length);
        for (int i = 0; i < length; i++)
        {
            ReceiptListViewItem item = new ReceiptListViewItem(i, paymentHistoryList[i]);
            base.itemList.Add(item);
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public int GetClickResult() => 
        this.callbackIndex;

    public ReceiptListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ReceiptListViewItem);
        }
        return null;
    }

    public string GetNextName(string name)
    {
        ReceiptListViewItem item;
        for (int i = 0; i < (base.itemList.Count - 1); i++)
        {
            item = base.itemList[i] as ReceiptListViewItem;
            if (item.Path == name)
            {
                item = base.itemList[i + 1] as ReceiptListViewItem;
                return item.Path;
            }
        }
        item = base.itemList[0] as ReceiptListViewItem;
        return item.Path;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        this.callbackIndex = obj.Index;
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
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
                if (base.scrollView != null)
                {
                    base.scrollView.UpdateScrollbars(true);
                }
                if (!base.IsInput && (this.callbackFunc != null))
                {
                    System.Action callbackFunc = this.callbackFunc;
                    this.callbackFunc = null;
                    callbackFunc();
                }
            }
        }
    }

    protected void RequestListObject(ReceiptListViewObject.InitMode mode)
    {
        List<ReceiptListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ReceiptListViewObject obj2 in objectList)
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

    protected void RequestListObject(ReceiptListViewObject.InitMode mode, float delay)
    {
        List<ReceiptListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ReceiptListViewObject obj2 in objectList)
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
        this.SetMode(mode, null);
    }

    public void SetMode(InitMode mode, System.Action callback)
    {
        this.initMode = mode;
        this.callbackFunc = callback;
        this.callbackCount = base.ObjectSum;
        base.IsInput = mode == InitMode.INPUT;
        List<ReceiptListViewObject> objectList = this.ObjectList;
        switch (mode)
        {
            case InitMode.INTO:
                this.callbackCount = base.ObjectSum;
                if (this.callbackCount > 0)
                {
                    for (int i = 0; i < objectList.Count; i++)
                    {
                        objectList[i].Init(ReceiptListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f * (i + 1));
                    }
                }
                else
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                }
                break;

            case InitMode.INPUT:
                this.callbackIndex = -1;
                this.RequestListObject(ReceiptListViewObject.InitMode.INPUT);
                break;
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ReceiptListViewObject obj2 = obj as ReceiptListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ReceiptListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ReceiptListViewObject.InitMode.VALID);
        }
    }

    public List<ReceiptListViewObject> ObjectList
    {
        get
        {
            List<ReceiptListViewObject> list = new List<ReceiptListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ReceiptListViewObject component = obj2.GetComponent<ReceiptListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public enum InitMode
    {
        NONE,
        INTO,
        INPUT
    }
}

