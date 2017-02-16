namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerFriendlyName("Set Culling Mask"), USequencerEvent("Camera/Set Culling Mask"), ExecuteInEditMode]
    public class USCameraSetCullingMask : USEventBase
    {
        private Camera cameraToAffect;
        [SerializeField]
        private LayerMask newLayerMask;
        private int prevLayerMask;

        public override void EndEvent()
        {
        }

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                this.cameraToAffect = base.AffectedObject.GetComponent<Camera>();
            }
            if (this.cameraToAffect != null)
            {
                this.prevLayerMask = this.cameraToAffect.cullingMask;
                this.cameraToAffect.cullingMask = (int) this.newLayerMask;
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (this.cameraToAffect != null)
            {
                this.cameraToAffect.cullingMask = this.prevLayerMask;
            }
        }
    }
}

