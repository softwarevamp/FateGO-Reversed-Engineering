using System;
using System.Runtime.InteropServices;

public class EventMissionItemListViewItem : ListViewItem
{
    protected string condMsg;
    protected MissionProgressType.Type condMsgType;
    protected int currentEventId;
    protected int currentMissionId;
    protected UserEventMissionEntity[] currentUsrMissionList;
    protected int dispNo;
    protected EventMissionEntity eventMissionEnt;
    protected GiftEntity giftEnt;
    protected int iconId;
    protected bool isNew;
    protected bool isNowMission;
    protected bool isTargetEnd;
    protected ItemEntity itemEnt;
    protected string nameTxt;
    protected long progNum;
    protected ProgStatus progStatus;
    protected string progTxt;
    protected MissionProgressType.Type progType;
    protected float progVal;
    protected string rewardExtraDetailTxt;
    protected int rewardObjectId;
    protected EventRewardSetEntity rewardSetEnt;
    protected ServantEntity svtEnt;
    protected long targetNum;

    public EventMissionItemListViewItem(UserMissionProgressInfo userMissionInfo)
    {
        this.eventMissionEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT_MISSION).getEntityFromId<EventMissionEntity>(userMissionInfo.missionId);
        this.currentEventId = userMissionInfo.eventId;
        this.currentMissionId = userMissionInfo.missionId;
        this.dispNo = this.eventMissionEnt.dispNo;
        this.progStatus = (ProgStatus) userMissionInfo.currentProgStatus;
        this.progType = (MissionProgressType.Type) userMissionInfo.currentProgressType;
        this.progNum = userMissionInfo.progNum;
        this.targetNum = userMissionInfo.targetNum;
        this.condMsgType = (MissionProgressType.Type) userMissionInfo.condMsgType;
        EventMissionConditionEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionConditionMaster>(DataNameKind.Kind.EVENT_MISSION_CONDITION).getMissionCondListByType(this.currentEventId, this.currentMissionId, (int) this.condMsgType);
        int num = 0;
        if (entityArray.Length > 0)
        {
            foreach (EventMissionConditionEntity entity in entityArray)
            {
                if (num > 0)
                {
                    this.condMsg = this.condMsg + "\n" + entity.conditionMessage;
                }
                else
                {
                    this.condMsg = entity.conditionMessage;
                }
                num++;
            }
            if (this.progStatus != ProgStatus.LOCK)
            {
                this.progTxt = $"{this.progNum}/{this.targetNum}";
                this.progVal = ((float) this.progNum) / ((float) this.targetNum);
            }
            else
            {
                this.progTxt = string.Format("{0}/{0}", LocalizationManager.Get("UNKNOWN_NAME"));
                this.progVal = 0f;
            }
        }
        this.setRewardInfo();
        base.sortValue2 = -this.eventMissionEnt.id;
    }

    public EventMissionItemListViewItem(EventMissionEntity missionData, UserEventMissionEntity[] usrMissionList)
    {
        this.eventMissionEnt = missionData;
        this.currentEventId = missionData.eventId;
        this.currentMissionId = missionData.id;
        this.currentUsrMissionList = null;
        if ((usrMissionList != null) && (usrMissionList.Length > 0))
        {
            this.currentUsrMissionList = usrMissionList;
        }
        this.isNew = false;
        this.dispNo = missionData.dispNo;
        this.condMsg = string.Empty;
        this.targetNum = 0L;
        this.progNum = 0L;
        this.progVal = 0f;
        this.progTxt = string.Empty;
        this.progStatus = ProgStatus.LOCK;
        this.progType = MissionProgressType.Type.CLEAR;
        this.isNowMission = this.eventMissionEnt.isNowMission();
        this.isTargetEnd = false;
        this.checkMissionCond();
        this.setRewardInfo();
        base.sortValue2 = -this.eventMissionEnt.id;
    }

    public void checkMissionCond()
    {
        this.isNowMission = this.eventMissionEnt.isNowMission();
        this.progType = MissionProgressType.Type.CLEAR;
        EventMissionConditionEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionConditionMaster>(DataNameKind.Kind.EVENT_MISSION_CONDITION).getMissionCondList(this.currentEventId, this.currentMissionId);
        if (entityArray.Length > 0)
        {
            foreach (EventMissionConditionEntity entity in entityArray)
            {
                if (!entity.getMissionProgress())
                {
                    int num2 = entity.missionProgressType - 1;
                    this.progType = (MissionProgressType.Type) num2;
                    break;
                }
            }
            this.setMissionCondInfo();
        }
    }

    ~EventMissionItemListViewItem()
    {
    }

    public bool GetProgInfo(out string progTxt, out float barExp)
    {
        progTxt = this.progTxt;
        barExp = this.progVal;
        return true;
    }

    public void ModifyItem(bool isRecieveReward)
    {
        this.progStatus = ProgStatus.ACHIVE;
        UserMissionProgressManager.SetAchiveMission(this.currentMissionId, (int) this.progStatus);
        UserMissionProgressManager.WriteData();
    }

    private void setGiftData()
    {
        if (this.giftEnt != null)
        {
            switch (this.giftEnt.type)
            {
                case 1:
                case 6:
                case 7:
                    this.svtEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.giftEnt.objectId);
                    break;

                case 2:
                    this.itemEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.giftEnt.objectId);
                    break;
            }
            this.rewardObjectId = this.giftEnt.objectId;
        }
    }

    private void setMissionCondInfo()
    {
        UserEventMissionEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEventMissionMaster>(DataNameKind.Kind.USER_EVENT_MISSION).getUserEventMissionList(this.currentEventId);
        switch (this.progType)
        {
            case MissionProgressType.Type.OPEN_CONDITION:
                this.progStatus = ProgStatus.NOSTART;
                this.condMsgType = MissionProgressType.Type.START;
                break;

            case MissionProgressType.Type.START:
                this.progStatus = ProgStatus.PROGRESS;
                this.condMsgType = MissionProgressType.Type.CLEAR;
                break;

            case MissionProgressType.Type.CLEAR:
                this.progStatus = ProgStatus.CLEAR;
                this.condMsgType = MissionProgressType.Type.CLEAR;
                if (entityArray != null)
                {
                    for (int i = 0; i < entityArray.Length; i++)
                    {
                        UserEventMissionEntity entity = entityArray[i];
                        if (entity.missionId == this.currentMissionId)
                        {
                            this.progStatus = (entity.missionProgressType != 4) ? ProgStatus.ACHIVE : ProgStatus.CLEAR;
                        }
                    }
                }
                break;

            default:
                this.progStatus = ProgStatus.LOCK;
                this.condMsgType = MissionProgressType.Type.OPEN_CONDITION;
                break;
        }
        this.setMissionCondMsg();
    }

    private void setMissionCondMsg()
    {
        this.condMsg = string.Empty;
        this.targetNum = 0L;
        this.progNum = 0L;
        this.progVal = 0f;
        this.progTxt = string.Empty;
        EventMissionConditionEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionConditionMaster>(DataNameKind.Kind.EVENT_MISSION_CONDITION).getMissionCondListByType(this.currentEventId, this.currentMissionId, (int) this.condMsgType);
        int num = 0;
        if (entityArray.Length > 0)
        {
            foreach (EventMissionConditionEntity entity in entityArray)
            {
                if (num > 0)
                {
                    this.condMsg = this.condMsg + "\n" + entity.conditionMessage;
                }
                else
                {
                    this.condMsg = entity.conditionMessage;
                }
                if (this.progStatus != ProgStatus.LOCK)
                {
                    this.progNum += entity.getProgressNum();
                    this.targetNum += entity.targetNum;
                }
                num++;
            }
            if (this.progStatus != ProgStatus.LOCK)
            {
                this.progTxt = $"{this.progNum}/{this.targetNum}";
                this.progVal = ((float) this.progNum) / ((float) this.targetNum);
            }
            else
            {
                this.progTxt = string.Format("{0}/{0}", LocalizationManager.Get("UNKNOWN_NAME"));
                this.progVal = 0f;
            }
        }
    }

    public void SetOpenMissionItem(bool isOpen)
    {
    }

    private void setRewardInfo()
    {
        switch (this.eventMissionEnt.rewardType)
        {
            case 1:
                this.giftEnt = this.eventMissionEnt.getGiftData();
                this.setGiftData();
                break;

            case 3:
                this.rewardSetEnt = this.eventMissionEnt.getSetRewardData();
                if (this.rewardSetEnt != null)
                {
                    this.nameTxt = this.rewardSetEnt.name;
                    this.iconId = this.rewardSetEnt.iconId;
                    this.rewardExtraDetailTxt = this.rewardSetEnt.detail;
                }
                break;
        }
    }

    public override bool SetSortValue(ListViewSort sort)
    {
        base.isTermination = false;
        base.isTerminationSpace = false;
        base.sortValue1 = -1L;
        switch (this.progStatus)
        {
            case ProgStatus.LOCK:
            case ProgStatus.NOSTART:
                if (sort.GetFilter(ListViewSort.FilterKind.MISSION_NOSTART))
                {
                    break;
                }
                return false;

            case ProgStatus.PROGRESS:
                if (sort.GetFilter(ListViewSort.FilterKind.MISSION_PROGRESS))
                {
                    break;
                }
                return false;

            case ProgStatus.CLEAR:
                if (sort.GetFilter(ListViewSort.FilterKind.MISSION_CLEAR))
                {
                    break;
                }
                return false;

            case ProgStatus.ACHIVE:
                if (sort.GetFilter(ListViewSort.FilterKind.MISSION_COMPLETE))
                {
                    break;
                }
                return false;
        }
        base.sortValue1 = this.eventMissionEnt.id;
        return true;
    }

    public string CondMsg =>
        this.condMsg;

    public ProgStatus CurrentStatus
    {
        get => 
            this.progStatus;
        set
        {
            this.progStatus = value;
        }
    }

    public int DispNo =>
        this.dispNo;

    public EventMissionEntity EventMissionEntity
    {
        get
        {
            if (this.eventMissionEnt != null)
            {
                return this.eventMissionEnt;
            }
            return null;
        }
    }

    public RewardType.Type eventRewardType =>
        ((RewardType.Type) this.eventMissionEnt.rewardType);

    public string ExtraDetailTXt =>
        this.rewardExtraDetailTxt;

    public bool IsEndMission
    {
        get => 
            this.isNowMission;
        set
        {
            this.isNowMission = value;
        }
    }

    public bool IsNew =>
        (this.isNew = ((this.condMsgType == MissionProgressType.Type.CLEAR) && (this.progStatus == ProgStatus.PROGRESS)) && (this.progNum <= 0L));

    public bool IsOpenMission =>
        (this.progStatus == ProgStatus.NOSTART);

    public bool IsShowRewardInfo =>
        (((this.progStatus == ProgStatus.LOCK) || (this.progStatus == ProgStatus.NOSTART)) || (this.progStatus == ProgStatus.PROGRESS));

    public ItemEntity ItemEntity
    {
        get
        {
            if (this.itemEnt != null)
            {
                return this.itemEnt;
            }
            return null;
        }
    }

    public int MissionId =>
        this.eventMissionEnt.id;

    public string NameText =>
        this.nameTxt;

    public int RewardObjId =>
        this.rewardObjectId;

    public int SetExtraIconId =>
        this.iconId;

    public ServantEntity SvtEntity
    {
        get
        {
            if (this.svtEnt != null)
            {
                return this.svtEnt;
            }
            return null;
        }
    }

    public Gift.Type Type
    {
        get
        {
            if (this.giftEnt != null)
            {
                return (Gift.Type) this.giftEnt.type;
            }
            return (Gift.Type) 0;
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

