namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Unregister this server from the master server.\n\nDoes nothing if the server is not registered or has already unregistered.")]
    public class MasterServerUnregisterHost : FsmStateAction
    {
        public override void OnEnter()
        {
            MasterServer.UnregisterHost();
            base.Finish();
        }
    }
}

