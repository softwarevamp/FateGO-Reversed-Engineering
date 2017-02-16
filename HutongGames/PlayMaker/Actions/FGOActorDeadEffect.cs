namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorDeadEffect : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject targetObject;

        public override void OnEnter()
        {
            GameObject obj2 = this.targetObject.Value;
            if (obj2 != null)
            {
                obj2.GetComponent<BattleActorControl>().startDeadEffect();
            }
        }
    }
}

