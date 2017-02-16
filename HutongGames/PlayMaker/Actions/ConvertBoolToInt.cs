namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Convert), Tooltip("Converts a Bool value to an Integer value.")]
    public class ConvertBoolToInt : FsmStateAction
    {
        [UIHint(UIHint.Variable), RequiredField, Tooltip("The Bool variable to test.")]
        public FsmBool boolVariable;
        [Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
        public bool everyFrame;
        [Tooltip("Integer value if Bool variable is false.")]
        public FsmInt falseValue;
        [RequiredField, Tooltip("The Integer variable to set based on the Bool variable value."), UIHint(UIHint.Variable)]
        public FsmInt intVariable;
        [Tooltip("Integer value if Bool variable is false.")]
        public FsmInt trueValue;

        private void DoConvertBoolToInt()
        {
            this.intVariable.Value = !this.boolVariable.Value ? this.falseValue.Value : this.trueValue.Value;
        }

        public override void OnEnter()
        {
            this.DoConvertBoolToInt();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoConvertBoolToInt();
        }

        public override void Reset()
        {
            this.boolVariable = null;
            this.intVariable = null;
            this.falseValue = 0;
            this.trueValue = 1;
            this.everyFrame = false;
        }
    }
}

