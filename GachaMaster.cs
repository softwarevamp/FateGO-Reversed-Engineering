using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GachaMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<GachaEntity> <>f__am$cache0;
    [CompilerGenerated]
    private static Comparison<GachaEntity> <>f__am$cache1;

    public GachaMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.GACHA);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new GachaEntity[1]);
        }
    }

    public GachaEntity getFriendPointGachaEntity()
    {
        List<GachaEntity> list = new List<GachaEntity>(this.getListByPayType(PayType.Type.FRIEND_POINT));
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => b.priority - a.priority;
        }
        list.Sort(<>f__am$cache0);
        return list[0];
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<GachaEntity>(obj);

    public GachaEntity[] getListByPayType(PayType.Type gachaType)
    {
        long num = NetworkManager.getTime();
        List<GachaEntity> list = new List<GachaEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            GachaEntity item = base.list[i] as GachaEntity;
            if (((item != null) && (item.type == gachaType)) && ((num >= item.openedAt) && (num <= item.closedAt)))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public GachaEntity[] getListValidData()
    {
        long num = NetworkManager.getTime();
        List<GachaEntity> list = new List<GachaEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            GachaEntity item = base.list[i] as GachaEntity;
            if (((item != null) && (num >= item.openedAt)) && (num <= item.closedAt))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public int getVaildPayType()
    {
        List<GachaEntity> list = new List<GachaEntity>(this.getListValidData());
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = (a, b) => b.priority - a.priority;
        }
        list.Sort(<>f__am$cache1);
        return list[0].type;
    }
}

