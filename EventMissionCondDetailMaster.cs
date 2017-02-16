using System;

public class EventMissionCondDetailMaster : DataMasterBase
{
    public EventMissionCondDetailMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_MISSION_COND_DETAIL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventMissionCondDetailEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventMissionCondDetailEntity>(obj);
}

