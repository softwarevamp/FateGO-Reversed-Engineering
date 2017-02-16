namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Input), HutongGames.PlayMaker.Tooltip("Sends an Event when the user hits any Key or Mouse Button.")]
    public class AnyKey : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Event to send when any Key or Mouse Button is pressed."), RequiredField]
        public FsmEvent sendEvent;

        public override void OnUpdate()
        {
            if (Input.anyKeyDown)
            {
                base.Fsm.Event(this.sendEvent);
            }
        }

        public override void Reset()
        {
            this.sendEvent = null;
        }
    }
}

