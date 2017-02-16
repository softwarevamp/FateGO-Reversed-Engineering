using System;

public class ServantExceedMaster : DataMasterBase
{
    public ServantExceedMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_EXCEED);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantExceedEntity[1]);
        }
    }

    public ServantExceedEntity getEntityFromId(int rarity, int exceedCount)
    {
        string key = rarity.ToString() + ":" + exceedCount.ToString();
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as ServantExceedEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantExceedEntity>(obj);
}

