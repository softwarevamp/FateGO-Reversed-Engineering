namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorCheckCombo : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmEvent FLASH;
        public FsmEvent NOCOMBO;
        public FsmEvent PAIR_FLASH;
        public FsmEvent THREE;
        public FsmEvent THREE_FLASH;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            FsmEvent nOCOMBO = this.NOCOMBO;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component.isThreeFlash())
                {
                    nOCOMBO = this.THREE_FLASH;
                }
                else if (component.isPairFlash())
                {
                    nOCOMBO = this.PAIR_FLASH;
                }
                else if (component.isFlash())
                {
                    nOCOMBO = this.FLASH;
                }
                else if (component.isThree())
                {
                    nOCOMBO = this.THREE;
                }
            }
            base.Fsm.Event(nOCOMBO);
            base.Finish();
        }
    }
}

