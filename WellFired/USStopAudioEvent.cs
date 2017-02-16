namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("Audio/Stop Audio"), USequencerFriendlyName("Stop Audio")]
    public class USStopAudioEvent : USEventBase
    {
        public override void FireEvent()
        {
            if (base.AffectedObject == null)
            {
                Debug.Log("USSequencer is trying to play an audio clip, but you didn't give it Audio To Play from USPlayAudioEvent::FireEvent");
            }
            else
            {
                AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
                if (component == null)
                {
                    Debug.Log("USSequencer is trying to play an audio source, but the GameObject doesn't contain an AudioClip from USPlayAudioEvent::FireEvent");
                }
                else
                {
                    component.Stop();
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

