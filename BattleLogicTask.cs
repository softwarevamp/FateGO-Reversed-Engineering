using System;
using UnityEngine;

public class BattleLogicTask
{
    public ACTIONTYPE actiontype;
    public int[] actorIdlist;
    public ACTORTYPE actortype;
    public BattleComboData combo;
    public BattleCommandData command;
    public bool grandoderflg;
    public MESSAGE_TYPE messageType;
    public string motionMessage;
    public string motionName;
    public int ordertype;
    public int[] ptTarget;
    public BattleSkillInfoData skillInfo;
    public int status;
    public float systemTime;
    public int[] targetIdlist = new int[0];
    public GameObject targetObject;
    public int treasureDvcId;

    public bool checkActorId(BattleLogicTask task)
    {
        if (task == null)
        {
            return false;
        }
        return (this.getActorId() == task.getActorId());
    }

    public bool checkTargetId(BattleLogicTask task)
    {
        if (task == null)
        {
            return false;
        }
        if (this.targetIdlist.Length <= 0)
        {
            return false;
        }
        int[] numArray = task.getTargetlist();
        if (numArray.Length <= 0)
        {
            return false;
        }
        return (this.targetIdlist[0] == numArray[0]);
    }

    public int getActorId()
    {
        if (this.actorIdlist == null)
        {
            return -1;
        }
        return this.actorIdlist[0];
    }

    public BattleComboData getCombo() => 
        this.combo;

    public BattleCommandData getCommand() => 
        this.command;

    public int getTarget() => 
        this.targetIdlist[0];

    public int[] getTargetlist() => 
        this.targetIdlist;

    public bool isAddAttack() => 
        (ACTIONTYPE.ADDATTACK == this.actiontype);

    public bool isArts() => 
        BattleCommand.isARTS(this.ordertype);

    public bool isBackStep() => 
        (ACTIONTYPE.BACKSTEP == this.actiontype);

    public bool isBuster() => 
        BattleCommand.isBUSTER(this.ordertype);

    public bool isCheck(ACTIONTYPE ckType) => 
        (ckType == this.actiontype);

    public bool isCheckEntryFunction() => 
        (ACTIONTYPE.FUNCTIONCHECK_ENTRY == this.actiontype);

    public bool isComboOrder() => 
        (ACTIONTYPE.COMBO_ORDER == this.actiontype);

    public bool isCommandAction() => 
        (ACTIONTYPE.COMMAND_BATTLE == this.actiontype);

    public bool isCommandSpell() => 
        (ACTIONTYPE.COMMAND_SPELL == this.actiontype);

    public bool isDead() => 
        (ACTIONTYPE.DEAD == this.actiontype);

    public bool isEnemyLogicDead() => 
        (ACTIONTYPE.ENEMYLOGIC_ENEMYDEAD == this.actiontype);

    public bool isEnemyLogicEndTurn() => 
        (ACTIONTYPE.ENEMYLOGIC_ENDTURN == this.actiontype);

    public bool isEnemyLogicPlayerActionEnd() => 
        (ACTIONTYPE.ENEMYLOGIC_PLAYERACTIONEND == this.actiontype);

    public bool isEnemyLogicStartTurn() => 
        (ACTIONTYPE.ENEMYLOGIC_STARTTURN == this.actiontype);

    public bool isGrandOrder() => 
        this.grandoderflg;

    public bool isPlayMotion() => 
        (ACTIONTYPE.PLAY_MOTION == this.actiontype);

    public bool isProcBuffDead() => 
        (ACTIONTYPE.PROC_BUFFDEAD == this.actiontype);

    public bool isQuick() => 
        BattleCommand.isQUICK(this.ordertype);

    public bool isReservationSkill() => 
        (ACTIONTYPE.RESERVATION_SKILL == this.actiontype);

    public bool isResurrection() => 
        (ACTIONTYPE.RESURRECTION == this.actiontype);

    public bool isSKill() => 
        (ACTIONTYPE.SKILL == this.actiontype);

    public bool isSystem() => 
        (ACTIONTYPE.SYSTEM == this.actiontype);

    public bool isTreasureDvc() => 
        (ACTIONTYPE.TREASURE_DEVICE == this.actiontype);

    public bool isTurnEndEnemy() => 
        (ACTIONTYPE.ENDTURN_ENEMY == this.actiontype);

    public bool isTurnEndPlayer() => 
        (ACTIONTYPE.ENDTURN_PLAYER == this.actiontype);

    public void setActionCommand(BattleComboData combo, BattleCommandData command, int index)
    {
        this.actiontype = ACTIONTYPE.COMMAND_BATTLE;
        this.combo = combo;
        this.command = command;
        this.command.setCombo(combo, index);
    }

    public void setActionSkill(BattleSkillInfoData skillInfo, int[] targetlist, int[] ptTarget)
    {
        this.actiontype = ACTIONTYPE.SKILL;
        this.skillInfo = skillInfo;
        this.targetIdlist = targetlist;
        this.ptTarget = ptTarget;
    }

    public void setActionTreasureDvc(int treDvcId)
    {
        this.actiontype = ACTIONTYPE.TREASURE_DEVICE;
        this.treasureDvcId = treDvcId;
    }

    public void setActor(ACTORTYPE type, int uniqueID)
    {
        int[] uniqueIDList = new int[] { uniqueID };
        this.setActor(type, uniqueIDList);
    }

