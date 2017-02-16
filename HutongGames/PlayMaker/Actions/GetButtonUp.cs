namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sends an Event when a Button is released."), ActionCategory(ActionCategory.Input)]
    public class GetButtonUp : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("The name of the button. Set in the Unity Input Manager.")]
        public FsmString buttonName;
        [HutongGames.PlayMaker.Tooltip("Event to send if the button is released.")]
        public FsmEvent sendEvent;
        [HutongGames.PlayMaker.Tooltip("Set to True if the button is released."), UIHint(UIHint.Variable)]
        public FsmBool storeResult;

        public override void OnUpdate()
        {
            bool buttonUp = Input.GetButtonUp(this.buttonName.Value);
            if (buttonUp)
            {
                base.Fsm.Event(this.sendEvent);
            }
            this.storeResult.Value = buttonUp;
        }

        public override void Reset()
        {
            this.buttonName = "Fire1";
            this.sendEvent = null;
            this.storeResult = null;
        }
    }
}

