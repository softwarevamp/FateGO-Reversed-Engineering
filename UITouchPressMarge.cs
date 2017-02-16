using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/UI Touch Press Marge")]
public class UITouchPressMarge : MonoBehaviour
{
    protected bool isPress;
    public UITouchPress margeComponent;

    protected void OnClick()
    {
        if (this.margeComponent.IsEnabled)
        {
            this.margeComponent.OnClick();
        }
    }

    protected void OnHover(bool isSelect)
    {
        if (this.margeComponent.IsEnabled || this.isPress)
        {
            if (!isSelect)
            {
                this.isPress = false;
            }
            this.margeComponent.OnHover(isSelect);
        }
    }

    public void OnPress(bool isPressed)
    {
        if (this.margeComponent.IsEnabled || this.isPress)
        {
            this.isPress = isPressed;
            this.margeComponent.OnPress(isPressed);
        }
    }
}

