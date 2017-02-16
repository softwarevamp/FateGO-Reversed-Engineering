using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class ServantTreasureDvcMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<ServantTreasureDvcEntity> <>f__am$cache1;
    protected Dictionary<string, int[]> minmaxCache = new Dictionary<string, int[]>();

    public ServantTreasureDvcMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_TREASUREDEVICE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantTreasureDvcEntity[1]);
        }
    }

    protected void CreateAllPriorityCache()
    {
        foreach (DataEntityBase base2 in base.list)
        {
            ServantTreasureDvcEntity entity = (ServantTreasureDvcEntity) base2;
            string key = this.getPriorityKey(entity.svtId, entity.num);
            if (!this.minmaxCache.ContainsKey(key))
            {
                this.minmaxCache[key] = new int[] { 0x7fffffff, -2147483648 };
            }
            if (this.minmaxCache[key][0] > entity.priority)
            {
                this.minmaxCache[key][0] = (int[]) entity.priority;
            }
            if (this.minmaxCache[key][1] < entity.priority)
            {
                this.minmaxCache[key][1] = (int[]) entity.priority;
            }
        }
    }

    public ServantTreasureDvcEntity getEntityFromId(int svtId, int num, int priority)
    {
        object[] objArray1 = new object[] { string.Empty, svtId, ":", num, ":", priority };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as ServantTreasureDvcEntity);
        }
        return null;
    }

    public static ServantTreasureDvcEntity getEntityFromIDID(int svtId, int dvcId)
    {
        foreach (DataEntityBase base2 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantTreasureDvcMaster>(DataNameKind.Kind.SERVANT_TREASUREDEVICE).list)
        {
            ServantTreasureDvcEntity entity = (ServantTreasureDvcEntity) base2;
            if ((entity.svtId == svtId) && (entity.treasureDeviceId == dvcId))
            {
                return entity;
            }
        }
        return null;
    }

    public ServantTreasureDvcEntity getEntityFromSvtIdDvcId(int svtId, int dvcId)
    {
        foreach (DataEntityBase base2 in base.list)
        {
            ServantTreasureDvcEntity entity = base2 as ServantTreasureDvcEntity;
            if ((entity.svtId == svtId) && (entity.treasureDeviceId == dvcId))
            {
                return entity;
            }
        }
        return null;
    }

    public ServantTreasureDvcEntity[] GetEntityListFromIdNum(int svtId, int num)
    {
        List<ServantTreasureDvcEntity> list = new List<ServantTreasureDvcEntity>();
        int minPriority = this.GetMinPriority(svtId, num);
        int maxPriority = this.GetMaxPriority(svtId, num);
        for (int i = minPriority; i <= maxPriority; i++)
        {
            ServantTreasureDvcEntity item = this.getEntityFromId(svtId, num, i);
            if (item != null)
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = (a, b) => a.priority - b.priority;
        }
        list.Sort(<>f__am$cache1);
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantTreasureDvcEntity>(obj);

    public int GetMaxPriority(int svtId, int num)
    {
        string key = this.getPriorityKey(svtId, num);
        if (!this.minmaxCache.ContainsKey(key))
        {
            this.CreateAllPriorityCache();
        }
        if (this.minmaxCache.ContainsKey(key))
        {
            return this.minmaxCache[key][1];
        }
        return 1;
    }

    public int GetMinPriority(int svtId, int num)
    {
        string key = this.getPriorityKey(svtId, num);
        if (!this.minmaxCache.ContainsKey(key))
        {
            this.CreateAllPriorityCache();
        }
        if (this.minmaxCache.ContainsKey(key))
        {
            return this.minmaxCache[key][0];
        }
        return 1;
    }

    protected string getPriorityKey(int svtId, int num)
    {
        object[] objArray1 = new object[] { string.Empty, svtId, ":", num };
        return string.Concat(objArray1);
    }

    public bool GetRankInfo(out int tdRank, out int tdMaxRank, int svtId, int tdId)
    {
        ServantTreasureDvcEntity[] entityListFromIdNum = this.GetEntityListFromIdNum(svtId, 1);
        tdRank = 0;
        tdMaxRank = entityListFromIdNum.Length;
        int num = 0;
        foreach (ServantTreasureDvcEntity entity in entityListFromIdNum)
        {
            num++;
            if (entity.treasureDeviceId == tdId)
            {
                tdRank = num;
                return true;
            }
        }
        return false;
    }
}

