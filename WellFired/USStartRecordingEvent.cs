namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("Recording/Start Recording"), USequencerFriendlyName("Start Recording")]
    public class USStartRecordingEvent : USEventBase
    {
        public override void FireEvent()
        {
            if (!Application.isPlaying)
            {
                Debug.Log("Recording events only work when in play mode");
            }
            else
            {
                USRuntimeUtility.StartRecordingSequence(base.Sequence, USRecordRuntimePreferences.CapturePath, USRecord.GetFramerate(), USRecord.GetUpscaleAmount());
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

