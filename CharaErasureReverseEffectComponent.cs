using System;
using UnityEngine;

public class CharaErasureReverseEffectComponent : ProgramEffectComponent
{
    protected float addVolume;
    protected Vector2 bodySize;
    protected CommonEffectComponent childEffect;
    [SerializeField]
    protected Color erasureColor = Color.clear;
    protected UIStandFigureM figure;
    protected bool isWaitEndEffect;
    [SerializeField]
    protected GameObject subEffectBase;
    [SerializeField]
    protected string subEffectName;

    protected void EndCreateEffect(CommonEffectComponent effect)
    {
        this.childEffect = effect;
        this.SetTweenVolume(base.volume);
        TweenRenderVolume volume = TweenRenderVolume.Begin(base.gameObject, base.duration, 0f);
        volume.method = UITweener.Method.EaseIn;
        volume.eventReceiver = base.gameObject;
        volume.callWhenFinished = "OnEndEffect";
    }

    public void ErasureStart(UIStandFigureM figure)
    {
        this.figure = figure;
        if (base.duration <= 0f)
        {
            base.duration = 2f;
        }
        this.bodySize = this.figure.GetBodySize();
        float num = 0.2f;
        float g = (num <= 0f) ? 0f : num;
        float v = g + 1.003922f;
        this.addVolume = g;
        this.figure.SetSharder("Custom/Sprite-ScriptActionFigureErasureReverse");
        this.figure.SetFilterColor(this.erasureColor);
        this.figure.SetGradation(g);
        this.SetTweenVolume(v);
        if (base.isSkip)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else if ((this.subEffectBase != null) && !string.IsNullOrEmpty(this.subEffectName))
        {
            Vector3 localPosition = this.subEffectBase.transform.localPosition;
            Vector3 vector2 = base.transform.parent.localPosition;
            this.subEffectBase.transform.localPosition = new Vector3(-vector2.x, -vector2.y, localPosition.x);
            CommonEffectManager.Create(this.subEffectBase, "Talk/" + this.subEffectName, new CommonEffectLoadComponent.LoadEndHandler(this.EndCreateEffect));
        }
        else
        {
            this.EndCreateEffect(null);
        }
    }

    protected void OnDestroy()
    {
        if (this.figure != null)
        {
            this.figure.RecoverSharder();
            this.figure.SetAlpha(0f);
            this.figure = null;
        }
    }

    protected void OnEndEffect()
    {
        CommonEffectManager.Stop(this.subEffectBase, false, false);
        this.isWaitEndEffect = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (this.isWaitEndEffect && !CommonEffectManager.IsBusy(this.subEffectBase))
        {
            this.isWaitEndEffect = false;
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        this.figure.SetTweenColor(c);
    }

    public override void SetTweenVolume(float v)
    {
        base.volume = v;
        this.figure.SetVolume(v);
        if (this.childEffect != null)
        {
            float num = base.volume - this.addVolume;
            if (num > 0f)
            {
                this.childEffect.transform.localPosition = new Vector3(0f, this.bodySize.y * -num, 0f);
            }
            else if (this.childEffect != null)
            {
                this.childEffect.Stop(true);
                this.childEffect = null;
            }
        }
    }
}

