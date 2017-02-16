using System;
using System.Runtime.InteropServices;

public class ShopEntity : DataEntityBase
{
    public int baseShopId;
    public int bgImageId;
    public long closedAt;
    public string detail;
    public int eventId;
    public int id;
    public int imageId;
    public int[] itemIds;
    public int limitNum;
    public string name;
    public long openedAt;
    public int payType;
    public int[] prices;
    public int priority;
    public int purchaseType;
    public int setNum;
    public int targetId;
    public string warningMessage;

    public bool checkHoldDisp() => 
        ((this.purchaseType == 1) && !SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).isQP(this.targetId));

    public string GetCountText() => 
        LocalizationManager.GetUnitInfo(this.setNum);

    public int getHoldCount()
    {
        if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).isMana(this.targetId))
        {
            return SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).mana;
        }
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, this.targetId).num;
    }

    public void GetInfo(out string nameText, out string countText, bool isSend = false)
    {
        ItemEntity entity;
        ItemType.Type type;
        nameText = this.name;
        switch (this.purchaseType)
        {
            case 1:
                entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.targetId);
                type = (ItemType.Type) entity.type;
                switch (type)
                {
                    case ItemType.Type.QP:
                    case ItemType.Type.STONE:
                    case ItemType.Type.MANA:
                        goto Label_0078;
                }
                break;

            case 2:
            case 3:
            case 5:
                if (!isSend)
                {
                    countText = string.Format(LocalizationManager.Get("UNIT_INFO"), this.setNum);
                    return;
                }
                countText = string.Empty + this.setNum;
                return;

            case 4:
                if (!isSend)
                {
                    ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.targetId);
                    if (entity2.IsServant)
                    {
                        countText = string.Format(LocalizationManager.Get("SERVANT_UNIT"), this.setNum);
                    }
                    else if (entity2.IsServantEquip)
                    {
                        countText = string.Format(LocalizationManager.Get("SERVANT_EQUIP_UNIT"), this.setNum);
                    }
                    else
                    {
                        countText = string.Format(LocalizationManager.Get("EXCEPT_SERVANT_UNIT"), this.setNum);
                    }
                    return;
                }
                countText = string.Empty + this.setNum;
                return;

            default:
                countText = string.Empty;
                return;
        }
        if (type != ItemType.Type.FRIEND_POINT)
        {
            if (isSend)
            {
                countText = string.Empty + this.setNum;
            }
            else
            {
                countText = entity.GetCountText(this.setNum);
            }
            return;
        }
    Label_0078:
        countText = null;
    }

    public int GetItemCount() => 
        this.itemIds.Length;

    public int GetItemID() => 
        this.itemIds[0];

    public int GetItemIDs(int num) => 
        this.itemIds[num];

    public PayType.Type GetPayType() => 
        ((PayType.Type) this.payType);

    public int GetPrice() => 
        this.prices[0];

    public IconLabelInfo.IconKind GetPriceIcon()
    {
        switch (this.payType)
        {
            case 1:
                return IconLabelInfo.IconKind.STONE;

            case 2:
                return IconLabelInfo.IconKind.QP;

            case 3:
                return IconLabelInfo.IconKind.FRIEND_POINT;

            case 4:
                return IconLabelInfo.IconKind.MANA;

            case 6:
                return IconLabelInfo.IconKind.EVENT_ITEM;
        }
        return IconLabelInfo.IconKind.DATA;
    }

    public int GetPrices(int num) => 
        this.prices[num];

    public IconLabelInfo.IconKind GetPriceUnitIcon()
    {
        switch (this.payType)
        {
            case 1:
                return IconLabelInfo.IconKind.STONE_UNIT;

            case 2:
                return IconLabelInfo.IconKind.QP_UNIT;

            case 3:
                return IconLabelInfo.IconKind.FRIEND_POINT_UNIT;

            case 4:
                return IconLabelInfo.IconKind.MANA_UNIT;

            case 6:
                return IconLabelInfo.IconKind.EVENT_ITEM_UNIT;
        }
        return IconLabelInfo.IconKind.DATA;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int GetPurchaseShop() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopReleaseMaster>(DataNameKind.Kind.SHOP_RELEASE).GetPurchaseShop(this.id);

    public void GetSum(out int buyItemNum, out int buyServantNum, out int buyServantEquipNum)
    {
        buyItemNum = 0;
        buyServantNum = 0;
        buyServantEquipNum = 0;
        switch (this.purchaseType)
        {
            case 1:
            case 2:
                buyItemNum = this.setNum;
                break;

            case 4:
                if (!SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.targetId).IsServantEquip)
                {
                    buyServantNum = this.setNum;
                    break;
                }
                buyServantEquipNum = this.setNum;
                break;

            case 5:
                SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SetItemMaster>(DataNameKind.Kind.SET_ITEM).GetSum(out buyItemNum, out buyServantNum, out buyServantEquipNum, this.targetId);
                break;
        }
    }

    public bool IsClosed(long nowTime = 0)
    {
        if (nowTime == 0)
        {
            nowTime = NetworkManager.getTime();
        }
        return ((this.closedAt != 0) && (nowTime > this.closedAt));
    }

    public bool IsCondType()
    {
        UserShopEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserShopMaster>(DataNameKind.Kind.USER_SHOP).getEntityFromId(NetworkManager.UserId, this.id);
        if (this.limitNum > 0)
        {
            return (entity.num >= this.limitNum);
        }
        return (entity.num > 0);
    }

    public bool IsCondType(long userId)
    {
        UserShopEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserShopMaster>(DataNameKind.Kind.USER_SHOP).getEntityFromId(userId, this.id);
        if (this.limitNum > 0)
        {
            return (entity.num >= this.limitNum);
        }
        return (entity.num > 0);
    }

    public bool IsEnable(long nowTime = 0)
    {
        if (nowTime == 0)
        {
            nowTime = NetworkManager.getTime();
        }
        if ((nowTime < this.openedAt) || ((this.closedAt != 0) && (nowTime > this.closedAt)))
        {
            return false;
        }
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopReleaseMaster>(DataNameKind.Kind.SHOP_RELEASE).IsOpen(this.id);
    }

    public bool IsOpened(long nowTime = 0)
    {
        if (nowTime == 0)
        {
            nowTime = NetworkManager.getTime();
        }
        return ((nowTime >= this.openedAt) && SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopReleaseMaster>(DataNameKind.Kind.SHOP_RELEASE).IsOpen(this.id));
    }

    public bool IsPreparation(out string message) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopReleaseMaster>(DataNameKind.Kind.SHOP_RELEASE).IsPreparation(out message, this.id);

    public bool IsSoldOut() => 
        ((this.limitNum > 0) && (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserShopMaster>(DataNameKind.Kind.USER_SHOP).getEntityFromId(NetworkManager.UserId, this.id).num >= this.limitNum));
}

