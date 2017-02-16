namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Connect to a server.")]
    public class NetworkConnect : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Event to send in case of an error connecting to the server."), ActionSection("Errors")]
        public FsmEvent errorEvent;
        [HutongGames.PlayMaker.Tooltip("Store the error string in a variable."), UIHint(UIHint.Variable)]
        public FsmString errorString;
        [HutongGames.PlayMaker.Tooltip("Optional password for the server.")]
        public FsmString password;
        [HutongGames.PlayMaker.Tooltip("IP address of the host. Either a dotted IP address or a domain name."), RequiredField]
        public FsmString remoteIP;
        [HutongGames.PlayMaker.Tooltip("The port on the remote machine to connect to."), RequiredField]
        public FsmInt remotePort;

        public override void OnEnter()
        {
            NetworkConnectionError error = Network.Connect(this.remoteIP.Value, this.remotePort.Value, this.password.Value);
            if (error != NetworkConnectionError.NoError)
            {
                this.errorString.Value = error.ToString();
                this.LogError(this.errorString.Value);
                base.Fsm.Event(this.errorEvent);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.remoteIP = "127.0.0.1";
            this.remotePort = 0x61a9;
            this.password = string.Empty;
            this.errorEvent = null;
            this.errorString = null;
        }
    }
}

