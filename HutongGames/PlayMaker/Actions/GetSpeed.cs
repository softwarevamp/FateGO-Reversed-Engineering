namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Physics), HutongGames.PlayMaker.Tooltip("Gets the Speed of a Game Object and stores it in a Float Variable. NOTE: The Game Object must have a rigid body.")]
    public class GetSpeed : ComponentAction<Rigidbody>
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject with a Rigidbody."), CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable), RequiredField, HutongGames.PlayMaker.Tooltip("Store the speed in a float variable.")]
        public FsmFloat storeResult;

        private void DoGetSpeed()
        {
            if (this.storeResult != null)
            {
                GameObject go = (this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner;
                if (base.UpdateCache(go))
                {
                    Vector3 velocity = base.rigidbody.velocity;
                    this.storeResult.Value = velocity.magnitude;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetSpeed();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetSpeed();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

