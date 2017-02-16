using System;
using System.Runtime.InteropServices;

public class PartyServantListViewItem : ListViewItem
{
    protected long amountSortValue;
    protected int classId;
    protected long[] equipIdList;
    protected ServantEntity equipServantEntity;
    protected UserServantEntity equipUserServantEntity;
    protected int friendship;
    protected int friendshipMax;
    protected IconLabelInfo iconLabelInfo;
    protected bool isBase;
    protected bool isCostOver;
    protected bool isEventJoin;
    protected bool isLock;
    protected bool isMainDeck;
    protected bool isSame;
    protected bool isSelectEmpty;
    protected bool isSelectEventJoin;
    protected bool isSelectLeader;
    protected int partyIndex;
    protected int rarity;
    protected ServantEntity servantEntity;
    protected int svtId;
    protected UserServantEntity userServantEntity;

    public PartyServantListViewItem(int index, UserServantEntity userServantEntity, PartyListViewItem partyItem, int num) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        PartyOrganizationListViewItem member = partyItem.GetMember(num);
        this.isSelectLeader = num == 0;
        this.isSelectEmpty = member.UserServant == null;
        this.isSelectEventJoin = member.IsEventJoin;
        this.isMainDeck = partyItem.IsMainDeck;
        this.userServantEntity = userServantEntity;
        this.svtId = this.userServantEntity.svtId;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.userServantEntity.limitCount);
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userServantEntity.userId, this.svtId).getFriendShipRankInfo(out this.friendship, out this.friendshipMax);
        this.isBase = false;
        this.partyIndex = -1;
        this.isCostOver = false;
        this.isSame = false;
        this.equipUserServantEntity = null;
        this.equipServantEntity = null;
        this.equipIdList = null;
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            PartyOrganizationListViewItem item2 = partyItem.GetMember(i);
            if (item2.UserServant != null)
            {
                if (this.userServantEntity.id == item2.UserServant.id)
                {
                    this.isBase = i == num;
                    this.isSame = false;
                    this.partyIndex = i;
                    this.SetEquipStatus(item2.GetEquipList());
                    break;
                }
                if ((this.servantEntity.baseSvtId == item2.Servant.baseSvtId) && (i != num))
                {
                    this.isSame = true;
                }
            }
        }
        if (this.partyIndex < 0)
        {
            this.isCostOver = ((partyItem.Cost - member.MargeCost) + this.servantEntity.cost) > partyItem.MaxCost;
        }
        this.isLock = this.userServantEntity.IsLock();
        this.isEventJoin = this.userServantEntity.IsEventJoin();
        base.sortValue2 = this.userServantEntity.id;
        this.amountSortValue = -1L;
        this.iconLabelInfo.Clear();
    }

    ~PartyServantListViewItem()
    {
    }

    public long[] GetEquipList() => 
        this.equipIdList;

    public bool GetNpInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId)
    {
        if (this.userServantEntity != null)
        {
            return this.userServantEntity.getTreasureDeviceInfo(out tdId, out tdLv, out tdMaxLv, out tdRank, out tdMaxRank, out tdName, out tdExplanation, out tdGuageCount, out tdCardId);
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

    public void Modify(UserServantEntity userServantEntity)
    {
        this.userServantEntity = userServantEntity;
        this.isLock = this.userServantEntity.IsLock();
        this.isEventJoin = this.userServantEntity.IsEventJoin();
    }

    protected void SetEquipStatus(long[] equipIdList = null)
    {
        this.equipUserServantEntity = null;
        this.equipServantEntity = null;
        this.equipIdList = null;
        if (this.userServantEntity != null)
        {
            if ((equipIdList == null) || (this.equipIdList != equipIdList))
            {
                this.equipIdList = (equipIdList == null) ? new long[BalanceConfig.SvtEquipMax] : ((long[]) equipIdList.Clone());
            }
            if (this.equipIdList[0] > 0L)
            {
                this.equipUserServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.equipIdList[0]);
                if ((this.equipUserServantEntity != null) && (this.equipUserServantEntity.svtId > 0))
                {
                    this.equipServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.equipUserServantEntity.svtId);
                }
            }
        }
    }

    public override bool SetSortValue(ListViewSort sort)
    {
        base.isTermination = false;
        base.isTerminationSpace = false;
        base.sortValue1 = -1L;
        if (!this.isBase)
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
                            goto Label_0165;
                        }
                        return false;

                    case 2:
                    case 14:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_2_14))
                        {
                            goto Label_0165;
                        }
                        return false;

                    case 3:
                    case 15:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_3_15))
                        {
                            goto Label_0165;
                        }
                        return false;

                    case 4:
                    case 0x10:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_4_16))
                        {
                            goto Label_0165;
                        }
                        return false;

                    case 5:
                    case 0x11:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_5_17))
                        {
                            goto Label_0165;
                        }
                        return false;

                    case 6:
                    case 0x12:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_6_18))
                        {
                            goto Label_0165;
                        }
                        return false;

                    case 7:
                    case 0x13:
                        if (sort.GetFilter(ListViewSort.FilterKind.CLASS_7_19))
                        {
                            goto Label_0165;
                        }
                        return false;
                }
                if (!sort.GetFilter(ListViewSort.FilterKind.CLASS_ETC))
                {
                    return false;
                }
            }
        }
    Label_0165:
        if (this.isBase)
        {
            base.sortValue0 = BalanceConfig.DeckMemberMax + 1;
        }
        else if (this.partyIndex >= 0)
        {
            base.sortValue0 = BalanceConfig.DeckMemberMax - this.partyIndex;
        }
        else
        {
            base.sortValue0 = 0L;
        }
        base.sortValue2 = this.userServantEntity.id;
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.PARTY:
                base.sortValue1 = this.partyIndex;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.CREATE:
                base.sortValue1 = this.userServantEntity.createdAt;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.RARITY:
                base.sortValue1 = this.rarity;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.RARITY, this.rarity, 0, 0, false, false);
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
                break;

            case ListViewSort.SortKind.HP:
                if (this.equipUserServantEntity == null)
                {
                    base.sortValue1 = this.userServantEntity.hp;
                    this.iconLabelInfo.Set(IconLabelInfo.IconKind.HP, this.userServantEntity.hp, this.userServantEntity.adjustHp, 0, false, false);
                    break;
                }
                base.sortValue1 = this.userServantEntity.hp + this.equipUserServantEntity.hp;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.EXTENTION_HP, this.userServantEntity.hp, this.userServantEntity.adjustHp, this.equipUserServantEntity.hp, false, false);
                break;

            case ListViewSort.SortKind.ATK:
                if (this.equipUserServantEntity == null)
                {
                    base.sortValue1 = this.userServantEntity.atk;
                    this.iconLabelInfo.Set(IconLabelInfo.IconKind.ATK, this.userServantEntity.atk, this.userServantEntity.adjustAtk, 0, false, false);
                    break;
                }
                base.sortValue1 = this.userServantEntity.atk + this.equipUserServantEntity.atk;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.EXTENTION_ATK, this.userServantEntity.atk, this.userServantEntity.adjustAtk, this.equipUserServantEntity.atk, false, false);
                break;

            case ListViewSort.SortKind.COST:
                base.sortValue1 = this.servantEntity.cost;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.CLASS:
                base.sortValue1 = this.classId;
                base.sortValue2 = this.userServantEntity.lv;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;

            case ListViewSort.SortKind.FRIENDSHIP:
                base.sortValue1 = this.friendship;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.FRIEND_RANK, this.friendship, this.friendshipMax, 0, false, false);
                break;

            case ListViewSort.SortKind.AMOUNT:
                if (this.amountSortValue < 0L)
                {
                    this.amountSortValue = (sort.GetManager() as PartyServantListViewManager).GetAmountSortValue(this.svtId);
                }
                base.sortValue1 = (this.amountSortValue << 0x20) + this.svtId;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
                break;
        }
        return true;
    }

    public int ClassId =>
        this.classId;

    public int Cost =>
        this.servantEntity.cost;

    public int EquipCost
    {
        get
        {
            if (this.equipServantEntity != null)
            {
                return this.equipServantEntity.cost;
            }
            return 0;
        }
    }

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public bool IsActionEventJoinLeader =>
        ((this.isSelectLeader && this.isEventJoin) || (this.isSelectEventJoin && (this.partyIndex == 0)));

    public bool IsBase =>
        this.isBase;

    public bool IsCostOver =>
        this.isCostOver;

    public bool IsEquip =>
        (this.equipServantEntity != null);

    public bool IsEventJoin =>
        this.isEventJoin;

    public bool IsLock =>
        this.isLock;

    public bool IsMainDeck =>
        this.isMainDeck;

    public bool IsMainDeckLeader =>
        (this.isMainDeck && (this.partyIndex == 0));

    public bool IsParty =>
        (this.partyIndex >= 0);

    public bool IsSame =>
        this.isSame;

    public bool IsSelectEmpty =>
        this.isSelectEmpty;

    public bool IsSelectEventJoin =>
        this.isSelectEventJoin;

    public bool IsSelectLeader =>
        this.isSelectLeader;

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

    public int PartyIndex =>
        this.partyIndex;

    public int Rarity =>
        this.rarity;

    public ServantEntity Servant =>
        this.servantEntity;

    public int SvtId =>
        this.svtId;

    public UserServantEntity UserServant =>
        this.userServantEntity;
}

