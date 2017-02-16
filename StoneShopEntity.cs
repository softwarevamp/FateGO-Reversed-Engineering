using System;
using System.Runtime.InteropServices;

public class StoneShopEntity : DataEntityBase
{
    public int closedAt;
    public string detail;
    public int effect;
    public int id;
    public string name;
    public int openedAt;
    public int price;

    public string GetCountText() => 
        LocalizationManager.GetCountInfo(1);

    public int GetPrice() => 
        this.price;

    public IconLabelInfo.IconKind GetPriceIcon() => 
        IconLabelInfo.IconKind.STONE;

    public IconLabelInfo.IconKind GetPriceUnitIcon() => 
        IconLabelInfo.IconKind.STONE_UNIT;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public bool IsClosed(long nowTime = 0)
    {
        if (nowTime == 0)
        {
            nowTime = NetworkManager.getTime();
        }
        return ((this.closedAt != 0) && (nowTime > this.closedAt));
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
        return true;
    }

    public bool IsOpened(long nowTime = 0)
    {
        if (nowTime == 0)
        {
            nowTime = NetworkManager.getTime();
        }
        return (nowTime >= this.openedAt);
    }
}

