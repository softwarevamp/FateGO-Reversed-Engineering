namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Transform/Rotate Object"), USequencerFriendlyName("Rotate")]
    public class USRotateObjectEvent : USEventBase
    {
        private Quaternion previousRotation = Quaternion.identity;
        public float rotateSpeedPerSecond = 90f;
        public Vector3 rotationAxis = Vector3.up;
        private Quaternion sourceOrientation = Quaternion.identity;

        public override void FireEvent()
        {
            this.previousRotation = base.AffectedObject.transform.rotation;
            this.sourceOrientation = base.AffectedObject.transform.rotation;
        }

        public override void ProcessEvent(float deltaTime)
        {
            base.AffectedObject.transform.rotation = this.sourceOrientation;
            base.AffectedObject.transform.Rotate(this.rotationAxis, (float) (this.rotateSpeedPerSecond * deltaTime));
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (base.AffectedObject != null)
            {
                base.AffectedObject.transform.rotation = this.previousRotation;
            }
        }

        public void Update()
        {
            if (base.Duration < 0f)
            {
                base.Duration = 4f;
            }
        }
    }
}

