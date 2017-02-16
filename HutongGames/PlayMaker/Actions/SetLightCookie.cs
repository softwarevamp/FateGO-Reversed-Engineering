namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Texture projected by a Light."), ActionCategory(ActionCategory.Lights)]
    public class SetLightCookie : ComponentAction<Light>
    {
        [CheckForComponent(typeof(Light)), RequiredField]
        public FsmOwnerDefault gameObject;
        public FsmTexture lightCookie;

        private void DoSetLightCookie()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.light.cookie = this.lightCookie.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetLightCookie();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.lightCookie = null;
        }
    }
}

