namespace WellFired
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public class SequenceTrigger : MonoBehaviour
    {
        public bool isMainCameraTrigger;
        public bool isPlayerTrigger;
        public USSequencer sequenceToPlay;
        public GameObject triggerObject;

        private void OnTriggerEnter(Collider other)
        {
            if (this.sequenceToPlay == null)
            {
                Debug.LogWarning("You have triggered a sequence in your scene, however, you didn't assign it a Sequence To Play", base.gameObject);
            }
            else if (!this.sequenceToPlay.IsPlaying)
            {
                if (other.CompareTag("MainCamera") && this.isMainCameraTrigger)
                {
                    this.sequenceToPlay.Play();
                }
                else if (other.CompareTag("Player") && this.isPlayerTrigger)
                {
                    this.sequenceToPlay.Play();
                }
                else if (other.gameObject == this.triggerObject)
                {
                    this.sequenceToPlay.Play();
                }
            }
        }
    }
}

