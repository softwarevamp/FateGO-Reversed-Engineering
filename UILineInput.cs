using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/UI Line Input"), RequireComponent(typeof(UIInput))]
public class UILineInput : MonoBehaviour
{
    protected UIInput mInput;

    public string GetText()
    {
        if (!this.Init())
        {
            return string.Empty;
        }
        string str = this.mInput.value;
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }
        return str;
    }

    protected bool Init()
    {
        if (this.mInput == null)
        {
            this.mInput = base.GetComponent<UIInput>();
            if (this.mInput == null)
            {
                return false;
            }
            this.mInput.label.maxLineCount = 1;
        }
        return true;
    }

    public void SetInputEnable(bool flag)
    {
        base.GetComponent<Collider>().enabled = flag;
        if (this.Init())
        {
            this.mInput.isSelected = false;
        }
    }
}

