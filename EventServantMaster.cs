using System;

public class EventServantMaster : DataMasterBase
{
    public EventServantMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_SERVANT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventServantEntity[1]);
        }
    }

    public EventServantEntity getEntity(int svtId)
    {
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            EventServantEntity entity = base.list[i] as EventServantEntity;
            if ((entity != null) && (entity.svtId == svtId))
            {
                return entity;
            }
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventServantEntity>(obj);
}