    public void setActor(ACTORTYPE type, int[] uniqueIDList)
    {
        this.actortype = type;
        this.actorIdlist = uniqueIDList;
    }

    public void setAddAttackCommand(BattleComboData combo, BattleCommandData command)
    {
        this.actiontype = ACTIONTYPE.ADDATTACK;
        this.combo = combo;
        this.command = command;
        this.command.setCombo(combo, 2);
        this.command.ActionIndex = 3;
    }

    public void setBackStep()
    {
        this.actiontype = ACTIONTYPE.BACKSTEP;
    }

    public void setBuffAddEnemy()
    {
        this.actiontype = ACTIONTYPE.BUFF_ADD_ENEMY;
    }

    public void setBuffAddPlayer()
    {
        this.actiontype = ACTIONTYPE.BUFF_ADD_PLAYER;
    }

    public void setCheckEntryFunction()
    {
        this.actiontype = ACTIONTYPE.FUNCTIONCHECK_ENTRY;
    }

    public void setComboOrder(BattleCommand.TYPE type, bool grdflg)
    {
        this.setComboOrder((int) type, grdflg);
    }

    public void setComboOrder(int type, bool grdflg)
    {
        this.actiontype = ACTIONTYPE.COMBO_ORDER;
        this.ordertype = type;
        this.grandoderflg = grdflg;
    }

    public void setCommandSpell(int skillId, int[] targetlist, int[] ptTarget)
    {
        this.actiontype = ACTIONTYPE.COMMAND_SPELL;
        this.skillInfo = new BattleSkillInfoData();
        this.skillInfo.type = BattleSkillInfoData.TYPE.MASTER_COMMAND;
        this.skillInfo.skillId = skillId;
        this.skillInfo.skilllv = 1;
        this.targetIdlist = targetlist;
        this.ptTarget = ptTarget;
    }

    public void setDead()
    {
        this.actiontype = ACTIONTYPE.DEAD;
    }

    public void setEndTurnEnemy()
    {
        this.actiontype = ACTIONTYPE.ENDTURN_ENEMY;
    }

    public void setEndTurnPlayer()
    {
        this.actiontype = ACTIONTYPE.ENDTURN_PLAYER;
    }

    public void setEnemyLogicDead()
    {
        this.actiontype = ACTIONTYPE.ENEMYLOGIC_ENEMYDEAD;
    }

    public void setEnemyLogicEndTurn()
    {
        this.actiontype = ACTIONTYPE.ENEMYLOGIC_ENDTURN;
    }

    public void setEnemyLogicPlayerActionEnd()
    {
        this.actiontype = ACTIONTYPE.ENEMYLOGIC_PLAYERACTIONEND;
    }

    public void setEnemyLogicStartTurn()
    {
        this.actiontype = ACTIONTYPE.ENEMYLOGIC_STARTTURN;
    }

    public void setMessage(string message, MESSAGE_TYPE type)
    {
        this.motionMessage = message;
        this.messageType = type;
    }

    public void setPlayMoiton(string motionName)
    {
        this.actiontype = ACTIONTYPE.PLAY_MOTION;
        this.motionName = motionName;
    }

    public void setProcBuffDead()
    {
        this.actiontype = ACTIONTYPE.PROC_BUFFDEAD;
    }

    public void setReservationSkill(BattleSkillInfoData inSkillInfo)
    {
        this.actiontype = ACTIONTYPE.RESERVATION_SKILL;
        this.skillInfo = inSkillInfo;
    }

    public void setResurrection()
    {
        this.actiontype = ACTIONTYPE.RESURRECTION;
    }

    public void setStartTurnEnemy()
    {
        this.actiontype = ACTIONTYPE.STARTTURN_ENEMY;
    }

    public void setStartTurnPlayer()
    {
        this.actiontype = ACTIONTYPE.STARTTURN_PLAYER;
    }

    public void setSystem()
    {
        this.actiontype = ACTIONTYPE.SYSTEM;
    }

    public void setTarget(int targetId)
    {
        int[] targetIdlist = new int[] { targetId };
        this.setTarget(targetIdlist);
    }

    public void setTarget(int[] targetIdlist)
    {
        this.targetIdlist = targetIdlist;
    }

    public void setTargetObject(GameObject obj)
    {
        this.targetObject = obj;
    }

    public enum ACTIONTYPE
    {
        NONE,
        COMMAND_BATTLE,
        ADDATTACK,
        SKILL,
        TREASURE_DEVICE,
        BACKSTEP,
        SYSTEM,
        COMBO_ORDER,
        COMMAND_SPELL,
        PLAY_MOTION,
        ENDTURN_PLAYER,
        ENDTURN_ENEMY,
        RESURRECTION,
        DEAD,
        PROC_BUFFDEAD,
        ENEMYLOGIC_ENEMYDEAD,
        ENEMYLOGIC_PLAYERACTIONEND,
        BUFF_ADD_PLAYER,
        BUFF_ADD_ENEMY,
        STARTTURN_PLAYER,
        STARTTURN_ENEMY,
        ENEMYLOGIC_STARTTURN,
        ENEMYLOGIC_ENDTURN,
        FUNCTIONCHECK_ENTRY,
        RESERVATION_SKILL
    }

    public enum ACTORTYPE
    {
        NONE,
        PLAYER_MASTER,
        PLAYER_SERVANT,
        ENEMY_SERVANT,
        COMMAND
    }

    public enum MESSAGE_TYPE
    {
        NONE,
        SUB_ENTRY
    }
}

