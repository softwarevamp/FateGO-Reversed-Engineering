using System;
using UnityEngine;

public class PartyOrganizationListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UILabel attackLabel;
    [SerializeField]
    protected UISprite base2Sprite;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;
    [SerializeField]
    protected ShiningIconComponent bounusIcon;
    [SerializeField]
    protected UILabel costLabel;
    [SerializeField]
    protected UISprite equipLimitCountSprite;
    [SerializeField]
    protected UIMeshSprite equipSprite;
    [SerializeField]
    protected UILabel hpLabel;
    [SerializeField]
    protected UILabel levelLabel;
    [SerializeField]
    protected UISprite memberTypeBaseSprite;
    [SerializeField]
    protected UISprite memberTypeSprite;
    [SerializeField]
    protected UISprite noneEquipSprite;
    [SerializeField]
    protected UISprite raritySprite;
    [SerializeField]
    protected ServantClassIconComponent servantClassIcon;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;
    [SerializeField]
    protected UINarrowFigureTexture servantNarrowTexture;
    [SerializeField]
    protected UILabel skillLevelListLabel;
    [SerializeField]
    protected UISprite supportSprite;
    [SerializeField]
    protected UISprite typeSprite;

    public void ClearItem()
    {
        base.gameObject.SetActive(false);
        this.servantNarrowTexture.ReleaseCharacter();
        if (this.noneEquipSprite != null)
        {
            this.noneEquipSprite.gameObject.SetActive(false);
            this.equipSprite.gameObject.SetActive(false);
        }
    }

    public void SetInput(PartyOrganizationListViewItem item, bool isInput)
    {
    }

    public void SetItem(PartyOrganizationListViewItem item, DispMode mode)
    {
        if (item != null)
        {
            if (mode == DispMode.INVISIBLE)
            {
                base.gameObject.SetActive(false);
                if (this.noneEquipSprite != null)
                {
                    this.noneEquipSprite.gameObject.SetActive(false);
                    this.equipSprite.gameObject.SetActive(false);
                    if (this.equipLimitCountSprite != null)
                    {
                        this.equipLimitCountSprite.gameObject.SetActive(false);
                    }
                }
            }
            else if (mode == DispMode.DRAG_INVISIBLE)
            {
                base.gameObject.SetActive(false);
                if (this.noneEquipSprite != null)
                {
                    if (item.EquipSvtId > 0)
                    {
                        this.noneEquipSprite.gameObject.SetActive(false);
                        this.equipSprite.gameObject.SetActive(false);
                    }
                    else
                    {
                        this.noneEquipSprite.gameObject.SetActive(false);
                        this.equipSprite.gameObject.SetActive(false);
                    }
                    if (this.equipLimitCountSprite != null)
                    {
                        this.equipLimitCountSprite.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                base.gameObject.SetActive(true);
                string str = null;
                string str2 = null;
                bool isDisp = false;
                bool flag2 = false;
                string levelList = string.Empty;
                if (!item.IsFollower)
                {
                    if (item.UserServant != null)
                    {
                        int[] numArray4;
                        int[] numArray5;
                        int[] numArray6;
                        string[] strArray3;
                        string[] strArray4;
                        if (this.servantFaceIcon != null)
                        {
                            this.servantFaceIcon.Set(item.UserServant, null);
                        }
                        this.servantNarrowTexture.SetCharacter(item.UserServant.svtId, item.UserServant.getDispCardImageLimitCount(false), null);
                        str = Rarity.getFormationBase(item.Rarity);
                        str2 = Rarity.getFormationFrame(item.Rarity);
                        this.typeSprite.gameObject.SetActive(false);
                        item.UserServant.getSkillInfo(out numArray4, out numArray5, out numArray6, out strArray3, out strArray4);
                        levelList = LocalizationManager.GetLevelList(numArray5);
                    }
                    else
                    {
                        str = "formation_blank_01";
                    }
                }
                else if (item.FollowerData == null)
                {
                    str = "formation_support";
                }
                else
                {
                    if (this.servantFaceIcon != null)
                    {
                        this.servantFaceIcon.Set(item.ServantLeader, null, false);
                    }
                    this.servantNarrowTexture.SetCharacter(item.SvtId, ImageLimitCount.GetCardImageLimitCount(item.SvtId, item.ServantLeader.limitCount, false, false), null);
                    str = Rarity.getFormationBase(item.Rarity);
                    str2 = Rarity.getFormationFrame(item.Rarity);
                    switch (item.FollowerData.type)
                    {
                        case 1:
                            this.typeSprite.gameObject.SetActive(true);
                            this.typeSprite.spriteName = "icon_friend";
                            this.typeSprite.MakePixelPerfect();
                            break;

                        case 3:
                            this.typeSprite.gameObject.SetActive(true);
                            this.typeSprite.spriteName = "icon_support_01";
                            this.typeSprite.MakePixelPerfect();
                            break;

                        default:
                            this.typeSprite.gameObject.SetActive(false);
                            break;
                    }
                    if (!flag2)
                    {
                        int[] numArray;
                        int[] numArray2;
                        int[] numArray3;
                        string[] strArray;
                        string[] strArray2;
                        item.ServantLeader.getSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
                        levelList = LocalizationManager.GetLevelList(numArray2);
                    }
                }
                if ((item.UserServant != null) || (item.FollowerData != null))
                {
                    if (this.servantClassIcon != null)
                    {
                        this.servantClassIcon.Set(item.ClassId, item.Rarity);
                    }
                    if (this.levelLabel != null)
                    {
                        this.levelLabel.text = string.Empty + item.Level;
                    }
                    if (this.raritySprite != null)
                    {
                        this.raritySprite.spriteName = "icon_rarity_" + item.Rarity;
                        this.raritySprite.MakePixelPerfect();
                    }
                    if (this.attackLabel != null)
                    {
                        Color color = (item.AdjustAtk <= 0) ? Color.white : Color.yellow;
                        this.attackLabel.color = color;
                        this.attackLabel.text = string.Empty + item.MargeAtk;
                    }
                    if (this.hpLabel != null)
                    {
                        Color color2 = (item.AdjustHp <= 0) ? Color.white : Color.yellow;
                        this.hpLabel.color = color2;
                        this.hpLabel.text = string.Empty + item.MargeHp;
                    }
                    if (this.costLabel != null)
                    {
                        int equipCost = item.EquipCost;
                        if (equipCost >= 0)
                        {
                            object[] objArray1 = new object[] { string.Empty, item.MainCost, "+", equipCost };
                            this.costLabel.text = string.Concat(objArray1);
                        }
                        else
                        {
                            this.costLabel.text = string.Empty + item.MainCost;
                        }
                    }
                    if (this.skillLevelListLabel != null)
                    {
                        this.skillLevelListLabel.text = levelList;
                    }
                    if (this.memberTypeBaseSprite != null)
                    {
                        if (item.Index == 0)
                        {
                            this.memberTypeBaseSprite.spriteName = "formation_txtbg_01";
                        }
                        else if (item.Index < BalanceConfig.DeckMainMemberMax)
                        {
                            this.memberTypeBaseSprite.spriteName = "formation_txtbg_02";
                        }
                        else
                        {
                            this.memberTypeBaseSprite.spriteName = "formation_txtbg_03";
                        }
                    }
                    if (this.memberTypeSprite != null)
                    {
                        this.memberTypeSprite.spriteName = "member_txt_" + (item.Index + 1);
                        this.memberTypeSprite.MakePixelPerfect();
                    }
                    if (this.supportSprite != null)
                    {
                        if ((item.UserServant != null) && item.UserServant.IsEventJoin())
                        {
                            this.supportSprite.spriteName = "icon_eventjoin_02";
                        }
                        else if (item.IsFollower)
                        {
                            this.supportSprite.spriteName = "icon_support_02";
                        }
                        else
                        {
                            this.supportSprite.spriteName = null;
                        }
                    }
                    if (this.noneEquipSprite != null)
                    {
                        int equipSvtId = item.EquipSvtId;
                        if (equipSvtId > 0)
                        {
                            this.noneEquipSprite.gameObject.SetActive(false);
                            this.equipSprite.gameObject.SetActive(true);
                            AtlasManager.SetEquipFace(this.equipSprite, equipSvtId);
                            if (this.equipLimitCountSprite != null)
                            {
                                int equipLimitCountMax = item.EquipLimitCountMax;
                                this.equipLimitCountSprite.gameObject.SetActive((equipLimitCountMax > 0) && (item.EquipLimitCount >= equipLimitCountMax));
                            }
                        }
                        else
                        {
                            this.noneEquipSprite.gameObject.SetActive(true);
                            this.equipSprite.gameObject.SetActive(false);
                            if (this.equipLimitCountSprite != null)
                            {
                                this.equipLimitCountSprite.gameObject.SetActive(false);
                            }
                        }
                    }
                    if (this.bounusIcon != null)
                    {
                        isDisp = item.IsEventUpVal();
                    }
                }
                else
                {
                    if (this.servantFaceIcon != null)
                    {
                        this.servantFaceIcon.Clear();
                    }
                    this.servantNarrowTexture.ReleaseCharacter();
                    this.typeSprite.gameObject.SetActive(false);
                    if (this.servantClassIcon != null)
                    {
                        this.servantClassIcon.Clear();
                    }
                    if (this.levelLabel != null)
                    {
                        this.levelLabel.text = string.Empty;
                    }
                    if (this.raritySprite != null)
                    {
                        this.raritySprite.spriteName = null;
                    }
                    if (this.attackLabel != null)
                    {
                        this.attackLabel.text = string.Empty;
                    }
                    if (this.hpLabel != null)
                    {
                        this.hpLabel.text = string.Empty;
                    }
                    if (this.costLabel != null)
                    {
                        this.costLabel.text = string.Empty;
                    }
                    if (this.skillLevelListLabel != null)
                    {
                        this.skillLevelListLabel.text = string.Empty;
                    }
                    if (this.memberTypeBaseSprite != null)
                    {
                        this.memberTypeBaseSprite.spriteName = null;
                    }
                    if (this.memberTypeSprite != null)
                    {
                        this.memberTypeSprite.spriteName = null;
                    }
                    if (this.supportSprite != null)
                    {
                        this.supportSprite.spriteName = null;
                    }
                    if (this.noneEquipSprite != null)
                    {
                        this.noneEquipSprite.gameObject.SetActive(false);
                        this.equipSprite.gameObject.SetActive(false);
                        if (this.equipLimitCountSprite != null)
                        {
                            this.equipLimitCountSprite.gameObject.SetActive(false);
                        }
                    }
                }
                if (this.baseSprite != null)
                {
                    this.baseSprite.spriteName = str;
                }
                if (this.base2Sprite != null)
                {
                    this.base2Sprite.spriteName = str2;
                }
                if (this.bounusIcon != null)
                {
                    this.bounusIcon.Set(isDisp);
                }
                if (this.baseButton != null)
                {
                    this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                }
            }
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT,
        DRAG_INVISIBLE
    }
}

