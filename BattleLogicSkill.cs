using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class BattleLogicSkill
{
    public BattleData data;
    public BattleLogic logic;
    public BattleLogicFunction logicfunction;
    public BattleLogicTarget logictarget;

    public void actPassiveSkill(BattleSkillInfoData skillInfo)
    {
        int svtUniqueId = skillInfo.svtUniqueId;
        int skillId = skillInfo.skillId;
        int skilllv = skillInfo.skilllv;
        BattleActionData action = new BattleActionData();
        SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(skillId);
        if ((entity != null) && !entity.isActive())
        {
            SkillLvEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(skillId, skilllv);
            action.actorId = svtUniqueId;
            action.motionId = entity.motion;
            this.logicfunction.procList(action, entity2.funcId, entity2.getDataValsList(), true);
        }
    }

    public BattleActionData createCommandSpell(BattleLogicTask task)
    {
        Debug.Log("BattleLogicSkill::createCommandSpell >> ");
        BattleActionData action = new BattleActionData();
        BattleSkillInfoData skillInfo = task.skillInfo;
        CommandSpellEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CommandSpellMaster>(DataNameKind.Kind.COMMAND_SPELL).getEntityFromId<CommandSpellEntity>(skillInfo.skillId);
        action.setStateField();
        action.actorId = -1;
        action.motionId = entity.motion;
        action.setPTTargetId(task.ptTarget);
        return this.logicfunction.procList(action, entity.funcId, entity.getDataValsList(), false);
    }

    public BattleActionData createSkillData(BattleLogicTask task)
    {
        Debug.Log(">>>>> BattleLogicSkill::createSkillData >> ");
        BattleActionData action = new BattleActionData();
        BattleSkillInfoData skillInfo = task.skillInfo;
        SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(skillInfo.skillId);
        SkillLvEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(skillInfo.skillId, skillInfo.skilllv);
        action.type = BattleActionData.TYPE_SKILL;
        action.actorId = task.getActorId();
        action.motionId = entity.motion;
        action.targetId = task.getTarget();
        action.setPTTargetId(task.ptTarget);
        action.skillMessage = entity.getName();
        if ((task.motionMessage != null) && (task.motionMessage.Length > 0))
        {
            action.motionMessage = task.motionMessage;
        }
        else
        {
            action.motionMessage = null;
        }
        if (task.actortype == BattleLogicTask.ACTORTYPE.PLAYER_MASTER)
        {
            action.setStateField();
        }
        action.setEffect(entity.getEffectList());
        action = this.logicfunction.procList(action, entity2.funcId, entity2.getDataValsList(), false);
        action.prevattackme = false;
        this.data.setCommandAttack(0, 0);
        Debug.Log("<< BattleLogicSkill::createSkillData");
        return action;
    }

    public BattleLogicTask[] taskCommandSpell(int skillId, int skillLv, int[] ptTargetList)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = null;
        Debug.Log(">>taskCommandSpell");
        item = new BattleLogicTask();
        item.setCommandSpell(skillId, null, ptTargetList);
        list.Add(item);
        return list.ToArray();
    }

    public BattleLogicTask[] taskSkill(BattleSkillInfoData skillInfo, int[] ptTargetList, int[] enemyTargetList = null)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = null;
        Debug.Log(">>LOGICTYPE.SKILL");
        item = new BattleLogicTask();
        if (enemyTargetList != null)
        {
            item.setActionSkill(skillInfo, enemyTargetList, ptTargetList);
        }
        else
        {
            int num = this.logictarget.getGlobalTargetId();
            int[] targetlist = new int[] { num };
            item.setActionSkill(skillInfo, targetlist, ptTargetList);
        }
        if (skillInfo.svtUniqueId == -1)
        {
            item.setActor(BattleLogicTask.ACTORTYPE.PLAYER_MASTER, skillInfo.svtUniqueId);
            list.Add(item);
        }
        else if (this.data.isPlayerID(skillInfo.svtUniqueId))
        {
            item.setActor(BattleLogicTask.ACTORTYPE.PLAYER_SERVANT, skillInfo.svtUniqueId);
            list.Add(item);
        }
        else
        {
            item.setActor(BattleLogicTask.ACTORTYPE.ENEMY_SERVANT, skillInfo.svtUniqueId);
            list.Add(item);
        }
        return list.ToArray();
    }
}

