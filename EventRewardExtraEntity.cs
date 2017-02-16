using System;

public class EventRewardExtraEntity : DataEntityBase
{
    public string detail;
    public int eventId;
    public int iconId;
    public string name;
    public int point;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.eventId, ":", this.point };
        return string.Concat(objArray1);
    }
}

