namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Set Layer Weight"), USequencerEvent("Animation (Mecanim)/Animator/Set Layer Weight"), USequencerEventHideDuration]
    public class USSetAnimatorLayerWeight : USEventBase
    {
        public int layerIndex = -1;
        public float layerWeight = 1f;
        private float prevLayerWeight;

        public override void FireEvent()
        {
            Animator component = base.AffectedObject.GetComponent<Animator>();
            if (component == null)
            {
                Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
            }
            else if (this.layerIndex < 0)
            {
                Debug.LogWarning("Set Animator Layer weight, incorrect index : " + this.layerIndex);
            }
            else
            {
                this.prevLayerWeight = component.GetLayerWeight(this.layerIndex);
                component.SetLayerWeight(this.layerIndex, this.layerWeight);
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
            if ((component != null) && (this.layerIndex >= 0))
            {
                component.SetLayerWeight(this.layerIndex, this.prevLayerWeight);
            }
        }
    }
}

