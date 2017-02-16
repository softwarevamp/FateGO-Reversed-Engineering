namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Remove the RPC function calls accociated with a Game Object.\n\nNOTE: The Game Object must have a NetworkView component attached."), ActionCategory(ActionCategory.Network)]
    public class NetworkViewRemoveRPCs : ComponentAction<NetworkView>
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("Remove the RPC function calls accociated with this Game Object.\n\nNOTE: The GameObject must have a NetworkView component attached."), CheckForComponent(typeof(NetworkView))]
        public FsmOwnerDefault gameObject;

        private void DoRemoveRPCsFromViewID()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                Network.RemoveRPCs(base.networkView.viewID);
            }
        }

        public override void OnEnter()
        {
            this.DoRemoveRPCsFromViewID();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
        }
    }
}

