using System;

public class EventMissionCondDetailEntity : DataEntityBase
{
    public int conditionLinkType;
    public int eventId;
    public int id;
    public int missionCondType;
    public int[] targetIds;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public enum MissionConditionLinkType
    {
        EVENT_START = 1,
        MISSION_START = 2
    }

    public enum MissionCondType
    {
        BATTLE_SVT_EQUIP_IN_DECK = 5,
        BATTLE_SVT_IN_DECK = 4,
        ENEMY_INDIVIDUALITY_KILL_NUM = 2,
        ENEMY_KILL_NUM = 1,
        ITEM_GET_TOTAL = 3
    }
}

