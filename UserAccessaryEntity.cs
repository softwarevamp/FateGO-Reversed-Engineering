using System;

public class UserAccessaryEntity : DataEntityBase
{
    public int accessaryId;
    public int num;
    public long userId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.accessaryId };
        return string.Concat(objArray1);
    }
}

