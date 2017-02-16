namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Animation (Mecanim)/Animator/Toggle Apply Root Motion"), USequencerEventHideDuration, USequencerFriendlyName("Toggle Apply Root Motion")]
    public class USToggleAnimatorApplyRootMotion : USEventBase
    {
        public bool applyRootMotion = true;
        private bool prevApplyRootMotion;

        public override void FireEvent()
        {
            Animator component = base.AffectedObject.GetComponent<Animator>();
            if (component == null)
            {
                Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
            }
            else
            {
                this.prevApplyRootMotion = component.applyRootMotion;
                component.applyRootMotion = this.applyRootMotion;
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
                component.applyRootMotion = this.prevApplyRootMotion;
            }
        }
    }
}

