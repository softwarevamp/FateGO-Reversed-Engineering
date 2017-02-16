using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class UserServantEntity : DataEntityBase
{
    public int adjustAtk;
    public int adjustHp;
    public int atk;
    public int commandCardLimitCount;
    public long createdAt;
    public int dispLimitCount;
    public int exceedCount;
    public int exp;
    public int hp;
    public int iconLimitCount;
    public long id;
    public int imageLimitCount;
    public int isLock;
    public int limitCount;
    public int lv;
    public int selectTreasureDeviceIdx;
    public int skillLv1;
    public int skillLv2;
    public int skillLv3;
    public int status;
    public int svtId;
    public const int TREASURE_DEVICE_MAX = 3;
    public int treasureDeviceLv1;
    public int treasureDeviceLv2;
    public int treasureDeviceLv3;
    public long userId;

    public UserServantEntity()
    {
        this.selectTreasureDeviceIdx = 1;
    }

    public UserServantEntity(EquipTargetInfo e)
    {
        this.selectTreasureDeviceIdx = 1;
        this.id = 0L;
        this.userId = e.userId;
        this.svtId = e.svtId;
        this.limitCount = e.limitCount;
        this.lv = e.lv;
        this.exp = e.exp;
        this.atk = e.atk;
        this.hp = e.hp;
    }

    public UserServantEntity(UserServantCollectionEntity e)
    {
        this.selectTreasureDeviceIdx = 1;
        this.userId = e.userId;
        this.svtId = e.svtId;
        this.limitCount = e.maxLimitCount;
        this.lv = e.maxLv;
        this.atk = e.maxAtk;
        this.hp = e.maxHp;
        this.skillLv1 = e.skillLv1;
        this.skillLv2 = e.skillLv2;
        this.skillLv3 = e.skillLv3;
        this.treasureDeviceLv1 = e.treasureDeviceLv1;
        this.treasureDeviceLv2 = e.treasureDeviceLv2;
        this.treasureDeviceLv3 = e.treasureDeviceLv3;
    }

    public UserServantEntity(UserServantEntity e)
    {
        this.selectTreasureDeviceIdx = 1;
        this.id = e.id;
        this.userId = e.userId;
        this.svtId = e.svtId;
        this.limitCount = e.limitCount;
        this.imageLimitCount = e.imageLimitCount;
        this.dispLimitCount = e.dispLimitCount;
        this.commandCardLimitCount = e.commandCardLimitCount;
        this.iconLimitCount = e.iconLimitCount;
        this.lv = e.lv;
        this.exp = e.exp;
        this.atk = e.atk;
        this.hp = e.hp;
        this.adjustAtk = e.adjustAtk;
        this.adjustHp = e.adjustHp;
        this.skillLv1 = e.skillLv1;
        this.skillLv2 = e.skillLv2;
        this.skillLv3 = e.skillLv3;
        this.treasureDeviceLv1 = e.treasureDeviceLv1;
        this.treasureDeviceLv2 = e.treasureDeviceLv2;
        this.treasureDeviceLv3 = e.treasureDeviceLv3;
        this.selectTreasureDeviceIdx = e.selectTreasureDeviceIdx;
        this.exceedCount = e.exceedCount;
        this.status = e.status;
        this.createdAt = e.createdAt;
    }

    public UserServantEntity(UserServantLearderEntity e)
    {
        this.selectTreasureDeviceIdx = 1;
        this.id = 0L;
        this.userId = e.userId;
        this.svtId = e.svtId;
        this.limitCount = e.limitCount;
        this.dispLimitCount = e.dispLimitCount;
        this.lv = e.lv;
        this.exp = e.exp;
        this.atk = e.atk;
        this.hp = e.hp;
        this.adjustAtk = e.adjustAtk;
        this.adjustHp = e.adjustHp;
        this.skillLv1 = e.skillLv1;
        this.skillLv2 = e.skillLv2;
        this.skillLv3 = e.skillLv3;
        this.exceedCount = e.exceedCount;
    }

    public bool ChangeLock() => 
        UserServantLockManager.ChangeLock(this.id);

    public bool checkID(long inId) => 
        (this.id == inId);

    public int checkTreasureDeviceLevelUp(int targetLv)
    {
        int num;
        int num2;
        this.getTreasureDeviceInfo(out num, out num2);
        int num3 = num + targetLv;
        return ((num3 < num2) ? num3 : num2);
    }

    public int getAdjustAtk() => 
        this.adjustAtk;

    public int getAdjustHp() => 
        this.adjustHp;

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

    protected bool getBaseEventUpVal(ref EventUpValInfo eventUpVallInfo)
    {
        int[] numArray;
        int[] numArray2;
        int[] numArray3;
        string[] strArray;
        string[] strArray2;
        bool[] flagArray;
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
        bool flag = entity.getEventUpVal(ref eventUpVallInfo);
        this.getSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2, out flagArray, -1);
        for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
        {
            if ((numArray[i] > 0) && (numArray2[i] > 0))
            {
                SkillLvEntity entity2 = master.getEntityFromId<SkillLvEntity>(numArray[i], numArray2[i]);
                if ((entity2 != null) && entity2.getEventUpVal(ref eventUpVallInfo))
                {
                    flag = true;
                }
            }
        }
        return flag;
    }

    protected bool getBaseEventUpVal(int wearersSvtId, EventUpValSetupInfo setupInfo)
    {
        int[] numArray;
        int[] numArray2;
        int[] numArray3;
        string[] strArray;
        string[] strArray2;
        bool[] flagArray;
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
        if (entity.getEventUpVal(wearersSvtId, setupInfo))
        {
            return true;
        }
        this.getSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2, out flagArray, -1);
        for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
        {
            if ((numArray[i] > 0) && (numArray2[i] > 0))
            {
                SkillLvEntity entity2 = master.getEntityFromId<SkillLvEntity>(numArray[i], numArray2[i]);
                if ((entity2 != null) && entity2.getEventUpVal(wearersSvtId, setupInfo))
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected int getBaseFriendPointUpVal()
    {
        foreach (ServantSkillEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL).getServantSkillList(this.svtId))
        {
            if (entity.isUse(NetworkManager.UserId, this.lv, this.limitCount, -1))
            {
                int num2 = entity.getFriendPointUpVal(this.getSkillLevel(entity.getServantIdx() - 1));
                if (num2 > 0)
                {
                    return num2;
                }
            }
        }
        return 0;
    }

    public int getCardImageLimitCount(bool isReal = true) => 
        ImageLimitCount.GetCardImageLimitCount(this.svtId, this.limitCount, true, isReal || this.HasStarus(StatusFlag.APRIL_FOOL_CANCEL));

    public int getCombineQp()
    {
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount);
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CombineQpMaster>(DataNameKind.Kind.COMBINE_QP).getEntityFromId<CombineQpEntity>(entity.rarity, this.lv).qp;
    }

    public int getCombineQpSvtEq()
    {
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount);
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CombineQpSvtEquipMaster>(DataNameKind.Kind.COMBINE_QP_SVT_EQUIP).getEntityFromId<CombineQpSvtEquipEntity>(entity.rarity, this.lv).qp;
    }

    public int getCombineQpSvtExceed() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExceedMaster>(DataNameKind.Kind.SERVANT_EXCEED).getEntityFromId<ServantExceedEntity>(this.getRarity(), this.exceedCount).qp;

    public int getCommandCardLimitCount()
    {
        if (this.commandCardLimitCount > 0)
        {
            return (this.commandCardLimitCount - 1);
        }
        return this.getMaxCommandCardLimitCount();
    }

    public int getCost() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).cost;

    public int getDispCardImageLimitCount(bool isReal = true)
    {
        if ((!isReal && !this.HasStarus(StatusFlag.APRIL_FOOL_CANCEL)) && ImageLimitCount.IsAprilFool)
        {
            return ImageLimitCount.GetCardImageLimitCount(this.svtId, this.limitCount, true, false);
        }
        if (this.imageLimitCount > 0)
        {
            return (this.imageLimitCount - 1);
        }
        return ImageLimitCount.GetCardImageLimitCount(this.svtId, this.limitCount, true, true);
    }

    public int getDispLimitCount()
    {
        if (this.dispLimitCount > 0)
        {
            return (this.dispLimitCount - 1);
        }
        return this.getMaxDispLimitCount();
    }

    public void getEquipSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList)
    {
        idList = new int[BalanceConfig.SvtEquipSkillListMax];
        idList[0] = 0;
        lvList = new int[BalanceConfig.SvtEquipSkillListMax];
        lvList[0] = this.skillLv1;
        chargeList = new int[BalanceConfig.SvtEquipSkillListMax];
        titleList = new string[BalanceConfig.SvtEquipSkillListMax];
        explanationList = new string[BalanceConfig.SvtEquipSkillListMax];
        ServantSkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL);
        for (int i = 0; i < BalanceConfig.SvtEquipSkillListMax; i++)
        {
            ServantSkillEntity entity;
            int num2 = 1;
        Label_0063:
            entity = master.getEntityFromId<ServantSkillEntity>(this.svtId, i + 1, num2);
            if (entity != null)
            {
                if (entity.isUse(this.userId, this.lv, this.limitCount, -1))
                {
                    idList[i] = entity.getSkillId();
                    entity.getEffectExplanation(out chargeList[i], out titleList[i], out explanationList[i], lvList[i], true);
                    goto Label_0119;
                }
                if (idList[i] == 0)
                {
                    idList[i] = entity.getSkillId();
                    lvList[i] = -1;
                    entity.getAcquisitionMethodExplanation(out titleList[i], out explanationList[i]);
                }
            }
            else if (idList[i] == 0)
            {
                lvList[i] = -1;
            }
            continue;
        Label_0119:
            num2++;
            goto Label_0063;
        }
    }

    public EventServantEntity getEventServant() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventServantMaster>(DataNameKind.Kind.EVENT_SERVANT).getEntity(this.svtId);

    public EventServantEntity getEventServant(bool is_finishedAt)
    {
        EventServantEntity entity = this.getEventServant();
        if ((entity != null) && SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId(entity.eventId).IsOpen(is_finishedAt))
        {
            return entity;
        }
        return null;
    }

    public bool getEventUpVal(EventUpValSetupInfo setupInfo, long[] equipIds)
    {
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        if (this.getBaseEventUpVal(this.svtId, setupInfo))
        {
            return true;
        }
        if (equipIds != null)
        {
            foreach (long num in equipIds)
            {
                if ((num > 0L) && master.getEntityFromId<UserServantEntity>(num).getBaseEventUpVal(this.svtId, setupInfo))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool getEventUpVal(out EventUpValInfo eventUpVallInfo, EventUpValSetupInfo setupInfo, long[] equipIds)
    {
        eventUpVallInfo = new EventUpValInfo(setupInfo, this.svtId);
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        bool flag = this.getBaseEventUpVal(ref eventUpVallInfo);
        if (equipIds != null)
        {
            foreach (long num in equipIds)
            {
                if (num > 0L)
                {
                    UserServantEntity entity = master.getEntityFromId<UserServantEntity>(num);
                    eventUpVallInfo.SetEquipSvtId(entity.svtId);
                    if (entity.getBaseEventUpVal(ref eventUpVallInfo))
                    {
                        flag = true;
                    }
                }
            }
        }
        return flag;
    }

    public int getExceedLvMax(int cnt)
    {
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount);
        ServantExceedEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExceedMaster>(DataNameKind.Kind.SERVANT_EXCEED).getEntityFromId(entity.rarity, cnt);
        return ((entity2 == null) ? this.getLevelMax() : (entity.lvMax + entity2.addLvMax));
    }

    public bool getExpInfo(out int exp, out int lateExp, out float barExp)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        int num = this.getLevelMax();
        if (this.lv < num)
        {
            ServantExpMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExpMaster>(DataNameKind.Kind.SERVANT_EXP);
            int num2 = 0;
            if (this.lv > 1)
            {
                num2 = master.getEntityFromId<ServantExpEntity>(entity.expType, this.lv - 1).exp;
            }
            ServantExpEntity entity3 = master.getEntityFromId<ServantExpEntity>(entity.expType, this.lv);
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

    public int getFigureImageLimitCount()
    {
        if (this.imageLimitCount <= 0)
        {
            return ImageLimitCount.GetImageLimitCount(this.svtId, this.limitCount);
        }
        if (this.imageLimitCount <= 3)
        {
            return (this.imageLimitCount - 1);
        }
        return 2;
    }

    public int getFigureLimitCount()
    {
        if (this.imageLimitCount > 0)
        {
            return ImageLimitCount.GetLimitCountByImageLimit(this.imageLimitCount - 1, this.limitCount);
        }
        return this.limitCount;
    }

    public string getFrameCardPrefix()
    {
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount);
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExceedMaster>(DataNameKind.Kind.SERVANT_EXCEED).getEntityFromId(entity.rarity, this.exceedCount).getFrameCardPrefix();
    }

    public int getFrameType() => 
        Rarity.getFrameTypeImage(SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount).rarity);

    public int getFrendIconLimitCount()
    {
        if (this.iconLimitCount <= 0)
        {
            return ImageLimitCount.GetCardImageLimitCount(this.svtId, this.limitCount, false, true);
        }
        if (this.iconLimitCount > 3)
        {
            return 2;
        }
        return (this.iconLimitCount - 1);
    }

    public int getFriendPointUpVal(long[] equipIds)
    {
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        int num = this.getBaseFriendPointUpVal();
        if (equipIds != null)
        {
            foreach (long num2 in equipIds)
            {
                if (num2 > 0L)
                {
                    int num4 = master.getEntityFromId<UserServantEntity>(num2).getBaseFriendPointUpVal();
                    if (num4 > num)
                    {
                        num = num4;
                    }
                }
            }
        }
        return num;
    }

    public int getFriendshipRank() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userId, this.svtId).friendshipRank;

    public int getIconLimitCount()
    {
        if (this.iconLimitCount > 0)
        {
            return (this.iconLimitCount - 1);
        }
        return ImageLimitCount.GetCardImageLimitCount(this.svtId, this.limitCount, true, true);
    }

    public int getLevelMax() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount).lvMax;

    public int getLimitCntMax() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).limitMax;

    public int getLimitCount() => 
        this.limitCount;

    public int getLockState() => 
        this.isLock;

    public int getMaxCardDispImageLimitCount() => 
        ImageLimitCount.GetCardImageLimitCount(this.svtId, this.limitCount, true, true);

    public int getMaxCommandCardLimitCount() => 
        ImageLimitCount.GetImageLimitCount(this.svtId, this.limitCount);

    public int getMaxDispLimitCount() => 
        ImageLimitCount.GetImageLimitCount(this.svtId, this.limitCount);

    public int getMaxFaceLimitCount() => 
        ImageLimitCount.GetCardImageLimitCount(this.svtId, this.limitCount, true, true);

    public void getNextUseSkillInfo(out int[] idList, out string[] skillNameList, int targetLv, int targetLimitCnt)
    {
        idList = new int[BalanceConfig.SvtSkillListMax];
        skillNameList = new string[BalanceConfig.SvtSkillListMax];
        ServantSkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL);
        for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
        {
            int num2 = 1;
            while (true)
            {
                ServantSkillEntity entity = master.getEntityFromId<ServantSkillEntity>(this.svtId, i + 1, num2);
                if (entity == null)
                {
                    break;
                }
                idList[i] = entity.getSkillId();
                if (entity.isUse(this.userId, this.lv, this.limitCount, -1))
                {
                    break;
                }
                if (entity.isUse(this.userId, targetLv, targetLimitCnt, -1))
                {
                    skillNameList[i] = entity.getSkillName();
                }
                num2++;
            }
        }
    }

    public override string getPrimarykey() => 
        this.id.ToString();

    public int getRarity() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount).rarity;

    public string getResourceFolder() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD).getBattleChrId(this.svtId, this.limitCount);

    public int getSellMana() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).sellMana;

    public int getSellQp() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).sellQp;

    public int getServantExp() => 
        this.exp;

    public int getServantLevel() => 
        this.lv;

    public int[] getSkillIdList()
    {
        int[] numArray = new int[BalanceConfig.SvtSkillListMax];
        ServantSkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL);
        for (int i = 1; i <= BalanceConfig.SvtSkillListMax; i++)
        {
            int priority = 1;
            while (true)
            {
                ServantSkillEntity entity = master.getEntityFromId(this.svtId, i, priority);
                if ((entity == null) || !entity.isUse(this.userId, this.lv, this.limitCount, -1))
                {
                    break;
                }
                numArray[i - 1] = entity.getSkillId();
                priority++;
            }
        }
        return numArray;
    }

    public void getSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList)
    {
        bool[] flagArray;
        this.getSkillInfo(out idList, out lvList, out chargeList, out titleList, out explanationList, out flagArray, -1);
    }

    public void getSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList, out bool[] tdIsUseList, int beforeClearQuestId = -1)
    {
        bool isServantEquip = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).IsServantEquip;
        idList = new int[BalanceConfig.SvtSkillListMax];
        lvList = this.getSkillLevelList();
        chargeList = new int[BalanceConfig.SvtSkillListMax];
        titleList = new string[BalanceConfig.SvtSkillListMax];
        explanationList = new string[BalanceConfig.SvtSkillListMax];
        tdIsUseList = new bool[BalanceConfig.SvtSkillListMax];
        ServantSkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL);
        for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
        {
            ServantSkillEntity entity2;
            int priority = 1;
            tdIsUseList[i] = false;
        Label_0082:
            entity2 = master.getEntityFromId(this.svtId, i + 1, priority);
            if (entity2 != null)
            {
                if (entity2.isUse(this.userId, this.lv, this.limitCount, beforeClearQuestId))
                {
                    idList[i] = entity2.getSkillId();
                    entity2.getEffectExplanation(out chargeList[i], out titleList[i], out explanationList[i], lvList[i], isServantEquip);
                    tdIsUseList[i] = true;
                    goto Label_0147;
                }
                if (idList[i] == 0)
                {
                    idList[i] = entity2.getSkillId();
                    lvList[i] = -1;
                    entity2.getAcquisitionMethodExplanation(out titleList[i], out explanationList[i]);
                }
            }
            else if (idList[i] == 0)
            {
                lvList[i] = -1;
            }
            continue;
        Label_0147:
            priority++;
            goto Label_0082;
        }
    }

    public int getSkillLevel(int iIdx)
    {
        switch (iIdx)
        {
            case 0:
                return this.skillLv1;

            case 1:
                return this.skillLv2;

            case 2:
                return this.skillLv3;
        }
        return 0;
    }

    public int[] getSkillLevelList()
    {
        int[] numArray = new int[BalanceConfig.SvtSkillListMax];
        numArray[0] = this.skillLv1;
        numArray[1] = this.skillLv2;
        numArray[2] = this.skillLv3;
        return numArray;
    }

    public void getStatusUpInfo(out bool isHpUp, out bool isAtkUp)
    {
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount);
        isHpUp = entity.hpBase > 0;
        isAtkUp = entity.atkBase > 0;
    }

    public int getSvtClassGroupType(int classId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantClassMaster>(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(classId).groupType;

    public int getSvtClassId() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).classId;

    public int getSvtId() => 
        this.svtId;

    private bool GetSvtIsLockNow()
    {
        foreach (UserServantEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getList())
        {
            if (this.id == entity.id)
            {
                return (entity.isLock == 1);
            }
        }
        Debug.LogError("not find this svt");
        return false;
    }

    public bool getTreasureDeviceInfo(out int tdLv, out int tdMaxLv)
    {
        tdMaxLv = 0;
        if (!SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).IsServant)
        {
            tdLv = -1;
            return false;
        }
        tdLv = this.getTreasureDeviceLv(this.selectTreasureDeviceIdx);
        if (tdLv <= 0)
        {
            return false;
        }
        ServantTreasureDvcMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantTreasureDvcMaster>(DataNameKind.Kind.SERVANT_TREASUREDEVICE);
        long userId = NetworkManager.UserId;
        int friendshipRank = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(userId, this.svtId).friendshipRank;
        ServantTreasureDvcEntity[] entityListFromIdNum = master.GetEntityListFromIdNum(this.svtId, this.selectTreasureDeviceIdx);
        bool flag = false;
        foreach (ServantTreasureDvcEntity entity3 in entityListFromIdNum)
        {
            if ((entity3 == null) || !entity3.isUse(this.userId, this.lv, friendshipRank, -1))
            {
                return flag;
            }
            tdMaxLv = entity3.getLevelMax();
            flag = true;
        }
        return flag;
    }

    public bool getTreasureDeviceInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId) => 
        this.getTreasureDeviceInfo(out tdId, out tdLv, out tdMaxLv, out tdRank, out tdMaxRank, out tdName, out tdExplanation, out tdGuageCount, out tdCardId, this.selectTreasureDeviceIdx, -1);

    public void getTreasureDeviceInfo(out int[] tdIdList, out int[] tdLvList, out int[] tdMaxLvList, out int[] tdRankList, out int[] tdMaxRankList, out string[] tdNameList, out string[] tdExplanationList, out int[] tdGuageCountList, out int[] tdCardIdList)
    {
        bool[] flagArray;
        this.getTreasureDeviceInfo(out tdIdList, out tdLvList, out tdMaxLvList, out tdRankList, out tdMaxRankList, out tdNameList, out tdExplanationList, out tdGuageCountList, out tdCardIdList, out flagArray, -1);
    }

    public bool getTreasureDeviceInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId, int idx, int beforeClearQuestId = -1)
    {
        tdId = 0;
        tdLv = 0;
        tdMaxLv = 0;
        tdRank = 0;
        tdMaxRank = 0;
        tdName = string.Empty;
        tdExplanation = string.Empty;
        tdGuageCount = 0;
        tdCardId = 0;
        switch (idx)
        {
            case 1:
                tdLv = this.treasureDeviceLv1;
                break;

            case 2:
                tdLv = this.treasureDeviceLv2;
                break;

            case 3:
                tdLv = this.treasureDeviceLv3;
                break;

            default:
                return false;
        }
        ServantTreasureDvcMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantTreasureDvcMaster>(DataNameKind.Kind.SERVANT_TREASUREDEVICE);
        int friendshipRank = this.getFriendshipRank();
        bool flag = false;
        ServantTreasureDvcEntity[] entityListFromIdNum = master.GetEntityListFromIdNum(this.svtId, idx);
        tdMaxRank = entityListFromIdNum.Length;
        foreach (ServantTreasureDvcEntity entity in entityListFromIdNum)
        {
            if ((entity == null) || !entity.isUse(this.userId, this.lv, friendshipRank, beforeClearQuestId))
            {
                return flag;
            }
            tdId = entity.treasureDeviceId;
            flag = entity.getEffectExplanation(out tdName, out tdExplanation, out tdMaxLv, out tdGuageCount, out tdCardId, tdLv);
            if (flag)
            {
                tdRank++;
            }
        }
        return flag;
    }

    public void getTreasureDeviceInfo(out int[] tdIdList, out int[] tdLvList, out int[] tdMaxLvList, out int[] tdRankList, out int[] tdMaxRankList, out string[] tdNameList, out string[] tdExplanationList, out int[] tdGuageCountList, out int[] tdCardIdList, out bool[] tdIsUseList, int beforeClearQuestId = -1)
    {
        int num = 3;
        tdIdList = new int[num];
        tdLvList = new int[num];
        tdMaxLvList = new int[num];
        tdRankList = new int[num];
        tdMaxRankList = new int[num];
        tdNameList = new string[num];
        tdExplanationList = new string[num];
        tdGuageCountList = new int[num];
        tdCardIdList = new int[num];
        tdIsUseList = new bool[num];
        for (int i = 0; i < num; i++)
        {
            tdIsUseList[i] = this.getTreasureDeviceInfo(out tdIdList[i], out tdLvList[i], out tdMaxLvList[i], out tdRankList[i], out tdMaxRankList[i], out tdNameList[i], out tdExplanationList[i], out tdGuageCountList[i], out tdCardIdList[i], i + 1, beforeClearQuestId);
        }
    }

    public int getTreasureDeviceLevelIcon()
    {
        int num;
        int num2;
        if (this.getTreasureDeviceInfo(out num, out num2))
        {
            if (num >= num2)
            {
                return 2;
            }
            if (num > 1)
            {
                return 1;
            }
        }
        return 0;
    }

    public int getTreasureDeviceLv() => 
        this.getTreasureDeviceLv(this.selectTreasureDeviceIdx);

    public int getTreasureDeviceLv(int idx)
    {
        switch (idx)
        {
            case 1:
                return this.treasureDeviceLv1;

            case 2:
                return this.treasureDeviceLv2;

            case 3:
                return this.treasureDeviceLv3;
        }
        return 0;
    }

    public int[] getTreasureDeviceLvList() => 
        new int[] { this.treasureDeviceLv1, this.treasureDeviceLv2, this.treasureDeviceLv3 };

    public long getUserId() => 
        this.userId;

    public ServantSkillEntity[] getUseSvtEqSkillInfo(int targetLv, int targetLimitCnt)
    {
        List<ServantSkillEntity> list = new List<ServantSkillEntity>();
        foreach (ServantSkillEntity entity in SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL).getServantSkillList(this.svtId))
        {
            if (!entity.isUse(this.userId, this.lv, this.limitCount, -1) && entity.isUse(this.userId, targetLv, targetLimitCnt, -1))
            {
                list.Add(entity);
            }
        }
        return list.ToArray();
    }

    public bool HasStarus(StatusFlag statusFlag) => 
        ((this.status & statusFlag) != 0);

    public bool isAdjustAtkMax()
    {
        int id = this.getRarity();
        ServantRarityEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantRarityMaster>(DataNameKind.Kind.SERVANT_RARITY).getEntityFromId<ServantRarityEntity>(id);
        return (this.adjustAtk == entity.atkAdjustMax);
    }

    public bool isAdjustHpMax()
    {
        int id = this.getRarity();
        ServantRarityEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantRarityMaster>(DataNameKind.Kind.SERVANT_RARITY).getEntityFromId<ServantRarityEntity>(id);
        return (this.adjustHp == entity.hpAdjustMax);
    }

    public bool IsCombineExp() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).checkIsCombineMaterialSvt();

    public bool IsEventJoin() => 
        this.HasStarus(StatusFlag.EVENT_JOIN);

    public bool isExceeded() => 
        (this.exceedCount > 0);

    public bool isExceedLvMax() => 
        (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExceedMaster>(DataNameKind.Kind.SERVANT_EXCEED).getEntityFromId(this.getRarity(), this.exceedCount + 1) == null);

    public bool IsHeroine() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).checkIsHeroineSvt();

    public bool isLevelMax() => 
        (this.getLevelMax() == this.lv);

    public bool isLimitCountMax() => 
        (this.getLimitCntMax() == this.limitCount);

    public bool IsLock()
    {
        bool flag = UserServantLockManager.IsLock(this.id);
        if (flag)
        {
            return flag;
        }
        if (this.GetSvtIsLockNow())
        {
            UserServantLockManager.SetLock(this.id, true);
            return true;
        }
        return false;
    }

    public bool IsModifyCommandCardLimitCount(int commandCardLimitCount)
    {
        int num = this.getCommandCardLimitCount();
        return (commandCardLimitCount != num);
    }

    public bool IsModifyDispLimitCount(int dispLimitCount)
    {
        int num = this.getDispLimitCount();
        return (dispLimitCount != num);
    }

    public bool IsModifyFaceLimitCount(int faceLimitCount)
    {
        int num = this.getIconLimitCount();
        return (faceLimitCount != num);
    }

    public bool IsModifyLock(bool isLock) => 
        (this.HasStarus(StatusFlag.LOCK) != isLock);

    public bool IsNew()
    {
        if (NetworkManager.UserId != this.userId)
        {
            return false;
        }
        return UserServantNewManager.IsNew(this.id);
    }

    public bool IsStatusUp() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).IsStatusUp;

    public bool IsWithdrawal() => 
        this.HasStarus(StatusFlag.WITHDRAWAL);

    public void SetLock(bool isLock)
    {
        UserServantLockManager.SetLock(this.id, isLock);
    }

    public void SetOld()
    {
        UserServantNewManager.SetOld(this.id);
    }

    public enum StatusFlag
    {
        APRIL_FOOL_CANCEL = 8,
        EVENT_JOIN = 2,
        LOCK = 1,
        WITHDRAWAL = 4
    }

    public enum StatusKind
    {
        LOCK,
        EVENT_JOIN,
        WITHDRAWAL,
        APRIL_FOOL_CANCEL
    }
}

