namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("AdStore Event Call."), ActionCategory("Ad")]
    public class AdStoreEvent : FsmStateAction
    {
        [RequiredField, Tooltip("Set AdStore Action Key.")]
        public FsmString actionKey;

        public override void OnEnter()
        {
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

