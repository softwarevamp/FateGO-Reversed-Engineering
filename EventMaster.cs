using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class EventMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<EventEntity> <>f__am$cache0;

    public EventMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventEntity[1]);
        }
    }

    public EventEntity[] GetEnableEntitiyList(GameEventType.TYPE eventType, bool is_finishedAt = true) => 
        this.GetEntitiyList(eventType, true, is_finishedAt);

    public EventEntity[] GetEntitiyList(GameEventType.TYPE eventType) => 
        this.GetEntitiyList(eventType, false, true);

    private EventEntity[] GetEntitiyList(GameEventType.TYPE eventType, bool is_open_get, bool is_finishedAt = true)
    {
        List<EventEntity> list = new List<EventEntity>();
        int count = base.list.Count;
        int num2 = (int) eventType;
        for (int i = 0; i < count; i++)
        {
            EventEntity item = base.list[i] as EventEntity;
            if (((item != null) && ((num2 <= 0) || (num2 == item.type))) && ((is_open_get && item.IsOpen(is_finishedAt)) || !is_open_get))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public EventEntity getEntityFromId(int id)
    {
        string key = string.Empty + id;
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as EventEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventEntity>(obj);

    public int getMyRoomBgImgId()
    {
        int count = base.list.Count;
        int myroomBgId = 0;
        for (int i = 0; i < count; i++)
        {
            EventEntity entity = base.list[i] as EventEntity;
            if (((entity != null) && (entity.myroomBgId > 0)) && entity.IsOpen(false))
            {
                myroomBgId = entity.myroomBgId;
            }
        }
        return myroomBgId;
    }

    public int getMyRoomBgmId()
    {
        int myroomBgmId = 0;
        foreach (EventEntity entity in base.list)
        {
            if (((entity != null) && (entity.myroomBgmId > 0)) && entity.IsOpen(false))
            {
                myroomBgmId = entity.myroomBgmId;
            }
        }
        return myroomBgmId;
    }

    public int GetPointEventImageId(int eventId)
    {
        EventDetailEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventDetailMaster>(DataNameKind.Kind.EVENT_DETAIL).getEntityFromId(eventId);
        if ((entity != null) && entity.isEventPoint)
        {
            return entity.pointImageId;
        }
        return 0;
    }

    public List<EventEntity> GetSortedEntityList()
    {
        List<EventEntity> list = new List<EventEntity>(this.GetEntitiyList(GameEventType.TYPE.EVENT_QUEST));
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate (EventEntity a, EventEntity b) {
                long num = b.getEventStartedAt() - a.getEventStartedAt();
                if (num < 0L)
                {
                    return -1;
                }
                if (num > 0L)
                {
                    return 1;
                }
                num = b.getEventEndedAt() - a.getEventEndedAt();
                if (num < 0L)
                {
                    return -1;
                }
                if (num > 0L)
                {
                    return 1;
                }
                return 0;
            };
        }
        list.Sort(<>f__am$cache0);
        return list;
    }
}

