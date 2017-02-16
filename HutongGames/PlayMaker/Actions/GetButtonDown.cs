namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sends an Event when a Button is pressed."), ActionCategory(ActionCategory.Input)]
    public class GetButtonDown : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The name of the button. Set in the Unity Input Manager."), RequiredField]
        public FsmString buttonName;
        [HutongGames.PlayMaker.Tooltip("Event to send if the button is pressed.")]
        public FsmEvent sendEvent;
        [HutongGames.PlayMaker.Tooltip("Set to True if the button is pressed."), UIHint(UIHint.Variable)]
        public FsmBool storeResult;

        public override void OnUpdate()
        {
            bool buttonDown = Input.GetButtonDown(this.buttonName.Value);
            if (buttonDown)
            {
                base.Fsm.Event(this.sendEvent);
            }
            this.storeResult.Value = buttonDown;
        }

        public override void Reset()
        {
            this.buttonName = "Fire1";
            this.sendEvent = null;
            this.storeResult = null;
        }
    }
}

