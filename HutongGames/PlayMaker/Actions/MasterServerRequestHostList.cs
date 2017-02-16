namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Request a host list from the master server.\n\nUse MasterServer Get Host Data to get info on each host in the host list.")]
    public class MasterServerRequestHostList : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("The unique game type name.")]
        public FsmString gameTypeName;
        [HutongGames.PlayMaker.Tooltip("Event sent when the host list has arrived. NOTE: The action will not Finish until the host list arrives.")]
        public FsmEvent HostListArrivedEvent;

        private void DoMasterServerRequestHost()
        {
            MasterServer.ClearHostList();
            MasterServer.RequestHostList(this.gameTypeName.Value);
        }

        public override void OnEnter()
        {
            this.DoMasterServerRequestHost();
        }

        public override void OnUpdate()
        {
            this.WatchServerRequestHost();
        }

        public override void Reset()
        {
            this.gameTypeName = null;
            this.HostListArrivedEvent = null;
        }

        private void WatchServerRequestHost()
        {
            if (MasterServer.PollHostList().Length != 0)
            {
                base.Fsm.Event(this.HostListArrivedEvent);
                base.Finish();
            }
        }
    }
}

