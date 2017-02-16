﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Math), HutongGames.PlayMaker.Tooltip("Clamps the value of Float Variable to a Min/Max range.")]
    public class FloatClamp : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeate every frame. Useful if the float variable is changing.")]
        public bool everyFrame;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Float variable to clamp."), RequiredField]
        public FsmFloat floatVariable;
        [HutongGames.PlayMaker.Tooltip("The maximum value."), RequiredField]
        public FsmFloat maxValue;
        [HutongGames.PlayMaker.Tooltip("The minimum value."), RequiredField]
        public FsmFloat minValue;

        private void DoClamp()
        {
            this.floatVariable.Value = Mathf.Clamp(this.floatVariable.Value, this.minValue.Value, this.maxValue.Value);
        }

        public override void OnEnter()
        {
            this.DoClamp();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoClamp();
        }

        public override void Reset()
        {
            this.floatVariable = null;
            this.minValue = null;
            this.maxValue = null;
            this.everyFrame = false;
        }
    }
}

