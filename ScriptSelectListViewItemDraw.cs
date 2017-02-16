using System;
using UnityEngine;

[AddComponentMenu("ScriptAction/ScriptSelect/ScriptSelectListViewItemDraw")]
public class ScriptSelectListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UIMessageButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;
    protected System.Action callbackFunc;
    [SerializeField]
    protected ScriptLineMessage effectMessageManager;
    protected string message;
    [SerializeField]
    protected ScriptLineMessage messageManager;

    protected void EndMove()
    {
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    public void EndSelectDecideMove1()
    {
        if (this.message != null)
        {
            this.effectMessageManager.Fadeout(0.2f);
        }
        base.Invoke("EndSelectDecideMove2", 0.5f);
    }

    public void EndSelectDecideMove2()
    {
        TweenAlpha.Begin(this.baseSprite.gameObject, 0.6f, 0f).method = UITweener.Method.EaseOutQuad;
        this.baseButton.Fadeout(0.6f);
        base.Invoke("EndMove", 0.6f);
    }

    public void NoSelectDecide(System.Action callback)
    {
        this.callbackFunc = callback;
        this.baseButton.UpdateColor(true, false);
        this.baseButton.GetComponent<Collider>().enabled = false;
        TweenAlpha.Begin(this.baseSprite.gameObject, 0.5f, 0f).method = UITweener.Method.EaseOutQuad;
        this.baseButton.Fadeout(0.5f);
        base.Invoke("EndMove", 0.5f);
    }

    public void SelectDecide(System.Action callback)
    {
        this.callbackFunc = callback;
        this.baseButton.UpdateColor(true, false);
        this.baseButton.GetComponent<Collider>().enabled = false;
        if (this.message != null)
        {
            this.effectMessageManager.SetText(this.message);
            this.effectMessageManager.EffectScale(2f, 0.3f);
        }
        base.Invoke("EndSelectDecideMove1", 0.1f);
    }

    public void SetItem(ScriptSelectListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            this.messageManager.DeleteLabels();
            this.effectMessageManager.DeleteLabels();
            this.message = null;
        }
        else
        {
            this.message = item.MessageText;
            if (mode != DispMode.INVISIBLE)
            {
                if (this.message != null)
                {
                    this.messageManager.SetText(this.message);
                }
                else
                {
                    this.messageManager.DeleteLabels();
                }
                this.effectMessageManager.DeleteLabels();
            }
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

