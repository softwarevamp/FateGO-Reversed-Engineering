using System;
using UnityEngine;

public class BattleButtonComponent : UIButton
{
    public virtual bool isHide
    {
        get
        {
            if (!base.enabled)
            {
                return false;
            }
            Collider component = base.GetComponent<Collider>();
            return ((component != null) && component.enabled);
        }
        set
        {
            Collider component = base.GetComponent<Collider>();
            if (component != null)
            {
                component.enabled = value;
            }
            else
            {
                base.enabled = value;
            }
        }
    }
}

