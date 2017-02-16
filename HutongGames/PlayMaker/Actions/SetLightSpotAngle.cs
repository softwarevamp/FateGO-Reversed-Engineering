namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Spot Angle of a Light."), ActionCategory(ActionCategory.Lights)]
    public class SetLightSpotAngle : ComponentAction<Light>
    {
        public bool everyFrame;
        [RequiredField, CheckForComponent(typeof(Light))]
        public FsmOwnerDefault gameObject;
        public FsmFloat lightSpotAngle;

        private void DoSetLightRange()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.light.spotAngle = this.lightSpotAngle.Value;
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
            this.lightSpotAngle = 20f;
            this.everyFrame = false;
        }
    }
}

