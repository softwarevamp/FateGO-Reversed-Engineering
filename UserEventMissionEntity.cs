using System;

public class UserEventMissionEntity : DataEntityBase
{
    public int eventId;
    public int missionId;
    public int missionProgressType;
    public long userId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.missionId };
        return string.Concat(objArray1);
    }
}

