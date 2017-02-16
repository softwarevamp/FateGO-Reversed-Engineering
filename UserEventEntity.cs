using System;

public class UserEventEntity : DataEntityBase
{
    public int eventId;
    public long rank;
    public long userId;
    public int value;

    public UserEventEntity()
    {
    }

    public UserEventEntity(long userId, int eventId)
    {
        this.userId = userId;
        this.eventId = eventId;
        this.value = 0;
        this.rank = 0L;
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.eventId };
        return string.Concat(objArray1);
    }
}

