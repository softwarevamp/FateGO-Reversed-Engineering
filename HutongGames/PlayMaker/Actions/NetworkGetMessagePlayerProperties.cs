namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Get the network OnPlayerConnected or OnPlayerDisConnected message player info.")]
    public class NetworkGetMessagePlayerProperties : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Get the external IP address of the network interface. This will only be populated after some external connection has been made."), UIHint(UIHint.Variable)]
        public FsmString externalIPAddress;
        [HutongGames.PlayMaker.Tooltip("Get the external port of the network interface. This will only be populated after some external connection has been made."), UIHint(UIHint.Variable)]
        public FsmInt externalPort;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the GUID for this connected player, used when connecting with NAT punchthrough.")]
        public FsmString guid;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the IP address of this connected player.")]
        public FsmString IpAddress;
        [HutongGames.PlayMaker.Tooltip("Get the port of this connected player."), UIHint(UIHint.Variable)]
        public FsmInt port;

        private void doGetOnPLayerConnectedProperties()
        {
            NetworkPlayer player = Fsm.EventData.Player;
            Debug.Log("hello " + player.ipAddress);
            this.IpAddress.Value = player.ipAddress;
            this.port.Value = player.port;
            this.guid.Value = player.guid;
            this.externalIPAddress.Value = player.externalIP;
            this.externalPort.Value = player.externalPort;
            base.Finish();
        }

        public override void OnEnter()
        {
            this.doGetOnPLayerConnectedProperties();
            base.Finish();
        }

        public override void Reset()
        {
            this.IpAddress = null;
            this.port = null;
            this.guid = null;
            this.externalIPAddress = null;
            this.externalPort = null;
        }
    }
}

