﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Get the number of hosts on the master server.\n\nUse MasterServer Get Host Data to get host data at a specific index.")]
    public class MasterServerGetHostCount : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The number of hosts on the MasterServer."), UIHint(UIHint.Variable), RequiredField]
        public FsmInt count;

        public override void OnEnter()
        {
            this.count.Value = MasterServer.PollHostList().Length;
            base.Finish();
        }
    }
}

