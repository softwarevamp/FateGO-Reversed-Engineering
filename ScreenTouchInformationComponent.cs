using System;
using UnityEngine;

public class ScreenTouchInformationComponent : MonoBehaviour
{
    public const float ANIM_TIME = 2f;
    [SerializeField]
    private UISprite overSp;

    private void Awake()
    {
        GameObject gameObject = this.overSp.gameObject;
        TweenScale component = gameObject.GetComponent<TweenScale>();
        if (component != null)
        {
            component.duration = 2f;
        }
        TweenAlpha alpha = gameObject.GetComponent<TweenAlpha>();
        if (alpha != null)
        {
            alpha.duration = 2f;
        }
    }

    public UISprite GetOverSprite() => 
        this.overSp;
}

