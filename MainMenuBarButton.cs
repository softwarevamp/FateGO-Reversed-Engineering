using System;
using UnityEngine;

public class MainMenuBarButton : MonoBehaviour
{
    protected Mode mode = Mode.DISNABLE;

    public void SetMode(Mode mode)
    {
        if (this.mode != mode)
        {
            this.mode = mode;
            UIButton component = base.GetComponent<UIButton>();
            if (component != null)
            {
                UIWidget widget = component.GetComponent<UIWidget>();
                UIButtonScale scale = component.GetComponent<UIButtonScale>();
                component.enabled = mode == Mode.ENABLE;
                if (mode == Mode.SELECT)
                {
                    widget.color = Color.grey;
                }
                scale.enabled = mode != Mode.SELECT;
            }
        }
    }

    public enum Kind
    {
        FORMATION,
        SUMMON,
        COMBINE,
        SHOP,
        FRIEND,
        MYROOM,
        SIZEOF
    }

    public enum Mode
    {
        NONE,
        DISNABLE,
        ENABLE,
        SELECT
    }
}

