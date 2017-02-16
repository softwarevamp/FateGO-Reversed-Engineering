using System;
using System.Runtime.InteropServices;

public class FollowerSelectItemListViewItem : ListViewItem
{
    protected int followerClassId;
    protected int followerIndex;
    protected FollowerInfo followerInfo;
    protected int friendPointUpVal;
    protected IconLabelInfo iconLabelInfo;
    protected bool isEventUpVal;
    protected ServantEntity servantEntity;
    protected EventUpValSetupInfo setupInfo;

    public FollowerSelectItemListViewItem(int index, FollowerInfo followerInfo, int followerClassId, int friendPointUpVal, EventUpValSetupInfo setupInfo) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.friendPointUpVal = friendPointUpVal;
        this.setupInfo = setupInfo;
        this.followerInfo = followerInfo;
        this.AnalyzeEntity(followerClassId);
    }

    protected void AnalyzeEntity(int followerClassId)
    {
        this.followerClassId = followerClassId;
        if (this.followerInfo == null)
        {
            this.followerIndex = 0;
            this.servantEntity = null;
            this.isEventUpVal = false;
            base.sortValue0 = 0L;
            base.sortValue2 = 0L;
            return;
        }
        Follower.Type type = (Follower.Type) this.followerInfo.type;
        this.followerIndex = this.followerInfo.getIndex(followerClassId);
        int id = this.followerInfo.getSvtId(this.followerIndex);
        if (id <= 0)
        {
            this.servantEntity = null;
            switch (type)
            {
                case Follower.Type.FRIEND:
                    base.sortValue0 = 1L;
                    base.sortValue2 = this.followerInfo.getUpdatedAt();
                    break;

                case Follower.Type.NOT_FRIEND:
                    base.sortValue0 = 0L;
                    base.sortValue2 = this.followerInfo.getUpdatedAt();
                    break;

                case Follower.Type.NPC:
                    base.sortValue0 = 2L;
                    base.sortValue2 = -this.followerInfo.userId;
                    break;
            }
        }
        else
        {
            this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(id);
            switch (type)
            {
                case Follower.Type.FRIEND:
                    base.sortValue0 = 4L;
                    base.sortValue2 = this.servantEntity.collectionNo;
                    break;

                case Follower.Type.NOT_FRIEND:
                    base.sortValue0 = 3L;
                    base.sortValue2 = this.servantEntity.collectionNo;
                    break;

                case Follower.Type.NPC:
                    base.sortValue0 = 5L;
                    base.sortValue2 = -this.followerInfo.userId;
                    break;
            }
            this.isEventUpVal = this.followerInfo.getEventUpVal(this.setupInfo, this.followerIndex);
            goto Label_01A4;
        }
        this.isEventUpVal = false;
    Label_01A4:
        this.iconLabelInfo.Clear();
    }

    ~FollowerSelectItemListViewItem()
    {
    }

    public int GetFriendPointUpVal()
    {
        if (this.followerInfo != null)
        {
            int num = this.followerInfo.getFriendPointUpVal(this.followerIndex);
            return ((this.friendPointUpVal <= num) ? num : this.friendPointUpVal);
        }
        return 0;
    }

    public bool GetNpInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId)
    {
        if (this.followerInfo != null)
        {
            return this.followerInfo.getTreasureDeviceInfo(out tdId, out tdLv, out tdMaxLv, out tdRank, out tdMaxRank, out tdName, out tdExplanation, out tdGuageCount, out tdCardId, this.followerIndex);
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

    public bool GetSkillInfo(out int[] skillIdList, out int[] skillLvList, out int[] skillChargeList, out string[] skillTitleList, out string[] skillExplanationList)
    {
        if (this.followerInfo != null)
        {
            this.followerInfo.getSkillInfo(out skillIdList, out skillLvList, out skillChargeList, out skillTitleList, out skillExplanationList, this.followerIndex);
            return true;
        }
        skillIdList = new int[BalanceConfig.SvtSkillListMax];
        skillLvList = new int[BalanceConfig.SvtSkillListMax];
        skillChargeList = new int[BalanceConfig.SvtSkillListMax];
        skillTitleList = new string[BalanceConfig.SvtSkillListMax];
        skillExplanationList = new string[BalanceConfig.SvtSkillListMax];
        return false;
    }

    public int GetTreasureDeviceLevelIcon()
    {
        if (this.followerInfo != null)
        {
            return this.followerInfo.getTreasureDeviceLevelIcon(this.followerIndex);
        }
        return 0;
    }

    public bool IsEventUpVal() => 
        this.isEventUpVal;

    public void SetClassId(int followerClassId)
    {
        this.AnalyzeEntity(followerClassId);
    }

    public override bool SetSortValue(ListViewSort sort)
    {
        base.isTermination = false;
        base.isTerminationSpace = false;
        base.sortValue1 = 0L;
        Follower.Type type = (Follower.Type) this.followerInfo.type;
        int num = this.followerInfo.getSvtId(this.followerIndex);
        if (sort.Kind != ListViewSort.SortKind.LOGIN_ACCESS)
        {
            if (num <= 0)
            {
                switch (type)
                {
                    case Follower.Type.FRIEND:
                        base.sortValue0 = 1L;
                        break;

                    case Follower.Type.NOT_FRIEND:
                        base.sortValue0 = 0L;
                        break;
                }
                base.sortValue2 = this.followerInfo.getUpdatedAt();
                this.iconLabelInfo.Clear();
                goto Label_0314;
            }
            switch (type)
            {
                case Follower.Type.FRIEND:
                    base.sortValue0 = 4L;
                    break;

                case Follower.Type.NOT_FRIEND:
                    base.sortValue0 = 3L;
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case Follower.Type.FRIEND:
                    base.sortValue0 = 4L;
                    break;

                case Follower.Type.NOT_FRIEND:
                    base.sortValue0 = 3L;
                    break;
            }
            base.sortValue1 = this.followerInfo.getUpdatedAt();
            if (num > 0)
            {
                base.sortValue2 = this.servantEntity.collectionNo;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.followerInfo.getLv(this.followerIndex), this.followerInfo.getMaxLv(this.followerIndex), 0, false, false);
            }
            else
            {
                base.sortValue2 = 0L;
                this.iconLabelInfo.Clear();
            }
            goto Label_0314;
        }
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.LEVEL:
                base.sortValue1 = this.followerInfo.getLv(this.followerIndex);
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.followerInfo.getLv(this.followerIndex), this.followerInfo.getMaxLv(this.followerIndex), 0, false, false);
                break;

            case ListViewSort.SortKind.HP:
                base.sortValue1 = this.followerInfo.getHp(this.followerIndex) + this.followerInfo.getEquipHp(this.followerIndex);
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.HP, this.followerInfo.getHp(this.followerIndex), this.followerInfo.getAdjustHp(this.followerIndex), this.followerInfo.getEquipHp(this.followerIndex), false, false);
                break;

            case ListViewSort.SortKind.ATK:
                base.sortValue1 = this.followerInfo.getAtk(this.followerIndex) + this.followerInfo.getEquipAtk(this.followerIndex);
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.ATK, this.followerInfo.getAtk(this.followerIndex), this.followerInfo.getAdjustAtk(this.followerIndex), this.followerInfo.getEquipAtk(this.followerIndex), false, false);
                break;

            default:
                base.sortValue1 = base.sortValue2;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.followerInfo.getLv(this.followerIndex), this.followerInfo.getMaxLv(this.followerIndex), 0, false, false);
                break;
        }
        base.sortValue2 = this.servantEntity.collectionNo;
    Label_0314:
        if (type == Follower.Type.NPC)
        {
            base.sortValue0 = 5L;
            base.sortValue1 = 0L;
            base.sortValue2 = -this.followerInfo.userId;
        }
        return true;
    }

    public EquipTargetInfo EquipInfo =>
        this.followerInfo.getEquipTarget1(this.followerInfo.getIndexForSupport(this.followerClassId));

    public FollowerInfo FollowerData =>
        this.followerInfo;

    public long FollowerId
    {
        get
        {
            if (this.followerInfo != null)
            {
                return this.followerInfo.userId;
            }
            return 0L;
        }
    }

    public int FollowerIndex =>
        this.followerIndex;

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public long LoginTime
    {
        get
        {
            if (this.followerInfo != null)
            {
                return this.followerInfo.getUpdatedAt();
            }
            return 0L;
        }
    }

    public int PlayerLevel
    {
        get
        {
            if (this.followerInfo != null)
            {
                return this.followerInfo.userLv;
            }
            return 1;
        }
    }

    public string PlayerNameText
    {
        get
        {
            if (this.followerInfo != null)
            {
                return this.followerInfo.userName;
            }
            return "error";
        }
    }

    public int SelectClassId
    {
        get
        {
            if (this.followerIndex == 0)
            {
                return 0;
            }
            return this.followerClassId;
        }
    }

    public ServantLeaderInfo ServantLeader
    {
        get
        {
            if (this.followerInfo != null)
            {
                return this.followerInfo.getServantLeaderInfo(this.followerIndex);
            }
            return null;
        }
    }

    public int SvtId
    {
        get
        {
            if (this.followerInfo != null)
            {
                return this.followerInfo.getSvtId(this.followerIndex);
            }
            return 0;
        }
    }

    public string SvtNameText
    {
        get
        {
            if ((this.followerInfo != null) && (this.followerInfo.type == 3))
            {
                return this.followerInfo.userName;
            }
            if (this.servantEntity != null)
            {
                return this.servantEntity.name;
            }
            return null;
        }
    }

    public Follower.Type Type
    {
        get
        {
            if (this.followerInfo != null)
            {
                return (Follower.Type) this.followerInfo.type;
            }
            return Follower.Type.NONE;
        }
    }
}

