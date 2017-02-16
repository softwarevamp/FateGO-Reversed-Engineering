namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Culling Mask used by the Camera."), ActionCategory(ActionCategory.Camera)]
    public class SetCameraCullingMask : ComponentAction<Camera>
    {
        [UIHint(UIHint.Layer), HutongGames.PlayMaker.Tooltip("Cull these layers.")]
        public FsmInt[] cullingMask;
        public bool everyFrame;
        [CheckForComponent(typeof(Camera)), RequiredField]
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

