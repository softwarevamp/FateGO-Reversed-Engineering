using System;
using UnityEngine;

public class ShiningIconComponent : BaseMonoBehaviour
{
    [SerializeField]
    protected UIWidget widget;

    protected void Awake()
    {
        this.widget.enabled = false;
    }

    public void Clear()
    {
        if (this.widget.enabled)
        {
            ShiningIconManager.RemoveIcon(this);
            this.widget.enabled = false;
        }
    }

    protected void onDestory()
    {
        this.Clear();
    }

    public void Set()
    {
        if (!this.widget.enabled)
        {
            this.widget.enabled = true;
            ShiningIconManager.AddIcon(this);
        }
    }

    public void Set(bool isDisp)
    {
        if (isDisp)
        {
            this.Set();
        }
        else
        {
            this.Clear();
        }
    }

    public void Set(string spriteName)
    {
        if (!this.widget.enabled)
        {
            this.widget.enabled = true;
            ShiningIconManager.AddIcon(this);
        }
        if (this.widget is UISprite)
        {
            UISprite widget = this.widget as UISprite;
            widget.spriteName = spriteName;
            if (spriteName != null)
            {
                widget.MakePixelPerfect();
            }
        }
    }

    public bool UpdateIcon()
    {
        if (this.widget == null)
        {
            return false;
        }
        if (!this.widget.enabled)
        {
            return false;
        }
        this.widget.enabled = false;
        this.widget.enabled = true;
        return true;
    }
}

