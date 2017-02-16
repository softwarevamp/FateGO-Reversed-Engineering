using System;
using System.Collections.Generic;

public class ItemMaster : DataMasterBase
{
    private int anonymousId;
    private int friendPointId;
    private int manaId;
    private int qpId;
    private int stoneFragmentsId;
    private int stoneId;

    public ItemMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.ITEM);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ItemEntity[1]);
        }
    }

    public ItemEntity[] GetEntitiyList()
    {
        List<ItemEntity> list = new List<ItemEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ItemEntity item = base.list[i] as ItemEntity;
            if ((item != null) && (item.sellQp > 0))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public ItemEntity GetEntityByType(int itemType)
    {
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ItemEntity entity = base.list[i] as ItemEntity;
            if ((entity != null) && (entity.type == itemType))
            {
                return entity;
            }
        }
        return null;
    }

    public ItemEntity getEntityFromId(int id)
    {
        string key = string.Empty + id;
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as ItemEntity);
        }
        return null;
    }

    public ItemEntity GetEventPoint(int eventId)
    {
        foreach (int num in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).GetEventItemList(eventId))
        {
            ItemEntity entity = this.getEntityFromId(num);
            if ((entity != null) && (entity.type == 14))
            {
                return entity;
            }
        }
        return null;
    }

    public ItemEntity[] GetIndividualityList(int individuality)
    {
        List<ItemEntity> list = new List<ItemEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ItemEntity item = base.list[i] as ItemEntity;
            if ((item != null) && (item.individuality != null))
            {
                foreach (int num3 in item.individuality)
                {
                    if (num3 == individuality)
                    {
                        list.Add(item);
                        break;
                    }
                }
            }
        }
        return list.ToArray();
    }

    public ItemEntity[] GetIndividualityList(int[] individualityList)
    {
        List<ItemEntity> list = new List<ItemEntity>();
        if (individualityList != null)
        {
            int count = base.list.Count;
            for (int i = 0; i < count; i++)
            {
                ItemEntity item = base.list[i] as ItemEntity;
                if ((item != null) && (item.individuality != null))
                {
                    foreach (int num3 in item.individuality)
                    {
                        foreach (int num5 in individualityList)
                        {
                            if (num3 == num5)
                            {
                                list.Add(item);
                                item = null;
                                break;
                            }
                        }
                        if (item == null)
                        {
                            break;
                        }
                    }
                }
            }
        }
        return list.ToArray();
    }

    public ItemEntity GetItemData(int itemId)
    {
        ItemEntity entity = this.getEntityFromId(itemId);
        if ((entity != null) && entity.IsEnable())
        {
            return entity;
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ItemEntity>(obj);

    public bool isFriendPoint(int id) => 
        (id == this.friendPointId);

    public bool isMana(int id) => 
        (id == this.manaId);

    public bool isQP(int id) => 
        (id == this.qpId);

    public override bool preProcess()
    {
        foreach (ItemEntity entity in base.list)
        {
            if (entity.type == 1)
            {
                this.qpId = entity.id;
            }
            else if (entity.type == 13)
            {
                this.friendPointId = entity.id;
            }
            else if (entity.type == 5)
            {
                this.manaId = entity.id;
            }
            else if (entity.type == 2)
            {
                this.stoneId = entity.id;
            }
        }
        return true;
    }
}

