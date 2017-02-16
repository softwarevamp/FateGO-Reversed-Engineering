using System;
using System.Collections;
using UnityEngine;

public class ObjectScaleEnabler : MonoBehaviour
{
    private bool isChildVisible = true;
    public bool suddenDeath = true;
    public Transform visibleCheckTarget;

    public void OnUpdate()
    {
        Transform visibleCheckTarget = this.visibleCheckTarget;
        if (visibleCheckTarget != null)
        {
            if ((visibleCheckTarget.localScale.z <= 0.001f) && this.isChildVisible)
            {
                if (this.suddenDeath)
                {
                    IEnumerator enumerator = base.transform.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            Transform current = (Transform) enumerator.Current;
                            if (current != this)
                            {
                                current.gameObject.SetActive(false);
                            }
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable == null)
                        {
                        }
                        disposable.Dispose();
                    }
                }
                else
                {
                    CommonEffectComponent component = base.GetComponent<CommonEffectComponent>();
                    if (component != null)
                    {
                        component.Stop(false);
                    }
                    else
                    {
                        foreach (ParticleSystem system in base.GetComponentsInChildren<ParticleSystem>())
                        {
                            system.Stop();
                        }
                        foreach (Animation animation in base.GetComponentsInChildren<Animation>())
                        {
                            animation.Stop();
                        }
                    }
                }
                this.isChildVisible = false;
            }
            else if ((visibleCheckTarget.localScale.z >= 1f) && !this.isChildVisible)
            {
                if (this.suddenDeath)
                {
                    IEnumerator enumerator2 = base.transform.GetEnumerator();
                    try
                    {
                        while (enumerator2.MoveNext())
                        {
                            Transform transform3 = (Transform) enumerator2.Current;
                            if (transform3 != this)
                            {
                                transform3.gameObject.SetActive(true);
                            }
                        }
                    }
                    finally
                    {
                        IDisposable disposable2 = enumerator2 as IDisposable;
                        if (disposable2 == null)
                        {
                        }
                        disposable2.Dispose();
                    }
                }
                else
                {
                    CommonEffectComponent component2 = base.GetComponent<CommonEffectComponent>();
                    if (component2 != null)
                    {
                        component2.ForceStart();
                    }
                    else
                    {
                        foreach (ParticleSystem system2 in base.GetComponentsInChildren<ParticleSystem>())
                        {
                            system2.Play();
                        }
                        foreach (Animation animation2 in base.GetComponentsInChildren<Animation>())
                        {
                            animation2.Play();
                        }
                    }
                }
                this.isChildVisible = true;
            }
        }
    }

    private void Start()
    {
        this.isChildVisible = true;
    }

    private void Update()
    {
        this.OnUpdate();
    }
}

