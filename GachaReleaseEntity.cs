using System;

public class GachaReleaseEntity : DataEntityBase
{
    public int gachaId;
    public int targetId;
    public int type;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.gachaId, ":", this.type, ":", this.targetId };
        return string.Concat(objArray1);
    }
}

