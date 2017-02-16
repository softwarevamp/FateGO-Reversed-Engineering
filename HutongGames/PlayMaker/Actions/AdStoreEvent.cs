namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("AdStore Event Call."), ActionCategory("Ad")]
    public class AdStoreEvent : FsmStateAction
    {
        [Tooltip("Set AdStore Action Key."), RequiredField]
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

