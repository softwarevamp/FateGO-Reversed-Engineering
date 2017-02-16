using System;

public class DropEntity : DataEntityBase
{
    public int condLv;
    public int giftId;
    public long id;
    public int num;
    public int probability;
    public int rarity;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.num };
        return string.Concat(objArray1);
    }
}

