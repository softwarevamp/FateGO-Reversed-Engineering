namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Controls whether physics affects the Game Object."), ActionCategory(ActionCategory.Physics)]
    public class SetIsKinematic : ComponentAction<Rigidbody>
    {
        [CheckForComponent(typeof(Rigidbody)), RequiredField]
        public FsmOwnerDefault gameObject;
        [RequiredField]
        public FsmBool isKinematic;

        private void DoSetIsKinematic()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.rigidbody.isKinematic = this.isKinematic.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetIsKinematic();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.isKinematic = 0;
        }
    }
}

