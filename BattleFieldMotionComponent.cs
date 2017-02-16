using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldMotionComponent : BaseMonoBehaviour
{
    private BattleActionData badata;
    public Transform[] battle_enemyTr;
    public Transform[] battle_playerTr;
    private bool flgStep;
    public PlayMakerFSM myFsm;
    private BattlePerformance perf;
    public Transform pop_enemyTr;
    public Transform pop_playerTr;
    private string replaceEndEvent;
    private Queue<BattleActionData.ReplaceMember> replaceMember = new Queue<BattleActionData.ReplaceMember>();
    public Transform[] tactical_playerTr;
    private BattleActionData.ReplaceMember targetReplaceData;

    public bool checkMotionEvent(string name)
    {
        foreach (FsmTransition transition in this.myFsm.FsmGlobalTransitions)
        {
            if (transition.EventName == name)
            {
                return true;
            }
        }
        return false;
    }

    public void endFieldStep()
    {
        this.flgStep = false;
    }

    public void endLoadReplaceActor()
    {
        this.myFsm.SendEvent(this.replaceEndEvent);
    }

    public void finishMotion()
    {
        this.perf.endActionData();
    }

    public Transform getEnemyPopPoint(int index)
    {
        if (index < this.battle_enemyTr.Length)
        {
            return this.battle_enemyTr[index];
        }
        return null;
    }

    public Transform getPlayerPopTr(int index)
    {
        if (index < this.battle_playerTr.Length)
        {
            return this.battle_playerTr[index];
        }
        return null;
    }

    public Transform getPlayerTacticalTr(int index)
    {
        if (index < this.tactical_playerTr.Length)
        {
            return this.tactical_playerTr[index];
        }
        return null;
    }

    public Transform getPopEnemy() => 
        this.pop_enemyTr;

    public Transform getPopPlayer() => 
        this.pop_playerTr;

    public bool isStep() => 
        this.flgStep;

    public void loadReplace(string endEvent)
    {
        this.replaceEndEvent = endEvent;
        this.perf.replaceMember(this.targetReplaceData, new System.Action(this.endLoadReplaceActor));
    }

    public void loadReplaceActor(string endEvent)
    {
        this.targetReplaceData = this.replaceMember.Dequeue();
        GameObject obj2 = this.perf.getServantGameObject(this.targetReplaceData.outUniqeId);
        this.myFsm.Fsm.Variables.FindFsmGameObject("ActorObject").Value = obj2;
        this.myFsm.SendEvent(endEvent);
    }

    public void playBattleActionData(BattleActionData badata)
    {
        this.badata = badata;
        string name = string.Empty;
        if (this.badata.motionname != null)
        {
            name = this.badata.motionname;
        }
        else
        {
            name = "MOTION_" + this.badata.motionId;
        }
        if (!this.checkMotionEvent(name))
        {
            throw new UnityException(" not found " + name + " in fieldFsm ");
        }
        this.myFsm.SendEvent(name);
    }

    public void procBuff()
    {
    }

    public void sendEvent(string eventstr)
    {
        this.myFsm.SendEvent(eventstr);
    }

    public void setPerf(BattlePerformance inperf)
    {
        this.perf = inperf;
        this.flgStep = false;
        this.myFsm.FsmVariables.GetFsmGameObject("Performance").Value = this.perf.gameObject;
    }

    public void startFieldStep()
    {
        this.flgStep = true;
    }

    public void startReplaceActor(string endEvent)
    {
        foreach (BattleActionData.ReplaceMember member in this.badata.getReplaceMember(-1))
        {
            this.replaceMember.Enqueue(member);
        }
        this.myFsm.SendEvent(endEvent);
    }
}

