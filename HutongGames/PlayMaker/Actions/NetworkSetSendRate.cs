namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Set the send rate for all networkViews. Default is 15"), ActionCategory(ActionCategory.Network)]
    public class NetworkSetSendRate : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("The send rate for all networkViews")]
        public FsmFloat sendRate;

        private void DoSetSendRate()
        {
            Network.sendRate = this.sendRate.Value;
        }

        public override void OnEnter()
        {
            this.DoSetSendRate();
            base.Finish();
        }

        public override void Reset()
        {
            this.sendRate = 15f;
        }
    }
}

