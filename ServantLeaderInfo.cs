using System;
using System.Runtime.InteropServices;

public class ServantLeaderInfo
{
    public int adjustAtk;
    public int adjustHp;
    public int atk;
    public int classId;
    public EquipTargetInfo equipTarget1;
    public int exceedCount;
    public int exp;
    public int hideFlag;
    public int hp;
    public int limitCount;
    public int lv;
    public int skillId1;
    public int skillId2;
    public int skillId3;
    public int skillLv1;
    public int skillLv2;
    public int skillLv3;
    public int svtId;
    public int treasureDeviceId;
    public int treasureDeviceLv;
    public long updatedAt;
    public long userId;
    public long userSvtId;

    public ServantLeaderInfo()
    {
    }

    public ServantLeaderInfo(int svtId)
    {
        this.userId = 1L;
        this.userSvtId = 1L;
        this.svtId = svtId;
        this.limitCount = 0;
        this.exceedCount = 0;
        this.lv = 1;
        this.exp = 0;
        this.hideFlag = 0;
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId);
        ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(svtId, this.limitCount);
        this.hp = entity2.hpBase;
        this.atk = entity2.atkBase;
        this.adjustAtk = 0;
        this.adjustHp = 0;
        this.skillLv1 = 1;
        this.skillLv2 = 1;
        this.skillLv3 = 1;
        int[] numArray = new int[BalanceConfig.SvtSkillListMax];
        ServantSkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL);
        for (int i = 1; i <= BalanceConfig.SvtSkillListMax; i++)
        {
            int priority = 1;
            while (true)
            {
                ServantSkillEntity entity3 = master.getEntityFromId(svtId, i, priority);
                if ((entity3 == null) || !entity3.isUse(this.userId, this.lv, this.limitCount, -1))
                {
                    break;
                }
                numArray[i - 1] = entity3.getSkillId();
                priority++;
            }
        }
        this.skillId1 = numArray[0];
        this.skillId2 = numArray[1];
        this.skillId3 = numArray[2];
        this.treasureDeviceId = 0;
        this.treasureDeviceLv = 1;
        if (entity.IsServant)
        {
            int num = 1;
            ServantTreasureDvcMaster master2 = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantTreasureDvcMaster>(DataNameKind.Kind.SERVANT_TREASUREDEVICE);
            int friendshipRank = 0;
            foreach (ServantTreasureDvcEntity entity4 in master2.GetEntityListFromIdNum(svtId, num))
            {
                if ((entity4 == null) || !entity4.isUse(this.userId, this.lv, friendshipRank, -1))
                {
                    break;
                }
                this.treasureDeviceId = entity4.treasureDeviceId;
            }
        }
        this.equipTarget1 = null;
        this.updatedAt = 0L;
    }

    ~ServantLeaderInfo()
    {
        this.equipTarget1 = null;
    }

    public bool GetAdjustMax(out int maxAjustHp, out int maxAjustAtk)
    {
        if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).IsServant)
        {
            ServantRarityEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantRarityMaster>(DataNameKind.Kind.SERVANT_RARITY).getEntityFromId<ServantRarityEntity>(this.getRarity());
            if (entity2 != null)
            {
                maxAjustHp = entity2.hpAdjustMax;
                maxAjustAtk = entity2.atkAdjustMax;
                return true;
            }
        }
        maxAjustHp = 0;
        maxAjustAtk = 0;
        return false;
    }

    protected bool getBaseEventUpVal(EventUpValSetupInfo setupInfo)
    {
        if (this.svtId > 0)
        {
            if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).getEventUpVal(this.svtId, setupInfo))
            {
                return true;
            }
            SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
            int[] numArray = this.getSkillIdList();
            int[] numArray2 = this.getSkillLevelList();
            for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
            {
                if (numArray[i] > 0)
                {
                    SkillLvEntity entity2 = master.getEntityFromId<SkillLvEntity>(numArray[i], numArray2[i]);
                    if ((entity2 != null) && entity2.getEventUpVal(this.svtId, setupInfo))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    protected bool getBaseEventUpVal(ref EventUpValInfo eventUpVallInfo)
    {
        bool flag = false;
        if (this.svtId > 0)
        {
            flag = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).getEventUpVal(ref eventUpVallInfo);
            SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
            int[] numArray = this.getSkillIdList();
            int[] numArray2 = this.getSkillLevelList();
            for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
            {
                if (numArray[i] > 0)
                {
                    SkillLvEntity entity2 = master.getEntityFromId<SkillLvEntity>(numArray[i], numArray2[i]);
                    if ((entity2 != null) && entity2.getEventUpVal(ref eventUpVallInfo))
                    {
                        flag = true;
                    }
                }
            }
        }
        return flag;
    }

    protected int getBaseFriendPointUpVal()
    {
        if (this.svtId > 0)
        {
            SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
            int[] numArray = this.getSkillIdList();
            int[] numArray2 = this.getSkillLevelList();
            for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
            {
                if (numArray[i] > 0)
                {
                    SkillLvEntity entity = master.getEntityFromId(numArray[i], numArray2[i]);
                    if (entity != null)
                    {
                        int num2 = entity.getFriendPointUpVal();
                        if (num2 > 0)
                        {
                            return num2;
                        }
                    }
                }
            }
        }
        return 0;
    }

    public bool getEquipExpInfo(out int exp, out int lateExp, out float barExp)
    {
        if ((this.equipTarget1 != null) && (this.equipTarget1.svtId > 0))
        {
            return this.equipTarget1.getExpInfo(out exp, out lateExp, out barExp);
        }
        exp = 0;
        lateExp = 0;
        barExp = 0f;
        return false;
    }

    public void getEquipSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList)
    {
        if ((this.equipTarget1 != null) && (this.equipTarget1.svtId > 0))
        {
            this.equipTarget1.getSkillInfo(out idList, out lvList, out chargeList, out titleList, out explanationList);
        }
        else
        {
            idList = new int[BalanceConfig.SvtEquipSkillListMax];
            lvList = new int[BalanceConfig.SvtEquipSkillListMax];
            chargeList = new int[BalanceConfig.SvtEquipSkillListMax];
            titleList = new string[BalanceConfig.SvtEquipSkillListMax];
            explanationList = new string[BalanceConfig.SvtEquipSkillListMax];
        }
    }

    public bool getEventUpVal(EventUpValSetupInfo setupInfo)
    {
        if (this.svtId > 0)
        {
            if (this.getBaseEventUpVal(setupInfo))
            {
                return true;
            }
            if ((this.equipTarget1 != null) && this.equipTarget1.getEventUpVal(this.svtId, setupInfo))
            {
                return true;
            }
        }
        return false;
    }

    public bool getEventUpVal(out EventUpValInfo eventUpVallInfo, EventUpValSetupInfo setupInfo)
    {
        eventUpVallInfo = new EventUpValInfo(setupInfo, this.svtId);
        bool flag = false;
        if (this.svtId > 0)
        {
            if (this.getBaseEventUpVal(ref eventUpVallInfo))
            {
                flag = true;
            }
            if (this.equipTarget1 != null)
            {
                eventUpVallInfo.SetEquipSvtId(this.equipTarget1.svtId);
                if (this.equipTarget1.getEventUpVal(ref eventUpVallInfo))
                {
                    flag = true;
                }
            }
        }
        return flag;
    }

    public bool getExpInfo(out int exp, out int lateExp, out float barExp)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        int num = this.getLevelMax();
        if (this.lv < num)
        {
            ServantExpMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExpMaster>(DataNameKind.Kind.SERVANT_EXP);
            int num2 = 0;
            if (this.lv > 1)
            {
                num2 = master2.getEntityFromId<ServantExpEntity>(entity.expType, this.lv - 1).exp;
            }
            ServantExpEntity entity3 = master2.getEntityFromId<ServantExpEntity>(entity.expType, this.lv);
            exp = this.exp - num2;
            lateExp = entity3.exp - this.exp;
            barExp = ((float) exp) / ((float) (entity3.exp - num2));
            return true;
        }
        exp = 0;
        lateExp = 0;
        barExp = 1f;
        return true;
    }

    public int getFrameType()
    {
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount);
        ServantExceedEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExceedMaster>(DataNameKind.Kind.SERVANT_EXCEED).getEntityFromId(entity.rarity, this.exceedCount);
        return ((entity2 == null) ? Rarity.getFrameTypeImage(entity.rarity) : entity2.frameType);
    }

    public int getFriendPointUpVal()
    {
        int num = 0;
        if (this.svtId > 0)
        {
            num = this.getBaseFriendPointUpVal();
            if (this.equipTarget1 != null)
            {
                int num2 = this.equipTarget1.getFriendPointUpVal();
                if (num2 > num)
                {
                    num = num2;
                }
            }
        }
        return num;
    }

    public int getLevelMax()
    {
        if (this.svtId > 0)
        {
            return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount).lvMax;
        }
        return 0;
    }

    public int getRarity() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount).rarity;

    public int getServantLevel() => 
        this.lv;

    public int[] getSkillIdList()
    {
        int[] numArray = new int[BalanceConfig.SvtSkillListMax];
        numArray[0] = this.skillId1;
        numArray[1] = this.skillId2;
        numArray[2] = this.skillId3;
        return numArray;
    }

    public void getSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList)
    {
        lvList = this.getSkillLevelList();
        idList = this.getSkillIdList();
        chargeList = new int[BalanceConfig.SvtSkillListMax];
        titleList = new string[BalanceConfig.SvtSkillListMax];
        explanationList = new string[BalanceConfig.SvtSkillListMax];
        SkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<SkillMaster>(DataNameKind.Kind.SKILL);
        int index = 0;
        for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
        {
            if (idList[i] > 0)
            {
                SkillEntity entity = master.getEntityFromId<SkillEntity>(idList[i]);
                if (entity != null)
                {
                    chargeList[index] = entity.getEffectChargeTurn(lvList[i]);
                    titleList[index] = entity.getEffectTitle(lvList[i]);
                    explanationList[index] = entity.getEffectExplanation(lvList[i]);
                    if (index != i)
                    {
                        idList[index] = idList[i];
                        lvList[index] = lvList[i];
                        idList[i] = 0;
                    }
                    index++;
                }
            }
        }
        while (index < BalanceConfig.SvtSkillListMax)
        {
            idList[index] = 0;
            lvList[index] = -1;
            index++;
        }
    }

    public int[] getSkillLevelList()
    {
        int[] numArray = new int[BalanceConfig.SvtSkillListMax];
        numArray[0] = this.skillLv1;
        numArray[1] = this.skillLv2;
        numArray[2] = this.skillLv3;
        return numArray;
    }

    public bool getTreasureDeviceInfo(out int tdLv, out int tdMaxLv)
    {
        tdLv = this.treasureDeviceLv;
        if (this.treasureDeviceId > 0)
        {
            TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(this.treasureDeviceId);
            if (entity != null)
            {
                tdMaxLv = entity.maxLv;
                return true;
            }
        }
        tdMaxLv = 0;
        return false;
    }

    public bool getTreasureDeviceInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId)
    {
        ServantTreasureDvcMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantTreasureDvcMaster>(DataNameKind.Kind.SERVANT_TREASUREDEVICE);
        ServantTreasureDvcEntity entity = master.getEntityFromSvtIdDvcId(this.svtId, this.treasureDeviceId);
        if (entity != null)
        {
            tdId = this.treasureDeviceId;
            tdLv = this.treasureDeviceLv;
            master.GetRankInfo(out tdRank, out tdMaxRank, this.svtId, this.treasureDeviceId);
            entity.getEffectExplanation(out tdName, out tdExplanation, out tdMaxLv, out tdGuageCount, out tdCardId, tdLv);
            return true;
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

    public int getTreasureDeviceLevelIcon()
    {
        if (this.treasureDeviceId > 0)
        {
            TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(this.treasureDeviceId);
            if (entity != null)
            {
                if (this.treasureDeviceLv >= entity.maxLv)
                {
                    return 2;
                }
                if (this.treasureDeviceLv > 1)
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    public bool IsHideSupport() => 
        NpcServantFollowerEntity.IsHideSupport(this.hideFlag);

    public bool IsEquip =>
        ((this.equipTarget1 != null) && (this.equipTarget1.svtId > 0));
}

