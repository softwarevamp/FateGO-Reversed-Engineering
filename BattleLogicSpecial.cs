using System;
using System.Collections.Generic;

public class BattleLogicSpecial
{
    public BattleData data;
    public BattleLogic logic;
    public BattleLogicFunction logicfunction;
    public BattleLogicTarget logictarget;

    public BattleActionData createSpecialData(BattleLogicTask task)
    {
        Debug.Log("createSpecialData >>>");
        BattleServantData data = this.data.getServantData(task.getActorId());
        BattleActionData action = new BattleActionData();
        int targetId = 0;
        action.actorId = task.getActorId();
        BattleCommandData command = task.getCommand();
        targetId = this.logictarget.getTargetBase(task.getTargetlist());
        action.targetId = targetId;
        if (targetId <= 0)
        {
            Debug.Log(" no target");
            return null;
        }
        TreasureDvcLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcLvMaster>(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(data.getTreasureDvcId(), data.getTreasureDvcLevel());
        data.usedTpWeapon(command.getChainBonus());
        action = this.logicfunction.procList(action, entity.funcId, entity.getDataValsList(data.getNpPer()), false);
        data.checkRegainNPUsedNoble();
        data.buffData.usedProgressing();
        data.tmpNp = data.np;
        data.changeNp(0, false);
        action.setTargetId(targetId);
        action.setTypeTA();
        action.setCommand(command);
        action.motionId = data.getTreasureDvcMotionId();
        action.prevattackme = false;
        this.data.setCommandAttack(0, 0);
        return action;
    }

    public BattleLogicTask[] taskEnemyTresureDvc(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = null;
        int index = 0;
        if (ltype == BattleLogic.LOGICTYPE.ENEMY_SPECIAL_1)
        {
            index = 0;
        }
        else if (ltype == BattleLogic.LOGICTYPE.ENEMY_SPECIAL_2)
        {
            index = 1;
        }
        else if (ltype == BattleLogic.LOGICTYPE.ENEMY_SPECIAL_3)
        {
            index = 2;
        }
        if (0 < data.e_entryid[index])
        {
            BattleServantData data2 = data.getEnemyServantData(data.e_entryid[index]);
            if (data2.checkReadySpecail())
            {
                item = new BattleLogicTask();
                item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, data2.getUniqueID());
                item.setTarget(data.getFieldPlayerServantIDList());
                BattleCommandData command = new BattleCommandData {
                    type = data2.getTreasureDvcCardId(),
                    svtlimit = data2.getDispLimitCount(),
                    uniqueId = data2.getUniqueID(),
                    svtId = data2.getSvtId(),
                    treasureDvc = data2.getTreasureDvcId()
                };
                item.setActionCommand(new BattleComboData(), command, index);
                item.setActionTreasureDvc(data2.getTreasureDvcId());
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskEnemyTresureDvcAlways(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = null;
        Debug.Log(">>LOGICTYPE.ENEMY_SP1_ALWAYS");
        int index = 0;
        if (ltype == BattleLogic.LOGICTYPE.ENEMY_SP1_ALWAYS)
        {
            index = 0;
        }
        else if (ltype == BattleLogic.LOGICTYPE.ENEMY_SP2_ALWAYS)
        {
            index = 1;
        }
        else if (ltype == BattleLogic.LOGICTYPE.ENEMY_SP3_ALWAYS)
        {
            index = 2;
        }
        if (0 < data.e_entryid[index])
        {
            BattleServantData data2 = data.getEnemyServantData(data.e_entryid[index]);
            if (data2.getTreasureDvcId() > 0)
            {
                item = new BattleLogicTask();
                item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, data2.getUniqueID());
                item.setTarget(data.getFieldPlayerServantIDList());
                BattleCommandData command = new BattleCommandData {
                    type = data2.getTreasureDvcCardId(),
                    svtlimit = data2.getDispLimitCount(),
                    uniqueId = data2.getUniqueID(),
                    svtId = data2.getSvtId(),
                    treasureDvc = data2.getTreasureDvcId()
                };
                item.setActionCommand(new BattleComboData(), command, index);
                item.setActionTreasureDvc(data2.getTreasureDvcId());
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskTresureDvc(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = null;
        int index = 0;
        if (ltype == BattleLogic.LOGICTYPE.PLAYER_SPECIAL_1)
        {
            index = 0;
        }
        else if (ltype == BattleLogic.LOGICTYPE.PLAYER_SPECIAL_2)
        {
            index = 1;
        }
        else if (ltype == BattleLogic.LOGICTYPE.PLAYER_SPECIAL_3)
        {
            index = 2;
        }
        BattleServantData data2 = data.getPlayerServantData(data.p_entryid[index]);
        if (data2 == null)
        {
            Debug.Log(string.Concat(new object[] { "No servant data.p_entryid [", index, "] = ", data.p_entryid[index] }));
            return list.ToArray();
        }
        if (data2.checkReadySpecail())
        {
            item = new BattleLogicTask();
            item.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, data2.getUniqueID());
            item.setTarget(data.getFieldEnemyServantIDList());
            BattleCommandData command = new BattleCommandData {
                type = data2.getTreasureDvcCardId(),
                svtlimit = data2.getCommandDispLimitCount(),
                uniqueId = data2.getUniqueID(),
                svtId = data2.getSvtId(),
                treasureDvc = data2.getTreasureDvcId()
            };
            item.setActionCommand(new BattleComboData(), command, index);
            item.setActionTreasureDvc(data2.getTreasureDvcId());
            list.Add(item);
        }
        return list.ToArray();
    }

    public BattleLogicTask[] taskTresureDvcAlways(BattleLogic.LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask task = null;
        Debug.Log(">>LOGICTYPE.PLAYER_SPn_ALWAYS");
        int index = 0;
        if (ltype == BattleLogic.LOGICTYPE.PLAYER_SPECIAL_1)
        {
            index = 0;
        }
        else if (ltype == BattleLogic.LOGICTYPE.PLAYER_SPECIAL_2)
        {
            index = 1;
        }
        else if (ltype == BattleLogic.LOGICTYPE.PLAYER_SPECIAL_3)
        {
            index = 2;
        }
        BattleServantData data2 = data.getPlayerServantData(data.p_entryid[index]);
        task = new BattleLogicTask();
        task.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, data2.getUniqueID());
        this.logictarget.getTargetBattleServantData(task);
        BattleCommandData command = new BattleCommandData {
            type = data2.getTreasureDvcCardId(),
            svtlimit = data2.getCommandDispLimitCount(),
            uniqueId = data2.getUniqueID(),
            svtId = data2.getSvtId(),
            treasureDvc = data2.getTreasureDvcId()
        };
        task.setActionCommand(new BattleComboData(), command, index);
        task.setActionTreasureDvc(data2.getTreasureDvcId());
        list.Add(task);
        return list.ToArray();
    }
}

