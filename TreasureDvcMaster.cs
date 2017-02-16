using System;

public class TreasureDvcMaster : DataMasterBase
{
    public TreasureDvcMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.TREASUREDEVICE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new TreasureDvcEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<TreasureDvcEntity>(obj);
}

