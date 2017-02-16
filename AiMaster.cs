using System;
using System.Collections.Generic;

public class AiMaster : DataMasterBase
{
    public AiMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.AI);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new AiEntity[1]);
        }
    }

    public void debugUpdate(AiEntity aiEnt)
    {
        string key = aiEnt.getPrimarykey();
        Debug.LogError("key:" + key);
        if (base.lookup.ContainsKey(key))
        {
            base.list.Remove(base.lookup[key]);
            base.lookup.Remove(key);
        }
        base.lookup.Add(key, aiEnt);
        base.list.Add(aiEnt);
        Debug.LogError("addEnd:");
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<AiEntity>(obj);

    public static AiEntity[] getListFormGroupId(int id)
    {
        List<AiEntity> list = new List<AiEntity>();
        foreach (DataEntityBase base2 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<AiMaster>(DataNameKind.Kind.AI).list)
        {
            AiEntity item = (AiEntity) base2;
            if (item.id == id)
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }
}

