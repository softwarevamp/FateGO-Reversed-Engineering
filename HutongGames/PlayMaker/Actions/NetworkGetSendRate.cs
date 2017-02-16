namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Store the current send rate for all NetworkViews")]
    public class NetworkGetSendRate : FsmStateAction
    {
        [RequiredField, UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Store the current send rate for NetworkViews")]
        public FsmFloat sendRate;

        private void DoGetSendRate()
        {
            this.sendRate.Value = Network.sendRate;
        }

        public override void OnEnter()
        {
            this.DoGetSendRate();
            base.Finish();
        }

        public override void Reset()
        {
            this.sendRate = null;
        }
    }
}

