using System;
using System.Collections.Generic;

public class StoneShopMaster : DataMasterBase
{
    public StoneShopMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.STONE_SHOP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new StoneShopEntity[1]);
        }
    }

    public StoneShopEntity[] GetEnableEntitiyList()
    {
        long num = NetworkManager.getTime();
        List<StoneShopEntity> list = new List<StoneShopEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            StoneShopEntity item = base.list[i] as StoneShopEntity;
            if (((item != null) && (num >= item.openedAt)) && (num <= item.closedAt))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public StoneShopEntity[] GetEnableEntitiyList(StoneShopEffect.Kind effect)
    {
        long num = NetworkManager.getTime();
        List<StoneShopEntity> list = new List<StoneShopEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            StoneShopEntity item = base.list[i] as StoneShopEntity;
            if (((item != null) && (item.effect == effect)) && ((num >= item.openedAt) && (num <= item.closedAt)))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<StoneShopEntity>(obj);
}

