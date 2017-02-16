namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Enable or disable the processing of network messages.\n\nIf this is disabled no RPC call execution or network view synchronization takes place."), ActionCategory(ActionCategory.Network)]
    public class NetworkSetIsMessageQueueRunning : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Is Message Queue Running. If this is disabled no RPC call execution or network view synchronization takes place")]
        public FsmBool isMessageQueueRunning;

        public override void OnEnter()
        {
            Network.isMessageQueueRunning = this.isMessageQueueRunning.Value;
            base.Finish();
        }

        public override void Reset()
        {
            this.isMessageQueueRunning = null;
        }
    }
}

