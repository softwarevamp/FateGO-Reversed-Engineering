using System;

public class CombineMaterialEntity : DataEntityBase
{
    public int id;
    public int lv;
    public int price;
    public int value;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.lv };
        return string.Concat(objArray1);
    }
}

