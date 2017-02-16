using System;
using System.Collections.Generic;

public class BankShopMaster : DataMasterBase
{
    public BankShopMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BANK_SHOP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BankShopEntity[1]);
        }
    }

    public BankShopEntity[] GetEnableEntitiyList()
    {
        long num = NetworkManager.getTime();
        List<BankShopEntity> list = new List<BankShopEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            BankShopEntity item = base.list[i] as BankShopEntity;
            if (((item != null) && (num >= item.openedAt)) && (num <= item.closedAt))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BankShopEntity>(obj);
}

