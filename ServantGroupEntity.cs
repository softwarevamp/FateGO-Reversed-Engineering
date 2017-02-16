using System;

public class ServantGroupEntity : DataEntityBase
{
    public int id;
    public int svtId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.svtId };
        return string.Concat(objArray1);
    }

    public int getServantGroupId() => 
        this.id;

    public int getServantId() => 
        this.svtId;
}

