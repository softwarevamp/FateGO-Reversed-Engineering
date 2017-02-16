namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Velocity of a Game Object. To leave any axis unchanged, set variable to 'None'. NOTE: Game object must have a rigidbody."), ActionCategory(ActionCategory.Physics)]
    public class SetVelocity : ComponentAction<Rigidbody>
    {
        public bool everyFrame;
        [RequiredField, CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;
        public Space space;
        [UIHint(UIHint.Variable)]
        public FsmVector3 vector;
        public FsmFloat x;
        public FsmFloat y;
        public FsmFloat z;

        public override void Awake()
        {
            base.Fsm.HandleFixedUpdate = true;
        }

        private void DoSetVelocity()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                Vector3 vector;
                if (this.vector.IsNone)
                {
                    vector = (this.space != Space.World) ? ownerDefaultTarget.transform.InverseTransformDirection(base.rigidbody.velocity) : base.rigidbody.velocity;
                }
                else
                {
                    vector = this.vector.Value;
                }
                if (!this.x.IsNone)
                {
                    vector.x = this.x.Value;
                }
                if (!this.y.IsNone)
                {
                    vector.y = this.y.Value;
                }
                if (!this.z.IsNone)
                {
                    vector.z = this.z.Value;
                }
                base.rigidbody.velocity = (this.space != Space.World) ? ownerDefaultTarget.transform.TransformDirection(vector) : vector;
            }
        }

        public override void OnEnter()
        {
            this.DoSetVelocity();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnFixedUpdate()
        {
            this.DoSetVelocity();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.vector = null;
            FsmFloat num = new FsmFloat {
                UseVariable = true
            };
            this.x = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.y = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.z = num;
            this.space = Space.Self;
            this.everyFrame = false;
        }
    }
}

