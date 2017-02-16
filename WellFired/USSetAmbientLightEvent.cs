namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("Light/Set Ambient Light"), USequencerFriendlyName("Set Ambient Light")]
    public class USSetAmbientLightEvent : USEventBase
    {
        public Color lightColor = Color.red;
        private Color prevLightColor;

        public override void FireEvent()
        {
            this.prevLightColor = RenderSettings.ambientLight;
            RenderSettings.ambientLight = this.lightColor;
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
            RenderSettings.ambientLight = this.prevLightColor;
        }
    }
}

