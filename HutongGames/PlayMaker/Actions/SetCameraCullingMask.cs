namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Camera), HutongGames.PlayMaker.Tooltip("Sets the Culling Mask used by the Camera.")]
    public class SetCameraCullingMask : ComponentAction<Camera>
    {
        [HutongGames.PlayMaker.Tooltip("Cull these layers."), UIHint(UIHint.Layer)]
        public FsmInt[] cullingMask;
        public bool everyFrame;
        [RequiredField, CheckForComponent(typeof(Camera))]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Invert the mask, so you cull all layers except those defined above.")]
        public FsmBool invertMask;

        private void DoSetCameraCullingMask()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.camera.cullingMask = ActionHelpers.LayerArrayToLayerMask(this.cullingMask, this.invertMask.Value);
            }
        }

        public override void OnEnter()
        {
            this.DoSetCameraCullingMask();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetCameraCullingMask();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.cullingMask = new FsmInt[0];
            this.invertMask = 0;
            this.everyFrame = false;
        }
    }
}

