﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Math), HutongGames.PlayMaker.Tooltip("Subtracts a value from a Float Variable.")]
    public class FloatSubtract : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The float variable to subtract from."), RequiredField]
        public FsmFloat floatVariable;
        [HutongGames.PlayMaker.Tooltip("Used with Every Frame. Adds the value over one second to make the operation frame rate independent.")]
        public bool perSecond;
        [HutongGames.PlayMaker.Tooltip("Value to subtract from the float variable."), RequiredField]
        public FsmFloat subtract;

        private void DoFloatSubtract()
        {
            if (!this.perSecond)
            {
                this.floatVariable.Value -= this.subtract.Value;
            }
            else
            {
                this.floatVariable.Value -= this.subtract.Value * Time.deltaTime;
            }
        }

        public override void OnEnter()
        {
            this.DoFloatSubtract();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoFloatSubtract();
        }

        public override void Reset()
        {
            this.floatVariable = null;
            this.subtract = null;
            this.everyFrame = false;
            this.perSecond = false;
        }
    }
}

