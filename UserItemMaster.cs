using System;
using System.Collections.Generic;

public class UserItemMaster : DataMasterBase
{
    public UserItemMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_ITEM);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserItemEntity[1]);
        }
    }

    public UserItemEntity getEntityFromId(long userId, int itemId)
    {
        object[] objArray1 = new object[] { string.Empty, userId, ":", itemId };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as UserItemEntity);
        }
        return new UserItemEntity(userId, itemId);
    }

    public UserItemEntity[] getList() => 
        this.getList(NetworkManager.UserId);

    public UserItemEntity[] getList(long userId)
    {
        int count = base.list.Count;
        List<UserItemEntity> list = new List<UserItemEntity>();
        for (int i = 0; i < count; i++)
        {
            UserItemEntity item = base.list[i] as UserItemEntity;
            if ((item != null) && (item.userId == userId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserItemEntity>(obj);

    public int getSum(long userId)
    {
        int count = base.list.Count;
        int num2 = 0;
        for (int i = 0; i < count; i++)
        {
            UserItemEntity entity = base.list[i] as UserItemEntity;
            if ((entity != null) && (entity.userId == userId))
            {
                num2++;
            }
        }
        return num2;
    }
}

