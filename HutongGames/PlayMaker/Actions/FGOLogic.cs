namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOLogic : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleLogic))]
        public FsmGameObject gameObject;
        public PROC proc;
        public FsmEvent sendEvent;

        public override void OnEnter()
        {
            GameObject obj2 = this.gameObject.Value;
            string endproc = string.Empty;
            if (this.sendEvent != null)
            {
                endproc = this.sendEvent.Name;
            }
            if (obj2 != null)
            {
                BattleLogic component = obj2.GetComponent<BattleLogic>();
                if (this.proc == PROC.INIT_QUEST)
                {
                    component.initQuest(endproc);
                }
                else if (this.proc == PROC.CHECK_COMBO)
                {
                    Debug.Log("err- PROC.CHECK_COMBO");
                }
                else if (this.proc == PROC.INIT_BATTLETURN)
                {
                    component.initCommandBattle(endproc);
                }
                else if (this.proc == PROC.START_BATTLETURN)
                {
                    component.actBattleTask();
                }
                else if (this.proc == PROC.SELECT_TACTICAL)
                {
                    component.selectTactical(endproc);
                }
                else if (this.proc == PROC.END_BATTLETURN)
                {
                    component.finishActionBattle(endproc);
                }
                else if (this.proc == PROC.CHECK_ENDBATTLE)
                {
                    component.checkEndBattle(endproc);
                }
                else if (this.proc == PROC.TURN_PROGRSSING)
                {
                    component.turnProgressing(endproc);
                }
                else if (this.proc == PROC.LOAD_N_STAGE)
                {
                    component.loadNstage(endproc);
                }
                else if (this.proc == PROC.LOAD_NEXT)
                {
                    component.loadNextstage(endproc);
                }
                else if (this.proc != PROC.ENTRY_SUBMEMBER)
                {
                    if (this.proc == PROC.CHECK_NEXTBATTLE)
                    {
                        component.checkNextBattle(endproc);
                    }
                    else if (this.proc == PROC.SET_NEXTBATTLE)
                    {
                        component.setNextBattle(endproc);
                    }
                    else if (this.proc == PROC.PROC_WIN)
                    {
                        component.procWin();
                    }
                    else if (this.proc == PROC.PROC_LOSE)
                    {
                        component.procLose();
                    }
                    else if (this.proc == PROC.END_BATTLE)
                    {
                        component.procEndBattle();
                    }
                    else if (this.proc == PROC.CONNECT_BATTLEEND)
                    {
                        component.StartResultRequest(endproc);
                    }
                    else if (this.proc == PROC.SHOW_RESULT)
                    {
                        component.StartShowResult(endproc);
                    }
                    else if (this.proc == PROC.START_CONTINUE)
                    {
                        component.startContinue();
                    }
                    else if (this.proc == PROC.RECOVERPT)
                    {
                        component.startRecoverPT();
                    }
                    else if (this.proc == PROC.START_COMMAND)
                    {
                        component.startCommand(endproc);
                    }
                    else if (this.proc == PROC.ENTRY_CHECK)
                    {
                        component.checkEntryMember(endproc);
                    }
                    else if (this.proc == PROC.ENTRY_WAIT)
                    {
                        component.startEntryMember(endproc);
                    }
                }
            }
            base.Finish();
        }

        public enum PROC
        {
            INIT_QUEST,
            DRAW_COMMAND,
            CHECK_COMBO,
            INIT_BATTLETURN,
            SELECT_TACTICAL,
            END_BATTLETURN,
            CHECK_ENDBATTLE,
            TURN_PROGRSSING,
            CHECK_BATTLEEND,
            TEST_INITBATTLETURN,
            LOAD_N_STAGE,
            ENTRY_SUBMEMBER,
            CHECK_NEXTBATTLE,
            SET_NEXTBATTLE,
            START_BATTLETURN,
            PROC_WIN,
            PROC_LOSE,
            END_BATTLE,
            CONNECT_BATTLEEND,
            SHOW_RESULT,
            LOAD_NEXT,
            START_CONTINUE,
            RECOVERPT,
            START_COMMAND,
            ENTRY_CHECK,
            ENTRY_WAIT
        }
    }
}

