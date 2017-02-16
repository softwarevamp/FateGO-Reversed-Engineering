using System;
using UnityEngine;

public class ServantStatusListViewItemDrawFlavorText : ServantStatusListViewItemDraw
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
    protected GameObject titleBase;

    public override ServantStatusListViewItemDraw.Kind GetKind() => 
        ServantStatusListViewItemDraw.Kind.FLAVOR_TEXT;

    public override void SetItem(ServantStatusListViewItem item, ServantStatusListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, mode);
        if (((item != null) && (mode != ServantStatusListViewItemDraw.DispMode.INVISIBLE)) && this.isFirst)
        {
            this.isFirst = false;
            ServantCommentEntity[] servantCommentDataList = item.ServantCommentDataList;
            string comment = string.Empty;
            for (int i = 0; i < servantCommentDataList.Length; i++)
            {
                ServantCommentEntity entity = servantCommentDataList[i];
                if (entity.IsConst())
                {
                    comment = entity.comment;
                    break;
                }
            }
            int height = this.messageLabel.height;
            this.messageLabel.height = 0x3e8;
            this.messageLabel.text = comment;
            Vector2 printedSize = this.messageLabel.printedSize;
            int num3 = ((int) printedSize.y) - height;
            if (this.baseButton != null)
            {
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
            }
            else if (this.baseSprite != null)
            {
            }
            if (this.baseCollider != null)
            {
                Vector3 size = this.baseCollider.size;
                size.y += num3;
                this.baseCollider.size = size;
            }
            if (this.baseSprite != null)
            {
                this.baseSprite.height += num3;
            }
            Vector3 localPosition = this.messageLabel.transform.localPosition;
            Vector3 vector4 = this.titleBase.transform.localPosition;
            this.messageLabel.height = (int) printedSize.y;
            localPosition.y = (printedSize.y / 2f) - 14f;
            vector4.y = (printedSize.y / 2f) + 20f;
            this.messageLabel.transform.localPosition = localPosition;
            this.titleBase.transform.localPosition = vector4;
        }
    }
}

