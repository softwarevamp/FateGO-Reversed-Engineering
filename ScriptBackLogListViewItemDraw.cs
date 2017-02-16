using System;
using UnityEngine;

[AddComponentMenu("ScriptAction/ScriptBackLog/ScriptBackLogListViewItemDraw")]
public class ScriptBackLogListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected GameObject imagePrefab;
    [SerializeField]
    protected GameObject mainPrefab;
    [SerializeField]
    protected GameObject rubyPrefab;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    public void SetItem(ScriptBackLogListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            item.Label.SetLogDraw(this.mainPrefab, this.rubyPrefab, this.imagePrefab);
        }
        else
        {
            item.Label.ClearLogDraw(this.mainPrefab, this.rubyPrefab, this.imagePrefab);
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

