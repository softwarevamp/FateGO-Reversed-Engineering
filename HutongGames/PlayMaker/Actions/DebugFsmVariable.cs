﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Print the value of an FSM Variable in the PlayMaker Log Window."), ActionCategory(ActionCategory.Debug)]
    public class DebugFsmVariable : FsmStateAction
    {
        [Tooltip("Variable to print to the PlayMaker log window."), UIHint(UIHint.Variable), HideTypeFilter]
        public FsmVar fsmVar;
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

        public override void OnEnter()
        {
            ActionHelpers.DebugLog(base.Fsm, this.logLevel, this.fsmVar.DebugString());
            base.Finish();
        }

        public override void Reset()
        {
            this.logLevel = LogLevel.Info;
            this.fsmVar = null;
        }
    }
}

