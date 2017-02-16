using System;
using System.Collections.Generic;

public class ServantMaster : DataMasterBase
{
    public ServantMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantEntity[1]);
        }
    }

    public int[] GetCollectionList()
    {
        int count = base.list.Count;
        List<int> list = new List<int>();
        for (int i = 0; i < count; i++)
        {
            ServantEntity entity = base.list[i] as ServantEntity;
            if ((entity.type == 1) || (entity.type == 2))
            {
                list.Add(entity.id);
            }
        }
        return list.ToArray();
    }

    public int[] GetCollectionList(bool isEquip)
    {
        int count = base.list.Count;
        List<int> list = new List<int>();
        if (isEquip)
        {
            for (int i = 0; i < count; i++)
            {
                ServantEntity entity = base.list[i] as ServantEntity;
                if ((entity.collectionNo > 0) && entity.IsServantEquip)
                {
                    list.Add(entity.id);
                }
            }
        }
        else
        {
            for (int j = 0; j < count; j++)
            {
                ServantEntity entity2 = base.list[j] as ServantEntity;
                if ((entity2.collectionNo > 0) && entity2.IsServantCollection)
                {
                    list.Add(entity2.id);
                }
            }
        }
        return list.ToArray();
    }

    public ServantEntity[] GetIndividualityList(int individuality)
    {
        List<ServantEntity> list = new List<ServantEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ServantEntity item = base.list[i] as ServantEntity;
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

    public ServantEntity[] GetIndividualityList(int[] individualityList)
    {
        List<ServantEntity> list = new List<ServantEntity>();
        if (individualityList != null)
        {
            int count = base.list.Count;
            for (int i = 0; i < count; i++)
            {
                ServantEntity item = base.list[i] as ServantEntity;
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

    public Dictionary<int, int> GetLimitCountMaxList()
    {
        int count = base.list.Count;
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        for (int i = 0; i < count; i++)
        {
            ServantEntity entity = base.list[i] as ServantEntity;
            if (!entity.IsEnemy)
            {
                dictionary.Add(entity.id, !entity.IsServant ? 0 : entity.limitMax);
            }
        }
        return dictionary;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantEntity>(obj);
}

