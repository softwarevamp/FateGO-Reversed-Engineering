namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get the maximum amount of connections/players allowed."), ActionCategory(ActionCategory.Network)]
    public class NetworkGetMaximumConnections : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Get the maximum amount of connections/players allowed."), UIHint(UIHint.Variable)]
        public FsmInt result;

        public override void OnEnter()
        {
            this.result.Value = Network.maxConnections;
            base.Finish();
        }

        public override void Reset()
        {
            this.result = null;
        }
    }
}

