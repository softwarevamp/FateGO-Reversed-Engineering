namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Destroys the Owner of the Fsm! Useful for spawned Prefabs that need to kill themselves, e.g., a projectile that explodes on impact."), ActionCategory(ActionCategory.GameObject)]
    public class DestroySelf : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Detach children before destroying the Owner.")]
        public FsmBool detachChildren;

        public override void OnEnter()
        {
            if (base.Owner != null)
            {
                if (this.detachChildren.Value)
                {
                    base.Owner.transform.DetachChildren();
                }
                UnityEngine.Object.Destroy(base.Owner);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.detachChildren = 0;
        }
    }
}

