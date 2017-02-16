namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get the current network time (seconds)."), ActionCategory(ActionCategory.Network)]
    public class NetworkGetTime : FsmStateAction
    {
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The network time.")]
        public FsmFloat time;

        public override void OnEnter()
        {
            this.time.Value = (float) Network.time;
            base.Finish();
        }

        public override void Reset()
        {
            this.time = null;
        }
    }
}

