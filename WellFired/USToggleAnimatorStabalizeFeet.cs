namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Toggle Stabalize Feet"), USequencerEvent("Animation (Mecanim)/Animator/Toggle Stabalize Feet"), USequencerEventHideDuration]
    public class USToggleAnimatorStabalizeFeet : USEventBase
    {
        private bool prevStabalizeFeet;
        public bool stabalizeFeet = true;

        public override void FireEvent()
        {
            Animator component = base.AffectedObject.GetComponent<Animator>();
            if (component == null)
            {
                Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
            }
            else
            {
                this.prevStabalizeFeet = component.stabilizeFeet;
                component.stabilizeFeet = this.stabalizeFeet;
            }
        }

        public override void ProcessEvent(float runningTime)
        {
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            Animator component = base.AffectedObject.GetComponent<Animator>();
            if (component != null)
            {
                component.stabilizeFeet = this.prevStabalizeFeet;
            }
        }
    }
}

