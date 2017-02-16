using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/DebugListViewItem")]
public class DebugListViewItem : ListViewItem
{
    protected FsmEventData eventData;

    public DebugListViewItem(int index, FsmEventData eventData) : base(index)
    {
        this.eventData = eventData;
    }

    ~DebugListViewItem()
    {
    }

    private string ToString() => 
        ("eventType " + this.eventData.Title);

    public string TitleText =>
        this.eventData.Title;
}

