namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Get the network OnDisconnectedFromServer.")]
    public class NetworkGetNetworkDisconnectionInfos : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The connection to the system has been closed.")]
        public FsmEvent disConnectedEvent;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Disconnection label")]
        public FsmString disconnectionLabel;
        [HutongGames.PlayMaker.Tooltip("The connection to the system has been lost, no reliable packets could be delivered.")]
        public FsmEvent lostConnectionEvent;

        private void doGetNetworkDisconnectionInfo()
        {
            NetworkDisconnection disconnectionInfo = Fsm.EventData.DisconnectionInfo;
            this.disconnectionLabel.Value = disconnectionInfo.ToString();
            switch (disconnectionInfo)
            {
                case NetworkDisconnection.Disconnected:
                    if (this.disConnectedEvent != null)
                    {
                        base.Fsm.Event(this.disConnectedEvent);
                    }
                    break;

                case NetworkDisconnection.LostConnection:
                    if (this.lostConnectionEvent != null)
                    {
                        base.Fsm.Event(this.lostConnectionEvent);
                    }
                    break;
            }
        }

        public override void OnEnter()
        {
            this.doGetNetworkDisconnectionInfo();
            base.Finish();
        }

        public override void Reset()
        {
            this.disconnectionLabel = null;
            this.lostConnectionEvent = null;
            this.disConnectedEvent = null;
        }
    }
}

