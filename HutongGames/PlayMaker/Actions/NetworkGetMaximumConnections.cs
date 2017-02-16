namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Get the maximum amount of connections/players allowed.")]
    public class NetworkGetMaximumConnections : FsmStateAction
    {
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Get the maximum amount of connections/players allowed.")]
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

