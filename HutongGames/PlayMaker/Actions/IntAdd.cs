﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Math), Tooltip("Adds a value to an Integer Variable.")]
    public class IntAdd : FsmStateAction
    {
        [RequiredField]
        public FsmInt add;
        public bool everyFrame;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmInt intVariable;

        public override void OnEnter()
        {
            this.intVariable.Value += this.add.Value;
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.intVariable.Value += this.add.Value;
        }

        public override void Reset()
        {
            this.intVariable = null;
            this.add = null;
            this.everyFrame = false;
        }
    }
}

