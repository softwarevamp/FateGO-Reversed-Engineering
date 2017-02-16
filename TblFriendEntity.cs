using System;

public class TblFriendEntity : DataEntityBase
{
    public long friendId;
    public int status;
    public long userId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.friendId };
        return string.Concat(objArray1);
    }
}

