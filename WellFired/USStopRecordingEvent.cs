namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("Recording/Stop Recording"), USequencerFriendlyName("Stop Recording")]
    public class USStopRecordingEvent : USEventBase
    {
        public override void FireEvent()
        {
            if (!Application.isPlaying)
            {
                Debug.Log("Recording events only work when in play mode");
            }
            else
            {
                USRuntimeUtility.StopRecordingSequence();
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

