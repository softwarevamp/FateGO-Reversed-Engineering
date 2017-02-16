namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Physics), HutongGames.PlayMaker.Tooltip("Adds a force to a Game Object. Use Vector3 variable and/or Float variables for each axis.")]
    public class AddForce : ComponentAction<Rigidbody>
    {
        [HutongGames.PlayMaker.Tooltip("Optionally apply the force at a position on the object. This will also add some torque. The position is often returned from MousePick or GetCollisionInfo actions."), UIHint(UIHint.Variable)]
        public FsmVector3 atPosition;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("The type of force to apply. See Unity Physics docs.")]
        public ForceMode forceMode;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject to apply the force to."), CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Apply the force in world or local space.")]
        public Space space;
        [HutongGames.PlayMaker.Tooltip("A Vector3 force to add. Optionally override any axis with the X, Y, Z parameters."), UIHint(UIHint.Variable)]
        public FsmVector3 vector;
        [HutongGames.PlayMaker.Tooltip("Force along the X axis. To leave unchanged, set to 'None'.")]
        public FsmFloat x;
        [HutongGames.PlayMaker.Tooltip("Force along the Y axis. To leave unchanged, set to 'None'.")]
        public FsmFloat y;
        [HutongGames.PlayMaker.Tooltip("Force along the Z axis. To leave unchanged, set to 'None'.")]
        public FsmFloat z;

        public override void Awake()
        {
            base.Fsm.HandleFixedUpdate = true;
        }

        private void DoAddForce()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                Vector3 force = !this.vector.IsNone ? this.vector.Value : new Vector3(this.x.Value, this.y.Value, this.z.Value);
                if (!this.x.IsNone)
                {
                    force.x = this.x.Value;
                }
                if (!this.y.IsNone)
                {
                    force.y = this.y.Value;
                }
                if (!this.z.IsNone)
                {
                    force.z = this.z.Value;
                }
                if (this.space == Space.World)
                {
                    if (!this.atPosition.IsNone)
                    {
                        base.rigidbody.AddForceAtPosition(force, this.atPosition.Value, this.forceMode);
                    }
                    else
                    {
                        base.rigidbody.AddForce(force, this.forceMode);
                    }
                }
                else
                {
                    base.rigidbody.AddRelativeForce(force, this.forceMode);
                }
            }
        }

        public override void OnEnter()
        {
            this.DoAddForce();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnFixedUpdate()
        {
            this.DoAddForce();
        }

        public override void Reset()
        {
            this.gameObject = null;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.atPosition = vector;
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
            this.space = Space.World;
            this.forceMode = ForceMode.Force;
            this.everyFrame = false;
        }
    }
}

