using System;
using UnityEngine;

[AddComponentMenu("Sample/Test2ListView/SampleEventListViewItem")]
public class SampleEventListViewItem : ListViewItem
{
    protected string eventText;
    protected int eventType;

    public SampleEventListViewItem(int index) : base(index)
    {
        this.eventType = index % 2;
        this.eventText = $"Event Number {index + 1}";
    }

    ~SampleEventListViewItem()
    {
    }

    private string ToString()
    {
        object[] objArray1 = new object[] { "eventType ", this.eventType, " ", this.eventText };
        return string.Concat(objArray1);
    }

    public string EventText =>
        this.eventText;

    public int EventType =>
        this.eventType;
}

