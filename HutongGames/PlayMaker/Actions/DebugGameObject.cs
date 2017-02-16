namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Debug), Tooltip("Logs the value of a Game Object Variable in the PlayMaker Log Window.")]
    public class DebugGameObject : FsmStateAction
    {
        [UIHint(UIHint.Variable), Tooltip("Prints the value of a GameObject variable in the PlayMaker log window.")]
        public FsmGameObject gameObject;
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

        public override void OnEnter()
        {
            string text = "None";
            if (!this.gameObject.IsNone)
            {
                text = this.gameObject.Name + ": " + this.gameObject;
            }
            ActionHelpers.DebugLog(base.Fsm, this.logLevel, text);
            base.Finish();
        }

        public override void Reset()
        {
            this.logLevel = LogLevel.Info;
            this.gameObject = null;
        }
    }
}

