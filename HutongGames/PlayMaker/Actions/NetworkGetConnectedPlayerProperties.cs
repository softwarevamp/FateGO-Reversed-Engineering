namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get connected player properties."), ActionCategory(ActionCategory.Network)]
    public class NetworkGetConnectedPlayerProperties : FsmStateAction
    {
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the external IP address of the network interface. This will only be populated after some external connection has been made.")]
        public FsmString externalIPAddress;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the external port of the network interface. This will only be populated after some external connection has been made.")]
        public FsmInt externalPort;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the GUID for this player, used when connecting with NAT punchthrough.")]
        public FsmString guid;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The player connection index.")]
        public FsmInt index;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the IP address of this player."), ActionSection("Result")]
        public FsmString IpAddress;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the port of this player.")]
        public FsmInt port;

        private void getPlayerProperties()
        {
            int index = this.index.Value;
            if ((index < 0) || (index >= Network.connections.Length))
            {
                this.LogError("Player index out of range");
            }
            else
            {
                NetworkPlayer player = Network.connections[index];
                this.IpAddress.Value = player.ipAddress;
                this.port.Value = player.port;
                this.guid.Value = player.guid;
                this.externalIPAddress.Value = player.externalIP;
                this.externalPort.Value = player.externalPort;
            }
        }

        public override void OnEnter()
        {
            this.getPlayerProperties();
            base.Finish();
        }

        public override void Reset()
        {
            this.index = null;
            this.IpAddress = null;
            this.port = null;
            this.guid = null;
            this.externalIPAddress = null;
            this.externalPort = null;
        }
    }
}

