using System;
using UnityEngine;

public class BattleNpGaugeSystemComponent : MonoBehaviour
{
    public UISprite frameSprite;
    public UISprite fullGauge;
    public bool isPercent = true;
    public UILabel label;
    public int lineCount;
    public int maxparam;
    public int nowparam;
    public UISprite overGauge;
    public int prevparam;
    public UIProgressBar[] sliderlist;

    public void changeParam(int param)
    {
        iTween component = base.gameObject.GetComponent<iTween>();
        if (component != null)
        {
            UnityEngine.Object.Destroy(component);
        }
        object[] args = new object[] { "from", this.prevparam, "to", param, "onupdate", "updateNpGauge", "time", 0.3f };
        iTween.ValueTo(base.gameObject, iTween.Hash(args));
    }

    public void resetSlider()
    {
        if (this.sliderlist != null)
        {
            for (int i = 0; i < this.sliderlist.Length; i++)
            {
                this.sliderlist[i].value = 0f;
            }
        }
        iTween component = base.gameObject.GetComponent<iTween>();
        if (component != null)
        {
            UnityEngine.Object.Destroy(component);
        }
        if (this.fullGauge != null)
        {
            this.fullGauge.gameObject.SetActive(false);
        }
        if (this.overGauge != null)
        {
            this.overGauge.gameObject.SetActive(false);
        }
    }

    public void setLineCount(int count)
    {
        this.lineCount = count;
    }

    public void setMaxParam(int maxparam)
    {
        this.maxparam = maxparam;
    }

    public void setNowParam(int nowparam)
    {
        this.nowparam = nowparam;
        if (this.maxparam != 0)
        {
            this.updateNpGauge(this.nowparam, this.maxparam);
        }
    }

    public void setUseNp(bool flg)
    {
        if (flg)
        {
            this.frameSprite.color = Color.white;
            this.label.color = Color.white;
        }
        else
        {
            this.frameSprite.color = Color.gray;
            this.label.color = Color.gray;
        }
    }

    public void updateNpGauge(int now)
    {
        this.updateNpGauge(now, this.maxparam);
    }

    public void updateNpGauge(int now, int max)
    {
        for (int i = 0; i < this.sliderlist.Length; i++)
        {
            float num2 = (((float) now) / ((float) (max / this.lineCount))) - i;
            float num3 = this.sliderlist[i].value;
            if (num2 <= 0f)
            {
                this.sliderlist[i].value = 0f;
            }
            else if (num2 < 1f)
            {
                this.sliderlist[i].value = num2;
            }
            else
            {
                this.sliderlist[i].value = 1f;
                if (num3 < 1f)
                {
                    SoundManager.playSe("ba16");
                }
            }
            if (this.fullGauge != null)
            {
                this.fullGauge.gameObject.SetActive(max <= now);
            }
        }
        if (this.overGauge != null)
        {
            this.overGauge.gameObject.SetActive(1f <= this.sliderlist[0].value);
        }
        this.prevparam = now;
        if (this.isPercent)
        {
            int num4 = Mathf.FloorToInt(((((float) now) / ((float) max)) * 100f) * this.lineCount);
            this.label.text = string.Format("{0,3}%", num4);
        }
        else
        {
            this.label.text = string.Empty + now;
        }
    }
}

