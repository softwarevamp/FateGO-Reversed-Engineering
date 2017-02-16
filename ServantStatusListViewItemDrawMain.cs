using System;
using UnityEngine;

public class ServantStatusListViewItemDrawMain : ServantStatusListViewItemDraw
{
    [SerializeField]
    protected UIIconLabel addAttackIconLabel;
    [SerializeField]
    protected UIIconLabel addHpIconLabel;
    [SerializeField]
    protected UIIconLabel attackIconLabel;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UILabel costLabel;
    [SerializeField]
    protected ShiningIconComponent eventJoinIcon;
    [SerializeField]
    protected UISprite eventJoinSprite;
    [SerializeField]
    protected UISlider expBar;
    [SerializeField]
    protected GameObject expBase;
    [SerializeField]
    protected UILabel expLabel;
    [SerializeField]
    protected ServantStatusFriendshipGauge friendshipGauge;
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
    protected UIIconLabel reinforceAttackIconLabel;
    [SerializeField]
    protected UILabel reinforceAttackMaxLabel;
    [SerializeField]
    protected UIIconLabel reinforceHpIconLabel;
    [SerializeField]
    protected UILabel reinforceHpMaxLabel;
    [SerializeField]
    protected UITexture servantTexture;

    public override ServantStatusListViewItemDraw.Kind GetKind() => 
        ServantStatusListViewItemDraw.Kind.MAIN;

    public override void ModifyCommandCard(ServantStatusListViewItem item)
    {
    }

    public override void PlayBattle(ServantStatusListViewItem item)
    {
        int svtId = item.SvtId;
        if (svtId > 0)
        {
            ServantAssetLoadManager.loadStatusFace(this.servantTexture, svtId, item.LimitCount);
        }
    }

    public override void SetItem(ServantStatusListViewItem item, ServantStatusListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, mode);
        if ((item != null) && (mode != ServantStatusListViewItemDraw.DispMode.INVISIBLE))
        {
            int num;
            int num2;
            int num3;
            int num4;
            float num5;
            this.levelLabel.text = string.Empty + item.Level;
            this.maxLevelLabel.text = string.Empty + item.MaxLevel;
            this.costLabel.text = string.Empty + item.Cost;
            if (item.Servant.IsStatusUp)
            {
                this.attackIconLabel.Set(IconLabelInfo.IconKind.ADD_ATK, item.Atk * BalanceConfig.StatusUpAdjustAtk, 0, 0, 0L, false, false);
                this.hpIconLabel.Set(IconLabelInfo.IconKind.ADD_HP, item.Hp * BalanceConfig.StatusUpAdjustHp, 0, 0, 0L, false, false);
            }
            else
            {
                this.attackIconLabel.Set(IconLabelInfo.IconKind.ATK, item.Atk, item.AdjustAtk, 0, 0L, false, false);
                this.hpIconLabel.Set(IconLabelInfo.IconKind.HP, item.Hp, item.AdjustHp, 0, 0L, false, false);
            }
            this.addAttackIconLabel.Clear();
            this.addHpIconLabel.Clear();
            if (item.GetAdjustMax(out num, out num2))
            {
                if (item.AdjustAtk < num2)
                {
                    this.reinforceAttackIconLabel.Set(IconLabelInfo.IconKind.REINFORCEMENT_ATK, item.AdjustAtk * BalanceConfig.StatusUpAdjustHp, num2 * BalanceConfig.StatusUpAdjustHp, 0, 0L, false, false);
                    this.reinforceAttackMaxLabel.text = string.Empty;
                }
                else
                {
                    this.reinforceAttackIconLabel.Clear();
                    this.reinforceAttackMaxLabel.text = LocalizationManager.Get("COMMON_MAX");
                }
                if (item.AdjustHp < num)
                {
                    this.reinforceHpIconLabel.Set(IconLabelInfo.IconKind.REINFORCEMENT_HP, item.AdjustHp * BalanceConfig.StatusUpAdjustHp, num * BalanceConfig.StatusUpAdjustHp, 0, 0L, false, false);
                    this.reinforceHpMaxLabel.text = string.Empty;
                }
                else
                {
                    this.reinforceHpIconLabel.Clear();
                    this.reinforceHpMaxLabel.text = LocalizationManager.Get("COMMON_MAX");
                }
            }
            else
            {
                this.reinforceAttackIconLabel.Clear();
                this.reinforceAttackMaxLabel.text = string.Empty;
                this.reinforceHpIconLabel.Clear();
                this.reinforceHpMaxLabel.text = string.Empty;
            }
            if (item.GetExpInfo(out num3, out num4, out num5))
            {
                this.expBase.SetActive(true);
                this.expLabel.text = string.Empty + num3;
                this.lateExpBase.SetActive(num4 > 0);
                this.lateExpLabel.text = string.Format(LocalizationManager.Get("SERVANT_STATUS_EXP_LATE"), LocalizationManager.GetNumberFormat(num4));
                this.expBar.value = num5;
            }
            else
            {
                this.expBase.SetActive(false);
            }
            this.limitCountGauge.Set(item.LimitCount, item.Servant.limitMax);
            if (item.UserServantCollection != null)
            {
                int num6;
                int num7;
                int num8;
                float num9;
                this.friendshipGauge.gameObject.SetActive(true);
                item.GetFriendshipInfo(out num6, out num7, out num8, out num9);
                this.friendshipGauge.Set(num6, num7, num8, num9);
            }
            else
            {
                this.friendshipGauge.gameObject.SetActive(false);
            }
            this.eventJoinSprite.gameObject.SetActive(item.IsEventJoin);
            if (this.baseButton != null)
            {
            }
        }
    }
}

