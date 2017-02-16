using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OrganizationTopListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;
    protected static OrganizationTopItemInfo[] itemInfo = new OrganizationTopItemInfo[] { new OrganizationTopItemInfo(1, "MASTER_ORGANIZATION", string.Empty, "MENU_MASTER_ORGANIZATION"), new OrganizationTopItemInfo(2, "PARTY_ORGANIZATION", string.Empty, "MENU_PARTY_ORGANIZATION"), new OrganizationTopItemInfo(3, "SERVANT_LIST", string.Empty, "MENU_SERVANT_LIST") };
    protected static int[] normalKindList = new int[] { 1, 2, 3 };

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(Kind kind)
    {
        int[] normalKindList = null;
        if (kind == Kind.NORMAL)
        {
            normalKindList = OrganizationTopListViewManager.normalKindList;
        }
        int sum = (normalKindList == null) ? 0 : normalKindList.Length;
        base.CreateList(sum);
        for (int i = 0; i < sum; i++)
        {
            int num3 = normalKindList[i];
            OrganizationTopItemInfo info = null;
            foreach (OrganizationTopItemInfo info2 in itemInfo)
            {
                if (info2.kind == num3)
                {
                    info = info2;
                    break;
                }
            }
            if (info != null)
            {
                OrganizationTopListViewItem item = new OrganizationTopListViewItem(base.itemList.Count, info);
                base.itemList.Add(item);
            }
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public OrganizationTopListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as OrganizationTopListViewItem);
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
            OrganizationTopListViewObject obj2 = obj as OrganizationTopListViewObject;
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

    protected void RequestListObject(OrganizationTopListViewObject.InitMode mode)
    {
        List<OrganizationTopListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (OrganizationTopListViewObject obj2 in objectList)
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

    protected void RequestListObject(OrganizationTopListViewObject.InitMode mode, float delay)
    {
        List<OrganizationTopListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (OrganizationTopListViewObject obj2 in objectList)
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
                this.RequestListObject(OrganizationTopListViewObject.InitMode.INPUT);
                break;

            case InitMode.INTO:
            {
                List<OrganizationTopListViewObject> clippingObjectList = this.ClippingObjectList;
                this.callbackCount = clippingObjectList.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    clippingObjectList[i].Init(OrganizationTopListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.ENTER:
            {
                List<OrganizationTopListViewObject> list2 = this.ClippingObjectList;
                this.callbackCount = list2.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int j = 0; j < list2.Count; j++)
                {
                    list2[j].Init(OrganizationTopListViewObject.InitMode.ENTER, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.RETRY:
            {
                List<OrganizationTopListViewObject> list3 = this.ClippingObjectList;
                this.callbackCount = list3.Count;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int k = 0; k < list3.Count; k++)
                {
                    list3[k].Init(OrganizationTopListViewObject.InitMode.RETRY, new System.Action(this.OnMoveEnd), 0.1f);
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
        OrganizationTopListViewObject obj2 = obj as OrganizationTopListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(OrganizationTopListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(OrganizationTopListViewObject.InitMode.VALID);
        }
    }

    public List<OrganizationTopListViewObject> ClippingObjectList
    {
        get
        {
            List<OrganizationTopListViewObject> list = new List<OrganizationTopListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    OrganizationTopListViewObject component = obj2.GetComponent<OrganizationTopListViewObject>();
                    OrganizationTopListViewItem item = component.GetItem();
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

    public List<OrganizationTopListViewObject> ObjectList
    {
        get
        {
            List<OrganizationTopListViewObject> list = new List<OrganizationTopListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    OrganizationTopListViewObject component = obj2.GetComponent<OrganizationTopListViewObject>();
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

