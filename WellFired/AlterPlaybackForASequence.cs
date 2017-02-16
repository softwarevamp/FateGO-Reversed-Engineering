namespace WellFired
{
    using System;
    using UnityEngine;

    public class AlterPlaybackForASequence : MonoBehaviour
    {
        private float runningTime;
        public USSequencer sequence;

        private void Update()
        {
            this.runningTime += Time.deltaTime;
            if ((this.sequence != null) && (this.runningTime <= 5f))
            {
                this.sequence.PlaybackRate -= Time.deltaTime * 1f;
            }
        }
    }
}

