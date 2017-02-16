using System;
using UnityEngine;

public class SetItemVolumeControl : MonoBehaviour
{
    public UISliderWithButton itemSlider;
    public ShopBuyBulkItemConfirmMenu menu;

    public void getChangeValue()
    {
        this.menu.sliderValueChange();
    }

    public void initSetVolume()
    {
        this.itemSlider.value = 0f;
    }

    public void reflectionVolume()
    {
    }

    private void setChangeValue()
    {
    }
}

