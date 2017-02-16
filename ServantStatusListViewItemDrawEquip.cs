using System;
using UnityEngine;

public class ServantStatusListViewItemDrawEquip : ServantStatusListViewItemDraw
{
    [SerializeField]
    protected UIIconLabel attackIconLabel;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UILabel costLabel;
    [SerializeField]
    protected UICommonButton equipButton;
    protected string[] equipExplanationMessageList;
    [SerializeField]
    protected ServantFaceIconComponent equipIcon;
    [SerializeField]
    protected UILabel[] equipSkillExplanationLabelList = new UILabel[BalanceConfig.SvtSkillListMax];
    [SerializeField]
    protected SkillIconComponent[] equipSkillIcon = new SkillIconComponent[BalanceConfig.SvtEquipSkillListMax];
    [SerializeField]
    protected UILabel[] equipSkillTitleLabelList = new UILabel[BalanceConfig.SvtSkillListMax];
    protected string[] equipTitleMessageList;
    [SerializeField]
    protected UISlider expBar;
    [SerializeField]
    protected GameObject expBase;
    [SerializeField]
    protected UILabel expLabel;
    [SerializeField]
    protected UILabel explanationLabel;
    [SerializeField]
    protected UIIconLabel hpIconLabel;
    [SerializeField]
    protected GameObject lateExpBase;
    [SerializeField]
    protected UILabel lateExpLabel;
    [SerializeField]
    protected UIExtrusionLabel levelLabel;
    [SerializeField]
    protected ServantStatusLimitCountGauge limitCountGauge;
    [SerializeField]
    protected UILabel maxLevelLabel;
    [SerializeField]
    protected UILabel nameLabel;

    public override ServantStatusListViewItemDraw.Kind GetKind() => 
        ServantStatusListViewItemDraw.Kind.EQUIP;

    public override void SetItem(ServantStatusListViewItem item, ServantStatusListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, mode);
        if ((item != null) && (mode != ServantStatusListViewItemDraw.DispMode.INVISIBLE))
        {
            int num;
            int num2;
            float num3;
            int[] numArray;
            int[] numArray2;
            int[] numArray3;
            this.explanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_EXPLANATION_EQUIP_ICON");
            if (item.UserServant != null)
            {
                if (item.EquipTargetId1 > 0L)
                {
                    this.equipIcon.Set(item.EquipTargetId1, null);
                }
                else
                {
                    this.equipIcon.NoMount();
                }
            }
            else if (item.ServantLeaderData != null)
            {
                if (item.ServantLeaderData.equipTarget1 != null)
                {
                    if (item.ServantLeaderData.equipTarget1.svtId > 0)
                    {
                        this.equipIcon.Set(item.ServantLeaderData.equipTarget1, null);
                    }
                    else
                    {
                        this.equipIcon.NoMount();
                    }
                }
                else
                {
                    this.equipIcon.NoMount();
                }
            }
            else
            {
                this.equipIcon.NoMount();
            }
            this.nameLabel.text = (item.EquipServant == null) ? string.Empty : item.EquipServant.name;
            this.levelLabel.text = string.Empty + item.EquipLevel;
            this.maxLevelLabel.text = string.Empty + item.EquipMaxLevel;
            this.costLabel.text = string.Empty + item.EquipCost;
            this.attackIconLabel.Set(IconLabelInfo.IconKind.ATK, item.EquipAtk, 0, 0, 0L, false, false);
            this.hpIconLabel.Set(IconLabelInfo.IconKind.HP, item.EquipHp, 0, 0, 0L, false, false);
            if (item.GetEquipExpInfo(out num, out num2, out num3))
            {
                this.expBase.SetActive(true);
                this.expLabel.text = string.Empty + num;
                this.lateExpBase.SetActive(num2 > 0);
                this.lateExpLabel.text = string.Format(LocalizationManager.Get("SERVANT_STATUS_EXP_LATE"), num2);
                this.expBar.value = num3;
            }
            else
            {
                this.expBase.SetActive(false);
            }
            if (item.EquipServant != null)
            {
                this.limitCountGauge.Set(item.EquipLimitCount, item.EquipServant.limitMax);
            }
            else
            {
                this.limitCountGauge.Set(0, 0);
            }
            item.GetEquipSkillInfo(out numArray, out numArray2, out numArray3, out this.equipTitleMessageList, out this.equipExplanationMessageList);
            for (int i = 0; i < this.equipSkillIcon.Length; i++)
            {
                if ((i < numArray.Length) && item.IsEquip)
                {
                    Color color = (numArray2[i] < 0) ? Color.gray : Color.white;
                    this.equipSkillIcon[i].Set(numArray[i], numArray2[i]);
                    this.equipSkillTitleLabelList[i].color = color;
                    this.equipSkillExplanationLabelList[i].color = color;
                    this.equipSkillTitleLabelList[i].text = this.equipTitleMessageList[i];
                    this.equipSkillExplanationLabelList[i].text = this.equipExplanationMessageList[i];
                }
                else
                {
                    this.equipSkillIcon[i].Clear();
                    this.equipSkillTitleLabelList[i].text = string.Empty;
                    this.equipSkillExplanationLabelList[i].text = string.Empty;
                }
            }
            if (this.equipButton != null)
            {
                bool isEnable = item.IsEquipChangeMode || item.IsEquip;
                this.equipButton.SetColliderEnable(isEnable, true);
            }
        }
    }
}

