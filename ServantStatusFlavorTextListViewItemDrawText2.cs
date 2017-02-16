using System;
using UnityEngine;

public class ServantStatusFlavorTextListViewItemDrawText2 : ServantStatusFlavorTextListViewItemDraw
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected BoxCollider baseCollider;
    [SerializeField]
    protected UISprite baseSprite;
    protected bool isFirst = true;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected ShiningIconComponent newIcon;
    [SerializeField]
    protected GameObject titleBase;
    [SerializeField]
    protected UILabel titleLabel;

    public override ServantStatusFlavorTextListViewItemDraw.Kind GetKind() => 
        ServantStatusFlavorTextListViewItemDraw.Kind.TEXT2;

    public override void SetItem(ServantStatusListViewItem item, bool isOpen, bool isNew, string text, string text2, ServantStatusFlavorTextListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, isOpen, isNew, text, text2, mode);
        if (((item != null) && (mode != ServantStatusFlavorTextListViewItemDraw.DispMode.INVISIBLE)) && this.isFirst)
        {
            this.isFirst = false;
            this.titleLabel.text = text2;
            int height = this.messageLabel.height;
            this.messageLabel.height = 0x3e8;
            WrapControlText.textAdjust(this.messageLabel, text);
            Vector2 printedSize = this.messageLabel.printedSize;
            int num2 = ((int) printedSize.y) - height;
            if ((this.baseButton == null) && (this.baseSprite != null))
            {
            }
            if (this.baseCollider != null)
            {
                Vector3 size = this.baseCollider.size;
                size.y += num2;
                this.baseCollider.size = size;
            }
            if (this.baseSprite != null)
            {
                this.baseSprite.height += num2;
            }
            Vector3 localPosition = this.messageLabel.transform.localPosition;
            Vector3 vector4 = this.titleBase.transform.localPosition;
            this.messageLabel.height = (int) printedSize.y;
            localPosition.y = (printedSize.y / 2f) - 14f;
            vector4.y = (printedSize.y / 2f) + 20f;
            this.messageLabel.transform.localPosition = localPosition;
            this.titleBase.transform.localPosition = vector4;
            if (this.newIcon != null)
            {
                this.newIcon.Set(isNew);
            }
        }
    }
}

