using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleLogicEnemyAi
{
    private List<AiEntity> _thinkList;
    [CompilerGenerated]
    private static Comparison<BattleServantData> <>f__am$cache7;
    [CompilerGenerated]
    private static Predicate<AiEntity> <>f__am$cache8;
    [CompilerGenerated]
    private static Predicate<AiEntity> <>f__am$cache9;
    [CompilerGenerated]
    private static Predicate<AiEntity> <>f__am$cacheA;
    [CompilerGenerated]
    private static Predicate<AiEntity> <>f__am$cacheB;
    [CompilerGenerated]
    private static Comparison<AiEntity> <>f__am$cacheC;
    public List<int> actlist = new List<int>();
    private AiActMaster aiActMst;
    public BattleData data;
    public BattleLogic logic;
    public BattleLogicNomal logicNomal;
    public BattleLogicTarget logictarget;

    private bool checkThinking(BattleServantData svtData, AiEntity aiEnt, int turn, int actcnt)
    {
        Ai.COND cond = Ai.getCond(aiEnt.cond);
        AiState state = svtData.getAiState();
        this.DebugLog(string.Concat(new object[] { "[[ checkThinking {", aiEnt.getPrimarykey(), "}]] > 条件 : ", cond.ToString(), ":vals[ ", aiEnt.getVal(), " ]" }));
        switch (cond)
        {
            case Ai.COND.NONE:
                return true;

            case Ai.COND.HP_HIGHER:
            {
                int num = Mathf.FloorToInt((((float) svtData.getNowHp()) / ((float) svtData.getMaxHp())) * 1000f);
                int num2 = aiEnt.getVal();
                this.DebugLog(string.Concat(new object[] { "=ck( ", num2, " <= ", num }));
                return (num2 <= num);
            }
            case Ai.COND.HP_LOWER:
            {
                int num3 = Mathf.FloorToInt((((float) svtData.getNowHp()) / ((float) svtData.getMaxHp())) * 1000f);
                int num4 = aiEnt.getVal();
                this.DebugLog(string.Concat(new object[] { "=ck( ", num4, " >= ", num3 }));
                return (num4 >= num3);
            }
            case Ai.COND.ACTCOUNT:
                this.DebugLog(string.Concat(new object[] { "=ck( ", actcnt, " == ", aiEnt.getVal() }));
                return (actcnt == aiEnt.getVal());

            case Ai.COND.ACTCOUNT_MULTIPLE:
                this.DebugLog(string.Concat(new object[] { "=ck( ", actcnt, " % ", aiEnt.getVal() }));
                return ((actcnt % aiEnt.getVal()) == 0);

            case Ai.COND.TURN:
                this.DebugLog(string.Concat(new object[] { "=ck( ", turn, " == ", aiEnt.getVal() }));
                return (turn == aiEnt.getVal());

            case Ai.COND.TURN_MULTIPLE:
                this.DebugLog(string.Concat(new object[] { "=ck( ", turn, " % ", aiEnt.getVal() }));
                return ((turn % aiEnt.getVal()) == 0);

            case Ai.COND.CHECK_SELF_BUFF:
                return svtData.checkBuffIndividualities(aiEnt.vals);

            case Ai.COND.CHECK_SELF_INDIVIDUALITY:
                return svtData.checkIndividualities(aiEnt.vals);

            case Ai.COND.CHECK_PT_BUFF:
                foreach (BattleServantData data in this.data.getFieldPTList(svtData.getUniqueID()))
                {
                    if (data.checkBuffIndividualities(aiEnt.vals) && data.isAlive())
                    {
                        return true;
                    }
                }
                break;

            case Ai.COND.CHECK_PT_INDIVIDUALITY:
                foreach (BattleServantData data2 in this.data.getFieldPTList(svtData.getUniqueID()))
                {
                    if (data2.checkIndividualities(aiEnt.vals) && data2.isAlive())
                    {
                        return true;
                    }
                }
                break;

            case Ai.COND.CHECK_OPPONENT_BUFF:
                foreach (BattleServantData data3 in this.data.getFieldOpponentList(svtData.getUniqueID()))
                {
                    if (data3.checkBuffId(aiEnt.vals) && data3.isAlive())
                    {
                        return true;
                    }
                }
                break;

            case Ai.COND.CHECK_OPPONENT_INDIVIDUALITY:
                foreach (BattleServantData data4 in this.data.getFieldOpponentList(svtData.getUniqueID()))
                {
                    if (data4.checkIndividualities(aiEnt.getVals()) && data4.isAlive())
                    {
                        return true;
                    }
                }
                break;

            case Ai.COND.BEFORE_ACT_TYPE:
                return state.beforeActType.Check(aiEnt.getVal());

            case Ai.COND.BEFORE_ACT_ID:
                return (state.beforeActId == aiEnt.getVal());

            case Ai.COND.BEFORE_NOT_ACT_TYPE:
                return !state.beforeActType.Check(aiEnt.getVal());

            case Ai.COND.BEFORE_NOT_ACT_ID:
                return (state.beforeActId != aiEnt.getVal());

            case Ai.COND.CHECK_SELF_BUFF_INDIVIDUALITY:
                return svtData.checkBuffIndividualities(aiEnt.getVals());

            case Ai.COND.CHECK_PT_BUFF_INDIVIDUALITY:
                foreach (BattleServantData data5 in this.data.getFieldPTList(svtData.getUniqueID()))
                {
                    if (data5.checkBuffIndividualities(aiEnt.getVals()) && data5.isAlive())
                    {
                        return true;
                    }
                }
                break;

            case Ai.COND.CHECK_OPPONENT_BUFF_INDIVIDUALITY:
                foreach (BattleServantData data6 in this.data.getFieldOpponentList(svtData.getUniqueID()))
                {
                    if (data6.checkBuffIndividualities(aiEnt.getVals()) && data6.isAlive())
                    {
                        return true;
                    }
                }
                break;

            case Ai.COND.CHECK_SELF_NPTURN:
                if (svtData.getMaxNextTDTurn() <= 0)
                {
                    return false;
                }
                return (svtData.getNextTDTurn() == aiEnt.getVal());

            case Ai.COND.CHECK_PT_LOWER_NPTURN:
            {
                int num11 = 0x3e7;
                foreach (BattleServantData data7 in this.data.getFieldPTList(svtData.getUniqueID()))
                {
                    if (((data7.getMaxNextTDTurn() <= 0) && data7.isAlive()) && (data7.getNextTDTurn() < num11))
                    {
                        num11 = data7.getNextTDTurn();
                    }
                }
                return (num11 == aiEnt.getVal());
            }
            case Ai.COND.CHECK_OPPONENT_HEIGHT_NPGAUGE:
            {
                int num13 = 0;
                foreach (BattleServantData data8 in this.data.getFieldOpponentList(svtData.getUniqueID()))
                {
                    if ((data8.hasTreasureDvc() && data8.isAlive()) && (num13 < data8.getNp()))
                    {
                        num13 = data8.getNp();
                    }
                }
                return (aiEnt.getVal() <= num13);
            }
            case Ai.COND.ACTCOUNT_THISTURN:
                this.DebugLog(string.Concat(new object[] { "=ck( ", svtData.getThisTurnActCount(), " == ", aiEnt.getVal() }));
                return (svtData.getThisTurnActCount() == aiEnt.getVal());
        }
        return false;
    }

    protected void DebugLog(string str)
    {
    }

    private AiEntity getAction(BattleServantData svtData, int turnCount, PROC_STATE procState, int countAct)
    {
        this.DebugLog("====< start BattleLogicEnemyAi::getAction >=====");
        this.DebugLog("====>> UniqueId " + svtData.getUniqueID());
        AiState state = svtData.getAiState();
        if (procState == PROC_STATE.NOMAL)
        {
            state.actCount++;
        }
        bool flag = true;
        int num = 0;
        AiEntity aiEnt = null;
        do
        {
            state = svtData.getAiState();
            this.updateThinkGroup(state.aiGroupId);
            this.DebugLog("updateThinkGroup : " + state.aiGroupId);
            num++;
            if (10 < num)
            {
                flag = false;
                this.DebugLog(" <== over think");
                break;
            }
            aiEnt = this.getBasicAct(svtData, turnCount - state.baseTurn, state.actCount, procState);
            if (aiEnt == null)
            {
                this.DebugLog(" <== no think entity");
                flag = false;
                break;
            }
            this.DebugLog(" aiActId : " + aiEnt.aiActId);
            if (this.aiActMst.getEntityFromId<AiActEntity>(aiEnt.aiActId).isThinkEnd())
            {
                flag = false;
            }
            if (flag)
            {
                this.DebugLog(" no think end -> ");
                this.procAiAct(svtData, aiEnt, countAct);
            }
        }
        while (flag);
        if (aiEnt == null)
        {
            aiEnt = new AiEntity {
                aiActId = 1
            };
        }
        this.DebugLog("====< end BattleLogicEnemyAi::getAction >=====");
        return aiEnt;
    }

    private AiEntity getBasicAct(BattleServantData svtData, int turn, int actcnt, PROC_STATE procState)
    {
        <getBasicAct>c__AnonStorey80 storey = new <getBasicAct>c__AnonStorey80 {
            actcnt = actcnt
        };
        this.DebugLog(string.Concat(new object[] { " :", procState, ":getBasicAct( ", turn, ",", storey.actcnt, "):", this._thinkList.Count }));
        List<AiEntity> list = new List<AiEntity>();
        if (procState == PROC_STATE.NOMAL)
        {
            list = this._thinkList.FindAll(new Predicate<AiEntity>(storey.<>m__A4));
        }
        else if (procState == PROC_STATE.DEAD)
        {
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = s => Ai.ACT_NUM.REACTION_DEAD.Check(s.actNum);
            }
            list = this._thinkList.FindAll(<>f__am$cache8);
        }
        else if (procState == PROC_STATE.PLAYERACTIONEND)
        {
            if (<>f__am$cache9 == null)
            {
                <>f__am$cache9 = s => Ai.ACT_NUM.REACTION_PLAYERACTIONEND.Check(s.actNum);
            }
            list = this._thinkList.FindAll(<>f__am$cache9);
        }
        else if (procState == PROC_STATE.TURN_START)
        {
            if (<>f__am$cacheA == null)
            {
                <>f__am$cacheA = s => Ai.ACT_NUM.REACTION_ENEMYTURN_START.Check(s.actNum);
            }
            list = this._thinkList.FindAll(<>f__am$cacheA);
        }
        else if (procState == PROC_STATE.TURN_END)
        {
            if (<>f__am$cacheB == null)
            {
                <>f__am$cacheB = s => Ai.ACT_NUM.REACTION_ENEMYTURN_END.Check(s.actNum);
            }
            list = this._thinkList.FindAll(<>f__am$cacheB);
        }
        if (<>f__am$cacheC == null)
        {
            <>f__am$cacheC = (a, b) => b.priority - a.priority;
        }
        list.Sort(<>f__am$cacheC);
        this.DebugLog(" > thinking.Count : " + list.Count);
        if (list.Count <= 0)
        {
            this.DebugLog(" no think list ");
            return null;
        }
        do
        {
            <getBasicAct>c__AnonStorey81 storey2 = new <getBasicAct>c__AnonStorey81();
            this.DebugLog(" == start => priority( " + list[0].priority);
            storey2.priority = list[0].priority;
            AiEntity[] entityArray = list.FindAll(new Predicate<AiEntity>(storey2.<>m__AA)).ToArray();
            this.DebugLog(" list : " + entityArray.Length);
            if (entityArray.Length <= 1)
            {
                if (this.checkThinking(svtData, entityArray[0], turn, storey.actcnt))
                {
                    this.DebugLog(" act ok : " + entityArray[0].id);
                    return entityArray[0];
                }
                list.Remove(entityArray[0]);
            }
            else
            {
                WeightRate<int> rate = new WeightRate<int>();
                for (int i = 0; i < entityArray.Length; i++)
                {
                    if (this.checkThinking(svtData, entityArray[i], turn, storey.actcnt))
                    {
                        this.DebugLog(string.Concat(new object[] { " ++ Set : ", entityArray[i].id, ":", entityArray[i].probability }));
                        rate.setWeight(entityArray[i].probability, i);
                    }
                    else
                    {
                        this.DebugLog(" -- Remove : " + entityArray[i].id);
                        list.Remove(entityArray[i]);
                    }
                }
                if (rate.getCount() == 0)
                {
                    this.DebugLog(" wr count 0 ");
                }
                else
                {
                    this.DebugLog(">< Check Weight )");
                    AiEntity aiEnt = entityArray[rate.getData(BattleRandom.getNext(rate.getTotalWeight()))];
                    if (this.checkThinking(svtData, aiEnt, turn, storey.actcnt))
                    {
                        this.DebugLog(" act ok : " + aiEnt.getPrimarykey());
                        return aiEnt;
                    }
                    this.DebugLog(" *** Do not act ");
                    list.Remove(aiEnt);
                }
            }
        }
        while (0 < list.Count);
        this.DebugLog(" end think list ");
        return null;
    }

    public BattleLogicTask[] procAiAct(BattleServantData svtData, AiEntity aiEnt, int countAct)
    {
        this.DebugLog(string.Concat(new object[] { " >> procAiAct( ", svtData.getUniqueID(), ", ", aiEnt.id, " count( ", countAct, " )" }));
        AiActEntity entity = this.aiActMst.getEntityFromId<AiActEntity>(aiEnt.aiActId);
        this.DebugLog("  aiEnt.aiActId : " + aiEnt.aiActId);
        AiAct.TYPE nONE = AiAct.TYPE.NONE;
        nONE = entity.getActType();
        this.DebugLog("  actType : " + nONE.ToString());
        if (AiAct.TYPE.CHANGE_THINKING.Check(entity.type))
        {
            svtData.getAiState().changeThinking(aiEnt.getChangeAiId());
            return null;
        }
        AiState state2 = svtData.getAiState();
        if (0 < aiEnt.getChangeAiId())
        {
            state2.changeThinking(aiEnt.getChangeAiId());
        }
        WeightRate<int> rate = new WeightRate<int>();
        switch (nONE)
        {
            case AiAct.TYPE.SKILL1:
                if (!svtData.isUseSkill())
                {
                    this.DebugLog(" dont act skill");
                    nONE = AiAct.TYPE.ATTACK;
                }
                if (!svtData.isUseSelfSkill(0))
                {
                    this.DebugLog(" has not skill0 ");
                    nONE = AiAct.TYPE.ATTACK;
                }
                break;

            case AiAct.TYPE.SKILL2:
                if (!svtData.isUseSkill())
                {
                    this.DebugLog(" dont act skill");
                    nONE = AiAct.TYPE.ATTACK;
                }
                if (!svtData.isUseSelfSkill(1))
                {
                    this.DebugLog(" has not skill1 ");
                    nONE = AiAct.TYPE.ATTACK;
                }
                break;

            case AiAct.TYPE.SKILL3:
                if (!svtData.isUseSkill())
                {
                    this.DebugLog(" dont act skill");
                    nONE = AiAct.TYPE.ATTACK;
                }
                if (!svtData.isUseSelfSkill(2))
                {
                    this.DebugLog(" has not skill2 ");
                    nONE = AiAct.TYPE.ATTACK;
                }
                break;

            case AiAct.TYPE.RANDOM:
                rate.setWeight(10, AiAct.TYPE.ATTACK.getInt());
                if (svtData.isUseSkill())
                {
                    if (svtData.isUseSelfSkill(0))
                    {
                        this.DebugLog("  skill0 ok");
                        rate.setWeight(10, AiAct.TYPE.SKILL1.getInt());
                    }
                    if (svtData.isUseSelfSkill(1))
                    {
                        this.DebugLog("  skill1 ok");
                        rate.setWeight(10, AiAct.TYPE.SKILL2.getInt());
                    }
                    if (svtData.isUseSelfSkill(2))
                    {
                        this.DebugLog("  skill2 ok");
                        rate.setWeight(10, AiAct.TYPE.SKILL3.getInt());
                    }
                }
                nONE = AiAct.getType(rate.getData(BattleRandom.getNext(rate.getTotalWeight())));
                break;

            case AiAct.TYPE.SKILL_RANDOM:
                if (svtData.isUseSkill())
                {
                    if (svtData.isUseSelfSkill(0))
                    {
                        this.DebugLog("  skill0 ok");
                        rate.setWeight(10, AiAct.TYPE.SKILL1.getInt());
                    }
                    if (svtData.isUseSelfSkill(1))
                    {
                        this.DebugLog("  skill1 ok");
                        rate.setWeight(10, AiAct.TYPE.SKILL2.getInt());
                    }
                    if (svtData.isUseSelfSkill(2))
                    {
                        this.DebugLog("  skill2 ok");
                        rate.setWeight(10, AiAct.TYPE.SKILL3.getInt());
                    }
                }
                if (0 < rate.getCount())
                {
                    nONE = AiAct.getType(rate.getData(BattleRandom.getNext(rate.getTotalWeight())));
                }
                else
                {
                    this.DebugLog("  No skill 1,2,3 or dont skill");
                    nONE = AiAct.TYPE.ATTACK;
                }
                break;

            case AiAct.TYPE.NOBLE_PHANTASM:
                if (!svtData.isNobleAction())
                {
                    nONE = AiAct.TYPE.ATTACK;
                }
                break;

            case AiAct.TYPE.SKILL_ID:
                if (entity.skillVals.Length <= 1)
                {
                    this.DebugLog("  No skillID or skillLevel");
                    nONE = AiAct.TYPE.NONE;
                }
                else
                {
                    int skillId = entity.skillVals[0];
                    int lv = entity.skillVals[1];
                    if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId(skillId, lv) == null)
                    {
                        this.DebugLog(string.Concat(new object[] { "  No skill ID:", skillId, " LV:", lv }));
                        nONE = AiAct.TYPE.NONE;
                    }
                    else
                    {
                        svtData.addSkillInfo(BattleSkillInfoData.TYPE.TEMP, 0, skillId, lv);
                    }
                }
                break;
        }
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = null;
        BattleCommandData command = null;
        item = new BattleLogicTask {
            motionMessage = aiEnt.infoText
        };
        item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, svtData.getUniqueID());
        int[] targetIdlist = new int[] { this.logictarget.getTargetAiAct(entity.getActTarget(), svtData.getUniqueID(), entity.targetIndividuality, this.data.getFieldPlayerServantIDList()) };
        int[] ptTarget = new int[] { this.logictarget.getTargetAiAct(entity.getActTarget(), svtData.getUniqueID(), entity.targetIndividuality, this.data.getFieldEnemyServantIDList()) };
        svtData.getAiState().setBeforeAction(nONE, aiEnt.aiActId);
        if (nONE == AiAct.TYPE.NONE)
        {
            this.DebugLog(" none ");
            return list.ToArray();
        }
        if ((nONE == AiAct.TYPE.ATTACK) || (nONE == AiAct.TYPE.ATTACK_CRITICAL))
        {
            this.DebugLog(" Attack ->");
            item.setTarget(targetIdlist);
            BattleCommand.TYPE wEAK = BattleCommand.TYPE.WEAK;
            if (!SvtType.IsEnemy(svtData.svtType))
            {
                WeightRate<int> rate2 = new WeightRate<int>();
                int[] numArray3 = svtData.getCommandList();
                for (int i = 0; i < numArray3.Length; i++)
                {
                    rate2.setWeight(10, numArray3[i]);
                }
                wEAK = BattleCommand.getType(rate2.getData(BattleRandom.getNext(rate2.getTotalWeight())));
            }
            this.DebugLog(" commandType : " + wEAK.ToString());
            command = new BattleCommandData(wEAK, svtData.getSvtId(), svtData.getDispLimitCount()) {
                uniqueId = svtData.getUniqueID()
            };
            if (nONE == AiAct.TYPE.ATTACK_CRITICAL)
            {
                command.checkCriticalRate(100, 0);
                nONE = AiAct.TYPE.ATTACK;
            }
            else
            {
                command.checkCriticalRate(svtData.getCriticalRate(), BattleRandom.getNext(0x3e8));
            }
            if ((wEAK == BattleCommand.TYPE.WEAK) && command.isCritical())
            {
                command.type = 11;
            }
            BattleComboData combo = new BattleComboData();
            item.setActionCommand(combo, command, countAct);
            list.Add(item);
        }
        else if (((nONE != AiAct.TYPE.SKILL1) && (nONE != AiAct.TYPE.SKILL2)) && ((nONE != AiAct.TYPE.SKILL3) && (nONE != AiAct.TYPE.SKILL_ID)))
        {
            switch (nONE)
            {
                case AiAct.TYPE.NOBLE_PHANTASM:
                    item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, svtData.getUniqueID());
                    item.setTarget(this.data.getFieldPlayerServantIDList());
                    command = new BattleCommandData {
                        type = svtData.getTreasureDvcCardId(),
                        svtlimit = svtData.getDispLimitCount(),
                        uniqueId = svtData.getUniqueID(),
                        svtId = svtData.getSvtId(),
                        treasureDvc = svtData.getTreasureDvcId()
                    };
                    command.checkCriticalRate(svtData.getCriticalRate(), BattleRandom.getNext(0x3e8));
                    item.setActionCommand(new BattleComboData(), command, countAct);
                    item.setActionTreasureDvc(svtData.getTreasureDvcId());
                    list.Add(item);
                    break;

                case AiAct.TYPE.PLAY_MOTION:
                {
                    int num5 = aiEnt.getActionValue();
                    if (0 < num5)
                    {
                        item.setPlayMoiton("MOTION_" + num5);
                        item.setTargetObject(this.logic.perf.getServantGameObject(svtData.getUniqueID()));
                        item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, svtData.getUniqueID());
                        list.Add(item);
                    }
                    break;
                }
            }
        }
        else
        {
            this.DebugLog(" Use Skill ->");
            BattleSkillInfoData skillInfo = null;
            switch (nONE)
            {
                case AiAct.TYPE.SKILL1:
                    skillInfo = svtData.getSelfSkillInfo(0);
                    break;

                case AiAct.TYPE.SKILL2:
                    skillInfo = svtData.getSelfSkillInfo(1);
                    break;

                case AiAct.TYPE.SKILL3:
                    skillInfo = svtData.getSelfSkillInfo(2);
                    break;

                case AiAct.TYPE.SKILL_ID:
                    skillInfo = svtData.getTempSkillInfo(0);
                    break;
            }
            if (skillInfo != null)
            {
                this.DebugLog("  use skill : " + skillInfo.skillId);
                item.setActionSkill(skillInfo, targetIdlist, ptTarget);
                item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, svtData.getUniqueID());
                list.Add(item);
            }
        }
        this.DebugLog(" >> end procAiAct");
        return list.ToArray();
    }

    public void resetAct()
    {
        this.actlist.Clear();
        this.data.countEnemyAttack = 0;
        foreach (BattleServantData data in this.data.getFieldEnemyServantList())
        {
            data.resetRetAttackCount();
        }
    }

    public BattleLogicTask[] taskAIAttack(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        if (data.perf != null)
        {
            data.perf.setOffTarget();
        }
        if (this.data.checkAlivePlayers())
        {
            if (3 <= this.data.countEnemyAttack)
            {
                return list.ToArray();
            }
            this.aiActMst = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<AiActMaster>(DataNameKind.Kind.AI_ACT);
            this.updateActPriorityList();
            if (this.actlist.Count <= 0)
            {
                this.data.countEnemyAttack++;
                return list.ToArray();
            }
            int uniqueId = this.actlist[0];
            this.actlist.RemoveAt(0);
            BattleServantData svtData = data.getServantData(uniqueId);
            if (!svtData.isAction())
            {
                svtData.restAttackCount--;
                this.data.countEnemyAttack++;
                return list.ToArray();
            }
            if (!svtData.isTDSeraled() && svtData.checkUseTDvc(true))
            {
                BattleLogicTask item = new BattleLogicTask();
                item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, svtData.getUniqueID());
                item.setTarget(this.logictarget.getTargetAiAct(AiAct.TARGET.RANDOM, svtData.getUniqueID(), null, data.getFieldPlayerServantIDList()));
                BattleCommandData command = new BattleCommandData {
                    type = svtData.getTreasureDvcCardId(),
                    svtlimit = svtData.getDispLimitCount(),
                    uniqueId = svtData.getUniqueID(),
                    svtId = svtData.getSvtId(),
                    treasureDvc = svtData.getTreasureDvcId()
                };
                command.checkCriticalRate(svtData.getCriticalRate(), BattleRandom.getNext(0x3e8));
                item.setActionCommand(new BattleComboData(), command, this.data.countEnemyAttack);
                item.setActionTreasureDvc(svtData.getTreasureDvcId());
                list.Add(item);
                svtData.restAttackCount--;
                this.data.countEnemyAttack++;
                return list.ToArray();
            }
            AiEntity aiEnt = this.getAction(svtData, data.turnCount, PROC_STATE.NOMAL, this.data.countEnemyAttack);
            list.AddRange(this.procAiAct(svtData, aiEnt, this.data.countEnemyAttack));
            svtData.restAttackCount--;
            this.data.countEnemyAttack++;
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskAIDead(int actUniqueId)
    {
        this.DebugLog("== taskAIDead[ " + actUniqueId + " ]");
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleServantData svtData = this.data.getServantData(actUniqueId);
        if (!svtData.isAlive())
        {
            if (!this.data.checkAlivePlayers())
            {
                return list.ToArray();
            }
            this.aiActMst = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<AiActMaster>(DataNameKind.Kind.AI_ACT);
            AiEntity aiEnt = this.getAction(svtData, this.data.turnCount, PROC_STATE.DEAD, 0);
            list.AddRange(this.procAiAct(svtData, aiEnt, 0));
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskAIEnemyEndTurn(int actUniqueId)
    {
        this.DebugLog("== taskAIEnemyEndTurn[ " + actUniqueId + " ]");
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleServantData svtData = this.data.getServantData(actUniqueId);
        if (svtData.isAlive())
        {
            if (!this.data.checkAlivePlayers())
            {
                return list.ToArray();
            }
            this.aiActMst = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<AiActMaster>(DataNameKind.Kind.AI_ACT);
            AiEntity aiEnt = this.getAction(svtData, this.data.turnCount, PROC_STATE.TURN_END, 0);
            list.AddRange(this.procAiAct(svtData, aiEnt, 0));
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskAIEnemyStartTurn(int actUniqueId)
    {
        this.DebugLog("== taskAIEnemyStartTurn[ " + actUniqueId + " ]");
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleServantData svtData = this.data.getServantData(actUniqueId);
        if (svtData.isAlive())
        {
            if (!this.data.checkAlivePlayers())
            {
                return list.ToArray();
            }
            this.aiActMst = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<AiActMaster>(DataNameKind.Kind.AI_ACT);
            AiEntity aiEnt = this.getAction(svtData, this.data.turnCount, PROC_STATE.TURN_START, 0);
            list.AddRange(this.procAiAct(svtData, aiEnt, 0));
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskAIPlayerActionEnd(int actUniqueId)
    {
        this.DebugLog("== taskAIPlayerActionEnd[ " + actUniqueId + " ]");
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleServantData svtData = this.data.getServantData(actUniqueId);
        if (svtData.isAlive())
        {
            if (!this.data.checkAlivePlayers())
            {
                return list.ToArray();
            }
            this.aiActMst = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<AiActMaster>(DataNameKind.Kind.AI_ACT);
            AiEntity aiEnt = this.getAction(svtData, this.data.turnCount, PROC_STATE.PLAYERACTIONEND, 0);
            list.AddRange(this.procAiAct(svtData, aiEnt, 0));
        }
        return list.ToArray();
    }

    public void updateActPriorityList()
    {
        if (this.actlist.Count <= 0)
        {
            BattleServantData[] array = this.data.getFieldEnemyServantList();
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = (a, b) => b.actPriority - a.actPriority;
            }
            Array.Sort<BattleServantData>(array, <>f__am$cache7);
            foreach (BattleServantData data in array)
            {
                if (data.isAlive() && (0 < data.restAttackCount))
                {
                    this.actlist.Add(data.getUniqueID());
                }
            }
        }
    }

    public void updateThinkGroup(int groupId)
    {
        if (((this._thinkList == null) || (this._thinkList.Count == 0)) || (this._thinkList[0].id != groupId))
        {
            this._thinkList = new List<AiEntity>(AiMaster.getListFormGroupId(groupId));
        }
    }

    [CompilerGenerated]
    private sealed class <getBasicAct>c__AnonStorey80
    {
        internal int actcnt;

        internal bool <>m__A4(AiEntity s) => 
            (Ai.ACT_NUM.ANYTIME.Check(s.actNum) || (s.actNum == this.actcnt));
    }

    [CompilerGenerated]
    private sealed class <getBasicAct>c__AnonStorey81
    {
        internal int priority;

        internal bool <>m__AA(AiEntity s) => 
            (s.priority == this.priority);
    }

    public enum PROC_STATE
    {
        DEAD = 2,
        NOMAL = 1,
        PLAYERACTIONEND = 3,
        TURN_END = 5,
        TURN_START = 4
    }
}

