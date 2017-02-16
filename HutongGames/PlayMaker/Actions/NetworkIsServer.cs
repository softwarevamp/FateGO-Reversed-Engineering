namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Test if your peer type is server."), ActionCategory(ActionCategory.Network)]
    public class NetworkIsServer : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Event to send if not running as server.")]
        public FsmEvent isNotServerEvent;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("True if running as server.")]
        public FsmBool isServer;
        [HutongGames.PlayMaker.Tooltip("Event to send if running as server.")]
        public FsmEvent isServerEvent;

        private void DoCheckIsServer()
        {
            this.isServer.Value = Network.isServer;
            if (Network.isServer && (this.isServerEvent != null))
            {
                base.Fsm.Event(this.isServerEvent);
            }
            else if (!Network.isServer && (this.isNotServerEvent != null))
            {
                base.Fsm.Event(this.isNotServerEvent);
            }
        }

        public override void OnEnter()
        {
            this.DoCheckIsServer();
            base.Finish();
        }

        public override void Reset()
        {
            this.isServer = null;
        }
    }
}

