using System;

public class EventCampaignEntity : DataEntityBase
{
    public int calcType;
    public int eventId;
    public int target;
    public int target2;
    public int value;

    public int getCalcType() => 
        this.calcType;

    public int getEventId() => 
        this.eventId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.eventId, ":", this.target, ":", this.target2 };
        return string.Concat(objArray1);
    }

    public int getTarget() => 
        this.target;

    public int getValue() => 
        this.value;
}

