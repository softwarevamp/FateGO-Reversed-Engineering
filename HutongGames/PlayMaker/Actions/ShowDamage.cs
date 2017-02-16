namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class ShowDamage : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject actObject;
        public FsmString attachNodename;
        public FsmInt countValue = 0;
        public FsmInt criticalEffectId;
        public FsmInt functionIndex = -1;
        public FsmInt nomalEffectId;
        public FsmInt startValue = 0;

        public override void OnEnter()
        {
            GameObject actObj = this.actObject.Value;
            string attachNodename = this.attachNodename.Value;
            if (actObj != null)
            {
                BattleActorControl component = actObj.GetComponent<BattleActorControl>();
                BattlePerformance performance = actObj.GetComponent<BattlePerformance>();
                if (component != null)
                {
                    performance = component.performance;
                }
                performance.ShowDamage(actObj, this.nomalEffectId.Value, this.criticalEffectId.Value, attachNodename, this.functionIndex.Value, this.startValue.Value, this.countValue.Value, false, new Vector3(0f, 0f, 0f), false);
            }
            base.Finish();
        }
    }
}

