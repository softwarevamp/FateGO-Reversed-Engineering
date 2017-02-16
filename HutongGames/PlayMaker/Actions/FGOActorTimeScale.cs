namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorTimeScale : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmFloat timescale;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            float time = this.timescale.Value;
            if (obj2 != null)
            {
                obj2.GetComponent<BattleActorControl>().setTimeScale(time);
            }
            base.Finish();
        }
    }
}

