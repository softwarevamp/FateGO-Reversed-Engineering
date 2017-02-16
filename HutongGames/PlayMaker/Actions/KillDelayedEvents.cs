namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Kill all queued delayed events. Delayed events are 'fire and forget', but sometimes this can cause problems."), Note("Kill all queued delayed events."), ActionCategory(ActionCategory.StateMachine)]
    public class KillDelayedEvents : FsmStateAction
    {
        public override void OnEnter()
        {
            base.Fsm.KillDelayedEvents();
            base.Finish();
        }
    }
}

