using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScriptBackLog : BaseMonoBehaviour
{
    protected ClickDelegate clickFunc;
    [SerializeField]
    protected ScriptBackLogListViewManager listViewManager;
    protected List<ScriptMessageLabel> logData = new List<ScriptMessageLabel>();
    protected float maxRangeY;
    [SerializeField]
    protected GameObject rootObject;

    public void AddLog(ScriptMessageLabel label)
    {
        this.logData.Add(label);
        float logRangeY = label.GetLogRangeY();
        if (logRangeY < this.maxRangeY)
        {
            this.maxRangeY = logRangeY;
        }
    }

    public void ClearLog()
    {
        this.logData.Clear();
        this.maxRangeY = 0f;
    }

    public void Close()
    {
        this.listViewManager.DestroyList();
        this.rootObject.SetActive(false);
    }

    public bool IsEmptyLog() => 
        (this.logData.Count <= 0);

    public bool IsOpen() => 
        this.rootObject.activeSelf;

    public void OnClickEnd(int index)
    {
        if (this.clickFunc != null)
        {
            this.clickFunc();
        }
    }

    public void Open()
    {
        this.Open(null);
    }

    public void Open(ClickDelegate func)
    {
        this.rootObject.SetActive(true);
        this.clickFunc = func;
        this.listViewManager.CreateList(this.logData, this.maxRangeY, new ScriptBackLogListViewManager.ClickDelegate(this.OnClickEnd));
        this.listViewManager.SetMode(ScriptBackLogListViewManager.InitMode.INPUT);
    }

    public delegate void ClickDelegate();
}

