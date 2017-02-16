using System;

public class FriendshipEntity : DataEntityBase
{
    public int friendship;
    public int id;
    public int rank;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.rank };
        return string.Concat(objArray1);
    }
}

