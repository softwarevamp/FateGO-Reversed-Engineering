namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Launch a server."), ActionCategory(ActionCategory.Network)]
    public class NetworkInitializeServer : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("The number of allowed incoming connections/number of players allowed in the game.")]
        public FsmInt connections;
        [HutongGames.PlayMaker.Tooltip("Event to send in case of an error creating the server."), ActionSection("Errors")]
        public FsmEvent errorEvent;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Store the error string in a variable.")]
        public FsmString errorString;
        [HutongGames.PlayMaker.Tooltip("Sets the password for the server. This must be matched in the NetworkConnect action.")]
        public FsmString incomingPassword;
        [HutongGames.PlayMaker.Tooltip("The port number we want to listen to."), RequiredField]
        public FsmInt listenPort;
        [HutongGames.PlayMaker.Tooltip("Run the server in the background, even if it doesn't have focus.")]
        public FsmBool runInBackground;
        [HutongGames.PlayMaker.Tooltip("Sets the NAT punchthrough functionality.")]
        public FsmBool useNAT;
        [HutongGames.PlayMaker.Tooltip("Unity handles the network layer by providing secure connections if you wish to use them. \nMost games will want to use secure connections. However, they add up to 15 bytes per packet and take time to compute so you may wish to limit usage to deployed games only.")]
        public FsmBool useSecurityLayer;

        public override void OnEnter()
        {
            Network.incomingPassword = this.incomingPassword.Value;
            if (this.useSecurityLayer.Value)
            {
                Network.InitializeSecurity();
            }
            if (this.runInBackground.Value)
            {
                Application.runInBackground = true;
            }
            NetworkConnectionError error = Network.InitializeServer(this.connections.Value, this.listenPort.Value, this.useNAT.Value);
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
            this.connections = 0x20;
            this.listenPort = 0x61a9;
            this.incomingPassword = string.Empty;
            this.errorEvent = null;
            this.errorString = null;
            this.useNAT = 0;
            this.useSecurityLayer = 0;
            this.runInBackground = 1;
        }
    }
}

