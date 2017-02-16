using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/TestListView/SampleCardListViewManager")]
public class SampleCardListViewManager : ListViewManager
{
    protected int callbackCount;
    protected static int[] deckData = new int[] { -1, 2, 4 };
    protected InitMode initMode;

    protected event System.Action callbackFunc;

    public void CreateList(int sum)
    {
        base.CreateList(sum);
        for (int i = 0; i < sum; i++)
        {
            SampleCardListViewItem item = new SampleCardListViewItem(i) {
                CardId = (i % 3) + 1
            };
            for (int j = 0; j < deckData.Length; j++)
            {
                if (i == deckData[j])
                {
                    item.IsDeck = true;
                    break;
                }
            }
            base.itemList.Add(item);
        }
        base.SortItem(-1, false, -1);
        if (base.dropObjectList != null)
        {
            for (int k = 0; k < base.dropObjectList.Count; k++)
            {
                SampleCardListViewDropObject obj2 = base.dropObjectList[k] as SampleCardListViewDropObject;
                if (obj2 != null)
                {
                    int num4 = deckData[k];
                    obj2.SetItem((num4 < 0) ? null : base.itemList[num4]);
                    obj2.SetManager(this);
                    obj2.SetDragPrefab(base.dropDragPrefab);
                }
            }
        }
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public SampleCardListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as SampleCardListViewItem);
        }
        return null;
    }

    public bool IsDropDropSurface(ListViewDropInfo info)
    {
        if (info.DropSurfaceObject != null)
        {
            SampleCardListViewDropObject component = info.ListViewObject.GetComponent<SampleCardListViewDropObject>();
            SampleCardUIDragDropListViewSurface surface = info.DropSurfaceObject.GetComponent<SampleCardUIDragDropListViewSurface>();
            if (((component != null) && (surface != null)) && (component.GetItem() != null))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsItemDropSurface(ListViewDropInfo info)
    {
        if (info.DropSurfaceObject != null)
        {
            SampleCardListViewObject component = info.ListViewObject.GetComponent<SampleCardListViewObject>();
            SampleCardUIDragDropListViewSurface surface = info.DropSurfaceObject.GetComponent<SampleCardUIDragDropListViewSurface>();
            if ((component != null) && (surface != null))
            {
                SampleCardListViewItem item = component.GetItem();
                if ((item != null) && !item.IsDeck)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
    }

    private void OnMoveEnd()
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

    private void RequestDropObject(SampleCardListViewDropObject.InitMode mode)
    {
        if (base.dropObjectList != null)
        {
            foreach (SampleCardListViewDropObject obj2 in base.dropObjectList)
            {
                if (obj2 != null)
                {
                    obj2.Init(mode, new System.Action(this.OnMoveEnd));
                }
            }
        }
    }

    private void RequestDropObject(SampleCardListViewDropObject.InitMode mode, float delay)
    {
        if (base.dropObjectList != null)
        {
            foreach (SampleCardListViewDropObject obj2 in base.dropObjectList)
            {
                if (obj2 != null)
                {
                    obj2.Init(mode, new System.Action(this.OnMoveEnd), delay);
                }
            }
        }
    }

    private void RequestListObject(SampleCardListViewObject.InitMode mode)
    {
        List<SampleCardListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (SampleCardListViewObject obj2 in objectList)
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

    private void RequestListObject(SampleCardListViewObject.InitMode mode, float delay)
    {
        List<SampleCardListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (SampleCardListViewObject obj2 in objectList)
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
        switch (mode)
        {
            case InitMode.INTO:
            {
                List<SampleCardListViewObject> clippingObjectList = this.ClippingObjectList;
                this.callbackCount = clippingObjectList.Count + base.DropObjectSum;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    clippingObjectList[i].Init(SampleCardListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.25f * (i + 1));
                }
                if (base.dropObjectList != null)
                {
                    for (int j = 0; j < base.dropObjectList.Count; j++)
                    {
                        SampleCardListViewDropObject obj3 = base.dropObjectList[j] as SampleCardListViewDropObject;
                        if (obj3 != null)
                        {
                            obj3.Init(SampleCardListViewDropObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.25f * (base.dropObjectList.Count - j));
                        }
                    }
                }
                break;
            }
            case InitMode.INTO_TURN:
                this.callbackCount = 0;
                this.RequestListObject(SampleCardListViewObject.InitMode.TURN);
                this.RequestDropObject(SampleCardListViewDropObject.InitMode.TURN);
                break;

            case InitMode.INPUT:
                this.callbackCount = 0;
                this.RequestListObject(SampleCardListViewObject.InitMode.INPUT);
                this.RequestDropObject(SampleCardListViewDropObject.InitMode.INPUT);
                break;
        }
        Debug.Log("SetMode " + mode);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        SampleCardListViewObject obj2 = obj as SampleCardListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.IsFront = true;
            obj2.Init(SampleCardListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(SampleCardListViewObject.InitMode.VALID);
        }
    }

    public List<SampleCardListViewObject> ClippingObjectList
    {
        get
        {
            List<SampleCardListViewObject> list = new List<SampleCardListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    SampleCardListViewObject component = obj2.GetComponent<SampleCardListViewObject>();
                    SampleCardListViewItem item = component.GetItem();
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

    public List<SampleCardListViewObject> ObjectList
    {
        get
        {
            List<SampleCardListViewObject> list = new List<SampleCardListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    SampleCardListViewObject component = obj2.GetComponent<SampleCardListViewObject>();
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
        INTO_TURN,
        INPUT
    }
}

