using System;
using System.Collections.Generic;

public class EventMissionConditionMaster : DataMasterBase
{
    public EventMissionConditionMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_MISSION_CONDITION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventMissionConditionEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventMissionConditionEntity>(obj);

    public EventMissionConditionEntity[] getMissionCondList(int eventId, int missionId)
    {
        List<EventMissionConditionEntity> list = new List<EventMissionConditionEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            EventMissionConditionEntity item = base.list[i] as EventMissionConditionEntity;
            if (((item != null) && (item.eventId == eventId)) && (item.missionId == missionId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public EventMissionConditionEntity[] getMissionCondListByType(int eventId, int missionId, int type)
    {
        EventMissionConditionEntity[] entityArray = this.getMissionCondList(eventId, missionId);
        List<EventMissionConditionEntity> list = new List<EventMissionConditionEntity>();
        if (entityArray.Length > 0)
        {
            foreach (EventMissionConditionEntity entity in entityArray)
            {
                if (entity.missionProgressType == type)
                {
                    list.Add(entity);
                }
            }
        }
        return list.ToArray();
    }
}

