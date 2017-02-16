using System;
using System.Runtime.InteropServices;

public class EquipTargetInfo
{
    public int atk;
    public int exp;
    public int hp;
    public int limitCount;
    public int lv;
    public int skillId1;
    public int skillLv1;
    public int svtId;
    public long updatedAt;
    public long userId;
    public long userSvtId;

    public EquipTargetInfo()
    {
    }

    public EquipTargetInfo(int svtId)
    {
        this.userId = 1L;
        this.userSvtId = 1L;
        this.svtId = svtId;
        this.limitCount = 0;
        this.lv = 1;
        this.exp = 0;
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(svtId, this.limitCount);
        this.hp = entity.hpBase;
        this.atk = entity.atkBase;
        this.skillLv1 = 1;
        int[] numArray = new int[BalanceConfig.SvtEquipSkillListMax];
        ServantSkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL);
        for (int i = 1; i <= BalanceConfig.SvtEquipSkillListMax; i++)
        {
            int priority = 1;
            while (true)
            {
                ServantSkillEntity entity2 = master.getEntityFromId(svtId, i, priority);
                if ((entity2 == null) || !entity2.isUse(this.userId, this.lv, this.limitCount, -1))
                {
                    break;
                }
                numArray[i - 1] = entity2.getSkillId();
                priority++;
            }
        }
        this.skillId1 = numArray[0];
        this.updatedAt = 0L;
    }

    public bool getEventUpVal(ref EventUpValInfo eventUpVallInfo)
    {
        if (this.svtId > 0)
        {
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).getEventUpVal(ref eventUpVallInfo);
            SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
            int[] numArray = new int[BalanceConfig.SvtEquipSkillListMax];
            int[] numArray2 = new int[BalanceConfig.SvtEquipSkillListMax];
            numArray[0] = this.skillId1;
            numArray2[0] = this.skillLv1;
            for (int i = 0; i < BalanceConfig.SvtEquipSkillListMax; i++)
            {
                if (numArray[i] > 0)
                {
                    SkillLvEntity entity2 = master.getEntityFromId<SkillLvEntity>(numArray[i], numArray2[i]);
                    if (entity2 != null)
                    {
                        entity2.getEventUpVal(ref eventUpVallInfo);
                    }
                }
            }
        }
        return false;
    }

    public bool getEventUpVal(int wearersSvtId, EventUpValSetupInfo setupInfo)
    {
        if (this.svtId > 0)
        {
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
            if (entity == null)
            {
                return false;
            }
            if (entity.getEventUpVal(wearersSvtId, setupInfo))
            {
                return true;
            }
            SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
            int[] numArray = new int[BalanceConfig.SvtEquipSkillListMax];
            int[] numArray2 = new int[BalanceConfig.SvtEquipSkillListMax];
            numArray[0] = this.skillId1;
            numArray2[0] = this.skillLv1;
            for (int i = 0; i < BalanceConfig.SvtEquipSkillListMax; i++)
            {
                if (numArray[i] > 0)
                {
                    SkillLvEntity entity2 = master.getEntityFromId<SkillLvEntity>(numArray[i], numArray2[i]);
                    if ((entity2 != null) && entity2.getEventUpVal(wearersSvtId, setupInfo))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool getExpInfo(out int exp, out int lateExp, out float barExp)
    {
        if (this.svtId > 0)
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
        exp = 0;
        lateExp = 0;
        barExp = 0f;
        return false;
    }

    public int getFriendPointUpVal()
    {
        if (this.svtId > 0)
        {
            SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
            int[] numArray = new int[BalanceConfig.SvtEquipSkillListMax];
            int[] numArray2 = new int[BalanceConfig.SvtEquipSkillListMax];
            numArray[0] = this.skillId1;
            numArray2[0] = this.skillLv1;
            for (int i = 0; i < BalanceConfig.SvtEquipSkillListMax; i++)
            {
                if (numArray[i] > 0)
                {
                    SkillLvEntity entity = master.getEntityFromId<SkillLvEntity>(numArray[i], numArray2[i]);
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

    public int getLevelMax()
    {
        if (this.svtId > 0)
        {
            return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount).lvMax;
        }
        return 0;
    }

    public int getServantLevel() => 
        this.lv;

    public void getSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList)
    {
        idList = new int[BalanceConfig.SvtEquipSkillListMax];
        lvList = new int[BalanceConfig.SvtEquipSkillListMax];
        chargeList = new int[BalanceConfig.SvtEquipSkillListMax];
        titleList = new string[BalanceConfig.SvtEquipSkillListMax];
        explanationList = new string[BalanceConfig.SvtEquipSkillListMax];
        if (this.svtId > 0)
        {
            idList[0] = this.skillId1;
            lvList[0] = this.skillLv1;
            SkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<SkillMaster>(DataNameKind.Kind.SKILL);
            for (int i = 0; i < BalanceConfig.SvtEquipSkillListMax; i++)
            {
                if (idList[i] > 0)
                {
                    SkillEntity entity = master.getEntityFromId<SkillEntity>(idList[i]);
                    if (entity != null)
                    {
                        chargeList[i] = entity.getEffectChargeTurn(lvList[i]);
                        titleList[i] = entity.getEffectTitle(0);
                        explanationList[i] = entity.getEffectExplanation(lvList[i]);
                    }
                    else
                    {
                        idList[i] = 0;
                        lvList[i] = 0;
                    }
                }
            }
        }
    }
}

