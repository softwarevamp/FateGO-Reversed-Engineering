namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOWorldTimeScale : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattlePerformance))]
        public FsmGameObject performanceObject;
        public FsmFloat timescale;

        public override void OnEnter()
        {
            GameObject obj2 = this.performanceObject.Value;
            float time = this.timescale.Value;
            if (obj2 != null)
            {
                obj2.GetComponent<BattlePerformance>().setTimeScale(time);
            }
            base.Finish();
        }
    }
}

