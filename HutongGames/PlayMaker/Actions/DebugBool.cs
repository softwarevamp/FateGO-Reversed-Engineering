﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Debug), Tooltip("Logs the value of a Bool Variable in the PlayMaker Log Window.")]
    public class DebugBool : FsmStateAction
    {
        [Tooltip("Prints the value of a Bool variable in the PlayMaker log window."), UIHint(UIHint.Variable)]
        public FsmBool boolVariable;
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

        public override void OnEnter()
        {
            string text = "None";
            if (!this.boolVariable.IsNone)
            {
                text = this.boolVariable.Name + ": " + this.boolVariable.Value;
            }
            ActionHelpers.DebugLog(base.Fsm, this.logLevel, text);
            base.Finish();
        }

        public override void Reset()
        {
            this.logLevel = LogLevel.Info;
            this.boolVariable = null;
        }
    }
}

