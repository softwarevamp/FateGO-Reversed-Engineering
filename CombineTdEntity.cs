using System;

public class CombineTdEntity : DataEntityBase
{
    public int id;
    public int itemId;
    public int itemNum;
    public int qp;
    public int treasureDeviceLv;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.treasureDeviceLv };
        return string.Concat(objArray1);
    }
}

