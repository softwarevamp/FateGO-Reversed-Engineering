namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Sends an Event in the next frame. Useful if you want to loop states every frame."), ActionCategory(ActionCategory.StateMachine)]
    public class NextFrameEvent : FsmStateAction
    {
        [RequiredField]
        public FsmEvent sendEvent;

        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            base.Finish();
            base.Fsm.Event(this.sendEvent);
        }

        public override void Reset()
        {
            this.sendEvent = null;
        }
    }
}

