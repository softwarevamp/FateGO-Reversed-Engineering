namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Lights), HutongGames.PlayMaker.Tooltip("Sets the Color of a Light.")]
    public class SetLightColor : ComponentAction<Light>
    {
        public bool everyFrame;
        [RequiredField, CheckForComponent(typeof(Light))]
        public FsmOwnerDefault gameObject;
        [RequiredField]
        public FsmColor lightColor;

        private void DoSetLightColor()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.light.color = this.lightColor.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetLightColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetLightColor();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.lightColor = Color.white;
            this.everyFrame = false;
        }
    }
}

