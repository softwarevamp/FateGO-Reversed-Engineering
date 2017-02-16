using System;

public class StonePurchaseListViewItem : ListViewItem
{
    protected BankShopEntity bankShop;
    protected ItemEntity entity;

    public StonePurchaseListViewItem(int index, BankShopEntity bankShop) : base(index)
    {
        this.bankShop = bankShop;
        this.entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(2);
    }

    ~StonePurchaseListViewItem()
    {
    }

    private string ToString() => 
        ("Item " + this.NameText);

    public BankShopEntity BankShop =>
        this.bankShop;

    public string CountDetailText
    {
        get
        {
            if (this.bankShop == null)
            {
                return string.Empty;
            }
            if (int.Parse(SingletonMonoBehaviour<DataManager>.Instance.getUserIdEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).getPey()[this.bankShop.id - 1]) >= this.bankShop.firstPayId)
            {
                return this.bankShop.numDetail;
            }
            return this.bankShop.getNumDetail;
        }
    }

    public string CountText
    {
        get
        {
            if (this.bankShop != null)
            {
                return this.bankShop.GetCountText(false);
            }
            if (this.entity != null)
            {
                return this.entity.GetCountText(1);
            }
            return "error";
        }
    }

    public string DetailText
    {
        get
        {
            if (this.entity != null)
            {
                return this.entity.detail;
            }
            return "error";
        }
    }

    public int ImageId =>
        ((this.entity == null) ? 0 : this.entity.imageId);

    public string NameText
    {
        get
        {
            if (this.bankShop != null)
            {
                return this.bankShop.name;
            }
            if (this.entity != null)
            {
                return this.entity.name;
            }
            return "error";
        }
    }

    public int Price
    {
        get
        {
            if (this.bankShop != null)
            {
                return this.bankShop.GetPrice();
            }
            if (this.entity != null)
            {
                return this.entity.GetPrice();
            }
            return 0;
        }
    }

    public string PriceDetilText
    {
        get
        {
            if (this.bankShop == null)
            {
                return string.Empty;
            }
            if (int.Parse(SingletonMonoBehaviour<DataManager>.Instance.getUserIdEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).getPey()[this.bankShop.id - 1]) >= this.bankShop.firstPayId)
            {
                return this.bankShop.priceDetail;
            }
            return this.bankShop.getPriceDetail;
        }
    }
}

