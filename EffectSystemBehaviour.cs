using System;
using UnityEngine;

[ExecuteInEditMode]
public class EffectSystemBehaviour : MonoBehaviour
{
    public void Awake()
    {
        if ((base.GetComponent<ParticleSystem>() != null) && (base.gameObject.GetComponent<EffectRenderBehaviour>() == null))
        {
            base.gameObject.AddComponent<EffectRenderBehaviour>();
        }
        ParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleSystem>();
        if (componentsInChildren != null)
        {
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                ParticleSystem system2 = componentsInChildren[i];
                if (system2.gameObject.GetComponent<EffectRenderBehaviour>() == null)
                {
                    system2.gameObject.AddComponent<EffectRenderBehaviour>();
                }
            }
        }
    }
}

