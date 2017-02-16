using System;
using System.Runtime.InteropServices;

public class FriendOperationItemListViewItem : ListViewItem
{
    protected int classPos;
    protected IconLabelInfo iconLabelInfo;
    protected FriendStatus.Kind kind;
    protected ServantEntity servantEntity;
    protected OtherUserGameEntity userGameEntity;

    public FriendOperationItemListViewItem(FriendStatus.Kind kind, int index, OtherUserGameEntity e, int classPos = 0) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.kind = kind;
        this.userGameEntity = e;
        this.classPos = classPos;
        this.AnalyzeEntity();
    }

    public FriendOperationItemListViewItem(FriendStatus.Kind kind, int index, int id, int classPos = 0) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.kind = kind;
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.OTHER_USER_GAME).getEntityFromId<OtherUserGameEntity>(id);
        this.classPos = classPos;
        this.AnalyzeEntity();
    }

    protected void AnalyzeEntity()
    {
        int id = this.userGameEntity.getSvtId(this.classPos);
        if (id == 0)
        {
            this.servantEntity = null;
            base.sortValue2 = 0L;
        }
        else
        {
            this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(id);
            base.sortValue2 = this.servantEntity.collectionNo;
        }
        this.iconLabelInfo.Clear();
    }

    ~FriendOperationItemListViewItem()
    {
    }

    public bool GetNpInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId)
    {
        if (this.userGameEntity != null)
        {
            return this.userGameEntity.getTreasureDeviceInfo(out tdId, out tdLv, out tdMaxLv, out tdRank, out tdMaxRank, out tdName, out tdExplanation, out tdGuageCount, out tdCardId, this.classPos);
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
        if (this.userGameEntity != null)
        {
            this.userGameEntity.getSkillInfo(out skillIdList, out skillLvList, out skillChargeList, out skillTitleList, out skillExplanationList, this.classPos);
            return true;
        }
        skillIdList = new int[BalanceConfig.SvtSkillListMax];
        skillLvList = new int[BalanceConfig.SvtSkillListMax];
        skillChargeList = new int[BalanceConfig.SvtSkillListMax];
        skillTitleList = new string[BalanceConfig.SvtSkillListMax];
        skillExplanationList = new string[BalanceConfig.SvtSkillListMax];
        return false;
    }

    public int GetTreasureDeviceLevelIcon() => 
        this.userGameEntity.getTreasureDeviceLevelIcon(this.classPos);

    public override bool SetSortValue(ListViewSort sort)
    {
        base.isTermination = false;
        base.isTerminationSpace = false;
        base.sortValue1 = -1L;
        if (this.servantEntity == null)
        {
            base.sortValue0 = -1L;
            return true;
        }
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.LEVEL:
                base.sortValue1 = this.userGameEntity.getLv(this.classPos);
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userGameEntity.getLv(this.classPos), this.userGameEntity.getMaxLv(this.classPos), 0, false, false);
                break;

            case ListViewSort.SortKind.HP:
                base.sortValue1 = this.userGameEntity.getHp(this.classPos) + this.userGameEntity.getEquipHp(this.classPos);
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.HP, this.userGameEntity.getHp(this.classPos), this.userGameEntity.getAdjustHp(this.classPos), this.userGameEntity.getEquipHp(this.classPos), false, false);
                break;

            case ListViewSort.SortKind.ATK:
                base.sortValue1 = this.userGameEntity.getAtk(this.classPos) + this.userGameEntity.getEquipAtk(this.classPos);
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.ATK, this.userGameEntity.getAtk(this.classPos), this.userGameEntity.getAdjustAtk(this.classPos), this.userGameEntity.getEquipAtk(this.classPos), false, false);
                break;

            case ListViewSort.SortKind.CLASS:
                base.sortValue1 = this.servantEntity.classId;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userGameEntity.getLv(this.classPos), this.userGameEntity.getMaxLv(this.classPos), 0, false, false);
                break;

            case ListViewSort.SortKind.LOGIN_ACCESS:
                base.sortValue1 = this.userGameEntity.getUpdatedAt(this.classPos);
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userGameEntity.getLv(this.classPos), this.userGameEntity.getMaxLv(this.classPos), 0, false, false);
                break;

            default:
                base.sortValue1 = base.sortValue2;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userGameEntity.getLv(this.classPos), this.userGameEntity.getMaxLv(this.classPos), 0, false, false);
                break;
        }
        return true;
    }

    private string ToString() => 
        ("Other " + this.PlayerNameText);

    public int ClassPos =>
        this.classPos;

    public int EquipSvtId
    {
        get
        {
            if (this.userGameEntity != null)
            {
                return this.userGameEntity.getEquipSvtId(this.classPos);
            }
            return 0;
        }
    }

    public OtherUserGameEntity GameUser =>
        this.userGameEntity;

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public FriendStatus.Kind Kind =>
        this.kind;

    public long LoginTime
    {
        get
        {
            if (this.userGameEntity != null)
            {
                return this.userGameEntity.getUpdatedAt(this.classPos);
            }
            return 0L;
        }
    }

    public int PlayerLevel
    {
        get
        {
            if (this.userGameEntity != null)
            {
                return this.userGameEntity.userLv;
            }
            return 1;
        }
    }

    public string PlayerNameText
    {
        get
        {
            if (this.userGameEntity != null)
            {
                return this.userGameEntity.userName;
            }
            return "error";
        }
    }

    public ServantEntity SvtEntity
    {
        get
        {
            if (this.servantEntity != null)
            {
                return this.servantEntity;
            }
            return null;
        }
    }

    public string SvtNameText
    {
        get
        {
            if (this.servantEntity != null)
            {
                return this.servantEntity.name;
            }
            return LocalizationManager.Get("COMMON_NO_ENTRY");
        }
    }
}

