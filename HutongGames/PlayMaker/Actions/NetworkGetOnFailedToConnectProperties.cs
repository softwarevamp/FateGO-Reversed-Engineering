namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get the network OnFailedToConnect or MasterServer OnFailedToConnectToMasterServer connection error message."), ActionCategory(ActionCategory.Network)]
    public class NetworkGetOnFailedToConnectProperties : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Cannot connect to two servers at once. Close the connection before connecting again.")]
        public FsmEvent AlreadyConnectedToAnotherServerEvent;
        [HutongGames.PlayMaker.Tooltip("We are already connected to this particular server (can happen after fast disconnect/reconnect).")]
        public FsmEvent AlreadyConnectedToServerEvent;
        [HutongGames.PlayMaker.Tooltip("We are banned from the system we attempted to connect to (likely temporarily).")]
        public FsmEvent ConnectionBannedEvent;
        [HutongGames.PlayMaker.Tooltip("onnection attempt failed, possibly because of internal connectivity problems.")]
        public FsmEvent ConnectionFailedEvent;
        [HutongGames.PlayMaker.Tooltip("Internal error while attempting to initialize network interface. Socket possibly already in use.")]
        public FsmEvent CreateSocketOrThreadFailureEvent;
        [HutongGames.PlayMaker.Tooltip("No host target given in Connect.")]
        public FsmEvent EmptyConnectTargetEvent;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Error label")]
        public FsmString errorLabel;
        [HutongGames.PlayMaker.Tooltip("Incorrect parameters given to Connect function.")]
        public FsmEvent IncorrectParametersEvent;
        [HutongGames.PlayMaker.Tooltip("Client could not connect internally to same network NAT enabled server.")]
        public FsmEvent InternalDirectConnectFailedEvent;
        [HutongGames.PlayMaker.Tooltip("The server is using a password and has refused our connection because we did not set the correct password.")]
        public FsmEvent InvalidPasswordEvent;
        [HutongGames.PlayMaker.Tooltip("NAT punchthrough attempt has failed. The cause could be a too restrictive NAT implementation on either endpoints.")]
        public FsmEvent NATPunchthroughFailedEvent;
        [HutongGames.PlayMaker.Tooltip("Connection lost while attempting to connect to NAT target.")]
        public FsmEvent NATTargetConnectionLostEvent;
        [HutongGames.PlayMaker.Tooltip("The NAT target we are trying to connect to is not connected to the facilitator server.")]
        public FsmEvent NATTargetNotConnectedEvent;
        [HutongGames.PlayMaker.Tooltip("No error occurred.")]
        public FsmEvent NoErrorEvent;
        [HutongGames.PlayMaker.Tooltip("We presented an RSA public key which does not match what the system we connected to is using.")]
        public FsmEvent RSAPublicKeyMismatchEvent;
        [HutongGames.PlayMaker.Tooltip("The server is at full capacity, failed to connect.")]
        public FsmEvent TooManyConnectedPlayersEvent;

        private void doGetNetworkErrorInfo()
        {
            NetworkConnectionError connectionError = Fsm.EventData.ConnectionError;
            this.errorLabel.Value = connectionError.ToString();
            NetworkConnectionError error2 = connectionError;
            switch (error2)
            {
                case NetworkConnectionError.ConnectionFailed:
                    if (this.ConnectionFailedEvent != null)
                    {
                        base.Fsm.Event(this.ConnectionFailedEvent);
                    }
                    return;

                case NetworkConnectionError.AlreadyConnectedToServer:
                    if (this.AlreadyConnectedToServerEvent != null)
                    {
                        base.Fsm.Event(this.AlreadyConnectedToServerEvent);
                    }
                    return;

                case NetworkConnectionError.TooManyConnectedPlayers:
                    if (this.TooManyConnectedPlayersEvent != null)
                    {
                        base.Fsm.Event(this.TooManyConnectedPlayersEvent);
                    }
                    return;

                case NetworkConnectionError.RSAPublicKeyMismatch:
                    if (this.RSAPublicKeyMismatchEvent != null)
                    {
                        base.Fsm.Event(this.RSAPublicKeyMismatchEvent);
                    }
                    return;

                case NetworkConnectionError.ConnectionBanned:
                    if (this.ConnectionBannedEvent != null)
                    {
                        base.Fsm.Event(this.ConnectionBannedEvent);
                    }
                    return;

                case NetworkConnectionError.InvalidPassword:
                    if (this.InvalidPasswordEvent != null)
                    {
                        base.Fsm.Event(this.InvalidPasswordEvent);
                    }
                    return;
            }
            switch ((error2 + 5))
            {
                case NetworkConnectionError.NoError:
                    if (this.InternalDirectConnectFailedEvent != null)
                    {
                        base.Fsm.Event(this.InternalDirectConnectFailedEvent);
                    }
                    break;

                case ~NetworkConnectionError.CreateSocketOrThreadFailure:
                    if (this.EmptyConnectTargetEvent != null)
                    {
                        base.Fsm.Event(this.EmptyConnectTargetEvent);
                    }
                    break;

                case ~NetworkConnectionError.IncorrectParameters:
                    if (this.IncorrectParametersEvent != null)
                    {
                        base.Fsm.Event(this.IncorrectParametersEvent);
                    }
                    break;

                case ~NetworkConnectionError.EmptyConnectTarget:
                    if (this.CreateSocketOrThreadFailureEvent != null)
                    {
                        base.Fsm.Event(this.CreateSocketOrThreadFailureEvent);
                    }
                    break;

                case ~NetworkConnectionError.InternalDirectConnectFailed:
                    if (this.AlreadyConnectedToAnotherServerEvent != null)
                    {
                        base.Fsm.Event(this.AlreadyConnectedToAnotherServerEvent);
                    }
                    break;

                case ((NetworkConnectionError) 5):
                    if (this.NoErrorEvent != null)
                    {
                        base.Fsm.Event(this.NoErrorEvent);
                    }
                    break;

                default:
                    switch (error2)
                    {
                        case NetworkConnectionError.NATTargetNotConnected:
                            if (this.NATTargetNotConnectedEvent != null)
                            {
                                base.Fsm.Event(this.NATTargetNotConnectedEvent);
                            }
                            break;

                        case NetworkConnectionError.NATTargetConnectionLost:
                            if (this.NATTargetConnectionLostEvent != null)
                            {
                                base.Fsm.Event(this.NATTargetConnectionLostEvent);
                            }
                            break;

                        case NetworkConnectionError.NATPunchthroughFailed:
                            if (this.NATPunchthroughFailedEvent != null)
                            {
                                base.Fsm.Event(this.NoErrorEvent);
                            }
                            break;
                    }
                    break;
            }
        }

        public override void OnEnter()
        {
            this.doGetNetworkErrorInfo();
            base.Finish();
        }

        public override void Reset()
        {
            this.errorLabel = null;
            this.NoErrorEvent = null;
            this.RSAPublicKeyMismatchEvent = null;
            this.InvalidPasswordEvent = null;
            this.ConnectionFailedEvent = null;
            this.TooManyConnectedPlayersEvent = null;
            this.ConnectionBannedEvent = null;
            this.AlreadyConnectedToServerEvent = null;
            this.AlreadyConnectedToAnotherServerEvent = null;
            this.CreateSocketOrThreadFailureEvent = null;
            this.IncorrectParametersEvent = null;
            this.EmptyConnectTargetEvent = null;
            this.InternalDirectConnectFailedEvent = null;
            this.NATTargetNotConnectedEvent = null;
            this.NATTargetConnectionLostEvent = null;
            this.NATPunchthroughFailedEvent = null;
        }
    }
}

