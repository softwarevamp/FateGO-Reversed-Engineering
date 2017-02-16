using System;
using UnityEngine;

public class FavoriteChangeListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite lockSprite;
    [SerializeField]
    protected FlashingIconComponent partyIcon;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;
    [SerializeField]
    protected UILabel statusLb;

    public void SetInput(FavoriteChangeListViewItem item, bool isInput)
    {
        if ((item != null) && (this.baseButton != null))
        {
            this.baseButton.GetComponent<Collider>().enabled = isInput;
            this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
        }
    }

    public void SetItem(FavoriteChangeListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.servantFaceIcon.Set(item.UserServant, item.IconInfo);
            this.statusLb.gameObject.SetActive(false);
            if (this.partyIcon != null)
            {
                this.partyIcon.Set(item.IsParty);
            }
            if (this.lockSprite != null)
            {
                this.lockSprite.gameObject.SetActive(false);
            }
            if (item.IsFavorite)
            {
                this.statusLb.gameObject.SetActive(true);
                this.statusLb.text = LocalizationManager.Get("MYROOM_FAVORITE_STATUS");
            }
            if (item.IsLock)
            {
                this.lockSprite.gameObject.SetActive(true);
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

