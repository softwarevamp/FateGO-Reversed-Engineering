using System;
using UnityEngine;

public class ServantStatusListViewItemDrawEquipMain : ServantStatusListViewItemDraw
{
    [SerializeField]
    protected UIIconLabel attackIconLabel;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UILabel costLabel;
    [SerializeField]
    protected UISlider expBar;
    [SerializeField]
    protected GameObject expBase;
    [SerializeField]
    protected UILabel expLabel;
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
    protected ServantFaceIconComponent servantFacceIcon;

    public override ServantStatusListViewItemDraw.Kind GetKind() => 
        ServantStatusListViewItemDraw.Kind.EQUIP_MAIN;

    public override void SetItem(ServantStatusListViewItem item, ServantStatusListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, mode);
        if ((item != null) && (mode != ServantStatusListViewItemDraw.DispMode.INVISIBLE))
        {
            int num;
            int num2;
            float num3;
            if (item.UserServant != null)
            {
                this.servantFacceIcon.Set(item.UserServant, null);
            }
            else if (item.UserServantCollection != null)
            {
                this.servantFacceIcon.Set(item.UserServantCollection, null);
            }
            else if (item.EquipTargetData != null)
            {
                this.servantFacceIcon.Set(item.EquipTargetData, null);
            }
            else
            {
                this.servantFacceIcon.NoMount();
            }
            this.levelLabel.text = string.Empty + item.Level;
            this.maxLevelLabel.text = string.Empty + item.MaxLevel;
            this.costLabel.text = string.Empty + (item.Cost + item.EquipCost);
            this.attackIconLabel.Set(IconLabelInfo.IconKind.ATK, item.Atk, item.AdjustAtk, 0, 0L, false, false);
            this.hpIconLabel.Set(IconLabelInfo.IconKind.HP, item.Hp, item.AdjustHp, 0, 0L, false, false);
            if (item.GetExpInfo(out num, out num2, out num3))
            {
                this.expBase.SetActive(true);
                this.expLabel.text = string.Empty + num;
                this.lateExpBase.SetActive(num2 > 0);
                this.lateExpLabel.text = string.Format(LocalizationManager.Get("SERVANT_STATUS_EXP_LATE"), LocalizationManager.GetNumberFormat(num2));
                this.expBar.value = num3;
            }
            else
            {
                this.expBase.SetActive(false);
            }
            this.limitCountGauge.Set(item.LimitCount, item.Servant.limitMax);
            if (this.baseButton != null)
            {
            }
        }
    }
}

