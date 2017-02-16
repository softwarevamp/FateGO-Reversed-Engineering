using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptAssetListViewItem")]
public class ScriptAssetListViewItem : ListViewItem
{
    protected string path;

    public ScriptAssetListViewItem(int index, string path) : base(index)
    {
        this.path = path;
    }

    ~ScriptAssetListViewItem()
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

