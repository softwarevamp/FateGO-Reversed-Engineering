using System;

public class EventDetailMaster : DataMasterBase
{
    public EventDetailMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_DETAIL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventDetailEntity[1]);
        }
    }

    public EventDetailEntity getEntityFromId(int eventId)
    {
        string key = string.Empty + eventId;
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as EventDetailEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventDetailEntity>(obj);
}

