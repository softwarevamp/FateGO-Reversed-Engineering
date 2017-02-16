using System;

public class BattleBgEntity : DataEntityBase
{
    public int id;
    public int imageId;
    public int priority;
    public int type;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.type };
        return string.Concat(objArray1);
    }
}

