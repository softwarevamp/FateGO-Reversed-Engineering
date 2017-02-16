using System;
using UnityEngine;

public class CuttingEffectComponent : ProgramEffectComponent
{
    [SerializeField]
    protected ExUIMeshRenderer leftMeshRenderer;
    [SerializeField]
    protected ExUIMeshRenderer rightMeshRenderer;

    public void CuttingStart(Texture texture)
    {
        if (base.duration <= 0f)
        {
            base.duration = 0.5f;
        }
        if (base.isSkip)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            this.leftMeshRenderer.SetImage(texture);
            this.rightMeshRenderer.SetImage(texture);
            this.leftMeshRenderer.SetTweenColor(Color.white);
            this.rightMeshRenderer.SetTweenColor(Color.white);
            Vector3 localPosition = this.leftMeshRenderer.transform.localPosition;
            Vector3 pos = this.rightMeshRenderer.transform.localPosition;
            localPosition.x -= 512f;
            pos.x += 512f;
            TweenPosition position = TweenPosition.Begin(this.leftMeshRenderer.gameObject, base.duration, localPosition);
            TweenPosition position2 = TweenPosition.Begin(this.rightMeshRenderer.gameObject, base.duration, pos);
            position.method = UITweener.Method.EaseIn;
            position2.method = UITweener.Method.EaseIn;
            position.eventReceiver = base.gameObject;
            position.callWhenFinished = "OnEndEffect";
        }
    }

    protected void OnEndEffect()
    {
        UnityEngine.Object.Destroy(base.gameObject);
    }
}

