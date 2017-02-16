using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("ScriptAction/ScriptBackLog/ScriptBackLogListViewManager")]
public class ScriptBackLogListViewManager : ListViewManager
{
    protected int callbackCount;
    protected ClickDelegate clickFunc;
    protected InitMode initMode;

    protected event System.Action callbackFunc;

    public void CreateList(List<ScriptMessageLabel> logList, float offsetY, ClickDelegate callbackFunc)
    {
        this.clickFunc = callbackFunc;
        int count = logList.Count;
        base.CreateList(count);
        float height = base.scrollView.panel.height;
        offsetY += height;
        base.scrollView.contentPivot = (offsetY <= 0f) ? UIWidget.Pivot.BottomLeft : UIWidget.Pivot.TopLeft;
        for (int i = 0; i < count; i++)
        {
            ScriptBackLogListViewItem item = new ScriptBackLogListViewItem(i, logList[i]);
            Vector2 mainPosition = item.Label.mainPosition;
            mainPosition.y -= offsetY;
            item.BasePosition = (Vector3) mainPosition;
            base.itemList.Add(item);
        }
        base.DispItem(-1, false, -1);
    }

    public void DestroyList()
    {
        this.clickFunc = null;
        base.DestroyList();
    }

    public ScriptBackLogListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ScriptBackLogListViewItem);
        }
        return null;
    }

    public void OnClickBack()
    {
        Debug.Log("Manager ListView OnClickBack");
        if (this.clickFunc != null)
        {
            this.clickFunc(-1);
        }
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

    protected void RequestListObject(ScriptBackLogListViewObject.InitMode mode)
    {
        List<ScriptBackLogListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptBackLogListViewObject obj2 in objectList)
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

    protected void RequestListObject(ScriptBackLogListViewObject.InitMode mode, float delay)
    {
        List<ScriptBackLogListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptBackLogListViewObject obj2 in objectList)
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
        List<ScriptBackLogListViewObject> objectList = this.ObjectList;
        if (mode == InitMode.INPUT)
        {
            this.RequestListObject(ScriptBackLogListViewObject.InitMode.INPUT);
        }
        Debug.Log("SetMode " + mode);
        foreach (ScriptBackLogListViewObject obj2 in objectList)
        {
            Debug.Log("    " + obj2.ToString());
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ScriptBackLogListViewObject obj2 = obj as ScriptBackLogListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ScriptBackLogListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ScriptBackLogListViewObject.InitMode.VALID);
        }
    }

    public List<ScriptBackLogListViewObject> ObjectList
    {
        get
        {
            List<ScriptBackLogListViewObject> list = new List<ScriptBackLogListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ScriptBackLogListViewObject component = obj2.GetComponent<ScriptBackLogListViewObject>();
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
        INPUT
    }
}

