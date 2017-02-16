namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get if network messages are enabled or disabled.\n\nIf disabled no RPC call execution or network view synchronization takes place"), ActionCategory(ActionCategory.Network)]
    public class NetworkGetIsMessageQueueRunning : FsmStateAction
    {
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Is Message Queue Running. If this is disabled no RPC call execution or network view synchronization takes place")]
        public FsmBool result;

        public override void OnEnter()
        {
            this.result.Value = Network.isMessageQueueRunning;
            base.Finish();
        }

        public override void Reset()
        {
            this.result = null;
        }
    }
}

