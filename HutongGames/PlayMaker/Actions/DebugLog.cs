namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Sends a log message to the PlayMaker Log Window."), ActionCategory(ActionCategory.Debug)]
    public class DebugLog : FsmStateAction
    {
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;
        [Tooltip("Text to print to the PlayMaker log window.")]
        public FsmString text;

        public override void OnEnter()
        {
            if (!string.IsNullOrEmpty(this.text.Value))
            {
                ActionHelpers.DebugLog(base.Fsm, this.logLevel, this.text.Value);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.logLevel = LogLevel.Info;
            this.text = string.Empty;
        }
    }
}

