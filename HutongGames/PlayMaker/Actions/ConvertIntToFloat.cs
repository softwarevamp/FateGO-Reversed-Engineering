namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Converts an Integer value to a Float value."), ActionCategory(ActionCategory.Convert)]
    public class ConvertIntToFloat : FsmStateAction
    {
        [Tooltip("Repeat every frame. Useful if the Integer variable is changing.")]
        public bool everyFrame;
        [RequiredField, UIHint(UIHint.Variable), Tooltip("Store the result in a Float variable.")]
        public FsmFloat floatVariable;
        [RequiredField, Tooltip("The Integer variable to convert to a float."), UIHint(UIHint.Variable)]
        public FsmInt intVariable;

        private void DoConvertIntToFloat()
        {
            this.floatVariable.Value = this.intVariable.Value;
        }

        public override void OnEnter()
        {
            this.DoConvertIntToFloat();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoConvertIntToFloat();
        }

        public override void Reset()
        {
            this.intVariable = null;
            this.floatVariable = null;
            this.everyFrame = false;
        }
    }
}

