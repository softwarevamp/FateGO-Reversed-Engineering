namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Connect to a server.")]
    public class NetworkConnect : FsmStateAction
    {
        [ActionSection("Errors"), HutongGames.PlayMaker.Tooltip("Event to send in case of an error connecting to the server.")]
        public FsmEvent errorEvent;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Store the error string in a variable.")]
        public FsmString errorString;
        [HutongGames.PlayMaker.Tooltip("Optional password for the server.")]
        public FsmString password;
        [RequiredField, HutongGames.PlayMaker.Tooltip("IP address of the host. Either a dotted IP address or a domain name.")]
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

