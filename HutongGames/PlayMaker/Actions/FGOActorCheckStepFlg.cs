namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorCheckStepFlg : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject actObject;
        public FsmEvent FalseEvent;
        public FsmEvent TrueEvent;

        public override void OnEnter()
        {
            GameObject obj2 = this.actObject.Value;
            bool flag = false;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    flag = component.checkStepFlg();
                }
            }
            if (flag)
            {
                base.Fsm.Event(this.TrueEvent);
            }
            else
            {
                base.Fsm.Event(this.FalseEvent);
            }
            base.Finish();
        }
    }
}

