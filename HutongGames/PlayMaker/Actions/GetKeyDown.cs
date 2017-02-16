﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sends an Event when a Key is pressed."), ActionCategory(ActionCategory.Input)]
    public class GetKeyDown : FsmStateAction
    {
        [RequiredField]
        public KeyCode key;
        public FsmEvent sendEvent;
        [UIHint(UIHint.Variable)]
        public FsmBool storeResult;

        public override void OnUpdate()
        {
            bool keyDown = Input.GetKeyDown(this.key);
            if (keyDown)
            {
                base.Fsm.Event(this.sendEvent);
            }
            this.storeResult.Value = keyDown;
        }

        public override void Reset()
        {
            this.sendEvent = null;
            this.key = KeyCode.None;
            this.storeResult = null;
        }
    }
}

