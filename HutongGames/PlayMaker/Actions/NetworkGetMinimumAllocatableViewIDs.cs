﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Get the minimum number of ViewID numbers in the ViewID pool given to clients by the server. The default value is 100.\n\nThe ViewID pools are given to each player as he connects and are refreshed with new numbers if the player runs out. The server and clients should be in sync regarding this value.\n\nSetting this higher only on the server has the effect that he sends more view ID numbers to clients, than they really want.\n\nSetting this higher only on clients means they request more view IDs more often, for example twice in a row, as the pools received from the server don't contain enough numbers. ")]
    public class NetworkGetMinimumAllocatableViewIDs : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Get the minimum number of ViewID numbers in the ViewID pool given to clients by the server. The default value is 100."), UIHint(UIHint.Variable)]
        public FsmInt result;

        public override void OnEnter()
        {
            this.result.Value = Network.minimumAllocatableViewIDs;
            base.Finish();
        }

        public override void Reset()
        {
            this.result = null;
        }
    }
}

