namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class ShowHeal : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject actObject;
        public FsmInt functionIndex = -1;

        public override void OnEnter()
        {
            Debug.Log("ShowDamage::OnEnter");
            GameObject actObj = this.actObject.Value;
            if (actObj != null)
            {
                BattlePerformance performance = null;
                BattleActorControl component = actObj.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    performance = component.performance;
                }
                else
                {
                    performance = actObj.GetComponent<BattlePerformance>();
                }
                performance.showHeal(actObj, this.functionIndex.Value);
            }
            base.Finish();
        }
    }
}

