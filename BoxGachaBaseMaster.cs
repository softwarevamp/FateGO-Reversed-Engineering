using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class BoxGachaBaseMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<BoxGachaBaseEntity> <>f__am$cache0;

    public BoxGachaBaseMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BOX_GACHA_BASE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BoxGachaBaseEntity[1]);
        }
    }

    public BoxGachaBaseEntity[] getGachaBaseList(int id)
    {
        List<BoxGachaBaseEntity> list = new List<BoxGachaBaseEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            BoxGachaBaseEntity item = base.list[i] as BoxGachaBaseEntity;
            if ((item != null) && ((id <= 0) || (id == item.id)))
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
        JsonManager.DeserializeArray<BoxGachaBaseEntity>(obj);
}

