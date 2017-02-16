using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/FigureViewListViewItem")]
public class FigureViewListViewItem : ListViewItem
{
    protected string path;

    public FigureViewListViewItem(int index, string path) : base(index)
    {
        this.path = path;
    }

    ~FigureViewListViewItem()
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

