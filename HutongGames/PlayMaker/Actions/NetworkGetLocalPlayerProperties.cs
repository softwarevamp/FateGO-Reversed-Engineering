namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get the local network player properties"), ActionCategory(ActionCategory.Network)]
    public class NetworkGetLocalPlayerProperties : FsmStateAction
    {
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The external IP address of the network interface. This will only be populated after some external connection has been made.")]
        public FsmString externalIPAddress;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Returns the external port of the network interface. This will only be populated after some external connection has been made.")]
        public FsmInt externalPort;
        [HutongGames.PlayMaker.Tooltip("The GUID for this player, used when connecting with NAT punchthrough."), UIHint(UIHint.Variable)]
        public FsmString guid;
        [HutongGames.PlayMaker.Tooltip("The IP address of this player."), UIHint(UIHint.Variable)]
        public FsmString IpAddress;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The port of this player.")]
        public FsmInt port;

        public override void OnEnter()
        {
            this.IpAddress.Value = Network.player.ipAddress;
            this.port.Value = Network.player.port;
            this.guid.Value = Network.player.guid;
            this.externalIPAddress.Value = Network.player.externalIP;
            this.externalPort.Value = Network.player.externalPort;
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

