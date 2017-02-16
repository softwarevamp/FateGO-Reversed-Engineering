using System;

public class EventServantEntity : DataEntityBase
{
    public long endedAt;
    public int eventId;
    public string getMessage;
    public string joinMessage;
    public string leaveMessage;
    public long startedAt;
    public int svtId;

    public DateTime getEndTime() => 
        NetworkManager.getLocalDateTime(this.endedAt);

    public string getEndTimeStr()
    {
        DateTime time = NetworkManager.getLocalDateTime(this.endedAt);
        string format = "{0}/{1:D2}/{2:D2} {3:D2}:{4:D2}";
        return string.Format(format, new object[] { time.Year, time.Month, time.Day, time.Hour, time.Minute });
    }

    public int getEventId() => 
        this.eventId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.eventId, ":", this.svtId };
        return string.Concat(objArray1);
    }

    public int getServantId() => 
        this.svtId;
}

