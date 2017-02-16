using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("ScriptAction/ScriptSelect/ScriptSelectListViewManager")]
public class ScriptSelectListViewManager : ListViewManager
{
    protected int callbackCount;
    protected ClickDelegate clickFunc;
    protected InitMode initMode;

    protected event System.Action callbackFunc;

    public void CreateList(string[] selectMessageList, ClickDelegate callbackFunc)
    {
        this.clickFunc = callbackFunc;
        int length = selectMessageList.Length;
        base.CreateList(length);
        for (int i = 0; i < length; i++)
        {
            ScriptSelectListViewItem item = new ScriptSelectListViewItem(i, selectMessageList[i]);
            base.itemList.Add(item);
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        this.clickFunc = null;
        base.DestroyList();
    }

    public ScriptSelectListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ScriptSelectListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        if (this.clickFunc != null)
        {
            this.clickFunc(obj.Index);
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
                if (this.callbackFunc != null)
                {
                    System.Action callbackFunc = this.callbackFunc;
                    this.callbackFunc = null;
                    callbackFunc();
                }
            }
        }
    }

    protected void RequestListObject(ScriptSelectListViewObject.InitMode mode)
    {
        List<ScriptSelectListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptSelectListViewObject obj2 in objectList)
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

    protected void RequestListObject(ScriptSelectListViewObject.InitMode mode, float delay)
    {
        List<ScriptSelectListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptSelectListViewObject obj2 in objectList)
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

    public void SetMode(InitMode mode, int index = 0, System.Action callback = null)
    {
        this.initMode = mode;
        this.callbackFunc = callback;
        this.callbackCount = base.ObjectSum;
        base.IsInput = mode == InitMode.INPUT;
        List<ScriptSelectListViewObject> objectList = this.ObjectList;
        switch (mode)
        {
            case InitMode.INTO:
                this.callbackCount = base.ObjectSum;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int i = 0; i < objectList.Count; i++)
                {
                    objectList[i].Init(ScriptSelectListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f * (i + 1));
                }
                break;

            case InitMode.INPUT:
                this.RequestListObject(ScriptSelectListViewObject.InitMode.INPUT);
                break;

            case InitMode.SELECT:
                this.callbackCount = base.ObjectSum;
                if (this.callbackCount <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                for (int j = 0; j < objectList.Count; j++)
                {
                    ScriptSelectListViewObject obj3 = objectList[j];
                    if (obj3.GetItem().Index == index)
                    {
                        obj3.Init(ScriptSelectListViewObject.InitMode.SELECT, new System.Action(this.OnMoveEnd), 0f);
                    }
                    else
                    {
                        obj3.Init(ScriptSelectListViewObject.InitMode.NO_SELECT, new System.Action(this.OnMoveEnd), 0f);
                    }
                }
                break;
        }
        Debug.Log("SetMode " + mode);
        foreach (ScriptSelectListViewObject obj4 in objectList)
        {
            Debug.Log("    " + obj4.ToString());
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ScriptSelectListViewObject obj2 = obj as ScriptSelectListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ScriptSelectListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ScriptSelectListViewObject.InitMode.VALID);
        }
    }

    public List<ScriptSelectListViewObject> ObjectList
    {
        get
        {
            List<ScriptSelectListViewObject> list = new List<ScriptSelectListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ScriptSelectListViewObject component = obj2.GetComponent<ScriptSelectListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void ClickDelegate(int select);

    public enum InitMode
    {
        NONE,
        INTO,
        INPUT,
        SELECT
    }
}

