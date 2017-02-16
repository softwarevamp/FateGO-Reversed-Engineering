using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptTextListViewItem")]
public class ScriptTextListViewItem : ListViewItem
{
    protected string lineText;

    public ScriptTextListViewItem(int index, string lineText) : base(index)
    {
        this.lineText = $"[{index + 1:D4}] {lineText}";
    }

    ~ScriptTextListViewItem()
    {
    }

    public string LineText =>
        this.lineText;
}

