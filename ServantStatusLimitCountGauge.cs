using System;
using UnityEngine;

public class ServantStatusLimitCountGauge : BaseMonoBehaviour
{
    [SerializeField]
    protected UISprite[] gaugeSpriteList = new UISprite[4];

    public void Set(int limitCount, int limitMax)
    {
        for (int i = 0; i < this.gaugeSpriteList.Length; i++)
        {
            if (i < limitMax)
            {
                this.gaugeSpriteList[i].spriteName = (i >= limitCount) ? "icon_limit_off" : "icon_limit_on";
            }
            else
            {
                this.gaugeSpriteList[i].spriteName = null;
            }
        }
    }
}

