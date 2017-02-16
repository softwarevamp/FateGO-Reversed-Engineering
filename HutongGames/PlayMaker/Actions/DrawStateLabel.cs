﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Draws a state label for this FSM in the Game View. The label is drawn on the game object that owns the FSM. Use this to override the global setting in the PlayMaker Debug menu."), ActionCategory(ActionCategory.Debug)]
    public class DrawStateLabel : FsmStateAction
    {
        [RequiredField, Tooltip("Set to True to show State labels, or Fals to hide them.")]
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

