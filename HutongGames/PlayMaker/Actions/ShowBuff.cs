namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class ShowBuff : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject actObject;
        public FsmInt functionIndex = -1;

        public override void OnEnter()
        {
            GameObject actObj = this.actObject.Value;
            int funcIndex = this.functionIndex.Value;
            if (actObj != null)
            {
                BattlePerformance performance = null;
                BattleActorControl component = actObj.GetComponent<BattleActorControl>();
                performance = actObj.GetComponent<BattlePerformance>();
                if (component != null)
                {
                    performance = component.performance;
                }
                if (performance != null)
                {
                    performance.ShowBuff(actObj, funcIndex);
                }
            }
            base.Finish();
        }
    }
}

