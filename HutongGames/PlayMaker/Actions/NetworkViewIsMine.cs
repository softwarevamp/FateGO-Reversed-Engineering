namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Test if the Network View is controlled by a GameObject.")]
    public class NetworkViewIsMine : FsmStateAction
    {
        private NetworkView _networkView;
        [RequiredField, CheckForComponent(typeof(NetworkView)), HutongGames.PlayMaker.Tooltip("The Game Object with the NetworkView attached.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("True if the network view is controlled by this object."), UIHint(UIHint.Variable)]
        public FsmBool isMine;
        [HutongGames.PlayMaker.Tooltip("Send this event if the network view controlled by this object.")]
        public FsmEvent isMineEvent;
        [HutongGames.PlayMaker.Tooltip("Send this event if the network view is NOT controlled by this object.")]
        public FsmEvent isNotMineEvent;

        private void _getNetworkView()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                this._networkView = ownerDefaultTarget.GetComponent<NetworkView>();
            }
        }

        private void checkIsMine()
        {
            if (this._networkView != null)
            {
                bool isMine = this._networkView.isMine;
                this.isMine.Value = isMine;
                if (isMine)
                {
                    if (this.isMineEvent != null)
                    {
                        base.Fsm.Event(this.isMineEvent);
                    }
                }
                else if (this.isNotMineEvent != null)
                {
                    base.Fsm.Event(this.isNotMineEvent);
                }
            }
        }

        public override void OnEnter()
        {
            this._getNetworkView();
            this.checkIsMine();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.isMine = null;
            this.isMineEvent = null;
            this.isNotMineEvent = null;
        }
    }
}

