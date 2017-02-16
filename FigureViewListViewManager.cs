using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/FigureViewListViewManager")]
public class FigureViewListViewManager : ListViewManager
{
    protected int callbackCount;
    protected int callbackIndex;
    protected InitMode initMode;

    protected event System.Action callbackFunc;

    public void CreateList(string[] list)
    {
        int length = list.Length;
        base.CreateList(length);
        for (int i = 0; i < length; i++)
        {
            FigureViewListViewItem item = new FigureViewListViewItem(i, list[i]);
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

    public FigureViewListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as FigureViewListViewItem);
        }
        return null;
    }

    public string GetNextName(string name)
    {
        FigureViewListViewItem item;
        for (int i = 0; i < (base.itemList.Count - 1); i++)
        {
            item = base.itemList[i] as FigureViewListViewItem;
            if (item.Path == name)
            {
                item = base.itemList[i + 1] as FigureViewListViewItem;
                return item.Path;
            }
        }
        item = base.itemList[0] as FigureViewListViewItem;
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

    protected void RequestListObject(FigureViewListViewObject.InitMode mode)
    {
        List<FigureViewListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (FigureViewListViewObject obj2 in objectList)
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

    protected void RequestListObject(FigureViewListViewObject.InitMode mode, float delay)
    {
        List<FigureViewListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (FigureViewListViewObject obj2 in objectList)
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
        if (mode == InitMode.INPUT)
        {
            this.callbackIndex = -1;
            this.RequestListObject(FigureViewListViewObject.InitMode.INPUT);
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        FigureViewListViewObject obj2 = obj as FigureViewListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(FigureViewListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(FigureViewListViewObject.InitMode.VALID);
        }
    }

    public List<FigureViewListViewObject> ObjectList
    {
        get
        {
            List<FigureViewListViewObject> list = new List<FigureViewListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    FigureViewListViewObject component = obj2.GetComponent<FigureViewListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public enum InitMode
    {
        NONE,
        INPUT
    }
}

