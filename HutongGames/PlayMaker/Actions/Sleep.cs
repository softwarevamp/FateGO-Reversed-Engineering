namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Forces a Game Object's Rigid Body to Sleep at least one frame."), ActionCategory(ActionCategory.Physics)]
    public class Sleep : ComponentAction<Rigidbody>
    {
        [RequiredField, CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;

        private void DoSleep()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.rigidbody.Sleep();
            }
        }

        public override void OnEnter()
        {
            this.DoSleep();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
        }
    }
}

