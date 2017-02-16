using System;

public class CombineLimitEntity : DataEntityBase
{
    public int id;
    public int[] itemIds;
    public int[] itemNums;
    public int qp;
    public int svtLimit;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.svtLimit };
        return string.Concat(objArray1);
    }
}

