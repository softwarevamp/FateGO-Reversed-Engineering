using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptConnectListViewItem")]
public class ScriptConnectListViewItem : ListViewItem
{
    protected string path;

    public ScriptConnectListViewItem(int index, string path) : base(index)
    {
        this.path = path;
    }

    ~ScriptConnectListViewItem()
    {
    }

    public string Path =>
        this.path;

    public string TitleText
    {
        get
        {
            int num = this.path.LastIndexOf('/');
            if (num >= 0)
            {
                return this.path.Substring(num + 1);
            }
            return this.path;
        }
    }
}

