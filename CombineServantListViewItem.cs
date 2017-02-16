using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class CombineServantListViewItem : ListViewItem
{
    protected int adjustAtkMax;
    protected int adjustHpMax;
    protected long amountSortValue;
    protected int atkBase;
    protected int classId;
    protected int currentLimitCnt;
    private List<int> enableSkillUp = new List<int>();
    public const float FIXED_VAL = 1000f;
    protected int friendship;
    protected int friendshipMax;
    protected int hpBase;
    protected IconLabelInfo iconLabelInfo = new IconLabelInfo();
    protected IconLabelInfo iconLabelInfo2 = new IconLabelInfo();
    protected bool isBaseLvMax;
    protected bool isBaseSvt;
    protected bool isCanLimitUp;
    protected bool isCanStUp;
    protected bool isEquiped;
    protected bool isEventJoin;
    protected bool isExpUpSvt;
    protected bool isFavorite;
    protected bool isHeroineSvt;
    protected bool isLimitCntMax;
    protected bool isLimitCntTarget;
    protected bool isLimitUpItemNum;
    protected bool isLock;
    protected bool isLvExceedItemNum;
    protected bool isLvExceedMax;
    protected bool isLvMax;
    protected bool isMaterialSvt;
    protected bool isMaxNextLv;
    protected bool isMaxSelect;
    protected bool isNeed;
    protected bool isPaty;
    protected bool isSameSvt;
    protected bool isSkillLvMax;
    protected bool isSkillUpItemNum;
    protected bool isStatusUpSvt;
    protected bool isTdLvMax;
    protected bool isUseSupport;
    private const int MATERIAL_ITEM = 0x1f3f;
    private const int MATERIAL_ITEM_NUM = 1;
    protected int materialExp;
    protected int maxLimitCnt;
    protected int priority;
    protected int rarity;
    protected ServantEntity servantEntity;
    protected string sortInfoText;
    protected int svtId;
    protected Type type;
    protected UserServantEntity userSvtEntity;

    public CombineServantListViewItem(Type type, int index, UserServantEntity userSvtEntity, bool isFavorite, bool isPaty, UserServantEntity baseUsrSvtData, bool isMtSvt)
    {
        this.type = type;
        base.index = index;
        this.userSvtEntity = userSvtEntity;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userSvtEntity.svtId);
        this.svtId = this.userSvtEntity.svtId;
        this.classId = this.servantEntity.classId;
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userSvtEntity.svtId, this.userSvtEntity.limitCount);
        this.rarity = entity.rarity;
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userSvtEntity.userId, this.userSvtEntity.svtId).getFriendShipRankInfo(out this.friendship, out this.friendshipMax);
        UserGameEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int qp = entity3.qp;
        this.isPaty = false;
        this.isFavorite = false;
        this.isLock = false;
        this.isLimitCntTarget = false;
        this.isBaseSvt = false;
        this.isMaterialSvt = false;
        this.isHeroineSvt = false;
        this.isStatusUpSvt = false;
        this.isMaxSelect = false;
        this.isLimitUpItemNum = false;
        this.isCanLimitUp = false;
        this.isSkillLvMax = false;
        this.isSkillUpItemNum = false;
        this.isTdLvMax = false;
        this.isSameSvt = false;
        this.isCanStUp = false;
        this.isExpUpSvt = false;
        this.isLvMax = false;
        this.isEventJoin = false;
        this.maxLimitCnt = this.userSvtEntity.getLimitCntMax();
        this.currentLimitCnt = 0;
        this.isBaseLvMax = false;
        this.isMaxNextLv = false;
        this.isUseSupport = false;
        this.isLvExceedItemNum = false;
        if (this.type == Type.BASE)
        {
            if ((this.userSvtEntity.isLevelMax() && this.userSvtEntity.isAdjustHpMax()) && this.userSvtEntity.isAdjustAtkMax())
            {
                this.isLvMax = true;
            }
            this.isLimitCntMax = userSvtEntity.isLimitCountMax();
            this.isPaty = isPaty;
            this.isLock = this.userSvtEntity.IsLock();
            this.isEventJoin = this.userSvtEntity.IsEventJoin();
            if ((baseUsrSvtData != null) && (this.userSvtEntity.id == baseUsrSvtData.id))
            {
                this.isBaseSvt = true;
            }
            if (this.userSvtEntity.IsCombineExp())
            {
                this.isExpUpSvt = true;
            }
            if (this.servantEntity.IsStatusUp)
            {
                this.isStatusUpSvt = true;
            }
        }
        if (this.type == Type.MATERIAL)
        {
            this.isMaterialSvt = isMtSvt;
            this.isPaty = isPaty;
            foreach (UserServantLearderEntity entity4 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantLearderMaster>(DataNameKind.Kind.USER_SERVANT_LEADER).getEntityList())
            {
                if (entity4.userSvtId == this.userSvtEntity.id)
                {
                    this.isUseSupport = true;
                    break;
                }
            }
            CombineMaterialEntity entity5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.COMBINE_MATERIAL).getEntityFromId<CombineMaterialEntity>(this.servantEntity.combineMaterialId, this.userSvtEntity.lv);
            this.materialExp = entity5.value;
            if (baseUsrSvtData != null)
            {
                if (this.userSvtEntity.svtId == baseUsrSvtData.svtId)
                {
                    this.isLimitCntTarget = true;
                }
                this.isBaseLvMax = baseUsrSvtData.isLevelMax();
                int groupType = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(this.servantEntity.classId).groupType;
                int classId = baseUsrSvtData.getSvtClassId();
                int num4 = baseUsrSvtData.getSvtClassGroupType(classId);
                if (groupType.Equals(3))
                {
                    double num5 = Math.Ceiling((double) ((this.materialExp * BalanceConfig.SameClassExp) / 1000.0));
                    this.materialExp = (int) num5;
                }
                else if (groupType.Equals(num4) && (this.servantEntity.classId == baseUsrSvtData.getSvtClassId()))
                {
                    double num6 = Math.Ceiling((double) ((this.materialExp * BalanceConfig.SameClassExp) / 1000.0));
                    this.materialExp = (int) num6;
                }
            }
            else
            {
                this.isLimitCntTarget = false;
            }
            this.isFavorite = isFavorite;
            this.isLock = this.userSvtEntity.IsLock();
            this.isEventJoin = this.userSvtEntity.IsEventJoin();
            this.hpBase = entity.hpBase;
            this.atkBase = entity.atkBase;
            if (this.userSvtEntity.IsHeroine())
            {
                this.isHeroineSvt = true;
            }
            if (this.servantEntity.IsStatusUp)
            {
                this.isStatusUpSvt = true;
                int num7 = baseUsrSvtData.getSvtClassId();
                int num8 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(this.servantEntity.classId).groupType;
                Debug.Log(string.Concat(new object[] { "**** !!!!! StatusUP SvtId: ", this.servantEntity.id, " _ClassGroupType: ", num8 }));
                if ((this.servantEntity.classId == num7) || num8.Equals(3))
                {
                    this.isCanStUp = true;
                }
            }
        }
        if (this.type == Type.LIMITUP_BASE)
        {
            this.isLimitCntMax = userSvtEntity.isLimitCountMax();
            this.currentLimitCnt = this.userSvtEntity.limitCount;
            this.isLvMax = this.userSvtEntity.isLevelMax();
            this.isPaty = isPaty;
            this.isLock = this.userSvtEntity.IsLock();
            this.isEventJoin = this.userSvtEntity.IsEventJoin();
            if ((baseUsrSvtData != null) && (this.userSvtEntity.id == baseUsrSvtData.id))
            {
                this.isBaseSvt = true;
            }
            if (this.userSvtEntity.IsHeroine())
            {
                this.isHeroineSvt = true;
            }
            else
            {
                int combineLimitId = this.servantEntity.combineLimitId;
                int limitCount = this.userSvtEntity.limitCount;
                CombineLimitEntity entity8 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CombineLimitMaster>(DataNameKind.Kind.COMBINE_LIMIT).getEntityFromId<CombineLimitEntity>(combineLimitId, limitCount);
                int[] itemIds = entity8.itemIds;
                int[] itemNums = entity8.itemNums;
                UserItemMaster master5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM);
                for (int i = 0; i < itemIds.Length; i++)
                {
                    int num12 = itemIds[i];
                    int num13 = itemNums[i];
                    long[] args = new long[] { this.userSvtEntity.userId, (long) num12 };
                    UserItemEntity entity9 = master5.getEntityFromId<UserItemEntity>(args);
                    if (entity9 != null)
                    {
                        if (num12 != entity9.itemId)
                        {
                            continue;
                        }
                        if (num13 <= entity9.num)
                        {
                            this.isLimitUpItemNum = true;
                            continue;
                        }
                        this.isLimitUpItemNum = false;
                    }
                    else
                    {
                        this.isLimitUpItemNum = false;
                    }
                    break;
                }
                if (entity3.qp < entity8.qp)
                {
                    this.isLimitUpItemNum = false;
                }
            }
        }
        if (this.type == Type.SKILL_BASE)
        {
            this.isPaty = isPaty;
            this.isLock = this.userSvtEntity.IsLock();
            this.isEventJoin = this.userSvtEntity.IsEventJoin();
            if ((baseUsrSvtData != null) && (this.userSvtEntity.id == baseUsrSvtData.id))
            {
                this.isBaseSvt = true;
            }
            int[] numArray3 = this.userSvtEntity.getSkillIdList();
            int[] numArray4 = this.userSvtEntity.getSkillLevelList();
            this.isSkillLvMax = true;
            for (int j = 0; j < numArray3.Length; j++)
            {
                int id = numArray3[j];
                int num16 = numArray4[j];
                if (id > 0)
                {
                    int maxLv = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(id).maxLv;
                    if (num16 < maxLv)
                    {
                        this.isSkillLvMax = false;
                        break;
                    }
                }
            }
            bool flag = false;
            for (int k = 0; k < numArray3.Length; k++)
            {
                int item = numArray3[k];
                int num20 = numArray4[k];
                if (item > 0)
                {
                    CombineSkillEntity entity11 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.COMBINE_SKILL).getEntityFromId<CombineSkillEntity>(this.servantEntity.combineSkillId, num20);
                    if (entity11 != null)
                    {
                        int[] numArray5 = entity11.itemIds;
                        int[] numArray6 = entity11.itemNums;
                        UserItemMaster master6 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM);
                        for (int m = 0; m < numArray5.Length; m++)
                        {
                            int num22 = numArray5[m];
                            int num23 = numArray6[m];
                            long[] numArray14 = new long[] { this.userSvtEntity.userId, (long) num22 };
                            UserItemEntity entity12 = master6.getEntityFromId<UserItemEntity>(numArray14);
                            if (entity12 != null)
                            {
                                if (num22 != entity12.itemId)
                                {
                                    continue;
                                }
                                if (num23 <= entity12.num)
                                {
                                    flag = true;
                                    continue;
                                }
                                flag = false;
                            }
                            else
                            {
                                flag = false;
                            }
                            break;
                        }
                        if (entity3.qp < entity11.qp)
                        {
                            flag = false;
                        }
                        if (flag)
                        {
                            this.enableSkillUp.Add(item);
                        }
                    }
                }
            }
            this.isSkillUpItemNum = this.enableSkillUp.Count > 0;
        }
        if (this.type == Type.NP_BASE)
        {
            this.isPaty = isPaty;
            this.isLock = this.userSvtEntity.IsLock();
            this.isEventJoin = this.userSvtEntity.IsEventJoin();
            if ((baseUsrSvtData != null) && (this.userSvtEntity.id == baseUsrSvtData.id))
            {
                this.isBaseSvt = true;
            }
            if (this.userSvtEntity.IsHeroine())
            {
                this.isHeroineSvt = true;
            }
            else
            {
                int[] numArray7;
                int[] numArray8;
                int[] numArray9;
                int[] numArray10;
                int[] numArray11;
                string[] strArray;
                string[] strArray2;
                int[] numArray12;
                int[] numArray13;
                bool[] flagArray;
                this.userSvtEntity.getTreasureDeviceInfo(out numArray7, out numArray8, out numArray9, out numArray10, out numArray11, out strArray, out strArray2, out numArray13, out numArray12, out flagArray, -1);
                this.isTdLvMax = true;
                int num24 = 0;
                int num25 = 0;
                for (int n = 0; n < numArray7.Length; n++)
                {
                    num24 = numArray7[n];
                    num25 = numArray8[n];
                    if (num24 > 0)
                    {
                        int num27 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(num24).maxLv;
                        if (num25 < num27)
                        {
                            this.isTdLvMax = false;
                            break;
                        }
                    }
                }
                UserServantEntity[] entityArray = master.getSameServantList(this.userSvtEntity);
                if ((entityArray != null) && (entityArray.Length > 0))
                {
                    this.isSameSvt = true;
                }
            }
        }
        if (this.type == Type.NP_MATERIAL)
        {
            this.isMaterialSvt = isMtSvt;
            this.isPaty = isPaty;
            foreach (UserServantLearderEntity entity14 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantLearderMaster>(DataNameKind.Kind.USER_SERVANT_LEADER).getEntityList())
            {
                if (entity14.userSvtId == this.userSvtEntity.id)
                {
                    this.isUseSupport = true;
                    break;
                }
            }
            this.isFavorite = isFavorite;
            this.isLock = this.userSvtEntity.IsLock();
            this.isEventJoin = this.userSvtEntity.IsEventJoin();
            if ((baseUsrSvtData != null) && (this.userSvtEntity.id == baseUsrSvtData.id))
            {
                this.isBaseSvt = true;
            }
        }
        if (this.type == Type.LVEXCEED_BASE)
        {
            this.isLvExceedMax = this.userSvtEntity.isExceedLvMax();
            this.isLimitCntMax = this.userSvtEntity.isLimitCountMax();
            this.currentLimitCnt = this.userSvtEntity.limitCount;
            this.isLvMax = this.userSvtEntity.isLevelMax();
            this.isPaty = isPaty;
            this.isLock = this.userSvtEntity.IsLock();
            this.isEventJoin = this.userSvtEntity.IsEventJoin();
            if ((baseUsrSvtData != null) && (this.userSvtEntity.id == baseUsrSvtData.id))
            {
                this.isBaseSvt = true;
            }
            if (this.userSvtEntity.IsHeroine())
            {
                this.isHeroineSvt = true;
            }
            else
            {
                int lvExceedItemId = BalanceConfig.LvExceedItemId;
                long[] numArray15 = new long[] { NetworkManager.UserId, (long) lvExceedItemId };
                UserItemEntity entity15 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId<UserItemEntity>(numArray15);
                if (entity15 != null)
                {
                    if (lvExceedItemId == entity15.itemId)
                    {
                        if (BalanceConfig.LvExceedNeedItemNum <= entity15.num)
                        {
                            this.isLvExceedItemNum = true;
                        }
                        else
                        {
                            this.isLvExceedItemNum = false;
                        }
                    }
                }
                else
                {
                    this.isLvExceedItemNum = false;
                }
                if (entity3.qp < this.userSvtEntity.getCombineQpSvtExceed())
                {
                    this.isLvExceedItemNum = false;
                }
            }
        }
        ServantClassEntity entity16 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(this.classId);
        this.priority = entity16.priority;
        base.sortValue1B = this.priority;
        base.sortValue2 = (this.classId << 0x20) + this.rarity;
        base.sortValue2 = -base.sortValue2;
        base.sortValue2B = this.servantEntity.collectionNo;
        this.amountSortValue = -1L;
        this.iconLabelInfo.Clear();
        this.iconLabelInfo2.Clear();
    }

    ~CombineServantListViewItem()
    {
    }

    public bool GetNpInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId)
    {
        if (this.userSvtEntity != null)
        {
            return this.userSvtEntity.getTreasureDeviceInfo(out tdId, out tdLv, out tdMaxLv, out tdRank, out tdMaxRank, out tdName, out tdExplanation, out tdGuageCount, out tdCardId, 1, -1);
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

    public bool GetSkillInfo(out int[] idList, out int[] lvList, out int[] skillChargeList, out string[] skillTitleList, out string[] skillExplanationList)
    {
        if (this.userSvtEntity != null)
        {
            this.userSvtEntity.getSkillInfo(out idList, out lvList, out skillChargeList, out skillTitleList, out skillExplanationList);
            return true;
        }
        idList = new int[BalanceConfig.SvtSkillListMax];
        lvList = new int[BalanceConfig.SvtSkillListMax];
        skillChargeList = new int[BalanceConfig.SvtSkillListMax];
        skillTitleList = new string[BalanceConfig.SvtSkillListMax];
        skillExplanationList = new string[BalanceConfig.SvtSkillListMax];
        return false;
    }

    public void ModifyItem(bool isFavorite)
    {
        this.isFavorite = isFavorite;
        this.isLock = this.userSvtEntity.IsLock();
    }

    public override bool SetSortValue(ListViewSort sort)
    {
        bool flag;
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
        flag = true;
        base.sortValue2 = (this.classId << 0x20) + this.rarity;
        base.sortValue2 = -base.sortValue2;
        base.sortValue2B = this.servantEntity.collectionNo;
        this.iconLabelInfo2.Set(IconLabelInfo.IconKind.RARITY_EXCEED, this.rarity, this.userSvtEntity.exceedCount, 0, false, false);
        switch (sort.Kind)
        {
            case ListViewSort.SortKind.PARTY:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = !this.isPaty ? ((long) 1) : ((long) 0);
                    break;
                }
                base.sortValue0 = 3L;
                break;

            case ListViewSort.SortKind.CREATE:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.userSvtEntity.id;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.SetTime(IconLabelInfo.IconKind.YEAR_DATE, this.userSvtEntity.createdAt, false, false);
                this.iconLabelInfo2.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                goto Label_06BB;

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
                goto Label_06BB;

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
                goto Label_06BB;

            case ListViewSort.SortKind.NP_LEVEL:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.userSvtEntity.getTreasureDeviceLv();
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                if ((this.ListType == Type.NP_BASE) || (this.ListType == Type.NP_MATERIAL))
                {
                    this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                }
                else
                {
                    int num;
                    int num2;
                    this.userSvtEntity.getTreasureDeviceInfo(out num, out num2);
                    base.sortValue1 = num;
                    this.iconLabelInfo.Set(IconLabelInfo.IconKind.NP_LEVEL, num, num2, 0, false, false);
                }
                flag = false;
                goto Label_06BB;

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
                goto Label_06BB;

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
                goto Label_06BB;

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
                flag = false;
                goto Label_06BB;

            case ListViewSort.SortKind.CLASS:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.classId;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                base.sortValue2 = this.userSvtEntity.lv;
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
                goto Label_06BB;

            case ListViewSort.SortKind.FRIENDSHIP:
                if (!this.isBaseSvt && !this.isMaterialSvt)
                {
                    base.sortValue1 = this.friendship;
                }
                else
                {
                    base.sortValue0 = 3L;
                }
                this.iconLabelInfo.Set(IconLabelInfo.IconKind.FRIEND_RANK, this.friendship, this.friendshipMax, 0, false, false);
                flag = false;
                goto Label_06BB;

            case ListViewSort.SortKind.AMOUNT:
                if (this.amountSortValue < 0L)
                {
                    this.amountSortValue = (sort.GetManager() as CombineServantListViewManager).GetAmountSortValue(this.svtId);
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
                goto Label_06BB;

            default:
                goto Label_06BB;
        }
        this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userSvtEntity.lv, this.userSvtEntity.getLevelMax(), 0, false, false);
    Label_06BB:
        if (!flag && (this.servantEntity.IsExpUp || this.servantEntity.IsStatusUp))
        {
            this.iconLabelInfo.Clear();
        }
        return true;
    }

    public void setUserServantEntity(UserServantEntity entity)
    {
        this.userSvtEntity = entity;
    }

    private string ToString() => 
        ("UserSvt " + this.NameText);

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

    public int GetAtkAdjustMax =>
        this.adjustAtkMax;

    public int GetAtkUpVal =>
        this.atkBase;

    public int GetCurrentLimitCnt =>
        this.currentLimitCnt;

    public List<int> GetEnableSkillupList =>
        this.enableSkillUp;

    public int GetHpAdjustMax =>
        this.adjustHpMax;

    public int GetHpUpVal =>
        this.hpBase;

    public int GetMaterialExp =>
        this.materialExp;

    public int GetMaxLimitCnt =>
        this.maxLimitCnt;

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

    public IconLabelInfo IconInfo2 =>
        this.iconLabelInfo2;

    public string ImageName =>
        string.Empty;

    public bool IsAtkAdjustMax =>
        this.userSvtEntity.isAdjustAtkMax();

    public bool IsBaseLvMax =>
        this.isBaseLvMax;

    public bool IsBaseSvt =>
        this.isBaseSvt;

    public bool IsCanNotBaseSelect
    {
        get
        {
            bool isSkillLvMax = true;
            if (this.type == Type.BASE)
            {
                isSkillLvMax = (this.isLvMax || this.isExpUpSvt) || this.isStatusUpSvt;
            }
            if (this.type == Type.LIMITUP_BASE)
            {
                isSkillLvMax = this.isLimitCntMax || this.IsHeroine;
            }
            if (this.type == Type.SKILL_BASE)
            {
                isSkillLvMax = this.isSkillLvMax;
            }
            if (this.type == Type.NP_BASE)
            {
                isSkillLvMax = this.isTdLvMax || this.IsHeroine;
            }
            if (this.type == Type.SVTEQ_BASE)
            {
                isSkillLvMax = this.isLvMax && this.isLimitCntMax;
            }
            if (this.type == Type.LVEXCEED_BASE)
            {
                isSkillLvMax = (this.isLvExceedMax || this.IsHeroine) || this.IsEventJoin;
            }
            return isSkillLvMax;
        }
    }

    public bool IsCanNotLimitUp =>
        this.isLimitCntMax;

    public bool IsCanNotSelect
    {
        get
        {
            if (((this.type == Type.BASE) || (this.type == Type.LIMITUP_BASE)) || ((this.type == Type.SKILL_BASE) || (this.type == Type.NP_BASE)))
            {
                return (this.isHeroineSvt || (this.isStatusUpSvt && !this.isCanStUp));
            }
            if (this.type == Type.NP_MATERIAL)
            {
                return (this.IsCanNotSelectMaterial || this.isBaseSvt);
            }
            return ((this.IsCanNotSelectMaterial || (this.isMaxNextLv && !this.isStatusUpSvt)) || (this.isBaseLvMax && !this.isStatusUpSvt));
        }
    }

    public bool IsCanNotSelectMaterial =>
        (((((this.isFavorite || this.isPaty) || (this.isLock || this.isHeroineSvt)) || (this.isLimitCntTarget || (this.isStatusUpSvt && !this.isCanStUp))) || this.isEventJoin) || this.isUseSupport);

    public bool IsCanStatusUp =>
        this.isCanStUp;

    public bool IsCombineEnableServant =>
        (!this.isHeroineSvt && !this.isEventJoin);

    public bool IsEquip =>
        this.isEquiped;

    public bool IsEventJoin =>
        this.isEventJoin;

    public bool IsExpUpSvt =>
        this.isExpUpSvt;

    public bool IsFavorite =>
        this.isFavorite;

    public bool IsHeroine =>
        this.isHeroineSvt;

    public bool IsHpAdjustMax =>
        this.userSvtEntity.isAdjustHpMax();

    public bool IsLimitCntMax =>
        this.isLimitCntMax;

    public bool IsLimitTarget =>
        this.isLimitCntTarget;

    public bool IsLimitUpItemNum =>
        this.isLimitUpItemNum;

    public bool IsLock =>
        this.isLock;

    public bool IsLvExceedItemNum =>
        this.isLvExceedItemNum;

    public bool IsLvExceedMax =>
        this.isLvExceedMax;

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

    public bool IsOrganization =>
        this.servantEntity.IsOrganization;

    public bool IsPaty =>
        this.isPaty;

    public bool IsSameServant =>
        this.isSameSvt;

    public bool IsSelectMax
    {
        get => 
            this.isMaxSelect;
        set
        {
            this.isMaxSelect = value;
        }
    }

    public bool IsSkillUpItemNum =>
        this.isSkillUpItemNum;

    public bool IsStatusUp =>
        this.isStatusUpSvt;

    public bool IsUseSupportServant =>
        this.isUseSupport;

    public string LevelText
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return LocalizationManager.GetLevelInfo(this.userSvtEntity.lv);
            }
            return "error";
        }
    }

    public Type ListType =>
        this.type;

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

    public int SvtId =>
        this.svtId;

    public int SvtRariry =>
        this.rarity;

    public UserServantEntity UserSvtEntity
    {
        get
        {
            long id = this.userSvtEntity.id;
            return (this.userSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id));
        }
    }

    public long UserSvtId =>
        this.userSvtEntity.id;

    public enum Type
    {
        BASE,
        MATERIAL,
        LIMITUP_BASE,
        SKILL_BASE,
        NP_BASE,
        NP_MATERIAL,
        SVTEQ_BASE,
        SVTEQ_MATERIAL,
        LVEXCEED_BASE,
        SUM
    }
}

