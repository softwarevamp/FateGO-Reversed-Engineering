namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Transform), HutongGames.PlayMaker.Tooltip("Gets the Angle between a GameObject's forward axis and a Target. The Target can be defined as a GameObject or a world Position. If you specify both, then the Position will be used as a local offset from the Target Object's position.")]
    public class GetAngleToTarget : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The game object whose forward axis we measure from. If the target is dead ahead the angle will be 0.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Ignore height differences when calculating the angle.")]
        public FsmBool ignoreHeight;
        [HutongGames.PlayMaker.Tooltip("Store the angle in a float variable."), UIHint(UIHint.Variable), RequiredField]
        public FsmFloat storeAngle;
        [HutongGames.PlayMaker.Tooltip("The target object to measure the angle to. Or use target position.")]
        public FsmGameObject targetObject;
        [HutongGames.PlayMaker.Tooltip("The world position to measure an angle to. If Target Object is also specified, this vector is used as an offset from that object's position.")]
        public FsmVector3 targetPosition;

        private void DoGetAngleToTarget()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                GameObject obj3 = this.targetObject.Value;
                if ((obj3 != null) || !this.targetPosition.IsNone)
                {
                    Vector3 vector;
                    if (obj3 != null)
                    {
                        vector = this.targetPosition.IsNone ? obj3.transform.position : obj3.transform.TransformPoint(this.targetPosition.Value);
                    }
                    else
                    {
                        vector = this.targetPosition.Value;
                    }
                    if (this.ignoreHeight.Value)
                    {
                        vector.y = ownerDefaultTarget.transform.position.y;
                    }
                    Vector3 from = vector - ownerDefaultTarget.transform.position;
                    this.storeAngle.Value = Vector3.Angle(from, ownerDefaultTarget.transform.forward);
                }
            }
        }

        public override void OnLateUpdate()
        {
            this.DoGetAngleToTarget();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.targetObject = null;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.targetPosition = vector;
            this.ignoreHeight = 1;
            this.storeAngle = null;
            this.everyFrame = false;
        }
    }
}

