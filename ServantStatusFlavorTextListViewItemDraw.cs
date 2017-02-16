using System;
using UnityEngine;

public class ServantStatusFlavorTextListViewItemDraw : MonoBehaviour
{
    protected DispMode dispMode;

    public virtual Kind GetKind() => 
        Kind.NONE;

    public virtual void SetItem(ServantStatusListViewItem item, bool isOpen, bool isNew, string text, string text2, DispMode mode)
    {
        this.dispMode = mode;
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }

    public enum Kind
    {
        NONE,
        TEXT,
        TEXT2,
        VOICE,
        PARAM,
        TERMINAL
    }
}

