using System;
using UnityEngine;

public class BattleHpGaugeBarComponent : MonoBehaviour
{
    public UIProgressBar damageGauge;
    public UIProgressBar frontGauge;
    public UISprite frontSprite;
    private int maxval;
    private int nowval;
    private int prevval;

    public void completeDamageGauge()
    {
        this.prevval = this.nowval;
        if (this.damageGauge != null)
        {
            this.damageGauge.value = ((float) this.nowval) / ((float) this.maxval);
        }
    }

    public void setInitValue(int now, int max)
    {
        this.maxval = max;
        this.updateNomalGauge(now);
        this.updateDamageGauge(now);
    }

    public bool setValue(int now, int max)
    {
        bool flag = false;
        flag |= now != this.nowval;
        flag |= this.maxval != max;
        this.nowval = now;
        this.maxval = max;
        this.updateNomalGauge(now);
        float num = 0.5f;
        iTween component = base.gameObject.GetComponent<iTween>();
        if (component != null)
        {
            UnityEngine.Object.Destroy(component);
            num = 0.2f;
        }
        object[] args = new object[] { "from", this.prevval, "to", now, "onupdate", "updateDamageGauge", "delay", num, "time", 0.3f };
        iTween.ValueTo(base.gameObject, iTween.Hash(args));
        return flag;
    }

    public void setZero()
    {
        this.updateNomalGauge(0);
        this.updateDamageGauge(0);
    }

    public void updateDamageGauge(int val)
    {
        this.prevval = val;
        if (this.damageGauge != null)
        {
            this.damageGauge.value = ((float) val) / ((float) this.maxval);
        }
    }

    public void updateNomalGauge(int val)
    {
        if (this.frontGauge != null)
        {
            this.frontGauge.value = ((float) val) / ((float) this.maxval);
            if (this.frontSprite != null)
            {
                this.frontSprite.spriteName = (this.frontGauge.value > 0.1f) ? "playerhp_1" : "playerhp_3";
            }
        }
    }
}

