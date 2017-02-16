using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class BattleBuffData
{
    private BuffMaster _tmpBuffMst;
    [CompilerGenerated]
    private static Predicate<BuffData> <>f__am$cache3;
    [CompilerGenerated]
    private static Predicate<BuffData> <>f__am$cache4;
    [CompilerGenerated]
    private static Predicate<BuffData> <>f__am$cache5;
    [CompilerGenerated]
    private static Predicate<BuffData> <>f__am$cache6;
    [CompilerGenerated]
    private static Predicate<BuffData> <>f__am$cache7;
    [CompilerGenerated]
    private static Predicate<BuffData> <>f__am$cache8;
    [CompilerGenerated]
    private static Predicate<BuffData> <>f__am$cache9;
    [CompilerGenerated]
    private static Predicate<BuffData> <>f__am$cacheA;
    private List<BuffData> activeList = new List<BuffData>();
    private List<BuffData> passiveList = new List<BuffData>();

    public void addBuff(BuffData buff)
    {
        if (buff.passive)
        {
            this.passiveList.Add(buff);
        }
        else
        {
            this.activeList.Add(buff);
        }
    }

    public int checkAddParam(BuffData[] bufflist, bool isRec = false)
    {
        int num = 0;
        foreach (BuffData data in bufflist)
        {
            if (this.checkBuffSuccessful(data, isRec))
            {
                num += data.param;
            }
        }
        return num;
    }

    public bool checkAvoidance(int[] individualities)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.AVOIDANCE };
        BuffData[] bufflist = this.getBuffList(types, individualities, null);
        return this.checkBuffSuccessfulIndividual(bufflist);
    }

    public bool checkAvoidInstantDeath(int[] individualities)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.AVOID_INSTANTDEATH };
        BuffData[] bufflist = this.getBuffList(types, individualities, null);
        return this.checkBuffSuccessfulIndividual(bufflist);
    }

    public bool checkBreakAvoidance(int[] individualities)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.BREAK_AVOIDANCE };
        BuffData[] bufflist = this.getBuffList(types, individualities, null);
        return this.checkBuffSuccessfulIndividual(bufflist);
    }

    public bool checkBuffAvoid(int[] individualities)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.AVOID_STATE };
        BuffData[] bufflist = this.getBuffList(types, individualities, null);
        return this.checkBuffSuccessfulIndividual(bufflist);
    }

    public bool checkBuffGroup(int groupId)
    {
        foreach (BuffData data in this.passiveList)
        {
            if (this.buffMst.getEntityFromId<BuffEntity>(data.buffId).buffGroup == groupId)
            {
                return true;
            }
        }
        foreach (BuffData data2 in this.activeList)
        {
            if (this.buffMst.getEntityFromId<BuffEntity>(data2.buffId).buffGroup == groupId)
            {
                return true;
            }
        }
        return false;
    }

    public bool checkBuffId(int[] idlist)
    {
        if ((idlist != null) && (idlist.Length > 0))
        {
            for (int i = 0; i < idlist.Length; i++)
            {
                foreach (BuffData data in this.passiveList)
                {
                    if (data.buffId == idlist[i])
                    {
                        return true;
                    }
                }
                foreach (BuffData data2 in this.activeList)
                {
                    if (data2.buffId == idlist[i])
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool checkBuffIndividualities(int[] individualities)
    {
        foreach (BuffData data in this.passiveList)
        {
            if (Individuality.CheckIndividualities(this.buffMst.getEntityFromId<BuffEntity>(data.buffId).vals, individualities))
            {
                return true;
            }
        }
        foreach (BuffData data2 in this.activeList)
        {
            if (Individuality.CheckIndividualities(this.buffMst.getEntityFromId<BuffEntity>(data2.buffId).vals, individualities))
            {
                return true;
            }
        }
        return false;
    }

    public bool checkBuffSpecialAttack(BuffData buff) => 
        (this.buffMst.getEntityFromId<BuffEntity>(buff.buffId).type == 11);

    public bool checkBuffSuccessful(BuffData buff, bool isRec = true)
    {
        if (!buff.isDecide)
        {
            buff.isDecide = true;
            if ((buff.buffRate == 0x3e8) || (buff.buffRate == 0))
            {
                buff.isUse = true;
                if (isRec)
                {
                    this.recBuff(buff);
                }
                return true;
            }
            int num = BattleRandom.getNext(0x3e8);
            buff.isUse = buff.buffRate >= num;
            if (buff.isUse && isRec)
            {
                this.recBuff(buff);
            }
        }
        return buff.isUse;
    }

    public bool checkBuffSuccessfulIndividual(BuffData[] bufflist)
    {
        if (0 < bufflist.Length)
        {
            for (int i = 0; i < bufflist.Length; i++)
            {
                if (this.checkBuffSuccessful(bufflist[i], false))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool checkBuffTurnCount(BuffData buff)
    {
        if ((0 >= buff.turn) && (0 >= buff.count))
        {
            return false;
        }
        return true;
    }

    public bool checkDontAct()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.DONOT_ACT };
        BuffData[] dataArray = this.getBuffList(types, null, null);
        return (0 < dataArray.Length);
    }

    public bool checkDontNoble()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.DONOT_NOBLE };
        BuffData[] dataArray = this.getBuffList(types, null, null);
        return (0 < dataArray.Length);
    }

    public bool checkDontSkill()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.DONOT_SKILL };
        BuffData[] dataArray = this.getBuffList(types, null, null);
        return (0 < dataArray.Length);
    }

    public bool checkGutsBuffSuccessful(BuffData buff, bool isRec = true)
    {
        if (!buff.isDecide)
        {
            buff.isDecide = true;
            if ((buff.buffRate == 0x3e8) || (buff.buffRate == 0))
            {
                buff.isUse = true;
                if (isRec)
                {
                    this.recBuff(buff);
                }
                return true;
            }
            int num = BattleRandom.getGutsNext(0x3e8);
            buff.isUse = buff.buffRate >= num;
            if (buff.isUse && isRec)
            {
                this.recBuff(buff);
            }
        }
        return buff.isUse;
    }

    public bool checkGutsBuffSuccessfulIndividual(BuffData[] bufflist)
    {
        if (0 < bufflist.Length)
        {
            for (int i = 0; i < bufflist.Length; i++)
            {
                if (this.checkGutsBuffSuccessful(bufflist[i], false))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool checkInvincible(int[] individualities)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.INVINCIBLE };
        BuffData[] bufflist = this.getBuffList(types, individualities, null);
        return this.checkBuffSuccessfulIndividual(bufflist);
    }

    public bool checkNoHeal()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.DONOT_RECOVERY };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.checkBuffSuccessfulIndividual(bufflist);
    }

    public bool checkPierceInvincible(int[] individualities)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.PIERCE_INVINCIBLE };
        BuffData[] bufflist = this.getBuffList(types, individualities, null);
        return this.checkBuffSuccessfulIndividual(bufflist);
    }

    public int checkRegainNPUsedNoble(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.REGAIN_NP_USED_NOBLE };
        int num2 = this.getBuffValue(typesPlus, null, targetSvt.getIndividualities(), null, out num);
        return this.getReturn(num2 - 0x3e8, num);
    }

    public void clearActiveBuff()
    {
        this.activeList.Clear();
    }

    public BuffData[] geCheckBuffSuccessful(BuffData[] bufflist)
    {
        List<BuffData> list = new List<BuffData>();
        foreach (BuffData data in bufflist)
        {
            if (this.checkBuffSuccessful(data, true))
            {
                list.Add(data);
            }
        }
        return list.ToArray();
    }

    public BuffData[] getActiveList() => 
        this.activeList.ToArray();

    public int[] getAddIndividualities()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.ADD_INDIVIDUALITY };
        BuffData[] dataArray = this.getBuffList(types, null, null);
        int[] numArray = new int[dataArray.Length];
        for (int i = 0; i < numArray.Length; i++)
        {
            numArray[i] = dataArray[i].param;
        }
        return numArray;
    }

    public BuffData[] getBuffList(BuffList.TYPE type, int[] targetIndividualities = null, BuffData[] checklist = null)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { type };
        return this.getBuffList(types, targetIndividualities, checklist);
    }

    public BuffData[] getBuffList(BuffList.TYPE[] types, int[] targetIndividualities = null, BuffData[] checklist = null)
    {
        List<BuffData> list = new List<BuffData>();
        if (checklist == null)
        {
            list.AddRange(this.activeList);
            list.AddRange(this.passiveList);
            checklist = list.ToArray();
            list.Clear();
        }
        foreach (BuffData data in checklist)
        {
            BuffEntity entity = this.buffMst.getEntityFromId<BuffEntity>(data.buffId);
            foreach (BuffList.TYPE type in types)
            {
                if (type.CheckType(entity.type))
                {
                    if (entity.tvals == null)
                    {
                        list.Add(data);
                    }
                    else if (entity.tvals.Length <= 0)
                    {
                        list.Add(data);
                    }
                    else if (Individuality.CheckIndividualities(targetIndividualities, entity.tvals))
                    {
                        list.Add(data);
                    }
                }
            }
        }
        return list.ToArray();
    }

    public float getBuffNonResistInstantDeath(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_NONRESIST_INSTANTDEATH };
        int num2 = this.getBuffValue(typesPlus, null, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public float getBuffResistInstantDeath(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_RESIST_INSTANTDEATH };
        int num2 = this.getBuffValue(typesPlus, null, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public BuffData getBuffSuccessfulIndividual(BuffData[] bufflist)
    {
        if (0 < bufflist.Length)
        {
            for (int i = 0; i < bufflist.Length; i++)
            {
                if (bufflist[i].isUse)
                {
                    return bufflist[i];
                }
            }
        }
        return null;
    }

    public int getBuffValue(BuffList.TYPE[] typesPlus, BuffList.TYPE[] typesMinus, int[] individualities, BuffData[] checklist, out int maxMag)
    {
        int num = 0x3e8;
        int num2 = 0x3e8;
        if (typesPlus != null)
        {
            foreach (BuffData data in this.getBuffList(typesPlus, individualities, checklist))
            {
                if (this.checkBuffSuccessful(data, true))
                {
                    num += data.param;
                    BuffEntity entity = this.buffMst.getEntityFromId<BuffEntity>(data.buffId);
                    num2 = (num2 >= entity.maxRate) ? num2 : entity.maxRate;
                }
            }
        }
        if (typesMinus != null)
        {
            foreach (BuffData data2 in this.getBuffList(typesMinus, individualities, checklist))
            {
                if (this.checkBuffSuccessful(data2, true))
                {
                    num -= data2.param;
                    BuffEntity entity2 = this.buffMst.getEntityFromId<BuffEntity>(data2.buffId);
                    num2 = (num2 >= entity2.maxRate) ? num2 : entity2.maxRate;
                }
            }
        }
        num = (num >= 1) ? num : 1;
        maxMag = num2;
        return num;
    }

    public int getBuffValueNoLimit(BuffList.TYPE[] typesPlus, BuffList.TYPE[] typesMinus, int[] individualities, BuffData[] checklist)
    {
        int num = 0;
        if (typesPlus != null)
        {
            foreach (BuffData data in this.getBuffList(typesPlus, individualities, checklist))
            {
                if (this.checkBuffSuccessful(data, true))
                {
                    num += data.param;
                }
            }
        }
        if (typesMinus != null)
        {
            foreach (BuffData data2 in this.getBuffList(typesMinus, individualities, checklist))
            {
                if (this.checkBuffSuccessful(data2, true))
                {
                    num -= data2.param;
                }
            }
        }
        return num;
    }

    public float getCommandAtk(BattleCommandData command, BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] collection = new BuffList.TYPE[] { BuffList.TYPE.UP_COMMANDATK, BuffList.TYPE.UP_COMMANDALL };
        BuffList.TYPE[] typeArray2 = new BuffList.TYPE[] { BuffList.TYPE.DOWN_COMMANDATK, BuffList.TYPE.DOWN_COMMANDALL };
        List<BuffList.TYPE> list = new List<BuffList.TYPE>();
        list.AddRange(collection);
        list.AddRange(typeArray2);
        BuffData[] checklist = this.getBuffList(list.ToArray(), targetSvt.getIndividualities(), null);
        int val = this.getBuffValue(collection, typeArray2, BattleCommand.getIndividuality(command.type, command.ActionIndex), checklist, out num);
        return this.getReturnMag(val, num);
    }

    public float getCommandNp(BattleCommandData command, BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] collection = new BuffList.TYPE[] { BuffList.TYPE.UP_COMMANDNP, BuffList.TYPE.UP_COMMANDALL };
        BuffList.TYPE[] typeArray2 = new BuffList.TYPE[] { BuffList.TYPE.DOWN_COMMANDNP, BuffList.TYPE.DOWN_COMMANDALL };
        List<BuffList.TYPE> list = new List<BuffList.TYPE>();
        list.AddRange(collection);
        list.AddRange(typeArray2);
        BuffData[] checklist = this.getBuffList(list.ToArray(), targetSvt.getIndividualities(), null);
        int val = this.getBuffValue(collection, typeArray2, BattleCommand.getIndividuality(command.type, command.ActionIndex), checklist, out num);
        return this.getReturnMag(val, num);
    }

    public BuffData[] getCommandSideEffectFunction(int[] individualities)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.COMMANDATTACK_FUNCTION };
        BuffData[] bufflist = this.getBuffList(types, individualities, null);
        return this.geCheckBuffSuccessful(bufflist);
    }

    public float getCommandStar(BattleCommandData command, BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] collection = new BuffList.TYPE[] { BuffList.TYPE.UP_COMMANDSTAR, BuffList.TYPE.UP_COMMANDALL };
        BuffList.TYPE[] typeArray2 = new BuffList.TYPE[] { BuffList.TYPE.DOWN_COMMANSTAR, BuffList.TYPE.DOWN_COMMANDALL };
        List<BuffList.TYPE> list = new List<BuffList.TYPE>();
        list.AddRange(collection);
        list.AddRange(typeArray2);
        BuffData[] checklist = this.getBuffList(list.ToArray(), targetSvt.getIndividualities(), null);
        int val = this.getBuffValue(collection, typeArray2, BattleCommand.getIndividuality(command.type, command.ActionIndex), checklist, out num);
        return this.getReturnMag(val, num);
    }

    public float getCriticalDamage(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_CRITICALDAMAGE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_CRITICALDAMAGE };
        int num2 = this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public float getCriticalPointMagnification(int[] commandIndivid)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_CRITICALPOINT };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_CRITICALPOINT };
        int num2 = this.getBuffValue(typesPlus, typesMinus, commandIndivid, null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public float getDamageDropNpMagnification(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_DAMAGEDROPNP };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_DAMAGEDROPNP };
        int val = this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(val, num);
    }

    public float getDamageMagnification(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_DAMAGE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_DAMAGE };
        int num2 = this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public int getDamegeValue(BattleServantData targetSvt)
    {
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.ADD_DAMAGE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.SUB_DAMAGE };
        return this.getBuffValueNoLimit(typesPlus, typesMinus, targetSvt.getIndividualities(), null);
    }

    public BuffData[] getDeadAttackSideEffectFunction(int[] individualities)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.DEADATTACK_FUNCTION };
        BuffData[] bufflist = this.getBuffList(types, individualities, null);
        return this.geCheckBuffSuccessful(bufflist);
    }

    public BuffData[] getDeadFunction()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.DEAD_FUNCTION };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.geCheckBuffSuccessful(bufflist);
    }

    public float getDropNpMagnification(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_DROPNP };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_DROPNP };
        int val = this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(val, num);
    }

    public BuffData[] getEntrySideEffectFunction()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.ENTRY_FUNCTION };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.geCheckBuffSuccessful(bufflist);
    }

    public float getGiveHealMagnification()
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_GIVEGAIN_HP };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_GIVEGAIN_HP };
        int val = this.getBuffValue(typesPlus, typesMinus, null, null, out num);
        return this.getReturnMag(val, num);
    }

    public float getGrantStateMagnification(int[] individualities)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_GRANTSTATE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_GRANTSTATE };
        int num2 = this.getBuffValue(typesPlus, typesMinus, individualities, null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public int getHealMagnification(out int digit)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_GAIN_HP };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_GAIN_HP };
        int val = this.getBuffValue(typesPlus, typesMinus, null, null, out num);
        digit = 0x3e8;
        return this.getReturn(val, num);
    }

    public float getNPDamageMagnification(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_NPDAMAGE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_NPDAMAGE };
        int num2 = this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public BuffData[] getPassiveList() => 
        this.passiveList.ToArray();

    public int[] getRectBuffList()
    {
        List<int> list = new List<int>();
        foreach (BuffData data in this.activeList)
        {
            if (data.isAct)
            {
                list.Add(data.buffId);
            }
        }
        foreach (BuffData data2 in this.passiveList)
        {
            if ((data2.isAct && (this.checkBuffSpecialAttack(data2) || this.checkBuffTurnCount(data2))) && !list.Contains(data2.buffId))
            {
                list.Add(data2.buffId);
                break;
            }
        }
        return list.ToArray();
    }

    public int getReduceHp()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.REDUCE_HP };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.checkAddParam(bufflist, false);
    }

    public int getReduceNP()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.REDUCE_NP };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.checkAddParam(bufflist, false);
    }

    public int getRegainHp()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.REGAIN_HP };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.checkAddParam(bufflist, false);
    }

    public int getRegainNP()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.REGAIN_NP };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.checkAddParam(bufflist, false);
    }

    public int getRegainStar() => 
        50;

    public int getReturn(int val, int maxVal)
    {
        val = (maxVal >= val) ? val : maxVal;
        return val;
    }

    public float getReturnMag(int val, int maxVal)
    {
        val = (maxVal >= val) ? val : maxVal;
        return (((float) val) / 1000f);
    }

    public SaveData getSaveData() => 
        new SaveData { 
            passive = this.passiveList.ToArray(),
            active = this.activeList.ToArray()
        };

    public float getSelfDamageMagnification(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_SELFDAMAGE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_SELFDAMAGE };
        int num2 = this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public int getSelfDamageValue(BattleServantData targetSvt)
    {
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.ADD_SELFDAMAGE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.SUB_SELFDAMAGE };
        return this.getBuffValueNoLimit(typesPlus, typesMinus, targetSvt.getIndividualities(), null);
    }

    public BuffData[] getShowBuffList()
    {
        List<BuffData> list = new List<BuffData>();
        list.AddRange(this.activeList);
        foreach (BuffData data in this.passiveList)
        {
            if (this.checkBuffSpecialAttack(data) || this.checkBuffTurnCount(data))
            {
                list.Add(data);
            }
        }
        return list.ToArray();
    }

    public BuffData[] getShowBuffListPassive()
    {
        List<BuffData> list = new List<BuffData>();
        foreach (BuffData data in this.passiveList)
        {
            if (this.checkBuffTurnCount(data))
            {
                list.Add(data);
            }
        }
        return list.ToArray();
    }

    public BuffData[] getStartWaveFunction()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.WAVESTART_FUNCTION };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.geCheckBuffSuccessful(bufflist);
    }

    public float getStarWeightMagnification(int[] commandIndivid)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_STARWEIGHT };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_STARWEIGHT };
        int val = this.getBuffValue(typesPlus, typesMinus, commandIndivid, null, out num);
        return this.getReturnMag(val, num);
    }

    public int[] getSubIndividualities()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.SUB_INDIVIDUALITY };
        BuffData[] dataArray = this.getBuffList(types, null, null);
        int[] numArray = new int[dataArray.Length];
        for (int i = 0; i < numArray.Length; i++)
        {
            numArray[i] = dataArray[i].param;
        }
        return numArray;
    }

    public float getToleranceMagnification(int[] individualities)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_TOLERANCE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_TOLERANCE };
        int num2 = this.getBuffValue(typesPlus, typesMinus, individualities, null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public BuffData[] getTTurnEndFunction()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.SELFTURNEND_FUNCTION };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.geCheckBuffSuccessful(bufflist);
    }

    public int getUpChageTd()
    {
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_CHAGETD };
        return this.getBuffValueNoLimit(typesPlus, null, null, null);
    }

    public float getUpDownAtk(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_ATK };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_ATK };
        return (((float) this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num)) / 1000f);
    }

    public int getUpDownCriticalRate()
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_CRITICALRATE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_CRITICALRATE };
        int num2 = this.getBuffValue(typesPlus, typesMinus, null, null, out num);
        return this.getReturn(num2 - 0x3e8, num);
    }

    public float getUpDownDef(BattleServantData targetSvt, bool pierce)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_DEFENCE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_DEFENCE };
        if (pierce)
        {
            typesPlus = null;
        }
        return (((float) this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num)) / 1000f);
    }

    public int getUpDownMaxHp(BattleServantData targetSvt)
    {
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.ADD_MAXHP };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.SUB_MAXHP };
        return this.getBuffValueNoLimit(typesPlus, typesMinus, targetSvt.getIndividualities(), null);
    }

    public float getUpDownMaxHpMagnification(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_MAXHP };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_MAXHP };
        int num2 = this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public float getUpDownSpecialDef(BattleServantData targetSvt)
    {
        int num;
        BuffList.TYPE[] typesPlus = new BuffList.TYPE[] { BuffList.TYPE.UP_SPECIALDEFENCE };
        BuffList.TYPE[] typesMinus = new BuffList.TYPE[] { BuffList.TYPE.DOWN_SPECIALDEFENCE };
        int num2 = this.getBuffValue(typesPlus, typesMinus, targetSvt.getIndividualities(), null, out num);
        return this.getReturnMag(num2 - 0x3e8, num);
    }

    public void Initialize()
    {
        this.passiveList.Clear();
        this.activeList.Clear();
    }

    public bool isCheckGender(int[] individualities)
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.DISABLE_GENDER };
        BuffData[] bufflist = this.getBuffList(types, individualities, null);
        return this.checkBuffSuccessfulIndividual(bufflist);
    }

    public bool isGuts()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.GUTS };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.checkGutsBuffSuccessfulIndividual(bufflist);
    }

    public bool isSphitBuff()
    {
        foreach (BuffData data in this.passiveList)
        {
            if (data.isAct && (this.buffMst.getEntityFromId<BuffEntity>(data.buffId).type == 11))
            {
                return true;
            }
        }
        foreach (BuffData data2 in this.activeList)
        {
            if (data2.isAct && (this.buffMst.getEntityFromId<BuffEntity>(data2.buffId).type == 11))
            {
                return true;
            }
        }
        return false;
    }

    public bool isUpHate()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.UP_HATE };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        return this.checkBuffSuccessfulIndividual(bufflist);
    }

    private void recBuff(BuffData buffData)
    {
        buffData.isAct = true;
    }

    public void setSaveData(SaveData sv)
    {
        this.passiveList.AddRange(sv.passive);
        this.activeList.AddRange(sv.active);
    }

    public void startBattleRec()
    {
        foreach (BuffData data in this.passiveList)
        {
            data.isAct = false;
        }
        foreach (BuffData data2 in this.activeList)
        {
            data2.isAct = false;
        }
    }

    public bool subBuffFromIndividualites(int[] individualities)
    {
        BuffData[] dataArray = this.activeList.ToArray();
        this.activeList.Clear();
        foreach (BuffData data in dataArray)
        {
            if (!Individuality.CheckIndividualities(this.buffMst.getEntityFromId<BuffEntity>(data.buffId).vals, individualities))
            {
                this.activeList.Add(data);
            }
        }
        return (dataArray.Length != this.activeList.Count);
    }

    public BuffData[] turnProgressing()
    {
        List<BuffData> list = new List<BuffData>();
        foreach (BuffData data in this.getPassiveList())
        {
            if (0 < data.turn)
            {
                data.turn--;
            }
        }
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = s => s.turn == 0;
        }
        list.AddRange(this.passiveList.FindAll(<>f__am$cache3));
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = s => s.turn == 0;
        }
        this.passiveList.RemoveAll(<>f__am$cache4);
        foreach (BuffData data2 in this.getActiveList())
        {
            if (0 < data2.turn)
            {
                data2.turn--;
            }
        }
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = s => s.turn == 0;
        }
        list.AddRange(this.activeList.FindAll(<>f__am$cache5));
        if (<>f__am$cache6 == null)
        {
            <>f__am$cache6 = s => s.turn == 0;
        }
        this.activeList.RemoveAll(<>f__am$cache6);
        return list.ToArray();
    }

    public void turnProgressingIncrease()
    {
        foreach (BuffData data in this.getPassiveList())
        {
            this.turnProgressingIncreaseCalc(data);
        }
        foreach (BuffData data2 in this.getActiveList())
        {
            this.turnProgressingIncreaseCalc(data2);
        }
    }

    public void turnProgressingIncreaseCalc(BuffData buff)
    {
        if (buff.paramAdd != 0)
        {
            buff.param += buff.paramAdd;
            if ((0 < buff.paramAdd) && (buff.paramMax < buff.param))
            {
                buff.param = buff.paramMax;
            }
            if ((0 > buff.paramAdd) && (buff.paramMax > buff.param))
            {
                buff.param = buff.paramMax;
            }
        }
    }

    public void usedProgressing()
    {
        BuffEntity entity = null;
        List<BuffData> list = new List<BuffData>();
        list.AddRange(this.getPassiveList());
        list.AddRange(this.getActiveList());
        foreach (BuffData data in list.ToArray())
        {
            if (data.isDecide)
            {
                entity = this.buffMst.getEntityFromId<BuffEntity>(data.buffId);
                if (!BuffList.TYPE.GUTS.CheckType(entity.type))
                {
                    if (data.isUse && (0 < data.count))
                    {
                        data.count--;
                    }
                    data.isUse = false;
                    data.isDecide = false;
                }
            }
        }
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = s => s.count == 0;
        }
        this.passiveList.RemoveAll(<>f__am$cache7);
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = s => s.count == 0;
        }
        this.activeList.RemoveAll(<>f__am$cache8);
    }

    public void usedProgressingGuts()
    {
        BuffEntity entity = null;
        List<BuffData> list = new List<BuffData>();
        list.AddRange(this.getPassiveList());
        list.AddRange(this.getActiveList());
        foreach (BuffData data in list.ToArray())
        {
            if (data.isDecide)
            {
                entity = this.buffMst.getEntityFromId<BuffEntity>(data.buffId);
                if (BuffList.TYPE.GUTS.CheckType(entity.type))
                {
                    if (data.isUse && (0 < data.count))
                    {
                        data.count--;
                    }
                    data.isUse = false;
                    data.isDecide = false;
                }
            }
        }
        if (<>f__am$cache9 == null)
        {
            <>f__am$cache9 = s => s.count == 0;
        }
        this.passiveList.RemoveAll(<>f__am$cache9);
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = s => s.count == 0;
        }
        this.activeList.RemoveAll(<>f__am$cacheA);
    }

    public int useGuts()
    {
        BuffList.TYPE[] types = new BuffList.TYPE[] { BuffList.TYPE.GUTS };
        BuffData[] bufflist = this.getBuffList(types, null, null);
        BuffData data = this.getBuffSuccessfulIndividual(bufflist);
        if (data != null)
        {
            return data.param;
        }
        return 0;
    }

    public BuffMaster buffMst
    {
        get
        {
            if (this._tmpBuffMst == null)
            {
                this._tmpBuffMst = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
            }
            return this._tmpBuffMst;
        }
    }

    public class BuffData
    {
        public int buffId;
        public int buffRate = 0x3e8;
        public int count;
        public bool isAct;
        public bool isDecide;
        public bool isUse;
        public int param;
        public int paramAdd;
        public int paramMax;
        public bool passive;
        public int turn;
        public int[] vals;
    }

    public class SaveData
    {
        public BattleBuffData.BuffData[] active;
        public BattleBuffData.BuffData[] passive;
    }
}

