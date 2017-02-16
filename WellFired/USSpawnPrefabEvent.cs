namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Spawn Prefab"), USequencerEvent("Spawn/Spawn Prefab"), USequencerEventHideDuration]
    public class USSpawnPrefabEvent : USEventBase
    {
        public GameObject spawnPrefab;
        public Transform spawnTransform;

        public override void FireEvent()
        {
            if (this.spawnPrefab == null)
            {
                Debug.Log("Attempting to spawn a prefab, but you haven't given a prefab to the event from USSpawnPrefabEvent::FireEvent");
            }
            else if (this.spawnTransform != null)
            {
                UnityEngine.Object.Instantiate(this.spawnPrefab, this.spawnTransform.position, this.spawnTransform.rotation);
            }
            else
            {
                UnityEngine.Object.Instantiate(this.spawnPrefab, Vector3.zero, Quaternion.identity);
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

