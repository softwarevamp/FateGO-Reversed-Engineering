using System;
using UnityEngine;

public class UIExtrusionLabel : UILabel
{
    [SerializeField]
    protected int extrusionBlankSize = 2;
    [SerializeField]
    protected GameObject extrusionObject;

    public string text
    {
        get => 
            base.text;
        set
        {
            base.text = value;
            if (this.extrusionObject != null)
            {
                Vector2 printedSize = base.printedSize;
                Vector3 localPosition = this.extrusionObject.transform.localPosition;
                NGUIText.Alignment alignment = base.alignment;
                if (alignment == NGUIText.Alignment.Left)
                {
                    localPosition.x = printedSize.x + this.extrusionBlankSize;
                }
                else if (alignment == NGUIText.Alignment.Center)
                {
                    localPosition.x = (printedSize.x / 2f) + this.extrusionBlankSize;
                }
                else
                {
                    localPosition.x = this.extrusionBlankSize;
                }
                this.extrusionObject.transform.localPosition = localPosition;
            }
        }
    }
}

