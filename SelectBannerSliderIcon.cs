using System;
using UnityEngine;

public class SelectBannerSliderIcon : MonoBehaviour
{
    public UISprite offImg;
    public UISprite onImg;

    public void setEnableOffImg(bool isOff)
    {
        this.offImg.gameObject.SetActive(isOff);
    }

    public void setEnableOnImg(bool isOn)
    {
        this.onImg.gameObject.SetActive(isOn);
    }
}

