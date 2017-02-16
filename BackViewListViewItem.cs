using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/BackViewListViewItem")]
public class BackViewListViewItem : ListViewItem
{
    protected string path;

    public BackViewListViewItem(int index, string path) : base(index)
    {
        this.path = path;
    }

    ~BackViewListViewItem()
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

