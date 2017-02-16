namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Background Color used by the Camera."), ActionCategory(ActionCategory.Camera)]
    public class SetBackgroundColor : ComponentAction<Camera>
    {
        [RequiredField]
        public FsmColor backgroundColor;
        public bool everyFrame;
        [CheckForComponent(typeof(Camera)), RequiredField]
        public FsmOwnerDefault gameObject;

        private void DoSetBackgroundColor()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.camera.backgroundColor = this.backgroundColor.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetBackgroundColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetBackgroundColor();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.backgroundColor = Color.black;
            this.everyFrame = false;
        }
    }
}

