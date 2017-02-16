namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get the IP address, port, update rate and dedicated server flag of the master server and store in variables."), ActionCategory(ActionCategory.Network)]
    public class MasterServerGetProperties : FsmStateAction
    {
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Flag to report if this machine is a dedicated server.")]
        public FsmBool dedicatedServer;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The IP address of the master server.")]
        public FsmString ipAddress;
        [HutongGames.PlayMaker.Tooltip("Event sent if this machine is a dedicated server")]
        public FsmEvent isDedicatedServerEvent;
        [HutongGames.PlayMaker.Tooltip("Event sent if this machine is not a dedicated server")]
        public FsmEvent isNotDedicatedServerEvent;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The connection port of the master server.")]
        public FsmInt port;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The minimum update rate for master server host information update. Default is 60 seconds")]
        public FsmInt updateRate;

        private void GetMasterServerProperties()
        {
            this.ipAddress.Value = MasterServer.ipAddress;
            this.port.Value = MasterServer.port;
            this.updateRate.Value = MasterServer.updateRate;
            bool dedicatedServer = MasterServer.dedicatedServer;
            this.dedicatedServer.Value = dedicatedServer;
            if (dedicatedServer && (this.isDedicatedServerEvent != null))
            {
                base.Fsm.Event(this.isDedicatedServerEvent);
            }
            if (!dedicatedServer && (this.isNotDedicatedServerEvent != null))
            {
                base.Fsm.Event(this.isNotDedicatedServerEvent);
            }
        }

        public override void OnEnter()
        {
            this.GetMasterServerProperties();
            base.Finish();
        }

        public override void Reset()
        {
            this.ipAddress = null;
            this.port = null;
            this.updateRate = null;
            this.dedicatedServer = null;
            this.isDedicatedServerEvent = null;
            this.isNotDedicatedServerEvent = null;
        }
    }
}

