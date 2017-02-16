using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptConnectListViewManager")]
public class ScriptConnectListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;

    protected event CallbackFunc callbackFunc;

    public void CreateList(string[] scriptFileList)
    {
        if (scriptFileList != null)
        {
            int length = scriptFileList.Length;
            base.CreateList(length);
            for (int i = 0; i < length; i++)
            {
                ScriptConnectListViewItem item = new ScriptConnectListViewItem(i, scriptFileList[i]);
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

    public ScriptConnectListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ScriptConnectListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
    }

    protected void OnClickSingleListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickSingle " + obj.Index);
        if (this.callbackFunc != null)
        {
            this.callbackFunc(ResultKind.PLAY, obj.GetItem() as ScriptConnectListViewItem);
        }
    }

    protected void OnLongPushListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnLongPush " + obj.Index);
        if (this.callbackFunc != null)
        {
            this.callbackFunc(ResultKind.VIEW_PLAY, obj.GetItem() as ScriptConnectListViewItem);
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

    protected void RequestListObject(ScriptConnectListViewObject.InitMode mode)
    {
        List<ScriptConnectListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptConnectListViewObject obj2 in objectList)
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

    protected void RequestListObject(ScriptConnectListViewObject.InitMode mode, float delay)
    {
        List<ScriptConnectListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptConnectListViewObject obj2 in objectList)
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
            this.RequestListObject(ScriptConnectListViewObject.InitMode.INPUT);
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ScriptConnectListViewObject obj2 = obj as ScriptConnectListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ScriptConnectListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ScriptConnectListViewObject.InitMode.VALID);
        }
    }

    public List<ScriptConnectListViewObject> ObjectList
    {
        get
        {
            List<ScriptConnectListViewObject> list = new List<ScriptConnectListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ScriptConnectListViewObject component = obj2.GetComponent<ScriptConnectListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(ScriptConnectListViewManager.ResultKind result, ScriptConnectListViewItem item);

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

