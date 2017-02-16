using System;
using UnityEngine;

public class NewIconComponent : BaseMonoBehaviour
{
    [SerializeField]
    protected UITexture iconTexture;

    protected void Awake()
    {
        this.iconTexture.enabled = false;
    }

    public void Clear()
    {
        if (this.iconTexture.enabled)
        {
            NewIconManager.RemoveIcon(this);
            this.iconTexture.enabled = false;
        }
    }

    protected void onDestory()
    {
        this.Clear();
    }

    public void Set()
    {
        if (base.gameObject.activeSelf && !this.iconTexture.enabled)
        {
            this.iconTexture.enabled = true;
            NewIconManager.SetIcon(this.iconTexture);
            NewIconManager.AddIcon(this);
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

    public bool UpdateIcon()
    {
        if (this.iconTexture == null)
        {
            return false;
        }
        if (!this.iconTexture.enabled)
        {
            return false;
        }
        this.iconTexture.enabled = false;
        this.iconTexture.enabled = true;
        return true;
    }
}

