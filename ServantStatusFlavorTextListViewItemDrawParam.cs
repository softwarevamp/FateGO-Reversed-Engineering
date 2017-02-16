using System;
using UnityEngine;

public class ServantStatusFlavorTextListViewItemDrawParam : ServantStatusFlavorTextListViewItemDraw
{
    [SerializeField]
    protected ServantStatusParameterGauge agilityGauge;
    [SerializeField]
    protected UISprite baseSprite;
    [SerializeField]
    protected ServantStatusParameterGauge defenseGauge;
    [SerializeField]
    protected ServantStatusParameterGauge luckGauge;
    [SerializeField]
    protected ServantStatusParameterGauge magicGauge;
    [SerializeField]
    protected ServantStatusParameterGauge npGauge;
    [SerializeField]
    protected ServantStatusParameterGauge powerGauge;

    public override ServantStatusFlavorTextListViewItemDraw.Kind GetKind() => 
        ServantStatusFlavorTextListViewItemDraw.Kind.PARAM;

    public override void SetItem(ServantStatusListViewItem item, bool isOpen, bool isNew, string text, string text2, ServantStatusFlavorTextListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, isOpen, isNew, text, text2, mode);
        if ((item != null) && (mode != ServantStatusFlavorTextListViewItemDraw.DispMode.INVISIBLE))
        {
            this.powerGauge.Set(ServantStatusParameterGauge.Kind.POWER, item.Power);
            this.defenseGauge.Set(ServantStatusParameterGauge.Kind.DEFENSE, item.Defense);
            this.agilityGauge.Set(ServantStatusParameterGauge.Kind.AGILITY, item.Agility);
            this.magicGauge.Set(ServantStatusParameterGauge.Kind.MAGIC, item.Magic);
            this.luckGauge.Set(ServantStatusParameterGauge.Kind.LUCK, item.Luck);
            this.npGauge.Set(ServantStatusParameterGauge.Kind.NP, item.Np);
        }
    }
}

