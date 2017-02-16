namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Physics), HutongGames.PlayMaker.Tooltip("Gets the Mass of a Game Object's Rigid Body.")]
    public class GetMass : ComponentAction<Rigidbody>
    {
        [CheckForComponent(typeof(Rigidbody)), HutongGames.PlayMaker.Tooltip("The GameObject that owns the Rigidbody"), RequiredField]
        public FsmOwnerDefault gameObject;
        [RequiredField, UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Store the mass in a float variable.")]
        public FsmFloat storeResult;

        private void DoGetMass()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                this.storeResult.Value = base.rigidbody.mass;
            }
        }

        public override void OnEnter()
        {
            this.DoGetMass();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.storeResult = null;
        }
    }
}

