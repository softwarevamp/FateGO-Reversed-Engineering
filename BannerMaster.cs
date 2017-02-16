using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class BannerMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<BannerEntity> <>f__am$cache0;

    public BannerMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BANNER);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BannerEntity[1]);
        }
    }

    public BannerEntity[] GetEnableEntitiyList()
    {
        List<BannerEntity> list = new List<BannerEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            BannerEntity item = base.list[i] as BannerEntity;
            if ((item != null) && item.IsEnable())
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => b.terminalBannerPriority - a.terminalBannerPriority;
        }
        list.Sort(<>f__am$cache0);
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BannerEntity>(obj);
}

