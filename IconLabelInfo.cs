using System;
using System.Runtime.InteropServices;

public class IconLabelInfo
{
    protected int adjustData;
    protected int data;
    protected int equipData;
    protected IconKind iconKind;
    protected bool isHide;
    protected bool isMaxHide;
    protected long time;

    public void Clear()
    {
        this.iconKind = IconKind.CLEAR;
        this.data = 0;
        this.adjustData = 0;
        this.equipData = 0;
        this.time = 0L;
        this.isHide = false;
        this.isMaxHide = false;
    }

    public void Set(IconLabelInfo info)
    {
        this.iconKind = info.iconKind;
        this.data = info.data;
        this.adjustData = info.adjustData;
        this.equipData = info.equipData;
        this.time = info.time;
        this.isHide = info.isHide;
        this.isMaxHide = info.isMaxHide;
    }

    public void Set(IconKind iconKind, int data = 0, int adjustData = 0, int equipData = 0, bool isHide = false, bool isMaxHide = false)
    {
        this.iconKind = iconKind;
        this.data = data;
        this.adjustData = adjustData;
        this.equipData = equipData;
        this.time = 0L;
        this.isHide = isHide;
        this.isMaxHide = isMaxHide;
    }

    public void SetPurchaseDecision(IconKind iconKind, int price = 0, int holdCount = -1)
    {
        this.iconKind = iconKind;
        this.data = price;
        this.adjustData = holdCount;
        this.equipData = 0;
        this.time = this.time;
        this.isHide = this.isHide;
        this.isMaxHide = this.isMaxHide;
    }

    public void SetTime(IconKind iconKind, long time, bool isHide = false, bool isMaxHide = false)
    {
        this.iconKind = iconKind;
        this.data = 0;
        this.adjustData = 0;
        this.equipData = 0;
        this.time = time;
        this.isHide = isHide;
        this.isMaxHide = isMaxHide;
    }

    public int AdjustData =>
        this.adjustData;

    public int Data =>
        this.data;

    public int EquipData =>
        this.equipData;

    public bool IsHide =>
        this.isHide;

    public bool IsMaxHide =>
        this.isMaxHide;

    public IconKind Kind =>
        this.iconKind;

    public long TimeData =>
        this.time;

    public enum IconKind
    {
        CLEAR,
        DATA,
        LEVEL,
        HP,
        HP_COMMA,
        ATK,
        ATK_COMMA,
        COST,
        ID,
        LIMIT_COUNT,
        FREE,
        UNIT,
        STONE_UNIT,
        MANA_UNIT,
        QP_UNIT,
        BANK_UNIT,
        FRIEND_POINT_UNIT,
        FRIEND_RANK_UNIT,
        EVENT_ITEM_UNIT,
        STONE_FRAGMENTS_UNIT,
        ANONYMOUS_UNIT,
        STONE,
        MANA,
        QP,
        BANK,
        FRIEND_POINT,
        EVENT_ITEM,
        STONE_FRAGMENTS,
        ANONYMOUS,
        FRIEND_RANK,
        NP_LEVEL,
        RARITY,
        RARITY_EXCEED,
        AMOUNT,
        ADD_HP,
        ADD_ATK,
        ADD_COST,
        CHANGE_HP,
        CHANGE_ATK,
        CHANGE_COST,
        DIFFER_HP,
        DIFFER_ATK,
        DIFFER_COST,
        REINFORCEMENT_HP,
        REINFORCEMENT_ATK,
        EXTENTION_HP,
        EXTENTION_ATK,
        FOLLOWER_SKILL_LEVEL,
        YEAR_DATE
    }
}

