namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("FGO Set Camera"), USequencerEvent("FGO/Camera/Set Camera")]
    public class USFGOSetCameraEvent : USEventBase
    {
        public Camera camera;
        public Transform cameraRoot;
        private Transform CameraTarget;
        public CameraPosition camPosition;
        private Transform originalParent;
        private Vector3 previousPosition;
        private Quaternion previousRotation;
        private Vector3 previousScale;

        public override void FireEvent()
        {
            this.CameraTarget = null;
            if (this.camera == null)
            {
                Debug.Log("Animation Camera not found from USFGOAnimationCamera.FireEvent");
            }
            else
            {
                CameraPosition camPosition = this.camPosition;
                this.originalParent = this.camera.transform.parent;
                this.previousPosition = this.camera.transform.localPosition;
                this.previousRotation = this.camera.transform.localRotation;
                this.previousScale = this.camera.transform.localScale;
                this.camera.transform.parent = this.cameraRoot;
                Debug.Log("L:CAMPOS:" + camPosition);
                Transform transform = SingletonMonoBehaviour<FGOSequenceManager>.Instance.getCameraTransform(camPosition.DisplayName());
                this.CameraTarget = transform;
                this.camera.transform.position = transform.position;
                this.camera.transform.rotation = transform.rotation;
                this.camera.transform.localScale = Vector3.one;
                base.Duration = 1f;
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            if (this.CameraTarget != null)
            {
                this.camera.transform.position = this.CameraTarget.position;
                this.camera.transform.rotation = this.CameraTarget.rotation;
                this.camera.transform.localScale = Vector3.one;
            }
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (this.camera != null)
            {
                this.camera.transform.parent = this.originalParent;
                this.camera.transform.localPosition = this.previousPosition;
                this.camera.transform.localRotation = this.previousRotation;
                this.camera.transform.localScale = this.previousScale;
            }
        }

        public void Update()
        {
            base.Duration = 1f;
        }
    }
}

