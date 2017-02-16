using System;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/Indicator")]
public class ListViewIndicator : MonoBehaviour
{
    public virtual void OnModifyCenterItem(ListViewManager manager, ListViewItem item, bool isTop, bool isBottom, bool isLeft, bool isRight)
    {
    }

    public virtual void OnModifyPosition(ListViewManager manager, ListViewItem item)
    {
    }

    public virtual void SetIndexMax(int max)
    {
    }
}

