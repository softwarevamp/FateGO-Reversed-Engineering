using System;
using System.Collections.Generic;

public class UserEventMaster : DataMasterBase
{
    public UserEventMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_EVENT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserEventEntity[1]);
        }
    }

    public UserEventEntity getEntityFromId(long userId, int eventId)
    {
        object[] objArray1 = new object[] { string.Empty, userId, ":", eventId };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as UserEventEntity);
        }
        return new UserEventEntity(userId, eventId);
    }

    public UserEventEntity[] getList() => 
        this.getList(NetworkManager.UserId);

    public UserEventEntity[] getList(long userId)
    {
        int count = base.list.Count;
        List<UserEventEntity> list = new List<UserEventEntity>();
        for (int i = 0; i < count; i++)
        {
            UserEventEntity item = base.list[i] as UserEventEntity;
            if ((item != null) && (item.userId == userId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserEventEntity>(obj);
}

