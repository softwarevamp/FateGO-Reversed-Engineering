using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ShopReleaseMaster : DataMasterBase
{
    public ShopReleaseMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SHOP_RELEASE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ShopReleaseEntity[1]);
        }
    }

    public ShopReleaseEntity[] GetEntitiyList(int shopId)
    {
        int count = base.list.Count;
        List<ShopReleaseEntity> list = new List<ShopReleaseEntity>();
        for (int i = 0; i < count; i++)
        {
            ShopReleaseEntity item = base.list[i] as ShopReleaseEntity;
            if ((item != null) && (item.shopId == shopId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public ShopReleaseEntity[] GetEntitiyList(int shopId, bool isClosedDisp)
    {
        int count = base.list.Count;
        List<ShopReleaseEntity> list = new List<ShopReleaseEntity>();
        for (int i = 0; i < count; i++)
        {
            ShopReleaseEntity item = base.list[i] as ShopReleaseEntity;
            if (((item != null) && (item.shopId == shopId)) && (item.IsClosedDisp == isClosedDisp))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ShopReleaseEntity>(obj);

    public int GetPurchaseShop(int shopId)
    {
        ShopReleaseEntity[] entitiyList = this.GetEntitiyList(shopId, false);
        int length = entitiyList.Length;
        for (int i = 0; i < length; i++)
        {
            ShopReleaseEntity entity = entitiyList[i];
            if ((entity.condType == 14) || (entity.condType == 15))
            {
                return entity.condValue;
            }
        }
        return 0;
    }

    public bool IsOpen(int shopId)
    {
        ShopReleaseEntity[] entitiyList = this.GetEntitiyList(shopId, false);
        int length = entitiyList.Length;
        for (int i = 0; i < length; i++)
        {
            ShopReleaseEntity entity = entitiyList[i];
            if (!entity.IsOpen())
            {
                return false;
            }
        }
        return true;
    }

    public bool IsPreparation(out string message, int shopId)
    {
        ShopReleaseEntity[] entitiyList = this.GetEntitiyList(shopId, true);
        int length = entitiyList.Length;
        for (int i = 0; i < length; i++)
        {
            ShopReleaseEntity entity = entitiyList[i];
            if (entity.IsPreparation())
            {
                message = entity.GetPreparationConditionText();
                return true;
            }
        }
        message = null;
        return false;
    }
}

