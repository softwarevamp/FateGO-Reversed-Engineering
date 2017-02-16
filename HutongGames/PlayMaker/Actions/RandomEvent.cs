namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sends a Random State Event after an optional delay. Use this to transition to a random state from the current state."), ActionCategory(ActionCategory.StateMachine)]
    public class RandomEvent : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Delay before sending the event."), HasFloatSlider(0f, 10f)]
        public FsmFloat delay;
        private DelayedEvent delayedEvent;
        private int lastEventIndex = -1;
        [HutongGames.PlayMaker.Tooltip("Don't repeat the same event twice in a row.")]
        public FsmBool noRepeat;
        private int randomEventIndex;

        private FsmEvent GetRandomEvent()
        {
            do
            {
                this.randomEventIndex = UnityEngine.Random.Range(0, base.State.Transitions.Length);
            }
            while ((this.noRepeat.Value && (base.State.Transitions.Length > 1)) && (this.randomEventIndex == this.lastEventIndex));
            this.lastEventIndex = this.randomEventIndex;
            return base.State.Transitions[this.randomEventIndex].FsmEvent;
        }

        public override void OnEnter()
        {
            if (base.State.Transitions.Length != 0)
            {
                if (this.lastEventIndex == -1)
                {
                    this.lastEventIndex = UnityEngine.Random.Range(0, base.State.Transitions.Length);
                }
                if (this.delay.Value < 0.001f)
                {
                    base.Fsm.Event(this.GetRandomEvent());
                    base.Finish();
                }
                else
                {
                    this.delayedEvent = base.Fsm.DelayedEvent(this.GetRandomEvent(), this.delay.Value);
                }
            }
        }

        public override void OnUpdate()
        {
            if (DelayedEvent.WasSent(this.delayedEvent))
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.delay = null;
        }
    }
}

