using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptTextListViewManager")]
public class ScriptTextListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;

    protected event CallbackFunc callbackFunc;

    public void CreateList(string[] textLineData)
    {
        if (textLineData != null)
        {
            int length = textLineData.Length;
            base.CreateList(length);
            for (int i = 0; i < length; i++)
            {
                ScriptTextListViewItem item = new ScriptTextListViewItem(i, textLineData[i]);
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

    public ScriptTextListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ScriptTextListViewItem);
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
            this.callbackFunc(ResultKind.PLAY, obj.GetItem() as ScriptTextListViewItem);
        }
    }

    protected void OnLongPushListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnLongPush " + obj.Index);
        if (this.callbackFunc != null)
        {
            this.callbackFunc(ResultKind.JUMP_PLAY, obj.GetItem() as ScriptTextListViewItem);
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

    protected void RequestListObject(ScriptTextListViewObject.InitMode mode)
    {
        List<ScriptTextListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptTextListViewObject obj2 in objectList)
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

    protected void RequestListObject(ScriptTextListViewObject.InitMode mode, float delay)
    {
        List<ScriptTextListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptTextListViewObject obj2 in objectList)
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
            this.RequestListObject(ScriptTextListViewObject.InitMode.INPUT);
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ScriptTextListViewObject obj2 = obj as ScriptTextListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ScriptTextListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ScriptTextListViewObject.InitMode.VALID);
        }
    }

    public List<ScriptTextListViewObject> ObjectList
    {
        get
        {
            List<ScriptTextListViewObject> list = new List<ScriptTextListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ScriptTextListViewObject component = obj2.GetComponent<ScriptTextListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(ScriptTextListViewManager.ResultKind result, ScriptTextListViewItem item);

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
        JUMP_PLAY
    }
}

