using System;

public class WarMaster : DataMasterBase
{
    public WarMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.WAR);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new WarEntity[1]);
        }
    }

    public WarEntity getByEventId(int event_id)
    {
        for (int i = 0; i < base.list.Count; i++)
        {
            WarEntity entity = base.list[i] as WarEntity;
            if (entity.eventId == event_id)
            {
                return entity;
            }
        }
        return null;
    }

    public WarEntity getByLastQuestId(int last_quest_id)
    {
        for (int i = 0; i < base.list.Count; i++)
        {
            WarEntity entity = base.list[i] as WarEntity;
            if (entity.lastQuestId == last_quest_id)
            {
                return entity;
            }
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<WarEntity>(obj);
}

