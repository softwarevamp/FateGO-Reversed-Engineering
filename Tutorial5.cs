using System;
using UnityEngine;

public class Tutorial5 : MonoBehaviour
{
    public void SetDurationToCurrentProgress()
    {
        foreach (UITweener tweener in base.GetComponentsInChildren<UITweener>())
        {
            tweener.duration = Mathf.Lerp(2f, 0.5f, UIProgressBar.current.value);
        }
    }
}

