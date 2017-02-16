namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Destroy the object across the network.\n\nThe object is destroyed locally and remotely.\n\nOptionally remove any RPCs accociated with the object."), ActionCategory(ActionCategory.Network)]
    public class NetworkDestroy : ComponentAction<NetworkView>
    {
        [HutongGames.PlayMaker.Tooltip("The Game Object to destroy.\nNOTE: The Game Object must have a NetworkView attached."), CheckForComponent(typeof(NetworkView)), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Remove all RPC calls associated with the Game Object.")]
        public FsmBool removeRPCs;

        private void DoDestroy()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                if (this.removeRPCs.Value)
                {
                    Network.RemoveRPCs(base.networkView.owner);
                }
                Network.DestroyPlayerObjects(base.networkView.owner);
            }
        }

        public override void OnEnter()
        {
            this.DoDestroy();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.removeRPCs = 1;
        }
    }
}

