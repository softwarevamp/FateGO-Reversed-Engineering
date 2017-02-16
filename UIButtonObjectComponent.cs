using System;
using UnityEngine;

public class UIButtonObjectComponent : MonoBehaviour
{
    public UISprite buttonsprite;
    public Collider col;
    public UILabel textlabel;

    public void setActive(bool flg)
    {
        if (this.col != null)
        {
            this.col.enabled = flg;
        }
        if (this.buttonsprite != null)
        {
            this.buttonsprite.color = !flg ? Color.gray : Color.white;
        }
        if (this.textlabel != null)
        {
            this.textlabel.color = !flg ? Color.gray : Color.white;
        }
    }
}

