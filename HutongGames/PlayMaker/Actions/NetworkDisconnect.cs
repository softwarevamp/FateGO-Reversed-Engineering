namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Disconnect from the server.")]
    public class NetworkDisconnect : FsmStateAction
    {
        public override void OnEnter()
        {
            Network.Disconnect();
            base.Finish();
        }
    }
}

