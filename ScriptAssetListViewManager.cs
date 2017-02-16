using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptAssetListViewManager")]
public class ScriptAssetListViewManager : ListViewManager
{
    protected int callbackCount;
    protected int callbackIndex;
    protected InitMode initMode;

    protected event System.Action callbackFunc;

    public void CreateList()
    {
        string[] strArray = AssetManager.getAssetStorageList("ScriptActionEncrypt");
        int length = strArray.Length;
        base.CreateList(length);
        for (int i = 0; i < length; i++)
        {
            ScriptAssetListViewItem item = new ScriptAssetListViewItem(i, strArray[i]);
            base.itemList.Add(item);
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public int GetClickResult() => 
        this.callbackIndex;

    public ScriptAssetListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ScriptAssetListViewItem);
        }
        return null;
    }

    public string GetNextName(string name)
    {
        ScriptAssetListViewItem item;
        for (int i = 0; i < (base.itemList.Count - 1); i++)
        {
            item = base.itemList[i] as ScriptAssetListViewItem;
            if (item.Path == name)
            {
                item = base.itemList[i + 1] as ScriptAssetListViewItem;
                return item.Path;
            }
        }
        item = base.itemList[0] as ScriptAssetListViewItem;
        return item.Path;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        this.callbackIndex = obj.Index;
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    protected void OnMoveEnd()
    {
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
                    System.Action callbackFunc = this.callbackFunc;
                    this.callbackFunc = null;
                    callbackFunc();
                }
            }
        }
    }

    protected void RequestListObject(ScriptAssetListViewObject.InitMode mode)
    {
        List<ScriptAssetListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptAssetListViewObject obj2 in objectList)
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

    protected void RequestListObject(ScriptAssetListViewObject.InitMode mode, float delay)
    {
        List<ScriptAssetListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ScriptAssetListViewObject obj2 in objectList)
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
        if (mode == InitMode.INPUT)
        {
            this.callbackIndex = -1;
            this.RequestListObject(ScriptAssetListViewObject.InitMode.INPUT);
        }
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ScriptAssetListViewObject obj2 = obj as ScriptAssetListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ScriptAssetListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ScriptAssetListViewObject.InitMode.VALID);
        }
    }

    public List<ScriptAssetListViewObject> ObjectList
    {
        get
        {
            List<ScriptAssetListViewObject> list = new List<ScriptAssetListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ScriptAssetListViewObject component = obj2.GetComponent<ScriptAssetListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public enum InitMode
    {
        NONE,
        INPUT
    }
}

