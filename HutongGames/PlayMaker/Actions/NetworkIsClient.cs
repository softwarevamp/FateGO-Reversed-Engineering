namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Test if your peer type is client."), ActionCategory(ActionCategory.Network)]
    public class NetworkIsClient : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("True if running as client."), UIHint(UIHint.Variable)]
        public FsmBool isClient;
        [HutongGames.PlayMaker.Tooltip("Event to send if running as client.")]
        public FsmEvent isClientEvent;
        [HutongGames.PlayMaker.Tooltip("Event to send if not running as client.")]
        public FsmEvent isNotClientEvent;

        private void DoCheckIsClient()
        {
            this.isClient.Value = Network.isClient;
            if (Network.isClient && (this.isClientEvent != null))
            {
                base.Fsm.Event(this.isClientEvent);
            }
            else if (!Network.isClient && (this.isNotClientEvent != null))
            {
                base.Fsm.Event(this.isNotClientEvent);
            }
        }

        public override void OnEnter()
        {
            this.DoCheckIsClient();
            base.Finish();
        }

        public override void Reset()
        {
            this.isClient = null;
        }
    }
}

