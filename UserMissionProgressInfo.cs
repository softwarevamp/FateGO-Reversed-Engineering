using System;

public class UserMissionProgressInfo
{
    public int condMsgType;
    public int currentProgressType;
    public int currentProgStatus;
    public int eventId;
    public int missionId;
    public long progNum;
    public long targetNum;

    public UserMissionProgressInfo()
    {
    }

    public UserMissionProgressInfo(int eventId, int missionId)
    {
        this.eventId = eventId;
        this.missionId = missionId;
        this.checkMissionCond();
    }

    public void checkMissionCond()
    {
        this.currentProgressType = 4;
        EventMissionConditionEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionConditionMaster>(DataNameKind.Kind.EVENT_MISSION_CONDITION).getMissionCondList(this.eventId, this.missionId);
        if (entityArray.Length > 0)
        {
            foreach (EventMissionConditionEntity entity in entityArray)
            {
                if (!entity.getMissionProgress())
                {
                    this.currentProgressType = entity.missionProgressType - 1;
                    break;
                }
            }
            this.setMissionCondInfo();
        }
    }

    ~UserMissionProgressInfo()
    {
    }

    private void setMissionCondInfo()
    {
        UserEventMissionEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEventMissionMaster>(DataNameKind.Kind.USER_EVENT_MISSION).getUserEventMissionList(this.eventId);
        switch (this.currentProgressType)
        {
            case 2:
                this.currentProgStatus = 1;
                this.condMsgType = 3;
                break;

            case 3:
                this.currentProgStatus = 2;
                this.condMsgType = 4;
                break;

            case 4:
                this.condMsgType = 4;
                if (entityArray != null)
                {
                    for (int i = 0; i < entityArray.Length; i++)
                    {
                        UserEventMissionEntity entity = entityArray[i];
                        if (entity.missionId == this.missionId)
                        {
                            this.currentProgStatus = (entity.missionProgressType != 4) ? 4 : 3;
                        }
                    }
                }
                break;

            default:
                this.currentProgStatus = 0;
                this.condMsgType = 2;
                break;
        }
        EventMissionConditionEntity[] entityArray2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionConditionMaster>(DataNameKind.Kind.EVENT_MISSION_CONDITION).getMissionCondListByType(this.eventId, this.missionId, this.condMsgType);
        int num2 = 0;
        this.targetNum = 0L;
        this.progNum = 0L;
        if (entityArray2.Length > 0)
        {
            foreach (EventMissionConditionEntity entity2 in entityArray2)
            {
                if (this.currentProgStatus != 0)
                {
                    this.progNum += entity2.getProgressNum();
                    this.targetNum += entity2.targetNum;
                }
                num2++;
            }
        }
    }

    public enum ProgStatus
    {
        LOCK,
        NOSTART,
        PROGRESS,
        CLEAR,
        ACHIVE,
        END
    }
}

