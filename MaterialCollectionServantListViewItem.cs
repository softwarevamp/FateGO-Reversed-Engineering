using System;

public class MaterialCollectionServantListViewItem : ListViewItem
{
    protected int classId;
    protected CollectionStatus.Kind collectiionStatus;
    protected IconLabelInfo iconLabelInfo;
    protected int rarity;
    protected ServantEntity servantEntity;
    protected UserServantCollectionEntity userSvtCollectionEntity;

    public MaterialCollectionServantListViewItem(int index, UserServantCollectionEntity userSvtCollectionEntity) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        base.index = index;
        this.userSvtCollectionEntity = userSvtCollectionEntity;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userSvtCollectionEntity.svtId);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userSvtCollectionEntity.svtId, this.userSvtCollectionEntity.maxLimitCount);
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        this.collectiionStatus = (CollectionStatus.Kind) this.userSvtCollectionEntity.status;
        base.sortValue2 = -this.servantEntity.collectionNo;
        this.iconLabelInfo.Clear();
    }

    ~MaterialCollectionServantListViewItem()
    {
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
        switch (this.collectiionStatus)
        {
            case CollectionStatus.Kind.NOT_GET:
                if (sort.GetFilter(ListViewSort.FilterKind.COLLECTION_NOT_GET))
                {
                    break;
                }
                return false;

            case CollectionStatus.Kind.FIND:
                if (sort.GetFilter(ListViewSort.FilterKind.COLLECTION_FIND))
                {
                    break;
                }
                return false;

            case CollectionStatus.Kind.GET:
                if (sort.GetFilter(ListViewSort.FilterKind.COLLECTION_GET))
                {
                    break;
                }
                return false;
        }
        if (sort.Kind == ListViewSort.SortKind.ID)
        {
            base.sortValue0 = 0L;
        }
        else
        {
            switch (this.collectiionStatus)
            {
                case CollectionStatus.Kind.FIND:
                    base.sortValue0 = 1L;
                    goto Label_020E;

                case CollectionStatus.Kind.GET:
                    base.sortValue0 = 2L;
                    goto Label_020E;
            }
            base.sortValue0 = 0L;
        }
    Label_020E:
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.RARITY:
                base.sortValue1 = this.rarity;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.ID, this.servantEntity.collectionNo, 0, 0, false, false);
                break;

            case ListViewSort.SortKind.LIMIT_COUNT:
                base.sortValue1 = this.userSvtCollectionEntity.maxLimitCount;
                if (this.userSvtCollectionEntity.maxLimitCount > 0)
                {
                    this.iconLabelInfo.Set(IconLabelInfo.IconKind.LIMIT_COUNT, this.userSvtCollectionEntity.maxLimitCount, 0, 0, false, false);
                }
                else
                {
                    this.iconLabelInfo.Clear();
                }
                break;

            case ListViewSort.SortKind.ID:
                base.sortValue1 = this.servantEntity.collectionNo;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.ID, this.servantEntity.collectionNo, 0, 0, false, false);
                break;

            default:
                base.sortValue1 = this.servantEntity.collectionNo;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.ID, this.servantEntity.collectionNo, 0, 0, false, false);
                break;
        }
        return true;
    }

    public string AttackText
    {
        get
        {
            if (this.userSvtCollectionEntity != null)
            {
                return LocalizationManager.GetAttackInfo(this.userSvtCollectionEntity.maxAtk);
            }
            return "error";
        }
    }

    public CollectionStatus.Kind CollectionKind
    {
        get
        {
            if (this.userSvtCollectionEntity != null)
            {
                return (CollectionStatus.Kind) this.userSvtCollectionEntity.status;
            }
            return CollectionStatus.Kind.NOT_GET;
        }
    }

    public string CostText
    {
        get
        {
            if (this.servantEntity != null)
            {
                return LocalizationManager.GetCostInfo(this.servantEntity.cost);
            }
            return "error";
        }
    }

    public string HpText
    {
        get
        {
            if (this.userSvtCollectionEntity != null)
            {
                return LocalizationManager.GetHpInfo(this.userSvtCollectionEntity.maxHp);
            }
            return "error";
        }
    }

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public bool IsCanNotSelect
    {
        get
        {
            if (this.servantEntity.IsEnemyCollectionDetail)
            {
                return (this.userSvtCollectionEntity.status == 0);
            }
            return (this.userSvtCollectionEntity.status != 2);
        }
    }

    public bool IsEnemyCollectionDetail =>
        this.servantEntity.IsEnemyCollectionDetail;

    public bool IsServantEquip =>
        ((this.servantEntity != null) && this.servantEntity.IsKeepServantEquip);

    public string LevelText
    {
        get
        {
            if (this.userSvtCollectionEntity != null)
            {
                return LocalizationManager.GetLevelInfo(this.userSvtCollectionEntity.maxLv);
            }
            return "error";
        }
    }

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

    public string RarityText =>
        LocalizationManager.GetRarityInfo(this.rarity);

    public UserServantCollectionEntity UserServantCollection =>
        this.userSvtCollectionEntity;
}

