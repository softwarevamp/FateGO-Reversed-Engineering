namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get the next connected player properties. \nEach time this action is called it gets the next child of a GameObject.This lets you quickly loop through all the connected player to perform actions on them."), ActionCategory(ActionCategory.Network)]
    public class NetworkGetNextConnectedPlayerProperties : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Get the external IP address of the network interface. This will only be populated after some external connection has been made."), UIHint(UIHint.Variable)]
        public FsmString externalIPAddress;
        [HutongGames.PlayMaker.Tooltip("Get the external port of the network interface. This will only be populated after some external connection has been made."), UIHint(UIHint.Variable)]
        public FsmInt externalPort;
        [HutongGames.PlayMaker.Tooltip("Event to send when there are no more children.")]
        public FsmEvent finishedEvent;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the GUID for this player, used when connecting with NAT punchthrough.")]
        public FsmString guid;
        [ActionSection("Result"), UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The player connection index.")]
        public FsmInt index;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the IP address of this player.")]
        public FsmString IpAddress;
        [HutongGames.PlayMaker.Tooltip("Event to send for looping."), ActionSection("Set up")]
        public FsmEvent loopEvent;
        private int nextItemIndex;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the port of this player.")]
        public FsmInt port;

        private void DoGetNextPlayerProperties()
        {
            if (this.nextItemIndex >= Network.connections.Length)
            {
                base.Fsm.Event(this.finishedEvent);
                this.nextItemIndex = 0;
            }
            else
            {
                NetworkPlayer player = Network.connections[this.nextItemIndex];
                this.index.Value = this.nextItemIndex;
                this.IpAddress.Value = player.ipAddress;
                this.port.Value = player.port;
                this.guid.Value = player.guid;
                this.externalIPAddress.Value = player.externalIP;
                this.externalPort.Value = player.externalPort;
                if (this.nextItemIndex >= Network.connections.Length)
                {
                    base.Fsm.Event(this.finishedEvent);
                    this.nextItemIndex = 0;
                }
                else
                {
                    this.nextItemIndex++;
                    if (this.loopEvent != null)
                    {
                        base.Fsm.Event(this.loopEvent);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetNextPlayerProperties();
            base.Finish();
        }

        public override void Reset()
        {
            this.finishedEvent = null;
            this.loopEvent = null;
            this.index = null;
            this.IpAddress = null;
            this.port = null;
            this.guid = null;
            this.externalIPAddress = null;
            this.externalPort = null;
        }
    }
}

