using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventMissionMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<EventMissionEntity> <>f__am$cache0;

    public EventMissionMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_MISSION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventMissionEntity[1]);
        }
    }

    public EventMissionEntity[] getEventMissionList(int eventId)
    {
        List<EventMissionEntity> list = new List<EventMissionEntity>();
        long num = NetworkManager.getTime();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            EventMissionEntity item = base.list[i] as EventMissionEntity;
            if (((item != null) && (eventId == item.eventId)) && ((num >= item.startedAt) && (num <= item.closedAt)))
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => a.id - b.id;
        }
        list.Sort(<>f__am$cache0);
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventMissionEntity>(obj);

    public int[] getMissionIdListByEvent(int eventId)
    {
        List<int> list = new List<int>();
        long num = NetworkManager.getTime();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            EventMissionEntity entity = base.list[i] as EventMissionEntity;
            if (((entity != null) && (entity.eventId == eventId)) && ((num >= entity.startedAt) && (num <= entity.closedAt)))
            {
                list.Add(entity.id);
            }
        }
        return list.ToArray();
    }
}

