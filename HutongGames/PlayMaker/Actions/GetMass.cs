namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Gets the Mass of a Game Object's Rigid Body."), ActionCategory(ActionCategory.Physics)]
    public class GetMass : ComponentAction<Rigidbody>
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject that owns the Rigidbody"), CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Store the mass in a float variable."), UIHint(UIHint.Variable), RequiredField]
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

