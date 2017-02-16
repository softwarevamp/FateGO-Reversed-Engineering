using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScriptSelectDialog : BaseMonoBehaviour
{
    protected ClickDelegate callbackFunc;
    protected int index;
    protected bool isOpen;
    [SerializeField]
    protected ScriptSelectListViewManager listViewManager;
    [SerializeField]
    protected GameObject rootObject;

    public void Close()
    {
        if (this.isOpen)
        {
            this.listViewManager.DestroyList();
            this.rootObject.SetActive(false);
            this.callbackFunc = null;
            this.isOpen = false;
        }
    }

    protected void EndSelectDecide()
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(this.index);
        }
    }

    public void OnClickSelect(int index)
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(index);
        }
    }

    public void Open(string[] selectMessageList, ClickDelegate callback = null)
    {
        this.rootObject.SetActive(true);
        this.isOpen = true;
        this.callbackFunc = callback;
        this.listViewManager.CreateList(selectMessageList, new ScriptSelectListViewManager.ClickDelegate(this.OnClickSelect));
        this.listViewManager.SetMode(ScriptSelectListViewManager.InitMode.INPUT, 0, null);
    }

    public void SelectDecide(int index, ClickDelegate callback)
    {
        this.index = index;
        this.callbackFunc = callback;
        this.listViewManager.SetMode(ScriptSelectListViewManager.InitMode.SELECT, index, new System.Action(this.EndSelectDecide));
    }

    public void SetActive(bool flag)
    {
        if (this.isOpen)
        {
            this.rootObject.SetActive(flag);
        }
    }

    public delegate void ClickDelegate(int index);
}

