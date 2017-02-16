using System;
using UnityEngine;

[AddComponentMenu("ScriptAction/ScriptSelect/ScriptSelectListViewItem")]
public class ScriptSelectListViewItem : ListViewItem
{
    protected string message;

    public ScriptSelectListViewItem(int index, string message) : base(index)
    {
        this.message = message;
    }

    ~ScriptSelectListViewItem()
    {
    }

    private string ToString() => 
        ("message " + this.message);

    public string MessageText =>
        this.message;
}

