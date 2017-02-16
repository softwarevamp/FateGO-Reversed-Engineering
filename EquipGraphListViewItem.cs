using System;
using System.Runtime.InteropServices;

public class EquipGraphListViewItem : ListViewItem
{
    protected long amountSortValue;
    protected int classId;
    protected IconLabelInfo iconLabelInfo;
    protected bool isBase;
    protected bool isLock;
    protected bool isOldBase;
    protected bool isUse;
    protected int rarity;
    protected ServantEntity servantEntity;
    protected UserServantEntity userSvtEntity;

    public EquipGraphListViewItem(int index, UserServantEntity userSvtEntity, long targetSvtEquipId, long oldTargetSvtEquipId, long[] useSvtEquipList) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.userSvtEntity = userSvtEntity;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userSvtEntity.svtId);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userSvtEntity.svtId, this.userSvtEntity.limitCount);
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        if (userSvtEntity.id == targetSvtEquipId)
        {
            this.isBase = true;
            this.isUse = false;
        }
        else
        {
            this.isBase = false;
            this.isUse = false;
            if (userSvtEntity.id != oldTargetSvtEquipId)
            {
                for (int i = 0; i < useSvtEquipList.Length; i++)
                {
                    if (userSvtEntity.id == useSvtEquipList[i])
                    {
                        this.isUse = true;
                        break;
                    }
                }
            }
        }
        this.isOldBase = this.isBase;
        this.isLock = this.userSvtEntity.IsLock();
        base.sortValue2 = this.servantEntity.collectionNo;
        this.amountSortValue = -1L;
        this.iconLabelInfo.Clear();
    }

    ~EquipGraphListViewItem()
    {
    }

    public bool GetNpInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId)
    {
        if (this.userSvtEntity != null)
        {
            return this.userSvtEntity.getTreasureDeviceInfo(out tdId, out tdLv, out tdMaxLv, out tdRank, out tdMaxRank, out tdName, out tdExplanation, out tdGuageCount, out tdCardId);
        }
        tdId = 0;
        tdLv = 0;
        tdMaxLv = 0;
        tdRank = 0;
        tdMaxRank = 0;
        tdName = string.Empty;
        tdExplanation = string.Empty;
        tdGuageCount = 0;
        tdCardId = 0;
        return false;
    }

    public void Modify(UserServantEntity userSvtEntity, long targetSvtEquipId, long oldTargetSvtEquipId)
    {
        this.userSvtEntity = userSvtEntity;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userSvtEntity.svtId);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userSvtEntity.svtId, this.userSvtEntity.limitCount);
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        if (userSvtEntity.id == targetSvtEquipId)
        {
            this.isBase = true;
            this.isUse = false;
        }
        else
        {
            this.isBase = false;
        }
        this.isOldBase = this.isBase;
        this.isLock = this.userSvtEntity.IsLock();
        base.sortValue2 = this.servantEntity.collectionNo;
        this.iconLabelInfo.Clear();
    }

    public override bool SetSortValue(ListViewSort sort)
    {
        base.isTermination = false;
        base.isTerminationSpace = false;
        base.sortValue1 = -1L;
        if (this.servantEntity.IsExpUp)
        {
            if (!sort.GetFilter(ListViewSort.FilterKind.CLASS_EXP_UP))
            {
                return false;
            }
        }
        else if (this.servantEntity.IsStatusUp)
        {
            if (!sort.GetFilter(ListViewSort.FilterKind.CLASS_STATUS_UP))
            {
                return false;
            }
        }
        else
        {
            switch (this.classId)
            {
                case 1:
                case 13:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_1_13))
                    {
                        goto Label_0155;
                    }
                    return false;

                case 2:
                case 14:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_2_14))
                    {
                        goto Label_0155;
                    }
                    return false;

                case 3:
                case 15:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_3_15))
                    {
                        goto Label_0155;
                    }
                    return false;

                case 4:
                case 0x10:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_4_16))
                    {
                        goto Label_0155;
                    }
                    return false;

                case 5:
                case 0x11:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_5_17))
                    {
                        goto Label_0155;
                    }
                    return false;

                case 6:
                case 0x12:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_6_18))
                    {
                        goto Label_0155;
                    }
                    return false;

                case 7:
                case 0x13:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_7_19))
                    {
                        goto Label_0155;
                    }
                    return false;
            }
            if (!sort.GetFilter(ListViewSort.FilterKind.CLASS_ETC))
            {
                return false;
            }
        }
    Label_0155:
        if (this.isOldBase)
        {
            base.sortValue0 = 2L;
        }
        else
        {
            base.sortValue0 = 1L;
        }
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.CREATE:
                base.sortValue1 = this.userSvtEntity.createdAt;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.RARITY:
                base.sortValue1 = this.rarity;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.LEVEL:
                base.sortValue1 = this.userSvtEntity.lv;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.HP:
                base.sortValue1 = this.userSvtEntity.hp;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.HP, this.userSvtEntity.hp, this.userSvtEntity.adjustHp, 0, false, false);
                break;

            case ListViewSort.SortKind.ATK:
                base.sortValue1 = this.userSvtEntity.atk;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.ATK, this.userSvtEntity.atk, this.userSvtEntity.adjustAtk, 0, false, false);
                break;

            case ListViewSort.SortKind.COST:
                base.sortValue1 = this.servantEntity.cost;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.AMOUNT:
                if (this.amountSortValue < 0L)
                {
                    this.amountSortValue = (sort.GetManager() as EquipGraphListViewManager).GetAmountSortValue(this.userSvtEntity.svtId);
                }
                base.sortValue1 = (this.amountSortValue << 0x20) + this.userSvtEntity.svtId;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;
        }
        return true;
    }

    private string ToString() => 
        ("Other " + this.NameText);

    public int AdjustAttack
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.adjustAtk;
            }
            return 0;
        }
    }

    public int AdjustHp
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.adjustHp;
            }
            return 0;
        }
    }

    public int Attack
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.atk;
            }
            return 0;
        }
    }

    public int Cost
    {
        get
        {
            if (this.servantEntity != null)
            {
                return this.servantEntity.cost;
            }
            return 0;
        }
    }

    public int Hp
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.hp;
            }
            return 0;
        }
    }

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public bool IsBase
    {
        get => 
            this.isBase;
        set
        {
            this.isBase = value;
        }
    }

    public bool IsCanNotSelect
    {
        get
        {
            if (this.isBase)
            {
                return false;
            }
            return this.isUse;
        }
    }

    public bool IsLock =>
        this.isLock;

    public bool IsUse =>
        this.isUse;

    public string NameText
    {
        get
        {
            if (this.servantEntity != null)
            {
                return this.servantEntity.name;
            }
            return "error";
        }
    }

    public ServantEntity Servant =>
        this.servantEntity;

    public UserServantEntity UserServant =>
        this.userSvtEntity;
}

