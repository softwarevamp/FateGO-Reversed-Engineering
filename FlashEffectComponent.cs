using System;
using UnityEngine;

public class FlashEffectComponent : ProgramEffectComponent
{
    protected float addVolume;
    [SerializeField]
    protected ExUIMeshRenderer backFlashMesh;
    [SerializeField]
    protected Color flashColor = Color.clear;
    protected bool isWaitEndEffect;
    protected AssetData wipeData;
    [SerializeField]
    protected string wipeName = "circleIn";

    protected void EndLoadWipe(AssetData data)
    {
        if (this.wipeData != null)
        {
            AssetManager.releaseAsset(this.wipeData);
        }
        this.wipeData = data;
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
        this.SetTweenVolume(v);
        TweenRenderVolume volume = TweenRenderVolume.Begin(base.gameObject, base.duration, 0f);
        volume.method = UITweener.Method.EaseIn;
        volume.eventReceiver = base.gameObject;
        volume.callWhenFinished = "OnEndEffect";
    }

    public void FlashStart()
    {
        if (base.duration <= 0f)
        {
            base.duration = 0.5f;
        }
        base.duration = 5f;
        if (base.isSkip)
        {
            ScriptManager.Fade("white", false, 0f);
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
    }

    protected void OnEndEffect()
    {
        ScriptManager.Fade("white", false, 1f);
        this.isWaitEndEffect = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (this.isWaitEndEffect && !ScriptManager.IsBusyFade())
        {
            this.isWaitEndEffect = false;
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        this.backFlashMesh.SetTweenColor(c);
    }

    public override void SetTweenVolume(float v)
    {
        Debug.Log("FlashEffectComponent:SetTweenVolume [" + v + "]");
        base.volume = v;
        this.backFlashMesh.SetTweenVolume(v);
    }
}

