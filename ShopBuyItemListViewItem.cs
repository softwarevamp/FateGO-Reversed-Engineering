using System;
using System.Runtime.InteropServices;

public class ShopBuyItemListViewItem : ListViewItem
{
    protected BankShopEntity bankShopEntity;
    protected ItemEntity[] eventItemEntity;
    protected ItemEntity itemEntity;
    protected ShopEntity shopEntity;
    protected StoneShopEntity stoneShopEntity;
    protected int totalNum;

    public ShopBuyItemListViewItem(int index, BankShopEntity bankShop) : base(index)
    {
        this.eventItemEntity = null;
        this.itemEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(2);
        this.shopEntity = null;
        this.stoneShopEntity = null;
        this.bankShopEntity = bankShop;
        this.totalNum = 0;
    }

    public ShopBuyItemListViewItem(int index, ShopEntity shop) : base(index)
    {
        ItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM);
        switch (shop.purchaseType)
        {
            case 1:
                this.itemEntity = master.getEntityFromId<ItemEntity>(shop.targetId);
                break;

            case 2:
                this.itemEntity = master.getEntityFromId<ItemEntity>(2);
                break;

            case 3:
                this.itemEntity = master.getEntityFromId<ItemEntity>(2);
                break;
        }
        this.shopEntity = shop;
        this.stoneShopEntity = null;
        this.bankShopEntity = null;
        UserShopEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserShopMaster>(DataNameKind.Kind.USER_SHOP).getEntityFromId(NetworkManager.UserId, shop.id);
        if (shop.GetItemID() >= 0)
        {
            int itemCount = shop.GetItemCount();
            this.eventItemEntity = new ItemEntity[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                this.eventItemEntity[i] = master.getEntityFromId<ItemEntity>(shop.GetItemIDs(i));
            }
        }
        else
        {
            this.eventItemEntity = null;
        }
        this.totalNum = entity.num;
    }

    public ShopBuyItemListViewItem(int index, StoneShopEntity stoneShop) : base(index)
    {
        this.eventItemEntity = null;
        this.itemEntity = null;
        this.shopEntity = null;
        this.stoneShopEntity = stoneShop;
        this.bankShopEntity = null;
        this.totalNum = 0;
    }

    public int eventPrice(int num) => 
        this.shopEntity.GetPrices(num);

    ~ShopBuyItemListViewItem()
    {
    }

    public bool GetSendType(out bool isTake, out bool isSend)
    {
        isTake = false;
        isSend = false;
        if (this.shopEntity == null)
        {
            return false;
        }
        int buyItemNum = 0;
        int buyServantNum = 0;
        int buyServantEquipNum = 0;
        this.shopEntity.GetSum(out buyItemNum, out buyServantNum, out buyServantEquipNum);
        if (buyItemNum > 0)
        {
            isTake = true;
        }
        if ((buyServantNum + buyServantEquipNum) > 0)
        {
            isSend = true;
        }
        return true;
    }

    public bool IsExchangeQP() => 
        ((this.itemEntity != null) && (this.itemEntity.type == 1));

    public bool IsPreparation(out string message)
    {
        if (this.shopEntity != null)
        {
            return this.shopEntity.IsPreparation(out message);
        }
        message = null;
        return true;
    }

    public void Modify(ShopEntity shop)
    {
        ItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM);
        switch (shop.purchaseType)
        {
            case 1:
                this.itemEntity = master.getEntityFromId<ItemEntity>(shop.targetId);
                break;

            case 2:
                this.itemEntity = master.getEntityFromId<ItemEntity>(2);
                break;

            case 3:
                this.itemEntity = master.getEntityFromId<ItemEntity>(2);
                break;
        }
        if (shop.GetItemID() >= 0)
        {
            int itemCount = shop.GetItemCount();
            this.eventItemEntity = new ItemEntity[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                this.eventItemEntity[i] = master.getEntityFromId<ItemEntity>(shop.GetItemIDs(i));
            }
        }
        else
        {
            this.eventItemEntity = null;
        }
        this.shopEntity = shop;
        this.stoneShopEntity = null;
        this.bankShopEntity = null;
        UserShopEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserShopMaster>(DataNameKind.Kind.USER_SHOP).getEntityFromId(NetworkManager.UserId, shop.id);
        this.totalNum = entity.num;
    }

    private string ToString() => 
        ("Item " + this.NameText);

    public long ActiveTime
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.closedAt;
            }
            if (this.stoneShopEntity != null)
            {
                return (long) this.stoneShopEntity.closedAt;
            }
            if (this.bankShopEntity != null)
            {
                return this.bankShopEntity.closedAt;
            }
            return 0L;
        }
    }

    public BankShopEntity BankShop =>
        this.bankShopEntity;

    public string DetailText
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.detail;
            }
            if (this.stoneShopEntity != null)
            {
                return this.stoneShopEntity.detail;
            }
            if (this.itemEntity != null)
            {
                return this.itemEntity.detail;
            }
            return LocalizationManager.GetUnknownName();
        }
    }

    public ItemEntity[] EventItem =>
        this.eventItemEntity;

    public int ImageId
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.imageId;
            }
            return ((this.itemEntity == null) ? 0 : this.itemEntity.imageId);
        }
    }

    public bool IsSoldOut =>
        ((this.shopEntity != null) && this.shopEntity.IsSoldOut());

    public int ItemCount =>
        this.shopEntity.GetItemCount();

    public int LimitNum
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.limitNum;
            }
            return 0;
        }
    }

    public string NameText
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.name;
            }
            if (this.stoneShopEntity != null)
            {
                return this.stoneShopEntity.name;
            }
            if (this.bankShopEntity != null)
            {
                return this.bankShopEntity.name;
            }
            if (this.itemEntity != null)
            {
                return this.itemEntity.name;
            }
            return "error";
        }
    }

    public string NumText
    {
        get
        {
            if (this.shopEntity != null)
            {
                string str;
                string str2;
                this.shopEntity.GetInfo(out str, out str2, false);
                return str2;
            }
            if (this.stoneShopEntity != null)
            {
                return this.stoneShopEntity.GetCountText();
            }
            if (this.bankShopEntity != null)
            {
                return this.bankShopEntity.GetCountText(false);
            }
            if (this.itemEntity != null)
            {
                return this.itemEntity.GetCountText(1);
            }
            return string.Empty;
        }
    }

    public int Price
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.GetPrice();
            }
            if (this.stoneShopEntity != null)
            {
                return this.stoneShopEntity.GetPrice();
            }
            if (this.bankShopEntity != null)
            {
                return this.bankShopEntity.GetPrice();
            }
            if (this.itemEntity != null)
            {
                return this.itemEntity.GetPrice();
            }
            return 0;
        }
    }

    public IconLabelInfo.IconKind PriceIcon
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.GetPriceIcon();
            }
            if (this.stoneShopEntity != null)
            {
                return this.stoneShopEntity.GetPriceIcon();
            }
            if (this.bankShopEntity != null)
            {
                return this.bankShopEntity.GetPriceIcon();
            }
            if (this.itemEntity != null)
            {
                return this.itemEntity.GetPriceIcon();
            }
            return IconLabelInfo.IconKind.DATA;
        }
    }

    public IconLabelInfo.IconKind PriceUnitIcon
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.GetPriceUnitIcon();
            }
            if (this.stoneShopEntity != null)
            {
                return this.stoneShopEntity.GetPriceUnitIcon();
            }
            if (this.bankShopEntity != null)
            {
                return this.bankShopEntity.GetPriceUnitIcon();
            }
            if (this.itemEntity != null)
            {
                return this.itemEntity.GetPriceUnitIcon();
            }
            return IconLabelInfo.IconKind.DATA;
        }
    }

    public Purchase.Type PurchaseType
    {
        get
        {
            if (this.shopEntity != null)
            {
                return (Purchase.Type) this.shopEntity.purchaseType;
            }
            return ((this.itemEntity == null) ? Purchase.Type.ALL : Purchase.Type.ITEM);
        }
    }

    public string SendBulkNameText
    {
        get
        {
            if (this.IsExchangeQP())
            {
                return LocalizationManager.Get("QP_UNIT");
            }
            return this.SendNameText;
        }
    }

    public string SendNameText
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.name;
            }
            if (this.stoneShopEntity != null)
            {
                return this.stoneShopEntity.name;
            }
            if (this.bankShopEntity != null)
            {
                return this.bankShopEntity.name;
            }
            if (this.itemEntity != null)
            {
                return this.itemEntity.name;
            }
            return "error";
        }
    }

    public string SendNumText
    {
        get
        {
            if (this.shopEntity != null)
            {
                string str;
                string str2;
                this.shopEntity.GetInfo(out str, out str2, true);
                return str2;
            }
            if (this.stoneShopEntity == null)
            {
                if (this.bankShopEntity != null)
                {
                    return (string.Empty + this.bankShopEntity.GetStoneNum());
                }
                if (this.itemEntity != null)
                {
                    return "1";
                }
            }
            return null;
        }
    }

    public ShopEntity Shop =>
        this.shopEntity;

    public StoneShopEntity StoneShop =>
        this.stoneShopEntity;

    public int TargetId
    {
        get
        {
            if (this.shopEntity != null)
            {
                return this.shopEntity.targetId;
            }
            return 0;
        }
    }

    public int ToTalNum =>
        this.totalNum;
}

