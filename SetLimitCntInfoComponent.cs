using System;
using UnityEngine;

public class SetLimitCntInfoComponent : MonoBehaviour
{
    public UISprite onImg;

    public void setEnableOnImg(bool isOn)
    {
        this.onImg.gameObject.SetActive(isOn);
    }
}

