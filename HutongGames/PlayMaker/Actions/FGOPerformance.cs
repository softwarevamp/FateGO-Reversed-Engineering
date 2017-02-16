namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOPerformance : FsmStateAction
    {
        public Effect effect;
        [CheckForComponent(typeof(BattlePerformance)), RequiredField]
        public FsmGameObject gameObject;
        public FsmEvent sendEvent;

        public override void OnEnter()
        {
            GameObject obj2 = this.gameObject.Value;
            if (obj2 != null)
            {
                BattlePerformance component = obj2.GetComponent<BattlePerformance>();
                if (this.effect == Effect.QUEST_START)
                {
                    component.effectStartQuest();
                }
                else if (this.effect == Effect.BATTLE_START)
                {
                    component.effectStartBattle();
                }
                else if (this.effect == Effect.END_SELECTCOMMAND)
                {
                    component.endSelectCommand();
                }
                else if (this.effect == Effect.MOVE_TACTICAL)
                {
                    component.movePositionToTactical();
                }
                else if (this.effect == Effect.MOVE_ACTION)
                {
                    component.movePositionToBattle();
                }
                else if (this.effect == Effect.BATTLE_WIN)
                {
                    component.playWinMotion();
                }
                else if (this.effect == Effect.BATTLE_LOSE)
                {
                    component.playLoseMotion();
                }
                else if (this.effect == Effect.MOVE_NEXTBATTLE)
                {
                    component.moveNextBattle("END_PROC");
                }
                else if (this.effect == Effect.QUEST_OVER)
                {
                    component.effectOverQuest();
                }
            }
            base.Finish();
        }

        public enum Effect
        {
            QUEST_START,
            QUEST_CLEAR,
            QUEST_OVER,
            BATTLE_START,
            BATTLE_WIN,
            BATTLE_LOSE,
            ACTION_BATTLE,
            END_SELECTCOMMAND,
            ACTION_ABILITY,
            MOVE_TACTICAL,
            MOVE_COMMAND,
            MOVE_ACTION,
            MOVE_NEXTBATTLE,
            PROC_BUFF
        }
    }
}

