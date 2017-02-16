namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Clear the host list which was received by MasterServer Request Host List"), ActionCategory(ActionCategory.Network)]
    public class MasterServerClearHostList : FsmStateAction
    {
        public override void OnEnter()
        {
            MasterServer.ClearHostList();
            base.Finish();
        }
    }
}

