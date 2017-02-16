using System;

public class UserGachaEntity : DataEntityBase
{
    public long freeDrawAt;
    public int gachaId;
    public int num;
    public long userId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.gachaId };
        return string.Concat(objArray1);
    }
}

