using System;

public class EventRewardSetEntity : DataEntityBase
{
    public string detail;
    public int eventId;
    public int iconId;
    public int id;
    public string name;
    public int rewardSetType;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.rewardSetType, ":", this.eventId, ":", this.id };
        return string.Concat(objArray1);
    }

    public enum RewardSetType
    {
        BOX_GACHA = 1,
        EVENT_MISSION = 3,
        EVENT_POINT = 2
    }
}

