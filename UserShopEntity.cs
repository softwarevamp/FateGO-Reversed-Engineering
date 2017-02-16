using System;

public class UserShopEntity : DataEntityBase
{
    public int num;
    public int shopId;
    public long userId;

    public UserShopEntity()
    {
    }

    public UserShopEntity(long userId, int shopId)
    {
        this.userId = userId;
        this.shopId = shopId;
        this.num = 0;
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.shopId };
        return string.Concat(objArray1);
    }
}

