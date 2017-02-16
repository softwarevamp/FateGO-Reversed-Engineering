using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleLogicNomal
{
    public BattleData data;
    public BattleLogic logic;
    public BattleLogicFunction logicfunction;
    public BattleLogicSkill logicskill;
    public BattleLogicTarget logictarget;

    public BattleActionData createBackStep(int uniqueId) => 
        new BattleActionData { 
            actorId = uniqueId,
            motionname = "MOTION_BACK"
        };

    public BattleActionData createBuffAddEnemy(BattleLogicTask task)
    {
        BattleServantData[] dataArray = this.data.getFieldEnemyServantList();
        for (int i = 0; i < dataArray.Length; i++)
        {
            if (dataArray[i].isAlive())
            {
                int num2 = dataArray[i].getMaxHp();
                dataArray[i].turnBuffProgressingIncrease();
                if (dataArray[i].checkUpdateUpdownHp(num2, true))
                {
                    dataArray[i].updateHp();
                }
            }
        }
        return null;
    }

    public BattleActionData createBuffAddPlayer(BattleLogicTask task)
    {
        BattleServantData[] dataArray = this.data.getFieldPlayerServantList();
        for (int i = 0; i < dataArray.Length; i++)
        {
            if (dataArray[i].isAlive())
            {
                int num2 = dataArray[i].getMaxHp();
                dataArray[i].turnBuffProgressingIncrease();
                if (dataArray[i].checkUpdateUpdownHp(num2, true))
                {
                    dataArray[i].updateHp();
                }
            }
        }
        return null;
    }

    public BattleActionData createComboOrder(BattleLogicTask task)
    {
        Debug.Log(" >> createComboOrder");
        BattleActionData data = new BattleActionData();
        data.setStateField();
        int[] numArray = task.getTargetlist();
        FunctionEntity entity = new FunctionEntity();
        if (task.isArts())
        {
            Debug.Log(" >> isArts");
            data.setTypeOrderArts();
            entity.vals = new int[] { 0x66, 1 };
            data.motionname = "FIELD_PLAYER";
        }
        else if (task.isQuick())
        {
            Debug.Log(" >> isQuick");
            data.setTypeOrderQuick();
            entity.vals = new int[] { 100, 1, 1 };
            data.motionname = "FIELD_PLAYER";
        }
        else if (task.isBuster())
        {
            Debug.Log(" >> isBuster");
            data.setTypeOrderBuster();
            entity.vals = new int[] { 0x73, 1, 1 };
            data.motionname = "FIELD_ENEMY";
        }
        Debug.Log("list:" + numArray.Length);
        bool flag = false;
        for (int i = 0; i < numArray.Length; i++)
        {
            Debug.Log(string.Concat(new object[] { "list[", i, "]:", numArray[i] }));
            if (this.data.getServantData(numArray[i]) != null)
            {
                flag = true;
            }
        }
        if (!flag)
        {
            return null;
        }
        return data;
    }

    public BattleActionData createCommandBattle(BattleLogicTask task)
    {
        BattleServantData actor = this.data.getServantData(task.getActorId());
        BattleServantData target = null;
        BattleActionData action = new BattleActionData();
        BattleCommandData command = task.getCommand();
        int uniqueId = 0;
        if (this.logic.bugfix_overkill)
        {
            this.logic.resetReducedHpAll();
        }
        action.actorId = task.getActorId();
        action.motionMessage = task.motionMessage;
        uniqueId = task.getTarget();
        if (uniqueId <= 0)
        {
            Debug.Log(" no target");
            return null;
        }
        target = this.data.getServantData(uniqueId);
        BattleActionData.DamageData data = this.logic.getDamagelist(actor, target, command);
        if (actor.checkPlayer())
        {
            target.setOverKillTargetId(actor.getUniqueID());
        }
        action.targetId = uniqueId;
        action.type = command.type;
        action.setCommand(command);
        action.setDamageData(data);
        action.actionIndex = command.ActionIndex;
        action.motionId = actor.getMotionId(command);
        action.attackcount = data.getAttackCount();
        action.prevattackme = this.data.isPrevAttackMe(actor.getUniqueID(), target.getUniqueID());
        BattleBuffData.BuffData[] dataArray = actor.getCommandSideEffect(command.getIndividualities());
        SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
        foreach (BattleBuffData.BuffData data6 in dataArray)
        {
            SkillLvEntity entity = master.getEntityFromId<SkillLvEntity>(data6.vals[0], data6.vals[1]);
            this.logicfunction.procList(action, entity.funcId, entity.getDataValsList(), false);
        }
        this.data.setCommandAttack(actor.getUniqueID(), target.getUniqueID());
        if (actor.getAttackType(command) == 2)
        {
            int[] numArray = null;
            if (this.data.isEnemyID(uniqueId))
            {
                numArray = this.data.getFieldEnemyServantIDList();
            }
            else
            {
                numArray = this.data.getFieldPlayerServantIDList();
            }
            foreach (int num3 in numArray)
            {
                if (num3 != uniqueId)
                {
                    target = this.data.getServantData(num3);
                    if (target.isAlive())
                    {
                        BattleActionData.DamageData data7 = this.logic.getDamagelist(actor, target, command);
                        if (actor.checkPlayer())
                        {
                            target.setOverKillTargetId(actor.getUniqueID());
                        }
                        action.setDamageData(data7);
                    }
                }
            }
        }
        this.logic.checkUsedBuff();
        return action;
    }

    public BattleActionData createEndTurnEnemy(BattleLogicTask task)
    {
        bool flag = false;
        BattleActionData actiondata = new BattleActionData();
        BuffMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
        this.logic.resetReducedHpAll();
        actiondata.setStateField();
        actiondata.motionname = "MOTION_ENEMY_TURN_END";
        Dictionary<int, BattleBuffData.BuffData[]> dictionary = new Dictionary<int, BattleBuffData.BuffData[]>();
        Dictionary<int, BattleBuffData.BuffData[]> dictionary2 = new Dictionary<int, BattleBuffData.BuffData[]>();
        SkillLvMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
        BattleServantData[] dataArray = this.data.getFieldPlayerServantList();
        for (int i = 0; i < dataArray.Length; i++)
        {
            if (dataArray[i].isBuffProgressFlg)
            {
                BattleBuffData.BuffData[] dataArray2 = dataArray[i].turnBuffProgressing(null);
                if ((dataArray[i].isAlive() && dataArray[i].provisionalDamage(0)) && (0 < dataArray2.Length))
                {
                    dictionary.Add(dataArray[i].getUniqueID(), dataArray2);
                }
            }
        }
        dataArray = this.data.getFieldEnemyServantList();
        for (int j = 0; j < dataArray.Length; j++)
        {
            if (dataArray[j].isAlive())
            {
                flag |= dataArray[j].turnProgressing(this.logic, this.data.checkAlivePlayers(), actiondata);
                if (dataArray[j].provisionalDamage(0))
                {
                    BattleBuffData.BuffData[] dataArray3 = dataArray[j].getTTurnEndBufflist();
                    if (0 < dataArray3.Length)
                    {
                        dictionary2.Add(dataArray[j].getUniqueID(), dataArray3);
                    }
                }
                dataArray[j].buffData.usedProgressing();
            }
            if (dataArray[j].isBuffProgressFlg)
            {
                BattleBuffData.BuffData[] dataArray4 = dataArray[j].turnBuffProgressing(null);
                if ((dataArray[j].isAlive() && dataArray[j].provisionalDamage(0)) && (0 < dataArray4.Length))
                {
                    dictionary.Add(dataArray[j].getUniqueID(), dataArray4);
                }
            }
        }
        foreach (KeyValuePair<int, BattleBuffData.BuffData[]> pair in dictionary2)
        {
            foreach (BattleBuffData.BuffData data2 in pair.Value)
            {
                BuffEntity entity = master.getEntityFromId<BuffEntity>(data2.buffId);
                BattleSkillInfoData skillInfo = new BattleSkillInfoData {
                    svtUniqueId = pair.Key,
                    skillId = data2.vals[0],
                    skilllv = data2.vals[1]
                };
                BattleServantData data4 = this.data.getServantData(skillInfo.svtUniqueId);
                SkillLvEntity entity2 = master2.getEntityFromId<SkillLvEntity>(skillInfo.skillId, skillInfo.skilllv);
                if (this.data.checkAliveOther(data4.getUniqueID()) || this.logic.checkPtTargetFunction(entity2.funcId))
                {
                    int num4 = data4.getRevengeTargetUniqueId();
                    int[] enemyTargetList = null;
                    if (num4 < 0)
                    {
                        enemyTargetList = Target.getTargetIds(this.data, skillInfo.svtUniqueId, -1, pair.Key, Target.TYPE.ENEMY_RANDOM);
                    }
                    else
                    {
                        enemyTargetList = new int[] { num4 };
                    }
                    int[] ptTargetList = new int[] { pair.Key };
                    BattleLogicTask[] tasklist = this.logicskill.taskSkill(skillInfo, ptTargetList, enemyTargetList);
                    this.logic.addBattleLogicTask(tasklist);
                }
            }
        }
        foreach (KeyValuePair<int, BattleBuffData.BuffData[]> pair2 in dictionary)
        {
            foreach (BattleBuffData.BuffData data5 in pair2.Value)
            {
                if (master.getEntityFromId<BuffEntity>(data5.buffId).isEndAct())
                {
                    BattleSkillInfoData data6 = new BattleSkillInfoData {
                        svtUniqueId = pair2.Key,
                        skillId = data5.vals[0],
                        skilllv = data5.vals[1]
                    };
                    BattleServantData data7 = this.data.getServantData(data6.svtUniqueId);
                    SkillLvEntity entity4 = master2.getEntityFromId<SkillLvEntity>(data6.skillId, data6.skilllv);
                    if (this.data.checkAliveOther(data7.getUniqueID()) || this.logic.checkPtTargetFunction(entity4.funcId))
                    {
                        int num6 = data7.getRevengeTargetUniqueId();
                        int[] numArray2 = null;
                        if (num6 < 0)
                        {
                            numArray2 = Target.getTargetIds(this.data, data6.svtUniqueId, -1, pair2.Key, Target.TYPE.ENEMY_RANDOM);
                        }
                        else
                        {
                            numArray2 = new int[] { num6 };
                        }
                        int[] numArray5 = new int[] { pair2.Key };
                        BattleLogicTask[] taskArray2 = this.logicskill.taskSkill(data6, numArray5, numArray2);
                        this.logic.addBattleLogicTask(taskArray2);
                    }
                }
            }
        }
        if (flag)
        {
            return actiondata;
        }
        return null;
    }

    public BattleActionData createEndTurnPlayer(BattleLogicTask task)
    {
        bool flag = false;
        BattleActionData actiondata = new BattleActionData();
        BuffMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
        this.logic.resetReducedHpAll();
        actiondata.setStateField();
        actiondata.motionname = "MOTION_PLAYER_TURN_END";
        Dictionary<int, BattleBuffData.BuffData[]> dictionary = new Dictionary<int, BattleBuffData.BuffData[]>();
        Dictionary<int, BattleBuffData.BuffData[]> dictionary2 = new Dictionary<int, BattleBuffData.BuffData[]>();
        SkillLvMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
        BattleServantData[] dataArray = this.data.getFieldPlayerServantList();
        for (int i = 0; i < dataArray.Length; i++)
        {
            if (dataArray[i].isAlive())
            {
                flag |= dataArray[i].turnProgressing(this.logic, this.data.checkAliveEnemys(), actiondata);
                if (dataArray[i].provisionalDamage(0))
                {
                    BattleBuffData.BuffData[] dataArray2 = dataArray[i].getTTurnEndBufflist();
                    if (0 < dataArray2.Length)
                    {
                        dictionary2.Add(dataArray[i].getUniqueID(), dataArray2);
                    }
                }
                dataArray[i].buffData.usedProgressing();
            }
            if (dataArray[i].isBuffProgressFlg)
            {
                BattleBuffData.BuffData[] dataArray3 = dataArray[i].turnBuffProgressing(null);
                if ((dataArray[i].isAlive() && dataArray[i].provisionalDamage(0)) && (0 < dataArray3.Length))
                {
                    dictionary.Add(dataArray[i].getUniqueID(), dataArray3);
                }
            }
        }
        dataArray = this.data.getFieldEnemyServantList();
        for (int j = 0; j < dataArray.Length; j++)
        {
            if (dataArray[j].isBuffProgressFlg)
            {
                BattleBuffData.BuffData[] dataArray4 = dataArray[j].turnBuffProgressing(null);
                if ((dataArray[j].isAlive() && dataArray[j].provisionalDamage(0)) && (0 < dataArray4.Length))
                {
                    dictionary.Add(dataArray[j].getUniqueID(), dataArray4);
                }
            }
        }
        foreach (KeyValuePair<int, BattleBuffData.BuffData[]> pair in dictionary2)
        {
            foreach (BattleBuffData.BuffData data2 in pair.Value)
            {
                BuffEntity entity = master.getEntityFromId<BuffEntity>(data2.buffId);
                BattleSkillInfoData skillInfo = new BattleSkillInfoData {
                    svtUniqueId = pair.Key,
                    skillId = data2.vals[0],
                    skilllv = data2.vals[1]
                };
                BattleServantData data4 = this.data.getServantData(skillInfo.svtUniqueId);
                SkillLvEntity entity2 = master2.getEntityFromId<SkillLvEntity>(skillInfo.skillId, skillInfo.skilllv);
                if (this.data.checkAliveOther(data4.getUniqueID()) || this.logic.checkPtTargetFunction(entity2.funcId))
                {
                    int num4 = data4.getRevengeTargetUniqueId();
                    int[] enemyTargetList = null;
                    if (num4 < 0)
                    {
                        enemyTargetList = Target.getTargetIds(this.data, skillInfo.svtUniqueId, -1, pair.Key, Target.TYPE.ENEMY_RANDOM);
                    }
                    else
                    {
                        enemyTargetList = new int[] { num4 };
                    }
                    int[] ptTargetList = new int[] { pair.Key };
                    BattleLogicTask[] tasklist = this.logicskill.taskSkill(skillInfo, ptTargetList, enemyTargetList);
                    this.logic.addBattleLogicTask(tasklist);
                }
            }
        }
        foreach (KeyValuePair<int, BattleBuffData.BuffData[]> pair2 in dictionary)
        {
            foreach (BattleBuffData.BuffData data5 in pair2.Value)
            {
                if (master.getEntityFromId<BuffEntity>(data5.buffId).isEndAct())
                {
                    BattleSkillInfoData data6 = new BattleSkillInfoData {
                        svtUniqueId = pair2.Key,
                        skillId = data5.vals[0],
                        skilllv = data5.vals[1]
                    };
                    BattleServantData data7 = this.data.getServantData(data6.svtUniqueId);
                    SkillLvEntity entity4 = master2.getEntityFromId<SkillLvEntity>(data6.skillId, data6.skilllv);
                    if (this.data.checkAliveOther(data7.getUniqueID()) || this.logic.checkPtTargetFunction(entity4.funcId))
                    {
                        int num6 = data7.getRevengeTargetUniqueId();
                        int[] numArray2 = null;
                        if (num6 < 0)
                        {
                            numArray2 = Target.getTargetIds(this.data, data6.svtUniqueId, -1, pair2.Key, Target.TYPE.ENEMY_RANDOM);
                        }
                        else
                        {
                            numArray2 = new int[] { num6 };
                        }
                        int[] numArray5 = new int[] { pair2.Key };
                        BattleLogicTask[] taskArray2 = this.logicskill.taskSkill(data6, numArray5, numArray2);
                        this.logic.addBattleLogicTask(taskArray2);
                    }
                }
            }
        }
        if (actiondata.addCriticalStars > 0)
        {
            SoundManager.playSe("ba12");
        }
        if (flag)
        {
            return actiondata;
        }
        return null;
    }

    public BattleActionData createPlayMotion(BattleLogicTask task)
    {
        BattleActionData data = new BattleActionData();
        data.setStateMotion();
        data.actorId = task.getActorId();
        data.motionname = task.motionName;
        data.motionMessage = task.motionMessage;
        data.targetObject = task.targetObject;
        return data;
    }

    public BattleActionData createStartTurn(BattleLogicTask task)
    {
        foreach (BattleServantData data in this.data.getFieldServantList())
        {
            data.isBuffProgressFlg = true;
        }
        return null;
    }

    public int getCountSubmember(BattleServantData[] svtList)
    {
        int num = 0;
        for (int i = 0; i < svtList.Length; i++)
        {
            BattleServantData data = svtList[i];
            if (((data != null) && data.isAlive()) && (!data.isEntry || data.isWaitRepop))
            {
                num++;
            }
        }
        return num;
    }

    public BattleLogicTask[] taskAddCommandAttack(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask task = null;
        BattleCommandData command = null;
        Debug.Log(">>LOGICTYPE.COMMAND_ADDATTACK");
        BattleComboData data3 = data.getSelectCombo();
        if (3 <= data3.samecount)
        {
            command = new BattleCommandData(data.getSelectCommand(2));
            BattleServantData data4 = this.data.getServantData(command.getUniqueId());
            if (!data4.isAlive())
            {
                return list.ToArray();
            }
            if (!data4.isAction())
            {
                return list.ToArray();
            }
            task = new BattleLogicTask();
            task.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, command.getUniqueId());
            this.logictarget.getTargetBattleServantData(task);
            command.setTypeAddAttack();
            task.setAddAttackCommand(data.getSelectCombo(), command);
            list.Add(task);
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskBuffAdd(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = null;
        if (ltype == BattleLogic.LOGICTYPE.BUFF_ADDPARAM_PLAYER)
        {
            item = new BattleLogicTask();
            item.setBuffAddPlayer();
        }
        else if (ltype == BattleLogic.LOGICTYPE.BUFF_ADDPARAM_ENEMY)
        {
            item = new BattleLogicTask();
            item.setBuffAddEnemy();
        }
        if (item != null)
        {
            list.Add(item);
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskComboOrderAfter(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        this.logic.resetOverKill();
        BattleLogicTask item = new BattleLogicTask();
        item.setSystem();
        list.Add(item);
        return list.ToArray();
    }

    public BattleLogicTask[] taskComboOrderBefore(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        return list.ToArray();
    }

    public BattleLogicTask[] taskCommandAttack(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask task = null;
        BattleCommandData command = null;
        int index = 0;
        if (ltype == BattleLogic.LOGICTYPE.COMMAND_ATTACK_1)
        {
            index = 0;
        }
        else if (ltype == BattleLogic.LOGICTYPE.COMMAND_ATTACK_2)
        {
            index = 1;
        }
        else if (ltype == BattleLogic.LOGICTYPE.COMMAND_ATTACK_3)
        {
            index = 2;
        }
        command = data.getSelectCommand(index);
        if (!command.isBlank())
        {
            int uniqueId = command.getUniqueId();
            BattleServantData data3 = this.data.getServantData(uniqueId);
            if (!data3.isAlive())
            {
                return list.ToArray();
            }
            if (!data3.isAction())
            {
                return list.ToArray();
            }
            command.checkCriticalRate(BattleRandom.getNext(0x3e8));
            if (data.isTutorial())
            {
                command.checkCriticalRate(0);
            }
            if (index == 0)
            {
                data.setFirstBonus(index, command.type);
            }
            task = new BattleLogicTask();
            task.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, command.getUniqueId());
            if (0 < command.treasureDvc)
            {
                this.logic.resetOverKill();
                task.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, data3.getUniqueID());
                this.logictarget.getTargetBattleServantData(task);
                task.setActionCommand(data.getSelectCombo(), command, index);
                task.setActionTreasureDvc(data3.getTreasureDvcId());
                list.Add(task);
                data.setTDChain(1);
                return list.ToArray();
            }
            data.setTDChain(0);
            this.logictarget.getTargetBattleServantData(task);
            task.setActionCommand(data.getSelectCombo(), command, index);
            list.Add(task);
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskPlayMotion(BattleServantData svtData, string motionName, Transform Tr)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = new BattleLogicTask();
        item.setPlayMoiton(motionName);
        item.setTargetObject(Tr.gameObject);
        if (!svtData.isEnemy)
        {
            item.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, svtData.getUniqueID());
        }
        else
        {
            item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, svtData.getUniqueID());
        }
        list.Add(item);
        return list.ToArray();
    }

    public BattleLogicTask[] taskPlaySubEntryMotion(BattleServantData svtData, Transform Tr)
    {
        string motionName = "MOTION_ENTRY";
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = new BattleLogicTask();
        item.setPlayMoiton(motionName);
        item.setTargetObject(Tr.gameObject);
        if (!svtData.isEnemy)
        {
            string message = string.Format(LocalizationManager.Get("BATTLE_SUBENTRY_PLAYER"), this.getCountSubmember(this.data.getPlayerServantList()));
            item.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, svtData.getUniqueID());
            item.setMessage(message, BattleLogicTask.MESSAGE_TYPE.SUB_ENTRY);
        }
        else
        {
            BattleEntity entity = this.data.getBattleEntity();
            int questId = entity.questId;
            int questPhase = entity.questPhase;
            int wavecount = this.data.wavecount;
            StageEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.STAGE).getEntityFromId<StageEntity>(questId, questPhase, wavecount + 1);
            string str3 = null;
            if (entity2.isNotDisplayRemain())
            {
                str3 = LocalizationManager.Get("BATTLE_SUBENTRY_ENEMY_UNKNOWN");
            }
            else
            {
                str3 = string.Format(LocalizationManager.Get("BATTLE_SUBENTRY_ENEMY"), this.getCountSubmember(this.data.getEnemyServantList()));
            }
            item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, svtData.getUniqueID());
            item.setMessage(str3, BattleLogicTask.MESSAGE_TYPE.SUB_ENTRY);
        }
        list.Add(item);
        return list.ToArray();
    }
}

