namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Set Mecanim Integer"), USequencerEvent("Animation (Mecanim)/Animator/Set Value/Integer"), USequencerEventHideDuration]
    public class USSetAnimatorInteger : USEventBase
    {
        private int hash;
        private int prevValue;
        public int Value;
        public string valueName = string.Empty;

        public override void FireEvent()
        {
            Animator component = base.AffectedObject.GetComponent<Animator>();
            if (component == null)
            {
                Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
            }
            else if (this.valueName.Length == 0)
            {
                Debug.LogWarning("Invalid name passed to the uSequencer Event Set Float", this);
            }
            else
            {
                this.hash = Animator.StringToHash(this.valueName);
                this.prevValue = component.GetInteger(this.hash);
                component.SetInteger(this.hash, this.Value);
            }
        }

        public override void ProcessEvent(float runningTime)
        {
            Animator component = base.AffectedObject.GetComponent<Animator>();
            if (component == null)
            {
                Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
            }
            else if (this.valueName.Length == 0)
            {
                Debug.LogWarning("Invalid name passed to the uSequencer Event Set Float", this);
            }
            else
            {
                this.hash = Animator.StringToHash(this.valueName);
                this.prevValue = component.GetInteger(this.hash);
                component.SetInteger(this.hash, this.Value);
            }
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            Animator component = base.AffectedObject.GetComponent<Animator>();
            if ((component != null) && (this.valueName.Length != 0))
            {
                component.SetInteger(this.hash, this.prevValue);
            }
        }
    }
}

