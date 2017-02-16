using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptTextListViewItemDraw")]
public class ScriptTextListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UILabel lineTextLabel;

    public void SetItem(ScriptTextListViewItem item, DispMode mode)
    {
        if ((item != null) && (mode != DispMode.INVISIBLE))
        {
            this.lineTextLabel.text = item.LineText;
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

