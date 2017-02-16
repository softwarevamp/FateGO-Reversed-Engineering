namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Gets the input values from a UIJoystick"), ActionCategory("NGUI")]
    public class NguiGetJoystickInput : FsmStateAction
    {
        private UIJoystick _joystick;
        [HutongGames.PlayMaker.Tooltip("The pad angle"), UIHint(UIHint.Variable)]
        public FsmFloat angle;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("The GameObject featuring the UIJoystick component."), RequiredField, CheckForComponent(typeof(UIJoystick))]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The Horyzontal and Vertical Input values")]
        public FsmVector2 input;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The pad input plus the  angle in the z value of the vector3. Use this for network synching, saves bandwidth")]
        public FsmVector3 inputAndAngle;

        private void doGetPadInputs()
        {
            if (!this.input.IsNone)
            {
                this.input.Value = this._joystick.padPosition;
            }
            if (!this.angle.IsNone)
            {
                this.angle.Value = this._joystick.padAngle;
            }
            if (!this.inputAndAngle.IsNone)
            {
                this.inputAndAngle.Value = this._joystick.padPositionAndAngle;
            }
        }

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                this._joystick = ownerDefaultTarget.GetComponent<UIJoystick>();
                if (this._joystick != null)
                {
                    this.doGetPadInputs();
                    if (!this.everyFrame)
                    {
                        base.Finish();
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            this.doGetPadInputs();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.input = null;
            this.angle = null;
            this.inputAndAngle = null;
            this.everyFrame = true;
        }
    }
}

