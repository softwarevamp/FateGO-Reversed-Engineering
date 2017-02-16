namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Warp To Object"), USequencerEventHideDuration, USequencerEvent("Transform/Warp To Object")]
    public class USWarpToObject : USEventBase
    {
        public GameObject objectToWarpTo;
        private Transform previousTransform;
        public bool useObjectRotation;

        public override void FireEvent()
        {
            if (this.objectToWarpTo != null)
            {
                base.AffectedObject.transform.position = this.objectToWarpTo.transform.position;
                if (this.useObjectRotation)
                {
                    base.AffectedObject.transform.rotation = this.objectToWarpTo.transform.rotation;
                }
            }
            else
            {
                Debug.LogError(base.AffectedObject.name + ": No Object attached to WarpToObjectSequencer Script");
            }
            this.previousTransform = base.AffectedObject.transform;
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
            if (this.previousTransform != null)
            {
                base.AffectedObject.transform.position = this.previousTransform.position;
                base.AffectedObject.transform.rotation = this.previousTransform.rotation;
            }
        }
    }
}

