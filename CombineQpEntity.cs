using System;

public class CombineQpEntity : DataEntityBase
{
    public int lv;
    public int qp;
    public int rarity;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.rarity, ":", this.lv };
        return string.Concat(objArray1);
    }
}

