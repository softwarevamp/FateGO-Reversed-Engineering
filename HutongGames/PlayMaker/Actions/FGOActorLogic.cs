namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorLogic : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public LOGIC logic;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (this.logic == LOGIC.END_FINISH)
                {
                    component.finishMotion();
                }
                else if (this.logic == LOGIC.ON_SKIP_VOICE)
                {
                    component.onTouchEvent();
                }
                else if (this.logic == LOGIC.OFF_SKIP_VOICE)
                {
                    component.offTouchEvent();
                }
                else if (this.logic == LOGIC.DROP_ITEM)
                {
                    component.startDropItem();
                }
            }
            base.Finish();
        }

        public enum LOGIC
        {
            NONE,
            END_FINISH,
            ON_SKIP_VOICE,
            OFF_SKIP_VOICE,
            DROP_ITEM
        }
    }
}

