using System;
using UnityEngine;

public class CharaCutEffectComponent : ProgramEffectComponent
{
    protected float addVolume;
    protected Vector2 bodySize;
    protected UIImageM image;
    protected bool isCutBusy;
    protected float mgd;
    protected AssetData wipeData;

    public void CutinStart(UIImageM image, string wipeName, float mgd)
    {
        this.isCutBusy = true;
        this.image = image;
        AssetManager.loadAssetStorage("Wipe/" + wipeName, new AssetLoader.LoadEndDataHandler(this.EndLoadWipe));
    }

    public void CutoutStart(float time, bool isSkip)
    {
        if (base.isSkip || (base.duration <= 0f))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            this.isCutBusy = true;
            TweenRenderVolume volume = TweenRenderVolume.Begin(base.gameObject, base.duration, 0f);
            volume.method = UITweener.Method.EaseIn;
            volume.eventReceiver = base.gameObject;
            volume.callWhenFinished = "OnEndCitoutEffect";
        }
    }

    protected void EndLoadWipe(AssetData data)
    {
        if (this.wipeData != null)
        {
            AssetManager.releaseAsset(this.wipeData);
        }
        this.wipeData = data;
        this.bodySize = this.image.GetBodySize();
        if (base.duration > 0f)
        {
            this.image.SetAlpha(1f);
        }
        float mgd = this.mgd;
        float g = (mgd <= 0f) ? 0f : mgd;
        float targetVolume = g + 1.003922f;
        this.addVolume = g;
        this.image.SetSharder("Custom/Sprite-ScriptActionFigureCut");
        this.image.SetWipeTexture(data.GetObject<Texture2D>());
        this.image.SetFilterColor(Color.white);
        this.image.SetGradation(g);
        if (base.isSkip || (base.duration <= 0f))
        {
            this.SetTweenVolume(1f);
            this.isCutBusy = false;
        }
        else
        {
            this.SetTweenVolume(0f);
            TweenRenderVolume volume = TweenRenderVolume.Begin(base.gameObject, base.duration, targetVolume);
            volume.method = UITweener.Method.EaseIn;
            volume.eventReceiver = base.gameObject;
            volume.callWhenFinished = "OnEndCutinEffect";
        }
    }

    public bool IsBusyCut() => 
        this.isCutBusy;

    protected void OnDestroy()
    {
        if (this.wipeData != null)
        {
            AssetManager.releaseAsset(this.wipeData);
            this.wipeData = null;
        }
        if (this.image != null)
        {
            this.image.RecoverSharder();
            this.image.SetAlpha(0f);
            this.image = null;
        }
    }

    protected void OnEndCitoutEffect()
    {
        this.isCutBusy = false;
        UnityEngine.Object.Destroy(base.gameObject);
    }

    protected void OnEndCutinEffect()
    {
        this.isCutBusy = false;
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        this.image.SetTweenColor(c);
    }

    public override void SetTweenVolume(float v)
    {
        Debug.Log("CharaCutEffectComponent:SetTweenVolume [" + v + "]");
        base.volume = v;
        this.image.SetVolume(v);
    }
}

