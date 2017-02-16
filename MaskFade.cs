using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MaskFade : BaseMonoBehaviour
{
    protected System.Action callbackFunc;
    protected bool isExecuteMask;
    [SerializeField]
    protected UISprite maskBase;
    protected Kind maskKind;

    protected void EndFadein()
    {
        TweenAlpha component = this.maskBase.gameObject.GetComponent<TweenAlpha>();
        if (component != null)
        {
            component.enabled = false;
        }
        this.maskBase.alpha = 0f;
        this.maskKind = Kind.NONE;
        this.maskBase.GetComponent<Collider>().enabled = false;
        this.isExecuteMask = false;
        System.Action callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    protected void EndFadeout()
    {
        this.maskBase.alpha = 1f;
        this.isExecuteMask = false;
        System.Action callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    public bool Fadein(float duration, System.Action callback = null)
    {
        if (this.isExecuteMask)
        {
            TweenAlpha component = this.maskBase.gameObject.GetComponent<TweenAlpha>();
            if ((component != null) && (component.to == 0f))
            {
                if (callback != null)
                {
                    callback();
                }
                return false;
            }
        }
        this.callbackFunc = callback;
        this.maskKind = Kind.NONE;
        if (duration > 0f)
        {
            TweenAlpha alpha2 = TweenAlpha.Begin(this.maskBase.gameObject, duration, 0f);
            if (alpha2 != null)
            {
                alpha2.eventReceiver = base.gameObject;
                alpha2.callWhenFinished = "EndFadein";
                this.isExecuteMask = true;
                return true;
            }
        }
        this.EndFadein();
        return true;
    }

    public bool Fadeout(Kind kind, float duration, System.Action callback = null)
    {
        if (this.isExecuteMask)
        {
            TweenAlpha component = this.maskBase.gameObject.GetComponent<TweenAlpha>();
            if ((component != null) && (component.to == 1f))
            {
                if (callback != null)
                {
                    callback();
                }
                return false;
            }
        }
        this.callbackFunc = callback;
        float num = this.maskBase.alpha;
        Kind kind2 = kind;
        if (kind2 != Kind.BLACK)
        {
            if (kind2 != Kind.WHITE)
            {
                return false;
            }
        }
        else
        {
            this.maskBase.color = Color.black;
            goto Label_009A;
        }
        this.maskBase.color = Color.white;
    Label_009A:
        this.maskKind = kind;
        this.maskBase.GetComponent<Collider>().enabled = true;
        if (duration > 0f)
        {
            this.maskBase.alpha = num;
            TweenAlpha alpha2 = TweenAlpha.Begin(this.maskBase.gameObject, duration, 1f);
            if (alpha2 != null)
            {
                this.isExecuteMask = true;
                alpha2.eventReceiver = base.gameObject;
                alpha2.callWhenFinished = "EndFadeout";
                return true;
            }
        }
        this.EndFadeout();
        return true;
    }

    public Kind GetFadeoutKind()
    {
        if (this.isExecuteMask)
        {
            return Kind.NONE;
        }
        return this.maskKind;
    }

    public void Init()
    {
        if (this.isExecuteMask)
        {
            TweenAlpha component = this.maskBase.GetComponent<TweenAlpha>();
            if (component != null)
            {
                component.enabled = false;
            }
            this.isExecuteMask = false;
        }
        this.maskBase.alpha = 0f;
        this.maskKind = Kind.NONE;
        this.maskBase.GetComponent<Collider>().enabled = false;
    }

    public bool IsBusy() => 
        this.isExecuteMask;

    public enum Kind
    {
        NONE,
        BLACK,
        WHITE
    }
}

