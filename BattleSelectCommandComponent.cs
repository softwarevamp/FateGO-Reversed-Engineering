using System;
using UnityEngine;

public class BattleSelectCommandComponent : MonoBehaviour
{
    private static readonly string[] frameFileList = new string[] { "commandcard_select_1st", "commandcard_select_2nd", "commandcard_select_3rd" };
    public UISprite markSprite;

    public void setIndex(int index)
    {
        this.markSprite.spriteName = frameFileList[index];
        TweenScale component = this.markSprite.gameObject.GetComponent<TweenScale>();
        if (component != null)
        {
            component.enabled = true;
        }
    }
}

