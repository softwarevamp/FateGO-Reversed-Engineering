namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("Render/Change Objects Texture"), USequencerFriendlyName("Change Texture")]
    public class USChangeTexture : USEventBase
    {
        public Texture newTexture;
        private Texture previousTexture;

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                if (this.newTexture == null)
                {
                    Debug.LogWarning("you've not given a texture to the USChangeTexture Event", this);
                }
                else if (!Application.isPlaying && Application.isEditor)
                {
                    this.previousTexture = base.AffectedObject.GetComponent<Renderer>().sharedMaterial.mainTexture;
                    base.AffectedObject.GetComponent<Renderer>().sharedMaterial.mainTexture = this.newTexture;
                }
                else
                {
                    this.previousTexture = base.AffectedObject.GetComponent<Renderer>().material.mainTexture;
                    base.AffectedObject.GetComponent<Renderer>().material.mainTexture = this.newTexture;
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
            if ((base.AffectedObject != null) && (this.previousTexture != null))
            {
                if (!Application.isPlaying && Application.isEditor)
                {
                    base.AffectedObject.GetComponent<Renderer>().sharedMaterial.mainTexture = this.previousTexture;
                }
                else
                {
                    base.AffectedObject.GetComponent<Renderer>().material.mainTexture = this.previousTexture;
                }
                this.previousTexture = null;
            }
        }
    }
}

