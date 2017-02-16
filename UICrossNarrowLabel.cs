using System;
using UnityEngine;

public class UICrossNarrowLabel : UILabel
{
    protected int baseWidth;
    protected bool isInit;

    public void SetCrossNarrowText(string text)
    {
        if (!this.isInit)
        {
            this.isInit = true;
            this.baseWidth = base.width;
            base.width = ManagerConfig.WIDTH;
        }
        base.text = text;
        Vector2 printedSize = base.printedSize;
        Vector3 localScale = base.transform.localScale;
        localScale.x = (printedSize.x <= this.baseWidth) ? 1f : (((float) this.baseWidth) / printedSize.x);
        base.transform.localScale = localScale;
    }
}

