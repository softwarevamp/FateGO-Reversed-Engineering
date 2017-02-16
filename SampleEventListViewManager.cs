using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/Test2ListView/SampleEventListViewManager")]
public class SampleEventListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;

    protected event System.Action callbackFunc;

    public void CreateList(int sum)
    {
        base.CreateList(sum);
        for (int i = 0; i < sum; i++)
        {
            SampleEventListViewItem item = new SampleEventListViewItem(i);
            base.itemList.Add(item);
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public SampleEventListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as SampleEventListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
    }

    protected void OnClickListViewDetail(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickDetail " + obj.Index);
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
                if (this.callbackFunc != null)
                {
                    System.Action callbackFunc = this.callbackFunc;
                    this.callbackFunc = null;
                    callbackFunc();
                }
            }
        }
    }

    protected void RequestListObject(SampleEventListViewObject.InitMode mode)
    {
        foreach (SampleEventListViewObject obj2 in this.ObjectList)
        {
            obj2.Init(mode, new System.Action(this.OnMoveEnd));
        }
    }

    protected void RequestListObject(SampleEventListViewObject.InitMode mode, float delay)
    {
        foreach (SampleEventListViewObject obj2 in this.ObjectList)
        {
            obj2.Init(mode, new System.Action(this.OnMoveEnd), delay);
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
        switch (mode)
        {
            case InitMode.INTO:
            {
                List<SampleEventListViewObject> clippingObjectList = this.ClippingObjectList;
                this.callbackCount = clippingObjectList.Count + base.DropObjectSum;
                if (this.callbackCount > 0)
                {
                    for (int i = 0; i < clippingObjectList.Count; i++)
                    {
                        clippingObjectList[i].Init(SampleEventListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f * (i + 1));
                    }
                }
                else
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                }
                break;
            }
            case InitMode.INPUT:
                this.callbackCount = 0;
                this.RequestListObject(SampleEventListViewObject.InitMode.INPUT);
                break;
        }
        Debug.Log("SetMode " + mode);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        SampleEventListViewObject obj2 = obj as SampleEventListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(SampleEventListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(SampleEventListViewObject.InitMode.VALID);
        }
    }

    public List<SampleEventListViewObject> ClippingObjectList
    {
        get
        {
            List<SampleEventListViewObject> list = new List<SampleEventListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    SampleEventListViewObject component = obj2.GetComponent<SampleEventListViewObject>();
                    SampleEventListViewItem item = component.GetItem();
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

    public List<SampleEventListViewObject> ObjectList
    {
        get
        {
            List<SampleEventListViewObject> list = new List<SampleEventListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    SampleEventListViewObject component = obj2.GetComponent<SampleEventListViewObject>();
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

