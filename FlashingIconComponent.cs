using System;
using UnityEngine;

public class FlashingIconComponent : BaseMonoBehaviour
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
            this.widget.enabled = false;
            FlashingIconManager.RemoveIcon(this);
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
            FlashingIconManager.AddIcon(this);
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
            FlashingIconManager.AddIcon(this);
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
    }

    public bool UpdateIcon(float alpha)
    {
        if (this.widget == null)
        {
            return false;
        }
        this.widget.alpha = alpha;
        return true;
    }
}

