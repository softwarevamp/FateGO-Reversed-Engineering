namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Range of a Light."), ActionCategory(ActionCategory.Lights)]
    public class SetLightRange : ComponentAction<Light>
    {
        public bool everyFrame;
        [CheckForComponent(typeof(Light)), RequiredField]
        public FsmOwnerDefault gameObject;
        public FsmFloat lightRange;

        private void DoSetLightRange()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.light.range = this.lightRange.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetLightRange();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetLightRange();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.lightRange = 20f;
            this.everyFrame = false;
        }
    }
}

