using System;
using UnityEngine;

public class CharaWipeEffectComponent : ProgramEffectComponent
{
    protected float addVolume;
    protected Vector2 bodySize;
    protected CommonEffectComponent childEffect;
    protected UIStandFigureM figure;
    protected bool isWaitEndEffect;
    [SerializeField]
    protected GameObject subEffectBase;
    [SerializeField]
    protected string subEffectName;
    [SerializeField]
    protected Color wipeColor = Color.clear;
    protected AssetData wipeData;
    [SerializeField]
    protected string wipeName = "circleOut";

    protected void EndCreateEffect(CommonEffectComponent effect)
    {
        this.childEffect = effect;
        this.SetTweenVolume(base.volume);
        TweenRenderVolume volume = TweenRenderVolume.Begin(base.gameObject, base.duration, 0f);
        volume.method = UITweener.Method.EaseIn;
        volume.eventReceiver = base.gameObject;
        volume.callWhenFinished = "OnEndEffect";
    }

    protected void EndLoadWipe(AssetData data)
    {
        if (this.wipeData != null)
        {
            AssetManager.releaseAsset(this.wipeData);
        }
        this.wipeData = data;
        this.bodySize = this.figure.GetBodySize();
        float num = 0.2f;
        float g = (num <= 0f) ? 0f : num;
        float v = g + 1.003922f;
        this.addVolume = g;
        this.figure.SetSharder("Custom/Sprite-ScriptActionFigureWipe");
        this.figure.SetWipeTexture(data.GetObject<Texture2D>());
        this.figure.SetFilterColor(this.wipeColor);
        this.figure.SetGradation(g);
        this.SetTweenVolume(v);
        if ((this.subEffectBase != null) && !string.IsNullOrEmpty(this.subEffectName))
        {
            CommonEffectManager.Create(this.subEffectBase, "Talk/" + this.subEffectName, new CommonEffectLoadComponent.LoadEndHandler(this.EndCreateEffect));
        }
        else
        {
            this.EndCreateEffect(null);
        }
    }

    protected void OnDestroy()
    {
        if (this.wipeData != null)
        {
            AssetManager.releaseAsset(this.wipeData);
            this.wipeData = null;
        }
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
        Debug.Log("CharaWipeEffectComponent:SetTweenVolume [" + v + "]");
        base.volume = v;
        this.figure.SetVolume(v);
    }

    public void WipeStart(UIStandFigureM figure)
    {
        this.figure = figure;
        if (base.duration <= 0f)
        {
            base.duration = 0.5f;
        }
        base.duration = 5f;
        if (base.isSkip)
        {
            this.figure.SetAlpha(0f);
            this.figure = null;
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            AssetManager.loadAssetStorage("Wipe/" + this.wipeName, new AssetLoader.LoadEndDataHandler(this.EndLoadWipe));
        }
    }
}

