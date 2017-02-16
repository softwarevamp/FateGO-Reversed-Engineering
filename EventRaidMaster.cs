using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class EventRaidMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<QuestReleaseEntity> <>f__am$cache0;
    [CompilerGenerated]
    private static Comparison<EventRaidEntity> <>f__am$cache1;

    public EventRaidMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_RAID);
        if (DataMasterBase._never)
        {
            Debug.Log(new EventRaidEntity[1].ToString());
        }
    }

    public int GetCurrentDay(int eventId)
    {
        QuestReleaseEntity[] raidDeadQuestReleaseEntityList = this.GetRaidDeadQuestReleaseEntityList(eventId, 0);
        long num = NetworkManager.getTime();
        int num2 = 0;
        int length = raidDeadQuestReleaseEntityList.Length;
        for (int i = 0; i < length; i++)
        {
            QuestReleaseEntity entity = raidDeadQuestReleaseEntityList[i];
            int qid = entity.getQuestId();
            int num6 = entity.getValue();
            EventRaidEntity entity2 = base.getEntityFromId<EventRaidEntity>(eventId, num6);
            if (entity2 == null)
            {
                return num2;
            }
            if (!SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(qid, false))
            {
                if (num >= entity2.startedAt)
                {
                    return num6;
                }
                return num2;
            }
            if (i == (length - 1))
            {
                EventEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(entity2.eventId);
                if (num < entity3.getEventEndedAt())
                {
                    return num6;
                }
            }
            num2 = num6;
        }
        return 0;
    }

    public EventRaidEntity[] GetCurrentGroupListByEntity(EventRaidEntity currentEventRaidEntity)
    {
        List<EventRaidEntity> list = new List<EventRaidEntity>();
        if (currentEventRaidEntity != null)
        {
            foreach (EventRaidEntity entity in base.list)
            {
                if (entity.groupIndex == currentEventRaidEntity.groupIndex)
                {
                    list.Add(entity);
                }
            }
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = (a, b) => a.day - b.day;
            }
            list.Sort(<>f__am$cache1);
        }
        return list.ToArray();
    }

    public EventRaidEntity[] GetCurrentGroupListByEventId(int eventId)
    {
        int currentDay = this.GetCurrentDay(eventId);
        EventRaidEntity currentEventRaidEntity = base.getEntityFromId<EventRaidEntity>(eventId, currentDay);
        return this.GetCurrentGroupListByEntity(currentEventRaidEntity);
    }

    public int GetDayCount(int eventId)
    {
        int num = 0;
        foreach (EventRaidEntity entity in base.list)
        {
            if (entity.eventId == eventId)
            {
                num++;
            }
        }
        return num;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventRaidEntity>(obj);

    public long GetMaxHp(int eventId)
    {
        long num = 0L;
        foreach (EventRaidEntity entity in base.list)
        {
            if (entity.eventId == eventId)
            {
                num += entity.maxHp;
            }
        }
        return num;
    }

    private int[] GetQuestIds(int eventId, CondType.Kind condType, int day)
    {
        List<int> list = new List<int>();
        QuestReleaseEntity[] entityArray = this.GetQuestReleaseEntityList(eventId, condType, day);
        int length = entityArray.Length;
        for (int i = 0; i < length; i++)
        {
            list.Add(entityArray[i].getQuestId());
        }
        return list.ToArray();
    }

    private QuestReleaseEntity[] GetQuestReleaseEntityList(int eventId, CondType.Kind condType, int day = 0)
    {
        int[] questIdListByGroupId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestGroupMaster>(DataNameKind.Kind.QUEST_GROUP).GetQuestIdListByGroupId(eventId, QuestGroupType.Type.EVENT_QUEST);
        QuestReleaseMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE);
        List<QuestReleaseEntity> list = new List<QuestReleaseEntity>();
        foreach (int num in questIdListByGroupId)
        {
            QuestReleaseEntity item = master2.getEntityFromId<QuestReleaseEntity>(num, (int) condType, eventId);
            if ((item != null) && ((day <= 0) || (item.getValue() == day)))
            {
                list.Add(item);
            }
        }
        if (day == 0)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = (a, b) => a.getValue() - b.getValue();
            }
            list.Sort(<>f__am$cache0);
        }
        return list.ToArray();
    }

    public int[] GetRaidAliveQuestIds(int eventId, int day) => 
        this.GetQuestIds(eventId, CondType.Kind.RAID_ALIVE, day);

    public int GetRaidDeadQuestId(int eventId, int day)
    {
        int[] numArray = this.GetQuestIds(eventId, CondType.Kind.RAID_DEAD, day);
        if (numArray.Length <= 0)
        {
            return 0;
        }
        return numArray[0];
    }

    private QuestReleaseEntity[] GetRaidDeadQuestReleaseEntityList(int eventId, int day = 0) => 
        this.GetQuestReleaseEntityList(eventId, CondType.Kind.RAID_DEAD, day);
}

