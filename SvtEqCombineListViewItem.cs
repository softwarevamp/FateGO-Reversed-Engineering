using System;

public class SvtEqCombineListViewItem : ListViewItem
{
    protected long amountSortValue;
    protected int classId;
    protected int currentLimitCnt;
    protected int friendship;
    protected int friendshipMax;
    protected IconLabelInfo iconLabelInfo = new IconLabelInfo();
    protected bool isBaseLvMax;
    protected bool isBaseSvt;
    protected bool isEquiped;
    protected bool isFavorite;
    protected bool isLimitCntMax;
    protected bool isLimitCntTarget;
    protected bool isLock;
    protected bool isLvMax;
    protected bool isMaterialSvt;
    protected bool isMaxNextLv;
    protected bool isMaxSelect;
    protected bool isSvtEqMaterial;
    protected bool isUseSupportEquip;
    protected int materialExp;
    protected int maxLimitCnt;
    protected int rarity;
    protected ServantEntity servantEntity;
    protected string sortInfoText;
    protected int svtId;
    protected Type type;
    protected UserServantEntity userSvtEntity;

    public SvtEqCombineListViewItem(Type type, int index, UserServantEntity userSvtEntity, bool isFavorite, bool isPaty, UserServantEntity baseUsrSvtData, bool isMtSvt)
    {
        this.type = type;
        base.index = index;
        this.userSvtEntity = userSvtEntity;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userSvtEntity.svtId);
        this.svtId = this.userSvtEntity.svtId;
        this.classId = this.servantEntity.classId;
        UserDeckMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userSvtEntity.svtId, this.userSvtEntity.limitCount);
        this.rarity = entity.rarity;
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userSvtEntity.userId, this.userSvtEntity.svtId).getFriendShipRankInfo(out this.friendship, out this.friendshipMax);
        this.isLock = false;
        this.isLimitCntTarget = false;
        this.isBaseSvt = false;
        this.isMaterialSvt = false;
        this.isMaxSelect = false;
        this.isSvtEqMaterial = false;
        this.maxLimitCnt = this.userSvtEntity.getLimitCntMax();
        this.currentLimitCnt = this.userSvtEntity.limitCount;
        this.isBaseLvMax = false;
        this.isMaxNextLv = false;
        this.isUseSupportEquip = false;
        if (this.type == Type.SVTEQ_BASE)
        {
            this.isLvMax = this.userSvtEntity.isLevelMax();
            this.isLimitCntMax = userSvtEntity.isLimitCountMax();
            this.isLock = this.userSvtEntity.IsLock();
            this.isEquiped = master.IsEquip(this.userSvtEntity.id);
            if ((baseUsrSvtData != null) && (userSvtEntity.id == baseUsrSvtData.id))
            {
                this.isBaseSvt = true;
            }
            if (this.servantEntity.IsServantEquipMaterial)
            {
                this.isSvtEqMaterial = true;
            }
        }
        if (type == Type.SVTEQ_MATERIAL)
        {
            this.isMaterialSvt = isMtSvt;
            CombineMaterialEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.COMBINE_MATERIAL).getEntityFromId<CombineMaterialEntity>(this.servantEntity.combineMaterialId, this.userSvtEntity.lv);
            this.materialExp = entity3.value;
            if (baseUsrSvtData != null)
            {
                if (this.userSvtEntity.svtId == baseUsrSvtData.svtId)
                {
                    this.isLimitCntMax = baseUsrSvtData.isLimitCountMax();
                    this.isLimitCntTarget = !this.isLimitCntMax;
                }
                this.isBaseLvMax = baseUsrSvtData.isLevelMax();
            }
            else
            {
                this.isLimitCntTarget = false;
            }
            this.isEquiped = master.IsEquip(this.userSvtEntity.id);
            foreach (UserServantLearderEntity entity4 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantLearderMaster>(DataNameKind.Kind.USER_SERVANT_LEADER).getEntityList())
            {
                if ((entity4.equipTarget1 != null) && (entity4.equipTarget1.userSvtId == this.userSvtEntity.id))
                {
                    this.isUseSupportEquip = true;
                    break;
                }
            }
            this.isLock = this.userSvtEntity.IsLock();
            this.isFavorite = isFavorite;
        }
        base.sortValue2 = this.userSvtEntity.svtId;
        this.amountSortValue = -1L;
        this.iconLabelInfo.Clear();
    }

    ~SvtEqCombineListViewItem()
    {
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
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.CREATE:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.userSvtEntity.id;
                    break;
                }
                base.sortValue0 = 3L;
                break;

            case ListViewSort.SortKind.RARITY:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.rarity;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                goto Label_0368;

            case ListViewSort.SortKind.LEVEL:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.userSvtEntity.lv;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                goto Label_0368;

            case ListViewSort.SortKind.HP:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.userSvtEntity.hp + this.userSvtEntity.adjustHp;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.HP, this.userSvtEntity.hp, this.userSvtEntity.adjustHp, 0, false, false);
                goto Label_0368;

            case ListViewSort.SortKind.ATK:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.userSvtEntity.atk + this.userSvtEntity.adjustAtk;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.ATK, this.userSvtEntity.atk, this.userSvtEntity.adjustAtk, 0, false, false);
                goto Label_0368;

            case ListViewSort.SortKind.COST:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.servantEntity.cost;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.COST, this.servantEntity.cost, 0, 0, false, false);
                goto Label_0368;

            case ListViewSort.SortKind.CLASS:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.classId;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                goto Label_0368;

            case ListViewSort.SortKind.AMOUNT:
                if (this.amountSortValue < 0L)
                {
                    this.amountSortValue = (sort.GetManager() as SvtEqCombineListViewManager).GetAmountSortValue(this.svtId);
                }
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = (this.amountSortValue << 0x20) + this.svtId;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                goto Label_0368;

            default:
                goto Label_0368;
        }
        this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
    Label_0368:
        return true;
    }

    public int GetCurrentLimitCnt =>
        this.currentLimitCnt;

    public int GetMaterialExp =>
        this.materialExp;

    public int GetMaxLimitCnt =>
        this.maxLimitCnt;

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public bool IsBaseLvMax =>
        this.isBaseLvMax;

    public bool IsBaseSvt =>
        this.isBaseSvt;

    public bool IsCanNotBaseSelect
    {
        get
        {
            bool flag = false;
            if (this.type != Type.SVTEQ_BASE)
            {
                return flag;
            }
            return ((this.isLvMax && this.isLimitCntMax) || this.isSvtEqMaterial);
        }
    }

    public bool IsCanNotSelect =>
        ((((this.isLock || this.isEquiped) || (this.isMaxNextLv && !this.isLimitCntTarget)) || (this.isBaseLvMax && !this.isLimitCntTarget)) || this.isUseSupportEquip);

    public bool IsEquip =>
        this.isEquiped;

    public bool IsLimitCntMax =>
        this.isLimitCntMax;

    public bool IsLimitTarget =>
        this.isLimitCntTarget;

    public bool IsLock =>
        this.isLock;

    public bool IsLvMax =>
        this.isLvMax;

    public bool IsMaxNextLv
    {
        get => 
            this.isMaxNextLv;
        set
        {
            this.isMaxNextLv = value;
        }
    }

    public bool IsMtSelect
    {
        get => 
            this.isMaterialSvt;
        set
        {
            this.isMaterialSvt = value;
        }
    }

    public bool IsSelectMax
    {
        get => 
            this.isMaxSelect;
        set
        {
            this.isMaxSelect = value;
        }
    }

    public bool IsSvtEqMaterial =>
        this.isSvtEqMaterial;

    public bool IsUseSupportEquip =>
        this.isUseSupportEquip;

    public Type ListType =>
        this.type;

    public int SvtId =>
        this.svtId;

    public int SvtRariry =>
        this.rarity;

    public UserServantEntity UserSvtEntity =>
        this.userSvtEntity;

    public long UserSvtId =>
        this.userSvtEntity.id;

    public enum Type
    {
        SVTEQ_BASE,
        SVTEQ_MATERIAL,
        SUM
    }
}

