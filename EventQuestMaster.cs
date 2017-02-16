using System;
using System.Runtime.InteropServices;

public class EventQuestMaster : DataMasterBase
{
    public EventQuestMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_QUEST);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventQuestEntity[1]);
        }
    }

    public EventQuestEntity getData(int quest_id, GameEventType.TYPE event_type = 0)
    {
        EventMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT);
        foreach (DataEntityBase base2 in base.list)
        {
            EventQuestEntity entity = base2 as EventQuestEntity;
            if ((entity != null) && (entity.questId == quest_id))
            {
                if (event_type == GameEventType.TYPE.NONE)
                {
                    return entity;
                }
                EventEntity entity2 = master.getEntityFromId<EventEntity>(entity.getEventId());
                if (event_type == entity2.getEventType())
                {
                    return entity;
                }
            }
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventQuestEntity>(obj);
}

