using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventRewardMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<EventRewardEntity> <>f__am$cache0;

    public EventRewardMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_REWARD);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventRewardEntity[1]);
        }
    }

    public EventRewardEntity[] GetEventRewardEntitiyList(int eventId)
    {
        List<EventRewardEntity> list = new List<EventRewardEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            EventRewardEntity item = base.list[i] as EventRewardEntity;
            if ((item != null) && (item.eventId == eventId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventRewardEntity>(obj);

    public EventRewardEntity GetNextEventRewardEntity(int eventId, int eventPoint)
    {
        EventRewardEntity[] eventRewardEntitiyList = this.GetEventRewardEntitiyList(eventId);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (x, y) => x.point - y.point;
        }
        Array.Sort<EventRewardEntity>(eventRewardEntitiyList, <>f__am$cache0);
        foreach (EventRewardEntity entity in eventRewardEntitiyList)
        {
            if (eventPoint < entity.point)
            {
                return entity;
            }
        }
        return null;
    }
}

