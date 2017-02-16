using System;
using System.Collections.Generic;
using UnityEngine;

public class TweenSynchronize : MonoBehaviour
{
    protected UITweener tweener;
    protected static List<UITweener> tweenerList = new List<UITweener>();

    private void Awake()
    {
        this.tweener = base.GetComponent<UITweener>();
        if (!tweenerList.Contains(this.tweener))
        {
            tweenerList.Add(this.tweener);
        }
    }

    private void OnDestroy()
    {
        tweenerList.Remove(this.tweener);
    }

    private void OnEnable()
    {
        this.synchronize();
    }

    public void synchronize()
    {
        foreach (UITweener tweener in tweenerList)
        {
            if ((tweener.gameObject.active && (tweener != this.tweener)) && (this.tweener.duration == tweener.duration))
            {
                this.tweener.SynchronizeTween(tweener);
                break;
            }
        }
    }
}

