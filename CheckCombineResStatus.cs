using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CheckCombineResStatus : MonoBehaviour
{
    [CompilerGenerated]
    private static Comparison<ServantSkillEntity> <>f__am$cache6;
    private int checkLv;
    private int expType;
    private int increLv;
    private static int LOT_RATE = 0x3e8;
    private int maxLv;
    private int totalExp;

    private bool checkIncrementLv(int lv)
    {
        if (lv < this.maxLv)
        {
            ServantExpEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_EXP).getEntityFromId<ServantExpEntity>(this.expType, lv);
            if (entity.exp >= this.totalExp)
            {
                this.increLv = entity.lv;
                return true;
            }
            this.checkLv++;
            return false;
        }
        this.increLv = this.maxLv;
        return true;
    }

    public void getCombineResStatus(out int afterHp, out int afterAtk, UserServantEntity baseData, int increLv)
    {
        int svtId = baseData.svtId;
        int limitCount = baseData.limitCount;
        int expType = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId).expType;
        ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(svtId, limitCount);
        ServantExpEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_EXP).getEntityFromId<ServantExpEntity>(expType, increLv);
        if (entity3 != null)
        {
            int hpBase = entity2.hpBase;
            int atkBase = entity2.atkBase;
            int hpMax = entity2.hpMax;
            int atkMax = entity2.atkMax;
            int curve = entity3.curve;
            afterHp = (((hpMax - hpBase) * curve) / LOT_RATE) + hpBase;
            afterAtk = (((atkMax - atkBase) * curve) / LOT_RATE) + atkBase;
            afterHp += baseData.adjustHp * BalanceConfig.StatusUpAdjustHp;
            afterAtk += baseData.adjustAtk * BalanceConfig.StatusUpAdjustAtk;
        }
        else
        {
            afterHp = baseData.hp;
            afterAtk = baseData.atk;
        }
    }

    public void getExpInfo(out int exp, out int lateExp, out float barExp, int targetExp, int startLv, int maxLv, int expType)
    {
        if (startLv < maxLv)
        {
            ServantExpMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExpMaster>(DataNameKind.Kind.SERVANT_EXP);
            int num = 0;
            if (startLv > 1)
            {
                num = master.getEntityFromId<ServantExpEntity>(expType, startLv - 1).exp;
            }
            ServantExpEntity entity2 = master.getEntityFromId<ServantExpEntity>(expType, startLv);
            exp = targetExp - num;
            lateExp = entity2.exp - targetExp;
            barExp = ((float) exp) / ((float) (entity2.exp - num));
        }
        else
        {
            exp = 0;
            lateExp = 0;
            barExp = 1f;
        }
    }

    public int getIncreLevel(int getExp, int svtExpType, int svtMaxLv, int startLv)
    {
        int num = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExpMaster>(DataNameKind.Kind.SERVANT_EXP).getLevel(getExp, svtExpType, svtMaxLv, startLv);
        Debug.Log("setSvtExp level: " + num);
        return num;
    }

    public int getIncrementLv(UserServantEntity baseData, int getExp)
    {
        this.increLv = 0;
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(baseData.svtId);
        this.expType = entity.expType;
        this.totalExp = getExp + baseData.exp;
        this.checkLv = baseData.lv;
        this.maxLv = baseData.getLevelMax();
        if (this.checkLv != this.maxLv)
        {
            while (!this.checkIncrementLv(this.checkLv))
            {
            }
        }
        else
        {
            this.increLv = this.maxLv;
        }
        return this.increLv;
    }

    public string getOpenSkillNameByCombine(UserServantEntity baseData, int increLv, int limitCntSum)
    {
        int[] numArray;
        string[] strArray;
        string str = string.Empty;
        baseData.getNextUseSkillInfo(out numArray, out strArray, increLv, limitCntSum);
        if ((strArray != null) && (strArray[0] != null))
        {
            str = strArray[0];
        }
        return str;
    }

    public string getSvtEqSkillByCombine(UserServantEntity baseData, int increLv, int limitCntSum)
    {
        string str = string.Empty;
        List<ServantSkillEntity> list = new List<ServantSkillEntity>(baseData.getUseSvtEqSkillInfo(increLv, limitCntSum));
        if (list.Count > 0)
        {
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = (a, b) => b.priority - a.priority;
            }
            list.Sort(<>f__am$cache6);
            str = list[0].getSkillName();
            Debug.Log("******!! getSvtEqSkillByCombine skillId : " + list[0].skillId);
            Debug.Log("******!! getSvtEqSkillByCombine getSkillName : " + list[0].getSkillName());
        }
        return str;
    }

    public void setSvtExp(out float expVal, out int lateExp, int targetExp, int startLv, int maxLv, int expType)
    {
        ServantExpMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExpMaster>(DataNameKind.Kind.SERVANT_EXP);
        int num = master.getLevel(targetExp, expType, maxLv, startLv);
        Debug.Log("setSvtExp level: " + num);
        int exp = master.getEntityFromId<ServantExpEntity>(expType, num).exp;
        Debug.Log("setSvtExp svtExpEnt.exp: " + exp);
        int num3 = 0;
        ServantExpEntity entity2 = master.getEntityFromId<ServantExpEntity>(expType, num - 1);
        if (entity2 != null)
        {
            num3 = entity2.exp;
        }
        expVal = 1f - (((float) (exp - targetExp)) / ((float) (exp - num3)));
        lateExp = exp - targetExp;
    }
}

