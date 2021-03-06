﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.StateMachine), Tooltip("Gets the event that caused the transition to the current state, and stores it in a String Variable.")]
    public class GetLastEvent : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        public FsmString storeEvent;

        public override void OnEnter()
        {
            this.storeEvent.Value = (base.Fsm.LastTransition != null) ? base.Fsm.LastTransition.EventName : "START";
            base.Finish();
        }

        public override void Reset()
        {
            this.storeEvent = null;
        }
    }
}

