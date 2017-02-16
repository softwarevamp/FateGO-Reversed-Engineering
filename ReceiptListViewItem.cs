using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ReceiptListViewItem")]
public class ReceiptListViewItem : ListViewItem
{
    protected string path;

    public ReceiptListViewItem(int index, string path) : base(index)
    {
        this.path = path;
    }

    ~ReceiptListViewItem()
    {
    }

    public string Path =>
        this.path;

    public string TimeText
    {
        get
        {
            int index = this.path.IndexOf('0');
            if (index >= 0)
            {
                long t = long.Parse(this.path.Substring(index, 20));
                if (t > 0L)
                {
                    return NetworkManager.getDateTime(t).ToString();
                }
            }
            return string.Empty;
        }
    }

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

