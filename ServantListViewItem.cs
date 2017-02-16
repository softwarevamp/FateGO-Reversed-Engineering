using System;
using System.Runtime.InteropServices;

public class ServantListViewItem : ListViewItem
{
    protected long amountSortValue;
    protected int classId;
    protected int friendship;
    protected int friendshipMax;
    protected IconLabelInfo iconLabelInfo;
    protected IconLabelInfo iconLabelInfo2;
    protected bool isLock;
    protected bool isUse;
    protected int partyIndex;
    protected int rarity;
    protected ServantEntity servantEntity;
    protected int svtId;
    protected UserServantEntity userSvtEntity;

    public ServantListViewItem(int index, UserServantEntity userSvtEntity, long[] partyUserServantList, long[] partyUserEquipList) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.iconLabelInfo2 = new IconLabelInfo();
        this.userSvtEntity = userSvtEntity;
        this.svtId = this.userSvtEntity.svtId;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.userSvtEntity.limitCount);
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userSvtEntity.userId, this.svtId).getFriendShipRankInfo(out this.friendship, out this.friendshipMax);
        this.partyIndex = -1;
        this.isUse = false;
        long id = this.userSvtEntity.id;
        if (this.servantEntity.IsKeepServantEquip)
        {
            for (int i = 0; i < partyUserEquipList.Length; i++)
            {
                if (id == partyUserEquipList[i])
                {
                    this.partyIndex = i;
                    this.isUse = true;
                    break;
                }
            }
        }
        else
        {
            for (int j = 0; j < partyUserServantList.Length; j++)
            {
                if (id == partyUserServantList[j])
                {
                    this.partyIndex = j;
                    break;
                }
            }
        }
        this.isLock = this.userSvtEntity.IsLock();
        base.sortValue2 = this.userSvtEntity.id;
        this.amountSortValue = -1L;
        this.iconLabelInfo.Clear();
        this.iconLabelInfo2.Clear();
    }

    ~ServantListViewItem()
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

    public void Modify(UserServantEntity userSvtEntity, long[] partyUserServantList, long[] partyUserEquipList)
    {
        this.userSvtEntity = userSvtEntity;
        this.svtId = this.userSvtEntity.svtId;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.userSvtEntity.limitCount);
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        UserServantCollectionEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userSvtEntity.userId, this.svtId);
        this.friendship = entity2.friendshipRank;
        this.partyIndex = -1;
        this.isUse = false;
        long id = this.userSvtEntity.id;
        if (this.servantEntity.IsKeepServantEquip)
        {
            for (int i = 0; i < partyUserEquipList.Length; i++)
            {
                if (id == partyUserEquipList[i])
                {
                    this.partyIndex = i;
                    this.isUse = true;
                    break;
                }
            }
        }
        else
        {
            for (int j = 0; j < partyUserServantList.Length; j++)
            {
                if (id == partyUserServantList[j])
                {
                    this.partyIndex = j;
                    break;
                }
            }
        }
        this.isLock = this.userSvtEntity.IsLock();
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
                        goto Label_0157;
                    }
                    return false;

                case 2:
                case 14:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_2_14))
                    {
                        goto Label_0157;
                    }
                    return false;

                case 3:
                case 15:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_3_15))
                    {
                        goto Label_0157;
                    }
                    return false;

                case 4:
                case 0x10:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_4_16))
                    {
                        goto Label_0157;
                    }
                    return false;

                case 5:
                case 0x11:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_5_17))
                    {
                        goto Label_0157;
                    }
                    return false;

                case 6:
                case 0x12:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_6_18))
                    {
                        goto Label_0157;
                    }
                    return false;

                case 7:
                case 0x13:
                    if (sort.GetFilter(ListViewSort.FilterKind.CLASS_7_19))
                    {
                        goto Label_0157;
                    }
                    return false;
            }
            if (!sort.GetFilter(ListViewSort.FilterKind.CLASS_ETC))
            {
                return false;
            }
        }
    Label_0157:
        base.sortValue0 = 0L;
        base.sortValue2 = this.userSvtEntity.id;
        bool flag = true;
        this.iconLabelInfo2.Set(IconLabelInfo.IconKind.RARITY_EXCEED, this.rarity, this.userSvtEntity.exceedCount, 0, false, false);
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.PARTY:
                base.sortValue0 = (this.partyIndex >= 0) ? ((long) 1) : ((long) 0);
                base.sortValue1 = this.partyIndex;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.CREATE:
                base.sortValue1 = this.userSvtEntity.createdAt;
                this.iconLabelInfo2.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                this.iconLabelInfo.SetTime(IconLabelInfo.IconKind.YEAR_DATE, this.userSvtEntity.createdAt, false, false);
                break;

            case ListViewSort.SortKind.RARITY:
                base.sortValue1 = this.rarity;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.LEVEL:
                base.sortValue1 = this.userSvtEntity.lv;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.NP_LEVEL:
                int num;
                int num2;
                this.userSvtEntity.getTreasureDeviceInfo(out num, out num2);
                base.sortValue1 = num;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.NP_LEVEL, num, num2, 0, false, false);
                flag = false;
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
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.COST, this.servantEntity.cost, 0, 0, false, false);
                flag = false;
                break;

            case ListViewSort.SortKind.CLASS:
                base.sortValue1 = this.classId;
                base.sortValue2 = this.userSvtEntity.lv;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.FRIENDSHIP:
                base.sortValue1 = this.friendship;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.FRIEND_RANK, this.friendship, this.friendshipMax, 0, false, false);
                flag = false;
                break;

            case ListViewSort.SortKind.AMOUNT:
                if (this.amountSortValue < 0L)
                {
                    this.amountSortValue = (sort.GetManager() as ServantListViewManager).GetAmountSortValue(this.svtId);
                }
                base.sortValue1 = (this.amountSortValue << 0x20) + this.svtId;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                break;
        }
        if (!flag && (this.servantEntity.IsExpUp || this.servantEntity.IsStatusUp))
        {
            this.iconLabelInfo.Clear();
        }
        return true;
    }

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public IconLabelInfo IconInfo2 =>
        this.iconLabelInfo2;

    public bool IsCanNotSelect =>
        false;

    public bool IsLock =>
        this.isLock;

    public bool IsParty =>
        (this.partyIndex >= 0);

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

    public int SvtId =>
        this.svtId;

    public UserServantEntity UserServant =>
        this.userSvtEntity;
}

