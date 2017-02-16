namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Physics), HutongGames.PlayMaker.Tooltip("Forces a Game Object's Rigid Body to wake up.")]
    public class WakeUp : ComponentAction<Rigidbody>
    {
        [RequiredField, CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;

        private void DoWakeUp()
        {
            GameObject go = (this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner;
            if (base.UpdateCache(go))
            {
                base.rigidbody.WakeUp();
            }
        }

        public override void OnEnter()
        {
            this.DoWakeUp();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
        }
    }
}

