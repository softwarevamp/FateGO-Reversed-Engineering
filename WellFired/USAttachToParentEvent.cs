namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Attach Object To Parent"), USequencerEvent("Attach/Attach To Parent"), USequencerEventHideDuration]
    public class USAttachToParentEvent : USEventBase
    {
        private Transform originalParent;
        public Transform parentObject;

        public override void FireEvent()
        {
            if (this.parentObject == null)
            {
                Debug.Log("USAttachEvent has been asked to attach an object, but it hasn't been given a parent from USAttachEvent::FireEvent");
            }
            else
            {
                this.originalParent = base.AffectedObject.transform.parent;
                base.AffectedObject.transform.parent = this.parentObject;
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
            if (base.AffectedObject != null)
            {
                base.AffectedObject.transform.parent = this.originalParent;
            }
        }
    }
}

