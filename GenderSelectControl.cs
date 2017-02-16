using System;
using UnityEngine;

public class GenderSelectControl : MonoBehaviour
{
    public UISprite genderImg;
    public int genderType;
    private int idx;

    public int getGenderType() => 
        this.genderType;

    public int getIdx() => 
        this.idx;

    public void setEnableGenderImg(bool isShow)
    {
        this.genderImg.gameObject.SetActive(isShow);
    }

    public void setIdx(int childIdx)
    {
        this.idx = childIdx;
    }
}

