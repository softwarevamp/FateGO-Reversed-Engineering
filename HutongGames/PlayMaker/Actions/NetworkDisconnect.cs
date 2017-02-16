namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Disconnect from the server."), ActionCategory(ActionCategory.Network)]
    public class NetworkDisconnect : FsmStateAction
    {
        public override void OnEnter()
        {
            Network.Disconnect();
            base.Finish();
        }
    }
}

