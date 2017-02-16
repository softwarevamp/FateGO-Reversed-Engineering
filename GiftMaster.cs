using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GiftMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<GiftEntity> <>f__am$cache0;

    public GiftMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.GIFT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new GiftEntity[1]);
        }
    }

    public GiftEntity getDataById(int id)
    {
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            GiftEntity entity = base.list[i] as GiftEntity;
            if ((entity != null) && ((id <= 0) || (id == entity.id)))
            {
                return entity;
            }
        }
        return null;
    }

    public GiftEntity[] GetGiftListById(int gift_id)
    {
        List<GiftEntity> list = new List<GiftEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            GiftEntity item = base.list[i] as GiftEntity;
            if ((item != null) && (gift_id == item.id))
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => b.priority - a.priority;
        }
        list.Sort(<>f__am$cache0);
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<GiftEntity>(obj);
}

