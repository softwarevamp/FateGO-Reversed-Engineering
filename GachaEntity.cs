using System;

public class GachaEntity : DataEntityBase
{
    public int bannerQuestId;
    public int bannerQuestPhase;
    public int beforeDrawNum;
    public int beforeGachaId;
    public long closedAt;
    public int condQuestId;
    public int condQuestPhase;
    public string detailUrl;
    public int drawNum1;
    public int drawNum2;
    public int freeDrawFlag;
    public int gachaSlot;
    public int id;
    public int imageId;
    public int maxDrawNum;
    public string name;
    public long openedAt;
    public int priority;
    public int shopId1;
    public int shopId2;
    public int ticketItemId;
    public int type;
    public int warId;

    public int getPayMultiTimePrice()
    {
        StoneShopEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<StoneShopMaster>(DataNameKind.Kind.STONE_SHOP).getEntityFromId<StoneShopEntity>(this.shopId2);
        if (entity != null)
        {
            return entity.price;
        }
        return 0;
    }

    public int getPayOneTimePrice()
    {
        StoneShopEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<StoneShopMaster>(DataNameKind.Kind.STONE_SHOP).getEntityFromId<StoneShopEntity>(this.shopId1);
        if (entity != null)
        {
            return entity.price;
        }
        return 0;
    }

    public int getPrice()
    {
        if (this.type == 3)
        {
            ShopEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).getEntityFromId<ShopEntity>(this.shopId1);
            if ((entity != null) && (entity.GetPayType() == PayType.Type.FRIEND_POINT))
            {
                return entity.GetPrice();
            }
        }
        return 0;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

