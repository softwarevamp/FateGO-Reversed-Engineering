using System;
using UnityEngine;

public class ServantStatusListViewItemDraw : MonoBehaviour
{
    protected DispMode dispMode;

    public virtual Kind GetKind() => 
        Kind.NONE;

    public virtual void ModifyCommandCard(ServantStatusListViewItem item)
    {
    }

    public virtual void ModifyFace(ServantStatusListViewItem item)
    {
    }

    public virtual void PlayBattle(ServantStatusListViewItem item)
    {
    }

    public virtual void SetItem(ServantStatusListViewItem item, DispMode mode)
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
        MAIN,
        EQUIP_MAIN,
        EQUIP,
        SKILL,
        CLASS_SKILL,
        NP,
        COMMAND,
        FACE,
        FLAVOR_TEXT,
        TERMINAL
    }
}

