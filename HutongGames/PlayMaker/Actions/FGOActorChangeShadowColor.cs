namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorChangeShadowColor : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmColor targetColor;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    component.ChangeShadowColor(this.targetColor.Value, 0.3f);
                }
            }
            base.Finish();
        }
    }
}

