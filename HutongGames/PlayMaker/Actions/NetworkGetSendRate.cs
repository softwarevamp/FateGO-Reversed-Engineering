namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Store the current send rate for all NetworkViews"), ActionCategory(ActionCategory.Network)]
    public class NetworkGetSendRate : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Store the current send rate for NetworkViews"), UIHint(UIHint.Variable), RequiredField]
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

