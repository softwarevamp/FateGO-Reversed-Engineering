﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Rotates a GameObject based on mouse movement. Minimum and Maximum values can be used to constrain the rotation."), ActionCategory(ActionCategory.Input)]
    public class MouseLook : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The axes to rotate around.")]
        public RotationAxes axes;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject to rotate.")]
        public FsmOwnerDefault gameObject;
        [HasFloatSlider(-360f, 360f), HutongGames.PlayMaker.Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
        public FsmFloat maximumX;
        [HasFloatSlider(-360f, 360f), HutongGames.PlayMaker.Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat maximumY;
        [HutongGames.PlayMaker.Tooltip("Clamp rotation around X axis. Set to None for no clamping."), HasFloatSlider(-360f, 360f)]
        public FsmFloat minimumX;
        [HasFloatSlider(-360f, 360f), HutongGames.PlayMaker.Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat minimumY;
        private float rotationX;
        private float rotationY;
        [RequiredField]
        public FsmFloat sensitivityX;
        [RequiredField]
        public FsmFloat sensitivityY;

        private static float ClampAngle(float angle, FsmFloat min, FsmFloat max)
        {
            if (!min.IsNone && (angle < min.Value))
            {
                angle = min.Value;
            }
            if (!max.IsNone && (angle > max.Value))
            {
                angle = max.Value;
            }
            return angle;
        }

        private void DoMouseLook()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                Transform transform = ownerDefaultTarget.transform;
                switch (this.axes)
                {
                    case RotationAxes.MouseXAndY:
                        transform.localEulerAngles = new Vector3(this.GetYRotation(), this.GetXRotation(), 0f);
                        break;

                    case RotationAxes.MouseX:
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, this.GetXRotation(), 0f);
                        break;

                    case RotationAxes.MouseY:
                        transform.localEulerAngles = new Vector3(-this.GetYRotation(), transform.localEulerAngles.y, 0f);
                        break;
                }
            }
        }

        private float GetXRotation()
        {
            this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX.Value;
            this.rotationX = ClampAngle(this.rotationX, this.minimumX, this.maximumX);
            return this.rotationX;
        }

        private float GetYRotation()
        {
            this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY.Value;
            this.rotationY = ClampAngle(this.rotationY, this.minimumY, this.maximumY);
            return this.rotationY;
        }

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget == null)
            {
                base.Finish();
            }
            else
            {
                Rigidbody component = ownerDefaultTarget.GetComponent<Rigidbody>();
                if (component != null)
                {
                    component.freezeRotation = true;
                }
                this.rotationX = ownerDefaultTarget.transform.localRotation.eulerAngles.y;
                this.rotationY = ownerDefaultTarget.transform.localRotation.eulerAngles.x;
                this.DoMouseLook();
                if (!this.everyFrame)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.DoMouseLook();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.axes = RotationAxes.MouseXAndY;
            this.sensitivityX = 15f;
            this.sensitivityY = 15f;
            FsmFloat num = new FsmFloat {
                UseVariable = true
            };
            this.minimumX = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.maximumX = num;
            this.minimumY = -60f;
            this.maximumY = 60f;
            this.everyFrame = true;
        }

        public enum RotationAxes
        {
            MouseXAndY,
            MouseX,
            MouseY
        }
    }
}

