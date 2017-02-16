using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleActionData
{
    public int actionIndex;
    public int actorId;
    public int addCriticalStars;
    public int attackcount;
    public BuffData[] buffdataArray;
    public List<BuffData> buffdatalist = new List<BuffData>();
    public int chainCount;
    public int commandattack = -1;
    public DamageData[] damagedataArray;
    public List<DamageData> damagedatalist = new List<DamageData>();
    public int[] effectlist;
    public string endcameraname;
    public bool flash;
    public HealData[] healdataArray;
    public List<HealData> healdatalist = new List<HealData>();
    public int motionId = -1;
    public string motionMessage;
    public string motionname;
    public bool nextattackme;
    public bool pair;
    public bool prevattackme;
    public int[] pttargetIds;
    public bool redrawCommandCard;
    public ReplaceMember[] replacememberArray;
    public List<ReplaceMember> replacememberlist = new List<ReplaceMember>();
    public string skillMessage;
    public int state;
    public float systemTime;
    public int targetId;
    public GameObject targetObject;
    public bool three;
    public TransformServant[] transformServantArray;
    public List<TransformServant> transformServantlist = new List<TransformServant>();
    public int treasureDvcId = -1;
    public int type;
    public static int TYPE_BACKSTEP = 0x67;
    public static int TYPE_DEAD = 100;
    public static int TYPE_ORDERARTS = 0x79;
    public static int TYPE_ORDERBUSTER = 0x7a;
    public static int TYPE_ORDERQUICK = 0x7b;
    public static int TYPE_RESURRECTION = 0x68;
    public static int TYPE_SKILL = 0x66;
    public static int TYPE_TW = 0x65;

    public void addAction(BattleActionData adddata)
    {
        if (adddata != null)
        {
            this.damagedatalist.AddRange(adddata.damagedatalist);
            this.buffdatalist.AddRange(adddata.buffdatalist);
            this.healdatalist.AddRange(adddata.healdatalist);
            this.replacememberlist.AddRange(adddata.replacememberlist);
            this.transformServantlist.AddRange(adddata.transformServantlist);
            this.redrawCommandCard |= adddata.redrawCommandCard;
        }
    }

    public void addCriticalStar(int num)
    {
        this.addCriticalStars += num;
    }

    public BuffData[] getBuffList(int funcIndex)
    {
        <getBuffList>c__AnonStorey74 storey = new <getBuffList>c__AnonStorey74 {
            funcIndex = funcIndex
        };
        if (storey.funcIndex == -1)
        {
            return this.buffdatalist.ToArray();
        }
        return this.buffdatalist.FindAll(new Predicate<BuffData>(storey.<>m__83)).ToArray();
    }

    public int[] GetBuffTargets(int funcIdx = -1) => 
        new int[0];

    public DamageData[] getDamageList(int funcIndex)
    {
        <getDamageList>c__AnonStorey73 storey = new <getDamageList>c__AnonStorey73 {
            funcIndex = funcIndex
        };
        if (storey.funcIndex == -1)
        {
            return this.damagedatalist.ToArray();
        }
        return this.damagedatalist.FindAll(new Predicate<DamageData>(storey.<>m__82)).ToArray();
    }

    public int[] GetDamageTargets(int funcIdx = -1)
    {
        int[] numArray = new int[this.damagedatalist.Count];
        int index = 0;
        foreach (DamageData data in this.damagedatalist)
        {
            numArray[index] = data.targetId;
            index++;
        }
        return numArray;
    }

    public int[] GetDebuffTargets(int funcIdx = -1) => 
        new int[0];

    public int getEffect(int index)
    {
        if ((this.effectlist != null) && (index < this.effectlist.Length))
        {
            return this.effectlist[index];
        }
        return -1;
    }

    public string getEndCamera() => 
        this.endcameraname;

    public HealData[] getHealList(int funcIndex)
    {
        <getHealList>c__AnonStorey75 storey = new <getHealList>c__AnonStorey75 {
            funcIndex = funcIndex
        };
        if (storey.funcIndex == -1)
        {
            return this.healdatalist.ToArray();
        }
        return this.healdatalist.FindAll(new Predicate<HealData>(storey.<>m__84)).ToArray();
    }

    public int getPTSubTargetId()
    {
        if ((this.pttargetIds != null) && (2 <= this.pttargetIds.Length))
        {
            return this.pttargetIds[1];
        }
        return 0;
    }

    public int getPTTargetId()
    {
        if ((this.pttargetIds != null) && (1 <= this.pttargetIds.Length))
        {
            return this.pttargetIds[0];
        }
        return 0;
    }

    public ReplaceMember[] getReplaceMember(int funcIndex)
    {
        <getReplaceMember>c__AnonStorey76 storey = new <getReplaceMember>c__AnonStorey76 {
            funcIndex = funcIndex
        };
        if (storey.funcIndex == -1)
        {
            return this.replacememberlist.ToArray();
        }
        return this.replacememberlist.FindAll(new Predicate<ReplaceMember>(storey.<>m__85)).ToArray();
    }

    public int GetTarget(int funcIdx = -1) => 
        this.targetId;

    public int getTotalDamage()
    {
        int num = 0;
        foreach (DamageData data in this.damagedatalist)
        {
            int[] numArray = data.getDamageList();
            if (numArray != null)
            {
                foreach (int num2 in numArray)
                {
                    num += num2;
                }
            }
        }
        return num;
    }

    public TransformServant[] getTransformServant(int funcIndex)
    {
        <getTransformServant>c__AnonStorey77 storey = new <getTransformServant>c__AnonStorey77 {
            funcIndex = funcIndex
        };
        if (storey.funcIndex == -1)
        {
            return this.transformServantlist.ToArray();
        }
        return this.transformServantlist.FindAll(new Predicate<TransformServant>(storey.<>m__86)).ToArray();
    }

    public bool isActors() => 
        (0 == this.state);

    public bool isArtsOrderAttack() => 
        (this.flash & BattleCommand.isARTS(this.type));

    public bool isBattleLiveSkill(BattleData data) => 
        ((this.type == TYPE_SKILL) && SingletonMonoBehaviour<DataManager>.Instance.IsBattleLiveOpen(data));

    public bool isCommandAttack() => 
        (((BattleCommand.isARTS(this.type) || BattleCommand.isQUICK(this.type)) || BattleCommand.isBUSTER(this.type)) || BattleCommand.isADDATTACK(this.type));

    public bool isDeadMotion() => 
        (this.type == TYPE_DEAD);

    public bool isEndCamera() => 
        (this.endcameraname != null);

    public bool isField() => 
        (2 == this.state);

    public bool isGrandArtsOrderAttack() => 
        ((this.flash & BattleCommand.isARTS(this.type)) & this.three);

    public bool isMotion() => 
        (3 == this.state);

    public bool isResurrectionMotion() => 
        (this.type == TYPE_RESURRECTION);

    public bool isSkill() => 
        (this.type == TYPE_SKILL);

    public bool isSystem() => 
        (1 == this.state);

    public bool isTypeOrderArts() => 
        (TYPE_ORDERARTS == this.type);

    public bool isTypeOrderBuster() => 
        (TYPE_ORDERBUSTER == this.type);

    public bool isTypeOrderQuick() => 
        (TYPE_ORDERQUICK == this.type);

    public bool isTypeTA() => 
        (TYPE_TW == this.type);

    public void setBuffData(BuffData data)
    {
        this.buffdatalist.Add(data);
    }

    public void setCommand(BattleCommandData command)
    {
        if (command != null)
        {
            this.flash = command.isFlash();
            this.three = command.isThree();
            this.pair = command.isPair();
            this.chainCount = command.ChainCount;
            this.commandattack = command.ActionIndex;
            this.treasureDvcId = command.treasureDvc;
        }
    }

    public void setDamageData(DamageData data)
    {
        this.damagedatalist.Add(data);
    }

    public void SetDataFromServer(int actorid, int targetid, int[] pttargetids, int motionid, int type, string motionname, bool flash, bool pair, bool three, bool prevattackme, bool nextattackme, int actionIndex, int attackcount, int chainCount, int commandattack, int treasureDvcId, float systemTime, string skillMessage, string motionMessage, int addCriticalStars, bool redrawCommandCard, int[] effectlist, int state, List<DamageData> damagedatalist, List<BuffData> buffdatalist, List<TransformServant> transformServantlist, List<ReplaceMember> replacememberlist, List<HealData> healdatalist)
    {
        this.actorId = actorid;
        this.targetId = targetid;
        this.pttargetIds = pttargetids;
        this.motionId = motionid;
        this.type = type;
        if (motionname == string.Empty)
        {
            this.motionname = null;
        }
        else
        {
            this.motionname = motionname;
        }
        this.flash = flash;
        this.pair = pair;
        this.three = three;
        this.prevattackme = prevattackme;
        this.nextattackme = nextattackme;
        this.actionIndex = actionIndex;
        this.attackcount = attackcount;
        this.chainCount = chainCount;
        this.commandattack = commandattack;
        this.treasureDvcId = treasureDvcId;
        this.systemTime = systemTime;
        this.skillMessage = skillMessage;
        this.motionMessage = motionMessage;
        this.addCriticalStars = addCriticalStars;
        this.redrawCommandCard = redrawCommandCard;
        this.effectlist = effectlist;
        this.state = state;
        this.damagedatalist = damagedatalist;
        this.buffdatalist = buffdatalist;
        this.transformServantlist = transformServantlist;
        this.replacememberlist = replacememberlist;
        this.healdatalist = healdatalist;
    }

    public void setEffect(int[] effectList)
    {
        this.effectlist = effectList;
    }

    public void setEndCamera(string name)
    {
        this.endcameraname = name;
    }

    public void setHealData(HealData data)
    {
        this.healdatalist.Add(data);
    }

    public void setHealData(int targetId, int healPoint, int funcIndex, int bufId)
    {
        HealData data = new HealData {
            targetId = targetId,
            healPoint = healPoint,
            functionIndex = funcIndex,
            buffId = bufId
        };
        this.setHealData(data);
    }

    public void SetListValue()
    {
        foreach (DamageData data in this.damagedataArray)
        {
            this.damagedatalist.Add(data);
        }
        foreach (BuffData data2 in this.buffdataArray)
        {
            this.buffdatalist.Add(data2);
        }
        foreach (TransformServant servant in this.transformServantArray)
        {
            this.transformServantlist.Add(servant);
        }
        foreach (ReplaceMember member in this.replacememberArray)
        {
            this.replacememberlist.Add(member);
        }
        foreach (HealData data3 in this.healdataArray)
        {
            this.healdatalist.Add(data3);
        }
    }

    public void setPTTargetId(int pttargetId)
    {
        this.pttargetIds = new int[] { pttargetId };
    }

    public void setPTTargetId(int[] pttargetIds)
    {
        this.pttargetIds = pttargetIds;
    }

    public void setReplaceMember(ReplaceMember data)
    {
        this.replacememberlist.Add(data);
    }

    public void setReplaceMember(int index, int inUniqueId, int outUniqueId, int funcIndex)
    {
        ReplaceMember data = new ReplaceMember {
            index = index,
            inUniqeId = inUniqueId,
            outUniqeId = outUniqueId,
            functionIndex = funcIndex
        };
        this.setReplaceMember(data);
    }

    public void setStateActors()
    {
        this.state = 0;
    }

    public void setStateField()
    {
        this.state = 2;
    }

    public void setStateMotion()
    {
        this.state = 3;
    }

    public void setStateSystem()
    {
        this.state = 1;
    }

    public void setTargetId(int targetId)
    {
        this.targetId = targetId;
    }

    public void setTransformServant(TransformServant data)
    {
        this.transformServantlist.Add(data);
    }

    public void setTransformServant(int index, int uniqueId, int funcIndex)
    {
        TransformServant servant = new TransformServant {
            functionIndex = funcIndex,
            index = index,
            uniqueId = uniqueId
        };
    }

    public void setTypeOrderArts()
    {
        this.type = TYPE_ORDERARTS;
    }

    public void setTypeOrderBuster()
    {
        this.type = TYPE_ORDERBUSTER;
    }

    public void setTypeOrderQuick()
    {
        this.type = TYPE_ORDERQUICK;
    }

    public void setTypeTA()
    {
        this.type = TYPE_TW;
    }

    public string toCutinName()
    {
        if (this.type == 2)
        {
            return $"effect/BitEffect/bit_buster{1:00}";
        }
        if (this.type == 1)
        {
            return $"effect/BitEffect/bit_arts{1:00}";
        }
        if (this.type == 3)
        {
            return $"effect/BitEffect/bit_quick{1:00}";
        }
        if (this.type == 4)
        {
            return "effect/BitEffect/bit_flash02";
        }
        return null;
    }

    [CompilerGenerated]
    private sealed class <getBuffList>c__AnonStorey74
    {
        internal int funcIndex;

        internal bool <>m__83(BattleActionData.BuffData s) => 
            (s.functionIndex == this.funcIndex);
    }

    [CompilerGenerated]
    private sealed class <getDamageList>c__AnonStorey73
    {
        internal int funcIndex;

        internal bool <>m__82(BattleActionData.DamageData s) => 
            (s.functionIndex == this.funcIndex);
    }

    [CompilerGenerated]
    private sealed class <getHealList>c__AnonStorey75
    {
        internal int funcIndex;

        internal bool <>m__84(BattleActionData.HealData s) => 
            (s.functionIndex == this.funcIndex);
    }

    [CompilerGenerated]
    private sealed class <getReplaceMember>c__AnonStorey76
    {
        internal int funcIndex;

        internal bool <>m__85(BattleActionData.ReplaceMember s) => 
            (s.functionIndex == this.funcIndex);
    }

    [CompilerGenerated]
    private sealed class <getTransformServant>c__AnonStorey77
    {
        internal int funcIndex;

        internal bool <>m__86(BattleActionData.TransformServant s) => 
            (s.functionIndex == this.funcIndex);
    }

    public class BuffData
    {
        public int buffId;
        public int[] effectList;
        public int functionIndex;
        public bool isMiss;
        public int popColor;
        public int popIcon;
        public string popLabel;
        public BuffProcType procType;
        public int procTypeTemp;
        public int procValue;
        public int targetId;

        public void SetProcType()
        {
            this.procType = (BuffProcType) this.procTypeTemp;
        }

        public enum BuffProcType
        {
            NONE,
            INSTANT_DEATH,
            UPDATE_HP,
            UPDATE_NP,
            UPDATE_CRITICAL,
            UPDATE_NPTURN
        }
    }

    public class DamageData
    {
        public int[] atkbufflist;
        public int[] atknplist;
        public bool avoidance;
        public bool critical;
        public int[] damagelist;
        public int[] defbufflist;
        public int[] defnplist;
        public int functionIndex;
        public bool invincible;
        public int overkillIndex;
        public bool regist;
        public bool sphit;
        public int[] starlist;
        public int targetId;
        public bool weak;

        public int getAtkNp(int index)
        {
            if ((this.atknplist != null) && (index < this.atknplist.Length))
            {
                return this.atknplist[index];
            }
            return 0;
        }

        public int getAttackCount() => 
            this.damagelist.Length;

        public bool getAvoidance() => 
            this.avoidance;

        public bool getCritical() => 
            this.critical;

        public bool getCriticalPoint(int index) => 
            (((this.starlist != null) && (index < this.starlist.Length)) && (0 < this.starlist[index]));

        public int getCriticalPointCount(int index)
        {
            if ((this.starlist != null) && (index < this.starlist.Length))
            {
                return this.starlist[index];
            }
            return 0;
        }

        public int getDamage(int index)
        {
            if ((this.damagelist != null) && (index < this.damagelist.Length))
            {
                return this.damagelist[index];
            }
            return -1;
        }

        public int[] getDamageList() => 
            this.damagelist;

        public int getDefNp(int index)
        {
            if ((this.defnplist != null) && (index < this.defnplist.Length))
            {
                return this.defnplist[index];
            }
            return 0;
        }

        public bool getInvincible() => 
            this.invincible;

        public bool getRegist() => 
            this.regist;

        public bool getWeak() => 
            this.weak;
    }

    public class HealData
    {
        public int buffId;
        public int functionIndex;
        public int healPoint;
        public int targetId;
    }

    public class ReplaceMember
    {
        public int functionIndex;
        public int index;
        public int inUniqeId;
        public int outUniqeId;
    }

    public enum STATE
    {
        ACTORS,
        SYSTEM,
        FIELD,
        MOTION
    }

    public class TransformServant
    {
        public int functionIndex;
        public int index;
        public int uniqueId;
    }
}

