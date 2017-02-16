using System;
using System.Collections.Generic;

public class BattleLogicReaction
{
    public BattleData data;
    public BattleLogic logic;
    public BattleLogicFunction logicfunction;
    public BattleLogicSkill logicskill;
    public BattlePerformance perf;

    public BattleLogicTask[] checkDead()
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        List<BattleServantData> list2 = new List<BattleServantData>();
        if (this.data.typeTurn == BattleData.TYPETURN.PLAYER)
        {
            list2.AddRange(this.data.getFieldEnemyServantList());
            list2.AddRange(this.data.getFieldPlayerServantList());
        }
        else
        {
            list2.AddRange(this.data.getFieldPlayerServantList());
            list2.AddRange(this.data.getFieldEnemyServantList());
        }
        foreach (BattleServantData data in list2)
        {
            if (((data.status == BattleServantData.STATUS.NOMAL) && !data.isAlive()) && (!data.isOverKill() && !data.isDeadAnimation()))
            {
                data.status = BattleServantData.STATUS.ACT_DEAD;
                BattleLogicTask item = new BattleLogicTask();
                item.setDead();
                if (!data.isEnemy)
                {
                    item.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, data.getUniqueID());
                }
                else
                {
                    item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, data.getUniqueID());
                }
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public BattleLogicTask[] checkEnemyEndTurn(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        foreach (BattleServantData data2 in this.data.getFieldEnemyServantList())
        {
            if ((data2.status == BattleServantData.STATUS.NOMAL) && data2.isAlive())
            {
                BattleLogicTask item = new BattleLogicTask();
                item.setEnemyLogicEndTurn();
                item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, data2.getUniqueID());
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public BattleLogicTask[] checkEnemyStartTurn(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        foreach (BattleServantData data2 in this.data.getFieldEnemyServantList())
        {
            if ((data2.status == BattleServantData.STATUS.NOMAL) && data2.isAlive())
            {
                BattleLogicTask item = new BattleLogicTask();
                item.setEnemyLogicStartTurn();
                item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, data2.getUniqueID());
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public BattleLogicTask[] checkPlayerActionEnd(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        foreach (BattleServantData data2 in this.data.getFieldEnemyServantList())
        {
            if ((data2.status == BattleServantData.STATUS.NOMAL) && data2.isAlive())
            {
                BattleLogicTask item = new BattleLogicTask();
                item.setEnemyLogicPlayerActionEnd();
                item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, data2.getUniqueID());
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public BattleLogicTask[] checkResurrection()
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        foreach (BattleServantData data in this.data.getFieldServantList())
        {
            if (((data.status == BattleServantData.STATUS.NOMAL) && !data.isAlive()) && !data.isOverKill())
            {
                if (!data.isGuts())
                {
                    this.logic.checkUsedGutsBuff(data.getUniqueID());
                }
                else
                {
                    data.status = BattleServantData.STATUS.ACT_RESURRECTION;
                    BattleLogicTask item = new BattleLogicTask();
                    item.setResurrection();
                    if (!data.isEnemy)
                    {
                        item.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, data.getUniqueID());
                    }
                    else
                    {
                        item.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, data.getUniqueID());
                    }
                    list.Add(item);
                }
            }
        }
        return list.ToArray();
    }

    public BattleActionData createDeadMotion(BattleLogicTask task)
    {
        BattleServantData data = this.data.getServantData(task.getActorId());
        if (data.isAlive())
        {
            data.status = BattleServantData.STATUS.NOMAL;
            return null;
        }
        BattleActionData data2 = new BattleActionData();
        this.perf.updateView();
        data.setDeadAnimeFlg(true);
        data2.actorId = data.getUniqueID();
        data2.type = BattleActionData.TYPE_DEAD;
        if (data.isDeadEscape())
        {
            data2.motionname = "MOTION_NO_DEAD";
        }
        else
        {
            data2.motionname = "MOTION_DEAD";
        }
        if (data.getUniqueID() == this.data.globaltargetId)
        {
            this.data.globaltargetId = -1;
        }
        this.perf.statusPerf.updateNokoriEnemyCount();
        int uniqueId = data.getDeadTargetUniqueId();
        if (0 < uniqueId)
        {
            BattleServantData data3 = this.data.getServantData(uniqueId);
            if (((data3 == null) || data3.isDeadAnimation()) || (data.isEnemy == data3.isEnemy))
            {
                return data2;
            }
            BattleActionData action = new BattleActionData {
                actorId = uniqueId,
                targetId = data.getUniqueID()
            };
            BattleServantData data5 = this.data.getServantData(uniqueId);
            BattleBuffData.BuffData[] dataArray = data5.getDeadAttackSideEffect(data.getIndividualities());
            if (0 < dataArray.Length)
            {
                SkillLvMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
                foreach (BattleBuffData.BuffData data6 in dataArray)
                {
                    SkillLvEntity entity = master.getEntityFromId<SkillLvEntity>(data6.vals[0], data6.vals[1]);
                    this.logicfunction.procList(action, entity.funcId, entity.getDataValsList(), false);
                }
                data2.addAction(action);
            }
            data5.buffData.usedProgressing();
        }
        return data2;
    }

    public BattleActionData createResurrection(BattleLogicTask task)
    {
        BattleServantData data = this.data.getServantData(task.getActorId());
        data.hp = 1;
        data.reducedhp = 0;
        BattleActionData data2 = new BattleActionData {
            actorId = data.getUniqueID(),
            type = BattleActionData.TYPE_RESURRECTION,
            motionname = "MOTION_RESURRECTION"
        };
        BattleActionData.BuffData data3 = new BattleActionData.BuffData {
            targetId = data.getUniqueID(),
            functionIndex = 0,
            buffId = 0,
            popLabel = "毅力",
            procType = BattleActionData.BuffData.BuffProcType.UPDATE_HP,
            effectList = new int[0]
        };
        int inhp = data.useGuts();
        data.setHp(inhp);
        this.logic.checkUsedGutsBuff(data.getUniqueID());
        data2.setBuffData(data3);
        data.status = BattleServantData.STATUS.NOMAL;
        return data2;
    }

    public BattleLogicTask[] createTaskDead(int uniqueId)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleServantData data = this.data.getServantData(uniqueId);
        BattleBuffData.BuffData[] dataArray = data.getDeadBufflist();
        BuffMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
        foreach (BattleBuffData.BuffData data2 in dataArray)
        {
            BuffEntity entity = master.getEntityFromId<BuffEntity>(data2.buffId);
            BattleSkillInfoData skillInfo = new BattleSkillInfoData {
                svtUniqueId = data.getUniqueID(),
                skillId = data2.vals[0],
                skilllv = data2.vals[1]
            };
            int num2 = data.getRevengeTargetUniqueId();
            int[] enemyTargetList = null;
            if (num2 < 0)
            {
                enemyTargetList = Target.getTargetIds(this.data, skillInfo.svtUniqueId, -1, skillInfo.svtUniqueId, Target.TYPE.ENEMY_RANDOM);
            }
            else
            {
                enemyTargetList = new int[] { num2 };
            }
            int[] ptTargetList = new int[] { data.getUniqueID() };
            BattleLogicTask[] collection = this.logicskill.taskSkill(skillInfo, ptTargetList, enemyTargetList);
            list.AddRange(collection);
        }
        data.buffData.usedProgressing();
        BattleLogicTask item = new BattleLogicTask();
        item.setProcBuffDead();
        item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, data.getUniqueID());
        list.Add(item);
        if (data.isEnemy)
        {
            BattleLogicTask task2 = new BattleLogicTask();
            task2.setEnemyLogicDead();
            task2.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, data.getUniqueID());
            list.Add(task2);
        }
        return list.ToArray();
    }

    public BattleLogicTask[] createTaskProcBuffDead(int uniqueId)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleServantData data = this.data.getServantData(uniqueId);
        BattleLogicTask item = new BattleLogicTask();
        item.setEnemyLogicDead();
        item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, data.getUniqueID());
        list.Add(item);
        return list.ToArray();
    }
}

