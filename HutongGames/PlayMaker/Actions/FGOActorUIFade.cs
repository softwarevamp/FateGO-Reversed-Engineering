namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorUIFade : FsmStateAction
    {
        [CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmGameObject battlePerformance;
        public FsmFloat targetAlpha;
        public FsmFloat time;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            BattleActorControl component = obj2.GetComponent<BattleActorControl>();
            if (component != null)
            {
                component.startBattleUIFade(this.time.Value, this.targetAlpha.Value);
            }
            else if (obj2 == null)
            {
                BattlePerformance performance = this.battlePerformance.Value.GetComponent<BattlePerformance>();
                if (performance != null)
                {
                    performance.startBattleUIFade(this.time.Value, this.targetAlpha.Value);
                }
            }
            base.Finish();
        }
    }
}

