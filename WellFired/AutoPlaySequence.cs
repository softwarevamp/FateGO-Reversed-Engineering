namespace WellFired
{
    using System;
    using UnityEngine;

    public class AutoPlaySequence : MonoBehaviour
    {
        private float currentTime;
        public float delay = 1f;
        private bool hasPlayed;
        public USSequencer sequence;

        private void Start()
        {
            if (this.sequence == null)
            {
                Debug.LogError("You have added an AutoPlaySequence, however you haven't assigned it a sequence", base.gameObject);
            }
        }

        private void Update()
        {
            if (!this.hasPlayed)
            {
                this.currentTime += Time.deltaTime;
                if ((this.currentTime >= this.delay) && (this.sequence != null))
                {
                    this.sequence.Play();
                    this.hasPlayed = true;
                }
            }
        }
    }
}

