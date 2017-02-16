using System;

public class BgmMaster : DataMasterBase
{
    public BgmMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BGM);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BgmEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BgmEntity>(obj);
}

