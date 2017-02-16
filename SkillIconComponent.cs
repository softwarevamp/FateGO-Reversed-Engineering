using System;
using UnityEngine;

public class SkillIconComponent : BaseMonoBehaviour
{
    [SerializeField]
    protected UISprite frameSprite;
    [SerializeField]
    protected UIIconLabel iconLabel;
    [SerializeField]
    protected UISprite maskSprite;
    [SerializeField]
    protected UISprite skillIconSprite;

    public void Clear()
    {
        this.skillIconSprite.spriteName = null;
        if (this.frameSprite != null)
        {
            this.frameSprite.spriteName = null;
        }
        if (this.iconLabel != null)
        {
            this.iconLabel.Clear();
        }
        if (this.maskSprite != null)
        {
            this.maskSprite.gameObject.SetActive(false);
        }
    }

    public string GetIconSpriteName() => 
        this.skillIconSprite.spriteName;

    public void Set(int skillId)
    {
        this.Set(skillId, 0);
    }

    public void Set(int skillId, int skillLv)
    {
        if (skillId <= 0)
        {
            this.Clear();
        }
        else
        {
            AtlasManager.SetSkillIcon(this.skillIconSprite, skillId);
            if (this.frameSprite != null)
            {
                this.frameSprite.spriteName = null;
            }
            if (this.iconLabel != null)
            {
                if (skillLv > 0)
                {
                    this.iconLabel.Set(IconLabelInfo.IconKind.LEVEL, skillLv, 0, 0, 0L, false, false);
                }
                else
                {
                    this.iconLabel.Clear();
                }
            }
            if (this.maskSprite != null)
            {
                this.maskSprite.gameObject.SetActive(skillLv < 0);
            }
        }
    }

    public void Set(int skillId, int skillLv, IconLabelInfo info)
    {
        if (skillId <= 0)
        {
            this.Clear();
        }
        else
        {
            AtlasManager.SetSkillIcon(this.skillIconSprite, skillId);
            if (this.frameSprite != null)
            {
                this.frameSprite.spriteName = null;
            }
            if (this.iconLabel != null)
            {
                if (info != null)
                {
                    this.iconLabel.Set(info);
                }
                else if (skillLv > 0)
                {
                    this.iconLabel.Set(IconLabelInfo.IconKind.LEVEL, skillLv, 0, 0, 0L, false, false);
                }
                else
                {
                    this.iconLabel.Clear();
                }
            }
            if (this.maskSprite != null)
            {
                this.maskSprite.gameObject.SetActive(skillLv < 0);
            }
        }
    }

    public void SetHide()
    {
        AtlasManager.SetHideSkillIcon(this.skillIconSprite);
        if (this.frameSprite != null)
        {
            this.frameSprite.spriteName = null;
        }
        if (this.iconLabel != null)
        {
            this.iconLabel.Clear();
        }
        if (this.maskSprite != null)
        {
            this.maskSprite.gameObject.SetActive(false);
        }
    }
}

