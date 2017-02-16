namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.String), Tooltip("Replace a substring with a new String.")]
    public class StringReplace : FsmStateAction
    {
        public bool everyFrame;
        public FsmString replace;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmString storeResult;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmString stringVariable;
        public FsmString with;

        private void DoReplace()
        {
            if ((this.stringVariable != null) && (this.storeResult != null))
            {
                this.storeResult.Value = this.stringVariable.Value.Replace(this.replace.Value, this.with.Value);
            }
        }

        public override void OnEnter()
        {
            this.DoReplace();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoReplace();
        }

        public override void Reset()
        {
            this.stringVariable = null;
            this.replace = string.Empty;
            this.with = string.Empty;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

