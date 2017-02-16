using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptPlayListViewItem")]
public class ScriptPlayListViewItem : ListViewItem
{
    protected string path;

    public ScriptPlayListViewItem(int index, string path) : base(index)
    {
        this.path = path;
    }

    ~ScriptPlayListViewItem()
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

