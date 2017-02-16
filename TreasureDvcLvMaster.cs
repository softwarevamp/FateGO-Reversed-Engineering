using System;

public class TreasureDvcLvMaster : DataMasterBase
{
    public TreasureDvcLvMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.TREASUREDEVICE_LEVEL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new TreasureDvcLvEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<TreasureDvcLvEntity>(obj);
}

