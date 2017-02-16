using System;

public class EventMissionConditionEntity : DataEntityBase
{
    public string conditionMessage;
    public int condType;
    public int eventId;
    public int id;
    public int missionId;
    public int missionProgressType;
    public int[] targetIds;
    public long targetNum;

    public bool getMissionProgress()
    {
        int num = 0;
        foreach (int num2 in this.targetIds)
        {
            int num4 = CondType.GetProgressNum((CondType.Kind) this.condType, num2, (int) this.targetNum, this.eventId);
            num += num4;
        }
        return (num >= this.targetNum);
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.missionId, ":", this.missionProgressType, ":", this.id };
        return string.Concat(objArray1);
    }

    public int getProgressNum()
    {
        int num = 0;
        foreach (int num2 in this.targetIds)
        {
            num += CondType.GetProgressNum((CondType.Kind) this.condType, num2, (int) this.targetNum, this.eventId);
        }
        return ((num < ((int) this.targetNum)) ? num : ((int) this.targetNum));
    }
}

