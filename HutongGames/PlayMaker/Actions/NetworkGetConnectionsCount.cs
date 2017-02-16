namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Get the number of connected players.\n\nOn a client this returns 1 (the server).")]
    public class NetworkGetConnectionsCount : FsmStateAction
    {
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Number of connected players.")]
        public FsmInt connectionsCount;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public override void OnEnter()
        {
            this.connectionsCount.Value = Network.connections.Length;
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.connectionsCount.Value = Network.connections.Length;
        }

        public override void Reset()
        {
            this.connectionsCount = null;
            this.everyFrame = true;
        }
    }
}

