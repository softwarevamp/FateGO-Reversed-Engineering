using System;
using UnityEngine;

public class MaterialCollectionServantListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UILabel maskLabel;
    [SerializeField]
    protected UISprite maskSprite;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;

    public void SetInput(MaterialCollectionServantListViewItem item, bool isInput)
    {
        bool flag = false;
        if (item != null)
        {
            if (item.IsCanNotSelect)
            {
                flag = true;
            }
        }
        else
        {
            isInput = false;
        }
        if (flag)
        {
            if (this.baseButton != null)
            {
                this.baseButton.isEnabled = true;
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
            }
        }
        else if (this.baseButton != null)
        {
            this.baseButton.isEnabled = true;
            this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
        }
    }

    public void SetItem(MaterialCollectionServantListViewItem item, DispMode mode)
    {
        if ((item != null) && (mode != DispMode.INVISIBLE))
        {
            this.servantFaceIcon.Set(item.UserServantCollection, item.IconInfo);
            if (item.IsEnemyCollectionDetail)
            {
                this.maskSprite.gameObject.SetActive(false);
                this.maskLabel.text = string.Empty;
            }
            else if (item.CollectionKind == CollectionStatus.Kind.FIND)
            {
                this.maskSprite.gameObject.SetActive(true);
                this.maskLabel.text = LocalizationManager.Get(!item.IsServantEquip ? "MATERIAL_FIND_SERVANT" : "MATERIAL_FIND_SERVANT_EQUIP");
            }
            else
            {
                this.maskSprite.gameObject.SetActive(false);
                this.maskLabel.text = string.Empty;
            }
            if (item.IsCanNotSelect)
            {
                if (this.baseButton != null)
                {
                    this.baseButton.isEnabled = true;
                    this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                }
            }
            else if (this.baseButton != null)
            {
                this.baseButton.isEnabled = true;
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
            }
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

