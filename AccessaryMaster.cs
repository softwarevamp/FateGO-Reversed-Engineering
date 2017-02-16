using System;

public class AccessaryMaster : DataMasterBase
{
    public AccessaryMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.ACCESSORY);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new AccessaryEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<AccessaryEntity>(obj);
}

