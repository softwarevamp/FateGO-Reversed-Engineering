namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Animation (Mecanim)/Animator/Set Value/Float"), USequencerEventHideDuration, USequencerFriendlyName("Set Mecanim Float")]
    public class USSetAnimatorFloat : USEventBase
    {
        private int hash;
        private float prevValue;
        public float Value;
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
                this.prevValue = component.GetFloat(this.hash);
                component.SetFloat(this.hash, this.Value);
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
                this.prevValue = component.GetFloat(this.hash);
                component.SetFloat(this.hash, this.Value);
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
                component.SetFloat(this.hash, this.prevValue);
            }
        }
    }
}

