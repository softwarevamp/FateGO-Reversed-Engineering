using System;

public class EventQuestEntity : DataEntityBase
{
    public int eventId;
    public int phase;
    public int questId;

    public int getEventId() => 
        this.eventId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.eventId, ":", this.questId, ":", this.phase };
        return string.Concat(objArray1);
    }
}

