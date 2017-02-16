using System;

public class TreasureDvcDetailMaster : DataMasterBase
{
    public TreasureDvcDetailMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.TREASUREDEVICE_DETAIL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new TreasureDvcDetailEntity[1]);
        }
    }

    public static string getDetail(int id)
    {
        TreasureDvcDetailMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcDetailMaster>(DataNameKind.Kind.TREASUREDEVICE_DETAIL);
        string key = string.Empty + id;
        if (master.lookup.ContainsKey(key))
        {
            return (master.lookup[key] as TreasureDvcDetailEntity).detail;
        }
        Debug.LogWarning("TreasureDvcDetailEntity[" + id + "] is null");
        return LocalizationManager.GetUnknownName();
    }

    public static string getDetailShort(int id)
    {
        TreasureDvcDetailMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcDetailMaster>(DataNameKind.Kind.TREASUREDEVICE_DETAIL);
        string key = string.Empty + id;
        if (master.lookup.ContainsKey(key))
        {
            return (master.lookup[key] as TreasureDvcDetailEntity).detailShort;
        }
        Debug.LogWarning("TreasureDvcDetailEntity[" + id + "] is null");
        return LocalizationManager.GetUnknownName();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<TreasureDvcDetailEntity>(obj);
}

