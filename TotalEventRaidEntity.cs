using System;

public class TotalEventRaidEntity : DataEntityBase
{
    public int day;
    public int eventId;
    public long totalDamage;

    public override string getPrimarykey() => 
        (this.eventId.ToString() + ":" + this.day.ToString());
}

