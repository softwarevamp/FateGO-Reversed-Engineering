using System;

public class SetItemEntity : DataEntityBase
{
    public int id;
    public int purchaseType;
    public int setNum;
    public int targetId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.purchaseType, ":", this.targetId };
        return string.Concat(objArray1);
    }
}

