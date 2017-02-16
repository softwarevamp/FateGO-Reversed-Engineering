using System;

public class UserBoxGachaEntity : DataEntityBase
{
    public int boxGachaId;
    public int boxIndex;
    public int drawNum;
    public bool isReset;
    public int resetNum;
    public long userId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.boxGachaId };
        return string.Concat(objArray1);
    }
}

