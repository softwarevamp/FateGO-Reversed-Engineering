namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOCheckBattleFaze : FsmStateAction
    {
        public FAZE Faze;
        [RequiredField, CheckForComponent(typeof(BattlePerformance))]
        public FsmGameObject gameObject;
        public FsmEvent sendEvent;

        public override void OnEnter()
        {
            GameObject obj2 = this.gameObject.Value;
            if (obj2 != null)
            {
                BattlePerformance component = obj2.GetComponent<BattlePerformance>();
                if ((this.Faze == FAZE.BATTLE) && component.isPositionBattle())
                {
                    base.Fsm.Event(this.sendEvent);
                }
                else if ((this.Faze == FAZE.TACTICAL) && component.isPositionTactical())
                {
                    base.Fsm.Event(this.sendEvent);
                }
            }
            base.Finish();
        }

        public enum FAZE
        {
            TACTICAL,
            BATTLE
        }
    }
}

