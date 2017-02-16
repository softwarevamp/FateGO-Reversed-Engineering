using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventMissionActionMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<EventMissionActionEntity> <>f__am$cache0;

    public EventMissionActionMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_MISSION_ACTION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventMissionActionEntity[1]);
        }
    }

    public EventMissionActionEntity getEntityFromId(int missionId, int missionProgressType, int id)
    {
        object[] objArray1 = new object[] { string.Empty, missionId, ":", missionProgressType, ":", id };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as EventMissionActionEntity);
        }
        return null;
    }

    public List<EventMissionActionEntity> getEntityListFromIDnType(int missionID, MissionProgressType.Type progressType)
    {
        List<EventMissionActionEntity> list = new List<EventMissionActionEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            EventMissionActionEntity item = base.list[i] as EventMissionActionEntity;
            if (((list != null) && (item.missionId == missionID)) && (item.missionProgressType == progressType))
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => a.id - b.id;
        }
        list.Sort(<>f__am$cache0);
        return list;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventMissionActionEntity>(obj);
}

