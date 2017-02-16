using System;
using System.Runtime.InteropServices;

public class FavoriteChangeListViewItem : ListViewItem
{
    protected long amountSortValue;
    protected int classId;
    protected int friendship;
    protected int friendshipMax;
    protected IconLabelInfo iconLabelInfo;
    protected bool isBase;
    protected bool isFavorite;
    protected bool isLock;
    protected bool isUse;
    protected int partyIndex;
    protected int rarity;
    protected ServantEntity servantEntity;
    protected int svtId;
    protected UserServantEntity userSvtEntity;

    public FavoriteChangeListViewItem(int index, UserServantEntity userSvtEntity, long[] partyUserSvtList, bool isFavorite) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.userSvtEntity = userSvtEntity;
        this.svtId = this.userSvtEntity.svtId;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.userSvtEntity.limitCount);
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        this.isFavorite = isFavorite;
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userSvtEntity.userId, this.svtId).getFriendShipRankInfo(out this.friendship, out this.friendshipMax);
        this.partyIndex = -1;
        long id = this.userSvtEntity.id;
        for (int i = 0; i < partyUserSvtList.Length; i++)
        {
            if (id == partyUserSvtList[i])
            {
                this.partyIndex = i;
                break;
            }
        }
        this.isLock = this.userSvtEntity.IsLock();
        base.sortValue2 = this.userSvtEntity.id;
        this.amountSortValue = -1L;
        this.iconLabelInfo.Clear();
    }

    ~FavoriteChangeListViewItem()
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

    public void ModifyItem(bool isFavorite)
    {
        this.isFavorite = isFavorite;
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
        bool flag = true;
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.PARTY:
                if (!this.isFavorite)
                {
                    base.sortValue0 = (this.partyIndex >= 0) ? ((long) 1) : ((long) 0);
                    base.sortValue1 = this.partyIndex;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                goto Label_0519;

            case ListViewSort.SortKind.CREATE:
                if (!this.isFavorite)
                {
                    base.sortValue1 = this.userSvtEntity.id;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                goto Label_0519;

            case ListViewSort.SortKind.RARITY:
                if (!this.isFavorite)
                {
                    base.sortValue1 = this.rarity;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                goto Label_0519;

            case ListViewSort.SortKind.LEVEL:
                if (!this.isFavorite)
                {
                    base.sortValue1 = this.userSvtEntity.lv;
                    break;
                }
                base.sortValue0 = 3L;
                break;

            case ListViewSort.SortKind.NP_LEVEL:
                int num;
                int num2;
                this.userSvtEntity.getTreasureDeviceInfo(out num, out num2);
                if (!this.isFavorite)
                {
                    base.sortValue1 = num;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.NP_LEVEL, num, num2, 0, false, false);
                flag = false;
                goto Label_0519;

            case ListViewSort.SortKind.HP:
                if (!this.isFavorite)
                {
                    base.sortValue1 = this.userSvtEntity.hp;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.HP, this.userSvtEntity.hp, this.userSvtEntity.adjustHp, 0, false, false);
                goto Label_0519;

            case ListViewSort.SortKind.ATK:
                if (!this.isFavorite)
                {
                    base.sortValue1 = this.userSvtEntity.atk;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.ATK, this.userSvtEntity.atk, this.userSvtEntity.adjustAtk, 0, false, false);
                goto Label_0519;

            case ListViewSort.SortKind.COST:
                if (!this.isFavorite)
                {
                    base.sortValue1 = this.servantEntity.cost;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.COST, this.servantEntity.cost, 0, 0, false, false);
                flag = false;
                goto Label_0519;

            case ListViewSort.SortKind.FRIENDSHIP:
                if (!this.isFavorite)
                {
                    base.sortValue1 = this.friendship;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.FRIEND_RANK, this.friendship, this.friendshipMax, 0, false, false);
                flag = false;
                goto Label_0519;

            case ListViewSort.SortKind.AMOUNT:
                if (this.amountSortValue < 0L)
                {
                    this.amountSortValue = (sort.GetManager() as FavoriteChangeListViewManager).GetAmountSortValue(this.svtId);
                }
                if (this.isFavorite)
                {
                    base.sortValue0 = 3L;
                }
                else
                {
                    base.sortValue1 = (this.amountSortValue << 0x20) + this.svtId;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                goto Label_0519;

            default:
                goto Label_0519;
        }
        this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
    Label_0519:
        if (!flag && (this.servantEntity.IsExpUp || this.servantEntity.IsStatusUp))
        {
            this.iconLabelInfo.Clear();
        }
        return true;
    }

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public bool IsBase =>
        this.isBase;

    public bool IsCanNotSelect =>
        this.isFavorite;

    public bool IsEventJoin =>
        ((this.userSvtEntity != null) && this.userSvtEntity.IsEventJoin());

    public bool IsFavorite =>
        this.isFavorite;

    public bool IsLock =>
        this.isLock;

    public bool IsParty =>
        (this.partyIndex >= 0);

    public ServantEntity Servant =>
        this.servantEntity;

    public int SvtId =>
        this.svtId;

    public UserServantEntity UserServant =>
        this.userSvtEntity;
}

