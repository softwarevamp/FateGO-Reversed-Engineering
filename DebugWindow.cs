using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DebugWindow : MonoBehaviour
{
    private bool IsDoneSetup;
    [SerializeField]
    private UIGrid mGrid;
    [SerializeField]
    private UILabel mInfoLabel;
    private string mInfoStr;
    private List<DebugWindowScrollItem> mItems;
    private DebugWindow mParentWin;
    [SerializeField]
    private GameObject mScrollItem;

    private void Close(bool is_open_parent = false)
    {
        base.gameObject.SetActive(false);
        if (is_open_parent && (this.mParentWin != null))
        {
            this.mParentWin.Open();
        }
    }

    public DebugWindowScrollItem GetItem(int idx)
    {
        if (this.mItems == null)
        {
            return null;
        }
        if (idx < 0)
        {
            return null;
        }
        if (this.mItems.Count <= idx)
        {
            return null;
        }
        return this.mItems[idx];
    }

    public string GetStr(int idx)
    {
        DebugWindowScrollItem item = this.GetItem(idx);
        return item?.Str;
    }

    public int GetVal(int idx)
    {
        DebugWindowScrollItem item = this.GetItem(idx);
        return item?.Val;
    }

    public bool IsActive() => 
        (this.IsDoneSetup && base.gameObject.activeSelf);

    public bool IsEnable(int idx) => 
        (this.GetVal(idx) != 0);

    public void OnClickCloseBtn()
    {
        this.Close(true);
    }

    public void Open()
    {
        if (this.mParentWin == null)
        {
            this.mInfoLabel.text = this.mInfoStr;
        }
        else
        {
            this.mInfoLabel.text = this.mParentWin.mInfoLabel.text + "> " + this.mInfoStr;
            this.mParentWin.Close(false);
        }
        base.gameObject.SetActive(true);
    }

    public void SetStr(int idx, string str)
    {
        DebugWindowScrollItem item = this.GetItem(idx);
        if (item != null)
        {
            item.Str = str;
        }
    }

    public void Setup(string info_str, DebugWindowScrollItem.ItemInfo[] iinfs, Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage> cb_act = null)
    {
        this.IsDoneSetup = false;
        this.mInfoStr = info_str;
        this.mItems = new List<DebugWindowScrollItem>();
        for (int i = 0; i < iinfs.Length; i++)
        {
            DebugWindowScrollItem.ItemInfo iinf = iinfs[i];
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.mScrollItem);
            obj2.transform.parent = this.mGrid.gameObject.transform;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localRotation = Quaternion.identity;
            obj2.transform.localScale = Vector3.one;
            DebugWindowScrollItem component = obj2.GetComponent<DebugWindowScrollItem>();
            component.Setup(this, i, iinf, cb_act);
            this.mItems.Add(component);
        }
        this.mGrid.Reposition();
        this.Close(false);
        this.IsDoneSetup = true;
    }

    public void SetVal(int idx, int val)
    {
        DebugWindowScrollItem item = this.GetItem(idx);
        if (item != null)
        {
            item.Val = val;
        }
    }

    public string InfoStr =>
        this.mInfoStr;

    public DebugWindow ParentWin
    {
        get => 
            this.mParentWin;
        set
        {
            this.mParentWin = value;
        }
    }
}

