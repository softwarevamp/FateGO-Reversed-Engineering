namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Render/Change Objects Color"), USequencerEventHideDuration, USequencerFriendlyName("Change Color")]
    public class USChangeColor : USEventBase
    {
        public Color newColor;
        private Color previousColor;

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                if (!Application.isPlaying && Application.isEditor)
                {
                    this.previousColor = base.AffectedObject.GetComponent<Renderer>().sharedMaterial.color;
                    base.AffectedObject.GetComponent<Renderer>().sharedMaterial.color = this.newColor;
                }
                else
                {
                    this.previousColor = base.AffectedObject.GetComponent<Renderer>().material.color;
                    base.AffectedObject.GetComponent<Renderer>().material.color = this.newColor;
                }
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
                if (!Application.isPlaying && Application.isEditor)
                {
                    base.AffectedObject.GetComponent<Renderer>().sharedMaterial.color = this.previousColor;
                }
                else
                {
                    base.AffectedObject.GetComponent<Renderer>().material.color = this.previousColor;
                }
            }
        }
    }
}

