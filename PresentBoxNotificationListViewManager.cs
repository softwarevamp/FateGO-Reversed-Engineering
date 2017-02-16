using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PresentBoxNotificationListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;
    [SerializeField]
    protected PlayMakerFSM targetFSM;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(Kind kind, UserPresentBoxEntity[] presentList)
    {
        int index = 0;
        if (kind == Kind.NORMAL)
        {
            base.CreateList(0);
            if (presentList != null)
            {
                for (int i = 0; i < presentList.Length; i++)
                {
                    if (presentList[i] != null)
                    {
                        PresentBoxNotificationListViewItem item = new PresentBoxNotificationListViewItem(index, presentList[i]);
                        base.itemList.Add(item);
                        index++;
                    }
                }
            }
        }
        base.scrollView.contentPivot = (base.itemList.Count <= 4) ? UIWidget.Pivot.Center : UIWidget.Pivot.Top;
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public PresentBoxNotificationListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as PresentBoxNotificationListViewItem);
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

    protected void RequestListObject(PresentBoxNotificationListViewObject.InitMode mode)
    {
        List<PresentBoxNotificationListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PresentBoxNotificationListViewObject obj2 in objectList)
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

    protected void RequestListObject(PresentBoxNotificationListViewObject.InitMode mode, float delay)
    {
        List<PresentBoxNotificationListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PresentBoxNotificationListViewObject obj2 in objectList)
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
        if (mode == InitMode.INPUT)
        {
            this.RequestListObject(PresentBoxNotificationListViewObject.InitMode.INPUT);
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
        PresentBoxNotificationListViewObject obj2 = obj as PresentBoxNotificationListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(PresentBoxNotificationListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(PresentBoxNotificationListViewObject.InitMode.VALID);
        }
    }

    public List<PresentBoxNotificationListViewObject> ClippingObjectList
    {
        get
        {
            List<PresentBoxNotificationListViewObject> list = new List<PresentBoxNotificationListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PresentBoxNotificationListViewObject component = obj2.GetComponent<PresentBoxNotificationListViewObject>();
                    PresentBoxNotificationListViewItem item = component.GetItem();
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

    public List<PresentBoxNotificationListViewObject> ObjectList
    {
        get
        {
            List<PresentBoxNotificationListViewObject> list = new List<PresentBoxNotificationListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PresentBoxNotificationListViewObject component = obj2.GetComponent<PresentBoxNotificationListViewObject>();
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

    public enum Kind
    {
        NORMAL
    }
}

