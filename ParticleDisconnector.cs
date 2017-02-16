using System;
using UnityEngine;

public class ParticleDisconnector : BaseMonoBehaviour
{
    public ParticleSystem[] particles = new ParticleSystem[0];

    public static ParticleDisconnector DisconnectParticles(Transform globalParent, Transform obj)
    {
        ParticleDisconnector disconnector = globalParent.gameObject.AddComponent<ParticleDisconnector>();
        disconnector.Initialize(obj);
        return disconnector;
    }

    public void Initialize(Transform obj)
    {
        this.particles = obj.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem system in this.particles)
        {
            system.transform.parent = base.transform;
            system.Stop();
        }
    }

    private void OnDestroy()
    {
        foreach (ParticleSystem system in this.particles)
        {
            if (system != null)
            {
                UnityEngine.Object.Destroy(system.gameObject);
            }
        }
    }

    private void Update()
    {
        int num = 0;
        foreach (ParticleSystem system in this.particles)
        {
            if (system != null)
            {
                num += system.particleCount;
            }
        }
        if (num == 0)
        {
            UnityEngine.Object.Destroy(this);
        }
    }
}

