using System;
using UnityEngine;

[AddComponentMenu("ScriptAction/ScriptBackLog/ScriptBackLogListViewItem")]
public class ScriptBackLogListViewItem : ListViewItem
{
    protected ScriptMessageLabel label;

    public ScriptBackLogListViewItem(int index, ScriptMessageLabel label) : base(index)
    {
        this.label = label;
    }

    ~ScriptBackLogListViewItem()
    {
    }

    private string ToString()
    {
        if (this.label.imageText != string.Empty)
        {
            return ("imageText " + this.label.imageText);
        }
        return ("mainText " + this.label.mainText);
    }

    public ScriptMessageLabel Label =>
        this.label;
}

