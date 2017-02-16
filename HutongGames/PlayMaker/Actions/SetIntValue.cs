namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Math), Tooltip("Sets the value of an Integer Variable.")]
    public class SetIntValue : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField]
        public FsmInt intValue;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmInt intVariable;

        public override void OnEnter()
        {
            this.intVariable.Value = this.intValue.Value;
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.intVariable.Value = this.intValue.Value;
        }

        public override void Reset()
        {
            this.intVariable = null;
            this.intValue = null;
            this.everyFrame = false;
        }
    }
}

