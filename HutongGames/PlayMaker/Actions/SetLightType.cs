namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Lights), HutongGames.PlayMaker.Tooltip("Set Spot, Directional, or Point Light type.")]
    public class SetLightType : ComponentAction<Light>
    {
        [RequiredField, CheckForComponent(typeof(Light))]
        public FsmOwnerDefault gameObject;
        public LightType lightType;

        private void DoSetLightType()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.light.type = this.lightType;
            }
        }

        public override void OnEnter()
        {
            this.DoSetLightType();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.lightType = LightType.Point;
        }
    }
}

