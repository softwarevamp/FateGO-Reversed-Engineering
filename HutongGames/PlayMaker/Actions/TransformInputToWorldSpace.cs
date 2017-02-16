namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Transforms 2d input into a 3d world space vector. E.g., can be used to transform input from a touch joystick to a movement vector."), ActionCategory(ActionCategory.Input)]
    public class TransformInputToWorldSpace : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The horizontal input."), UIHint(UIHint.Variable)]
        public FsmFloat horizontalInput;
        [HutongGames.PlayMaker.Tooltip("The world plane to map the 2d input onto."), RequiredField]
        public AxisPlane mapToPlane;
        [HutongGames.PlayMaker.Tooltip("Input axis are reported in the range -1 to 1, this multiplier lets you set a new range.")]
        public FsmFloat multiplier;
        [HutongGames.PlayMaker.Tooltip("Make the result relative to a GameObject, typically the main camera.")]
        public FsmGameObject relativeTo;
        [HutongGames.PlayMaker.Tooltip("Store the length of the direction vector."), UIHint(UIHint.Variable)]
        public FsmFloat storeMagnitude;
        [HutongGames.PlayMaker.Tooltip("Store the direction vector."), RequiredField, UIHint(UIHint.Variable)]
        public FsmVector3 storeVector;
        [HutongGames.PlayMaker.Tooltip("The vertical input."), UIHint(UIHint.Variable)]
        public FsmFloat verticalInput;

        public override void OnUpdate()
        {
            Vector3 forward = new Vector3();
            Vector3 right = new Vector3();
            if (this.relativeTo.Value == null)
            {
                switch (this.mapToPlane)
                {
                    case AxisPlane.XZ:
                        forward = Vector3.forward;
                        right = Vector3.right;
                        break;

                    case AxisPlane.XY:
                        forward = Vector3.up;
                        right = Vector3.right;
                        break;

                    case AxisPlane.YZ:
                        forward = Vector3.up;
                        right = Vector3.forward;
                        break;
                }
            }
            else
            {
                Transform transform = this.relativeTo.Value.transform;
                switch (this.mapToPlane)
                {
                    case AxisPlane.XZ:
                        forward = transform.TransformDirection(Vector3.forward);
                        forward.y = 0f;
                        forward = forward.normalized;
                        right = new Vector3(forward.z, 0f, -forward.x);
                        break;

                    case AxisPlane.XY:
                    case AxisPlane.YZ:
                        forward = Vector3.up;
                        forward.z = 0f;
                        forward = forward.normalized;
                        right = transform.TransformDirection(Vector3.right);
                        break;
                }
            }
            float num = !this.horizontalInput.IsNone ? this.horizontalInput.Value : 0f;
            float num2 = !this.verticalInput.IsNone ? this.verticalInput.Value : 0f;
            Vector3 vector3 = (Vector3) ((num * right) + (num2 * forward));
            vector3 = (Vector3) (vector3 * this.multiplier.Value);
            this.storeVector.Value = vector3;
            if (!this.storeMagnitude.IsNone)
            {
                this.storeMagnitude.Value = vector3.magnitude;
            }
        }

        public override void Reset()
        {
            this.horizontalInput = null;
            this.verticalInput = null;
            this.multiplier = 1f;
            this.mapToPlane = AxisPlane.XZ;
            this.storeVector = null;
            this.storeMagnitude = null;
        }

        public enum AxisPlane
        {
            XZ,
            XY,
            YZ
        }
    }
}

