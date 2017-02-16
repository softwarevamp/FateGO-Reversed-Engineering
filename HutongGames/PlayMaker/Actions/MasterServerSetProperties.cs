﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Set the IP address, port, update rate and dedicated server flag of the master server.")]
    public class MasterServerSetProperties : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Set if this machine is a dedicated server.")]
        public FsmBool dedicatedServer;
        [HutongGames.PlayMaker.Tooltip("Set the IP address of the master server.")]
        public FsmString ipAddress;
        [HutongGames.PlayMaker.Tooltip("Set the connection port of the master server.")]
        public FsmInt port;
        [HutongGames.PlayMaker.Tooltip("Set the minimum update rate for master server host information update. Default is 60 seconds.")]
        public FsmInt updateRate;

        public override void OnEnter()
        {
            this.SetMasterServerProperties();
            base.Finish();
        }

        public override void Reset()
        {
            this.ipAddress = "127.0.0.1";
            this.port = 0x2712;
            this.updateRate = 60;
            this.dedicatedServer = 0;
        }

        private void SetMasterServerProperties()
        {
            MasterServer.ipAddress = this.ipAddress.Value;
            MasterServer.port = this.port.Value;
            MasterServer.updateRate = this.updateRate.Value;
            MasterServer.dedicatedServer = this.dedicatedServer.Value;
        }
    }
}

