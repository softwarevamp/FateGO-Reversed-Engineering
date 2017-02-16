﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.StateMachine), Tooltip("Immediately return to the previously active state.")]
    public class GotoPreviousState : FsmStateAction
    {
        public override void OnEnter()
        {
            if (base.Fsm.PreviousActiveState != null)
            {
                this.Log("Goto Previous State: " + base.Fsm.PreviousActiveState.Name);
                base.Fsm.GotoPreviousState();
            }
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

