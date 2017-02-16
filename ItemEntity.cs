using System;

public class ItemEntity : DataEntityBase
{
    public int bgImageId;
    public string detail;
    public long endedAt;
    public int id;
    public int imageId;
    public int[] individuality;
    public bool isSell;
    public string name;
    public int priority;
    public int sellQp;
    public long startedAt;
    public int type;
    public string unit;
    public int value;

    public string GetCountText(int num)
    {
        if (string.IsNullOrEmpty(this.unit))
        {
            return ItemType.GetCountText((ItemType.Type) this.type, num);
        }
        return (num.ToString() + this.unit);
    }

    public int GetPrice() => 
        this.sellQp;

    public IconLabelInfo.IconKind GetPriceIcon() => 
        IconLabelInfo.IconKind.QP;

    public IconLabelInfo.IconKind GetPriceUnitIcon() => 
        IconLabelInfo.IconKind.QP_UNIT;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public bool IsEnable()
    {
        long num = NetworkManager.getTime();
        return ((num >= this.startedAt) && ((this.endedAt == 0) || (num <= this.endedAt)));
    }
}

