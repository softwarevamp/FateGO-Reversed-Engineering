using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/DebugListViewManager")]
public class DebugListViewManager : ListViewManager
{
    protected int callbackCount;
    [SerializeField]
    protected FsmEventDataList debugDataList;
    protected InitMode initMode;

    protected event System.Action callbackFunc;

    public void CreateList()
    {
        int length = this.debugDataList.Length;
        base.CreateList(length);
        for (int i = 0; i < length; i++)
        {
            DebugListViewItem item = new DebugListViewItem(i, this.debugDataList.Get(i));
            base.itemList.Add(item);
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public DebugListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as DebugListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        if (this.debugDataList != null)
        {
            this.debugDataList.SendEvent(obj.Index);
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
                if (this.callbackFunc != null)
                {
                    System.Action callbackFunc = this.callbackFunc;
                    this.callbackFunc = null;
                    callbackFunc();
                }
            }
        }
    }

    protected void RequestListObject(DebugListViewObject.InitMode mode)
    {
        List<DebugListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (DebugListViewObject obj2 in objectList)
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

    protected void RequestListObject(DebugListViewObject.InitMode mode, float delay)
    {
        List<DebugListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (DebugListViewObject obj2 in objectList)
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
            this.RequestListObject(DebugListViewObject.InitMode.INPUT);
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        DebugListViewObject obj2 = obj as DebugListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(DebugListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(DebugListViewObject.InitMode.VALID);
        }
    }

    public List<DebugListViewObject> ObjectList
    {
        get
        {
            List<DebugListViewObject> list = new List<DebugListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    DebugListViewObject component = obj2.GetComponent<DebugListViewObject>();
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

