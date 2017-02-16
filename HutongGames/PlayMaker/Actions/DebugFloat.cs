namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Debug), Tooltip("Logs the value of a Float Variable in the PlayMaker Log Window.")]
    public class DebugFloat : FsmStateAction
    {
        [Tooltip("Prints the value of a Float variable in the PlayMaker log window."), UIHint(UIHint.Variable)]
        public FsmFloat floatVariable;
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

        public override void OnEnter()
        {
            string text = "None";
            if (!this.floatVariable.IsNone)
            {
                text = this.floatVariable.Name + ": " + this.floatVariable.Value;
            }
            ActionHelpers.DebugLog(base.Fsm, this.logLevel, text);
            base.Finish();
        }

        public override void Reset()
        {
            this.logLevel = LogLevel.Info;
            this.floatVariable = null;
        }
    }
}

