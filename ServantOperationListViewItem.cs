using System;

public class ServantOperationListViewItem : ListViewItem
{
    protected long amountSortValue;
    protected ServantAttributeKind attribute;
    protected int classId;
    protected int friendship;
    protected int friendshipMax;
    protected IconLabelInfo iconLabelInfo;
    protected IconLabelInfo iconLabelInfo2;
    protected bool isFavorite;
    protected bool isLock;
    protected bool isPartyEquip;
    protected bool isUse;
    protected bool isUseSupport;
    protected bool isUseSupportEquip;
    protected int partyIndex;
    protected int priority;
    protected int rarity;
    protected ServantEntity servantEntity;
    protected int svtId;
    protected UserServantEntity userServantEntity;

    public ServantOperationListViewItem(int index, UserServantEntity userServantEntity, long[] partyUserServantList, long[] partyUserEquipList, bool isFavorite) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.iconLabelInfo2 = new IconLabelInfo();
        base.index = index;
        this.userServantEntity = userServantEntity;
        this.svtId = this.userServantEntity.svtId;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.userServantEntity.limitCount);
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        this.isFavorite = isFavorite;
        if (this.servantEntity.type == 2)
        {
            this.attribute = ServantAttributeKind.Heroine;
        }
        else if (this.userServantEntity.IsEventJoin())
        {
            this.attribute = ServantAttributeKind.Limited;
        }
        else
        {
            this.attribute = ServantAttributeKind.None;
        }
        this.isLock = this.userServantEntity.IsLock();
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userServantEntity.userId, this.userServantEntity.svtId).getFriendShipRankInfo(out this.friendship, out this.friendshipMax);
        this.partyIndex = -1;
        this.isUse = false;
        this.isPartyEquip = false;
        this.isUseSupport = false;
        this.isUseSupportEquip = false;
        long id = this.userServantEntity.id;
        if (!this.servantEntity.IsKeepServantEquip)
        {
            for (int i = 0; i < partyUserServantList.Length; i++)
            {
                if (id == partyUserServantList[i])
                {
                    this.partyIndex = i;
                    break;
                }
            }
        }
        else
        {
            for (int j = 0; j < partyUserEquipList.Length; j++)
            {
                if (id == partyUserEquipList[j])
                {
                    this.partyIndex = j;
                    this.isPartyEquip = true;
                    this.isUse = true;
                    break;
                }
            }
            foreach (UserServantLearderEntity entity3 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantLearderMaster>(DataNameKind.Kind.USER_SERVANT_LEADER).getEntityList())
            {
                if ((entity3.equipTarget1 != null) && (entity3.equipTarget1.userSvtId == id))
                {
                    this.isUseSupportEquip = true;
                    this.isUse = true;
                    break;
                }
            }
            goto Label_02C9;
        }
        foreach (UserServantLearderEntity entity4 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantLearderMaster>(DataNameKind.Kind.USER_SERVANT_LEADER).getEntityList())
        {
            if (entity4.userSvtId == id)
            {
                this.isUseSupport = true;
                break;
            }
        }
    Label_02C9:
        this.amountSortValue = -1L;
        this.iconLabelInfo.Clear();
        this.iconLabelInfo2.Clear();
    }

    ~ServantOperationListViewItem()
    {
    }

    public void ModifyItem(bool isFavorite)
    {
        this.isFavorite = isFavorite;
        this.isLock = this.userServantEntity.IsLock();
    }

    public override bool SetSortValue(ListViewSort sort)
    {
        base.isTermination = false;
        base.isTerminationSpace = false;
        base.sortValue0 = 0L;
        base.sortValue1 = -1L;
        if (!base.IsSelect)
        {
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
                            goto Label_016F;
                        }
                        return false;

                    case 2:
                    case 14:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_2_14))
                        {
                            goto Label_016F;
                        }
                        return false;

                    case 3:
                    case 15:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_3_15))
                        {
                            goto Label_016F;
                        }
                        return false;

                    case 4:
                    case 0x10:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_4_16))
                        {
                            goto Label_016F;
                        }
                        return false;

                    case 5:
                    case 0x11:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_5_17))
                        {
                            goto Label_016F;
                        }
                        return false;

                    case 6:
                    case 0x12:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_6_18))
                        {
                            goto Label_016F;
                        }
                        return false;

                    case 7:
                    case 0x13:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_7_19))
                        {
                            goto Label_016F;
                        }
                        return false;
                }
                if (!sort.GetFilter(ListViewSort.FilterKind.CLASS_ETC))
                {
                    return false;
                }
            }
        }
    Label_016F:
        base.sortValue2 = this.userServantEntity.id;
        bool flag = true;
        this.iconLabelInfo2.Set(IconLabelInfo.IconKind.RARITY_EXCEED, this.rarity, 0, 0, false, false);
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.CREATE:
                base.sortValue1 = this.userServantEntity.id;
                this.iconLabelInfo.SetTime(IconLabelInfo.IconKind.YEAR_DATE, this.userServantEntity.createdAt, false, false);
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.RARITY:
                base.sortValue1 = this.rarity;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.LEVEL:
                base.sortValue1 = this.userServantEntity.lv;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.NP_LEVEL:
                int num;
                int num2;
                this.userServantEntity.getTreasureDeviceInfo(out num, out num2);
                base.sortValue1 = num;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.NP_LEVEL, num, num2, 0, false, false);
                flag = false;
                break;

            case ListViewSort.SortKind.HP:
                base.sortValue1 = this.userServantEntity.hp;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.HP, this.userServantEntity.hp, this.userServantEntity.adjustHp, 0, false, false);
                break;

            case ListViewSort.SortKind.ATK:
                base.sortValue1 = this.userServantEntity.atk;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.ATK, this.userServantEntity.atk, this.userServantEntity.adjustAtk, 0, false, false);
                break;

            case ListViewSort.SortKind.COST:
                base.sortValue1 = this.servantEntity.cost;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.COST, this.servantEntity.cost, 0, 0, false, false);
                flag = false;
                break;

            case ListViewSort.SortKind.CLASS:
                base.sortValue1 = this.classId;
                base.sortValue2 = this.userServantEntity.lv;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.FRIENDSHIP:
                base.sortValue1 = this.friendship;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.FRIEND_RANK, this.friendship, this.friendshipMax, 0, false, false);
                flag = false;
                break;

            case ListViewSort.SortKind.AMOUNT:
                if (this.amountSortValue < 0L)
                {
                    this.amountSortValue = (sort.GetManager() as ServantOperationListViewManager).GetAmountSortValue(this.svtId);
                }
                base.sortValue1 = (this.amountSortValue << 0x20) + this.svtId;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;
        }
        if (!flag && (this.servantEntity.IsExpUp || this.servantEntity.IsStatusUp))
        {
            this.iconLabelInfo.Clear();
        }
        return true;
    }

    public void setUserServantEntity(UserServantEntity entity)
    {
        this.userServantEntity = entity;
    }

    public ServantAttributeKind GetAttribute =>
        this.attribute;

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public IconLabelInfo IconInfo2 =>
        this.iconLabelInfo2;

    public string ImageName =>
        string.Empty;

    public bool IsCanNotSelect =>
        ((((this.isFavorite || (this.partyIndex >= 0)) || (this.isUse || this.isPartyEquip)) || (((this.attribute != ServantAttributeKind.None) || this.isLock) || this.IsUseSupportServant)) || this.IsUseSupportEquip);

    public bool IsFavorite =>
        this.isFavorite;

    public bool IsLock =>
        this.isLock;

    public bool IsOrganization =>
        this.servantEntity.IsOrganization;

    public bool IsParty =>
        (this.partyIndex >= 0);

    public bool IsPartyEquip =>
        this.isPartyEquip;

    public bool IsSellEnableServant =>
        (this.attribute == ServantAttributeKind.None);

    public bool IsUse =>
        this.isUse;

    public bool IsUseSupportEquip =>
        this.isUseSupportEquip;

    public bool IsUseSupportServant =>
        this.isUseSupport;

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

    public int SvtId =>
        this.svtId;

    public UserServantEntity UserServant =>
        this.userServantEntity;

    public long UserSvtId =>
        this.userServantEntity.id;

    public enum ServantAttributeKind
    {
        None,
        Heroine,
        Limited
    }
}

