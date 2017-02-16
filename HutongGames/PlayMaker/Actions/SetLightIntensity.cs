namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Intensity of a Light."), ActionCategory(ActionCategory.Lights)]
    public class SetLightIntensity : ComponentAction<Light>
    {
        public bool everyFrame;
        [CheckForComponent(typeof(Light)), RequiredField]
        public FsmOwnerDefault gameObject;
        public FsmFloat lightIntensity;

        private void DoSetLightIntensity()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.light.intensity = this.lightIntensity.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetLightIntensity();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetLightIntensity();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.lightIntensity = 1f;
            this.everyFrame = false;
        }
    }
}

