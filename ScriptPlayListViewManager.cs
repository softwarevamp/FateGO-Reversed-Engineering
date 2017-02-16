using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptPlayListViewManager")]
public class ScriptPlayListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;
    public ScriptTextViewMenu scriptTextViewMenu;

    protected event CallbackFunc callbackFunc;

    public void CreateList(string[] scriptFileList)
    {
        if (scriptFileList != null)
        {
            int length = scriptFileList.Length;
            base.CreateList(length);
            for (int i = 0; i < length; i++)
            {
                ScriptPlayListViewItem item = new ScriptPlayListViewItem(i, scriptFileList[i]);
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

    public ScriptPlayListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ScriptPlayListViewItem);
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
            this.callbackFunc(ResultKind.PLAY, obj.GetItem() as ScriptPlayListViewItem);
        }
    }

    protected void OnLongPushListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnLongPush " + obj.Index);
        if (this.callbackFunc != null)
        {
            this.callbackFunc(ResultKind.VIEW_PLAY, obj.GetItem() as ScriptPlayListViewItem);
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

    protected void RequestListObject(ScriptPlayListViewObject.InitMode mode)
    {
        List<ScriptPlayListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptPlayListViewObject obj2 in objectList)
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

    protected void RequestListObject(ScriptPlayListViewObject.InitMode mode, float delay)
    {
        List<ScriptPlayListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptPlayListViewObject obj2 in objectList)
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
            this.RequestListObject(ScriptPlayListViewObject.InitMode.INPUT);
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ScriptPlayListViewObject obj2 = obj as ScriptPlayListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ScriptPlayListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ScriptPlayListViewObject.InitMode.VALID);
        }
    }

    public List<ScriptPlayListViewObject> ObjectList
    {
        get
        {
            List<ScriptPlayListViewObject> list = new List<ScriptPlayListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ScriptPlayListViewObject component = obj2.GetComponent<ScriptPlayListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(ScriptPlayListViewManager.ResultKind result, ScriptPlayListViewItem item);

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

