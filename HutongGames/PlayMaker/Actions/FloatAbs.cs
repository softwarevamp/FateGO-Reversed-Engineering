﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Math), HutongGames.PlayMaker.Tooltip("Sets a Float variable to its absolute value.")]
    public class FloatAbs : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the Float variable is changing.")]
        public bool everyFrame;
        [UIHint(UIHint.Variable), RequiredField, HutongGames.PlayMaker.Tooltip("The Float variable.")]
        public FsmFloat floatVariable;

        private void DoFloatAbs()
        {
            this.floatVariable.Value = Mathf.Abs(this.floatVariable.Value);
        }

        public override void OnEnter()
        {
            this.DoFloatAbs();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoFloatAbs();
        }

        public override void Reset()
        {
            this.floatVariable = null;
            this.everyFrame = false;
        }
    }
}

