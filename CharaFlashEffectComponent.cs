using System;
using UnityEngine;

public class CharaFlashEffectComponent : ProgramEffectComponent
{
    protected float addVolume;
    [SerializeField]
    protected ExUIMeshRenderer backFlashMesh;
    protected Vector2 bodySize;
    protected CommonEffectComponent childEffect;
    protected UIStandFigureM figure;
    [SerializeField]
    protected Color flashColor = Color.clear;
    protected bool isWaitEndEffect;
    [SerializeField]
    protected GameObject subEffectBase;
    [SerializeField]
    protected string subEffectName;
    protected AssetData wipeData;
    [SerializeField]
    protected string wipeName = "circleIn";

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
        float num2 = (num <= 0f) ? 0f : num;
        float v = num2 + 1.003922f;
        this.addVolume = num2;
        Texture2D tex = data.GetObject<Texture2D>();
        this.backFlashMesh.material = new Material(Shader.Find("Custom/BackFlashSheder"));
        this.backFlashMesh.SetImage(tex);
        this.backFlashMesh.SetTweenColor(this.flashColor);
        if (this.backFlashMesh.material.HasProperty("_Gradation"))
        {
            this.backFlashMesh.material.SetFloat("_Gradation", num2);
        }
        this.figure.SetSharder("Custom/Sprite-ScriptActionFigureFlash");
        this.figure.SetWipeTexture(tex);
        this.figure.SetFilterColor(this.flashColor);
        this.figure.SetGradation(num2);
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

    public void FlashStart(UIStandFigureM figure)
    {
        this.figure = figure;
        if (base.duration <= 0f)
        {
            base.duration = 0.5f;
        }
        base.duration = 5f;
        if (base.isSkip)
        {
            ScriptManager.Fade("white", false, 0f);
            this.figure.SetAlpha(0f);
            this.figure = null;
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            AssetManager.loadAssetStorage("Wipe/" + this.wipeName, new AssetLoader.LoadEndDataHandler(this.EndLoadWipe));
        }
    }

    protected void OnDestroy()
    {
        if (this.wipeData != null)
        {
            UnityEngine.Object.Destroy(this.backFlashMesh.material);
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
        ScriptManager.Fade("white", false, 1f);
        this.isWaitEndEffect = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if ((this.isWaitEndEffect && !ScriptManager.IsBusyFade()) && !CommonEffectManager.IsBusy(this.subEffectBase))
        {
            this.isWaitEndEffect = false;
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        this.figure.SetTweenColor(c);
        this.backFlashMesh.SetTweenColor(c);
    }

    public override void SetTweenVolume(float v)
    {
        Debug.Log("CharaWipeEffectComponent:SetTweenVolume [" + v + "]");
        base.volume = v;
        this.figure.SetVolume(v);
        this.backFlashMesh.SetTweenVolume(v);
    }
}

