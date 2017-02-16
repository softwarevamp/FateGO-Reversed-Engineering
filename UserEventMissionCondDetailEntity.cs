using System;

public class UserEventMissionCondDetailEntity : DataEntityBase
{
    public int conditionDetailId;
    public int eventId;
    public long progressNum;
    public long userId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.conditionDetailId };
        return string.Concat(objArray1);
    }
}

