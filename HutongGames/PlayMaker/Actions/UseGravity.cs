namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets whether a Game Object's Rigidy Body is affected by Gravity."), ActionCategory(ActionCategory.Physics)]
    public class UseGravity : ComponentAction<Rigidbody>
    {
        [RequiredField, CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;
        [RequiredField]
        public FsmBool useGravity;

        private void DoUseGravity()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.rigidbody.useGravity = this.useGravity.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoUseGravity();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.useGravity = 1;
        }
    }
}

