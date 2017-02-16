﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Debug), Tooltip("Draws a state label for this FSM in the Game View. The label is drawn on the game object that owns the FSM. Use this to override the global setting in the PlayMaker Debug menu.")]
    public class DrawStateLabel : FsmStateAction
    {
        [Tooltip("Set to True to show State labels, or Fals to hide them."), RequiredField]
        public FsmBool showLabel;

        public override void OnEnter()
        {
            base.Fsm.ShowStateLabel = this.showLabel.Value;
            base.Finish();
        }

        public override void Reset()
        {
            this.showLabel = 1;
        }
    }
}

