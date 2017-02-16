namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Camera), HutongGames.PlayMaker.Tooltip("Sets Field of View used by the Camera.")]
    public class SetCameraFOV : ComponentAction<Camera>
    {
        public bool everyFrame;
        [RequiredField]
        public FsmFloat fieldOfView;
        [RequiredField, CheckForComponent(typeof(Camera))]
        public FsmOwnerDefault gameObject;

        private void DoSetCameraFOV()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.camera.fieldOfView = this.fieldOfView.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetCameraFOV();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetCameraFOV();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.fieldOfView = 50f;
            this.everyFrame = false;
        }
    }
}

