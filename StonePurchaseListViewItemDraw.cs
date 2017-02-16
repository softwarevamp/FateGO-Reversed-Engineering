using System;
using UnityEngine;

public class StonePurchaseListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;
    [SerializeField]
    protected UILabel countTextLabel;
    [SerializeField]
    protected ItemIconComponent itemIcon;
    [SerializeField]
    protected UILabel nameTextLabel;
    [SerializeField]
    protected UISprite onlyOne;
    [SerializeField]
    protected UILabel priceTextLabel;
    [SerializeField]
    protected UILabel priceTitleTextLabel;

    public void SetInput(StonePurchaseListViewItem item, bool isInput)
    {
        if (this.baseButton != null)
        {
            this.baseButton.isEnabled = isInput;
            this.baseButton.SetState(UICommonButtonColor.State.Normal, false);
        }
    }

    public void SetItem(StonePurchaseListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.priceTitleTextLabel.text = string.Empty;
            this.countTextLabel.text = string.Empty;
            string countDetailText = item.CountDetailText;
            if (int.Parse(SingletonMonoBehaviour<DataManager>.Instance.getUserIdEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).getPey()[item.BankShop.id - 1]) < item.BankShop.firstPayId)
            {
                this.onlyOne.gameObject.SetActive(true);
            }
            else
            {
                this.onlyOne.gameObject.SetActive(false);
            }
            if (string.IsNullOrEmpty(countDetailText))
            {
                this.nameTextLabel.text = item.NameText + " " + item.CountText;
            }
            else
            {
                this.nameTextLabel.text = string.Format(countDetailText, item.NameText, item.CountText);
            }
            string priceDetilText = item.PriceDetilText;
            if (string.IsNullOrEmpty(priceDetilText))
            {
                this.priceTextLabel.text = string.Empty + LocalizationManager.GetPrice2Info(item.Price);
            }
            else
            {
                this.priceTextLabel.text = string.Format(priceDetilText, item.Price);
            }
            this.itemIcon.SetItemImage((ImageItem.Id) item.ImageId);
            if (this.baseButton != null)
            {
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

