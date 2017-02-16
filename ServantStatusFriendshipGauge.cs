using System;
using UnityEngine;

public class ServantStatusFriendshipGauge : BaseMonoBehaviour
{
    [SerializeField]
    protected UISlider[] gaugeSliderList = new UISlider[5];
    [SerializeField]
    protected UILabel latePointLabel;
    [SerializeField]
    protected UIExtrusionLabel levelLabel;
    [SerializeField]
    protected UILabel maxLevelLabel;

    public void Set(int count, int max, int late, float fraction)
    {
        this.levelLabel.text = string.Empty + count;
        this.maxLevelLabel.text = string.Empty + max;
        this.latePointLabel.text = (late <= 0) ? string.Empty : (string.Empty + LocalizationManager.GetNumberFormat(late));
        for (int i = 0; i < this.gaugeSliderList.Length; i++)
        {
            UISlider slider = this.gaugeSliderList[i];
            if (i < max)
            {
                slider.gameObject.SetActive(true);
                if (i < count)
                {
                    slider.value = 1f;
                }
                else if (i == count)
                {
                    slider.value = fraction;
                }
                else
                {
                    slider.value = 0f;
                }
            }
            else
            {
                slider.gameObject.SetActive(false);
            }
        }
    }
}

