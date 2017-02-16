using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptObjectListViewManager")]
public class ScriptObjectListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;

    protected event CallbackFunc callbackFunc;

    public void CreateList(AssetData data)
    {
        if (data != null)
        {
            string[] objectNameList = data.GetObjectNameList();
            int length = objectNameList.Length;
            base.CreateList(length);
            for (int i = 0; i < length; i++)
            {
                ScriptObjectListViewItem item = new ScriptObjectListViewItem(i, objectNameList[i]);
                base.itemList.Add(item);
            }
        }
        else
        {
            base.CreateList(0);
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public ScriptObjectListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ScriptObjectListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        if (this.callbackFunc != null)
        {
            this.callbackFunc(ResultKind.PLAY, obj.GetItem() as ScriptObjectListViewItem);
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
                    CallbackFunc callbackFunc = this.callbackFunc;
                    this.callbackFunc = null;
                    callbackFunc(ResultKind.NONE, null);
                }
            }
        }
    }

    protected void RequestListObject(ScriptObjectListViewObject.InitMode mode)
    {
        List<ScriptObjectListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptObjectListViewObject obj2 in objectList)
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

    protected void RequestListObject(ScriptObjectListViewObject.InitMode mode, float delay)
    {
        List<ScriptObjectListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptObjectListViewObject obj2 in objectList)
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

    public void SetMode(InitMode mode, CallbackFunc callback)
    {
        this.initMode = mode;
        this.callbackFunc = callback;
        this.callbackCount = base.ObjectSum;
        base.IsInput = mode == InitMode.INPUT;
        if (mode == InitMode.INPUT)
        {
            this.RequestListObject(ScriptObjectListViewObject.InitMode.INPUT);
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ScriptObjectListViewObject obj2 = obj as ScriptObjectListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ScriptObjectListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ScriptObjectListViewObject.InitMode.VALID);
        }
    }

    public List<ScriptObjectListViewObject> ObjectList
    {
        get
        {
            List<ScriptObjectListViewObject> list = new List<ScriptObjectListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ScriptObjectListViewObject component = obj2.GetComponent<ScriptObjectListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(ScriptObjectListViewManager.ResultKind result, ScriptObjectListViewItem item);

    public enum InitMode
    {
        NONE,
        INPUT
    }

    public enum ResultKind
    {
        NONE,
        CANCEL,
        PLAY,
        VIEW_PLAY
    }
}

