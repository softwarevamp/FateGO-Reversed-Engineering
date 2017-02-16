namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Converts an String value to an Int value."), ActionCategory(ActionCategory.Convert)]
    public class ConvertStringToInt : FsmStateAction
    {
        [Tooltip("Repeat every frame. Useful if the String variable is changing.")]
        public bool everyFrame;
        [UIHint(UIHint.Variable), Tooltip("Store the result in an Int variable."), RequiredField]
        public FsmInt intVariable;
        [RequiredField, Tooltip("The String variable to convert to an integer."), UIHint(UIHint.Variable)]
        public FsmString stringVariable;

        private void DoConvertStringToInt()
        {
            this.intVariable.Value = int.Parse(this.stringVariable.Value);
        }

        public override void OnEnter()
        {
            this.DoConvertStringToInt();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoConvertStringToInt();
        }

        public override void Reset()
        {
            this.intVariable = null;
            this.stringVariable = null;
            this.everyFrame = false;
        }
    }
}

