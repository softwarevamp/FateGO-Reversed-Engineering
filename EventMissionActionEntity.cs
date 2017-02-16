using System;

public class EventMissionActionEntity : DataEntityBase
{
    public int id;
    public int missionActionType;
    public int missionId;
    public int missionProgressType;
    public string[] vals;

    public MissionActionType GetMissionActionType() => 
        ((MissionActionType) this.missionActionType);

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.missionId, ":", this.missionProgressType, ":", this.id };
        return string.Concat(objArray1);
    }

    public int getValID()
    {
        if (this.missionActionType == 3)
        {
            return int.Parse(this.vals[0]);
        }
        return -1;
    }

    public string getValMessage()
    {
        switch (this.missionActionType)
        {
            case 1:
            case 2:
            case 5:
                return this.vals[0];
        }
        return string.Empty;
    }

    public enum MissionActionType
    {
        IMAGE_WINDOW = 6,
        SCROLL_MISSION = 3,
        SYSTEM_WINDOW = 2,
        TALK = 1,
        TRANSITION_TERMINAL = 4,
        VOICE = 5
    }
}

