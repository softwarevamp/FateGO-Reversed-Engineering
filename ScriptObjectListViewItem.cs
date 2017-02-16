using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptObjectListViewItem")]
public class ScriptObjectListViewItem : ListViewItem
{
    protected string path;

    public ScriptObjectListViewItem(int index, string path) : base(index)
    {
        this.path = path;
    }

    ~ScriptObjectListViewItem()
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

