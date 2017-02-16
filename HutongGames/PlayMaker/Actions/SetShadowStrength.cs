namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Lights), HutongGames.PlayMaker.Tooltip("Sets the strength of the shadows cast by a Light.")]
    public class SetShadowStrength : ComponentAction<Light>
    {
        public bool everyFrame;
        [CheckForComponent(typeof(Light)), RequiredField]
        public FsmOwnerDefault gameObject;
        public FsmFloat shadowStrength;

        private void DoSetShadowStrength()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.light.shadowStrength = this.shadowStrength.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetShadowStrength();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetShadowStrength();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.shadowStrength = 0.8f;
            this.everyFrame = false;
        }
    }
}

