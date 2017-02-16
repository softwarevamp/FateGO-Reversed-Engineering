using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class ProgramEffectComponent : UITweenRenderer
{
    protected float duration;
    protected Color effectColor = Color.white;
    [SerializeField]
    protected string effectName;
    protected bool isSkip;
    protected float totalTime;

    public void Init(float time, Color color, bool isSkip = false)
    {
        this.duration = time;
        this.effectColor = color;
        this.isSkip = isSkip;
        base.gameObject.name = "Effect(" + this.effectName + ")";
        if (base.transform.parent != null)
        {
            this.SetChildInit(base.transform, base.transform.parent.gameObject.layer);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        this.totalTime += Time.deltaTime;
    }

    protected void SetChildInit(Transform tf, int layer)
    {
        if (tf.gameObject.layer != layer)
        {
            tf.gameObject.layer = layer;
            IEnumerator enumerator = tf.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    this.SetChildInit(current, layer);
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
    }

    public virtual void Stop()
    {
        UnityEngine.Object.Destroy(base.gameObject);
    }

    public string EffectName =>
        this.effectName;
}

