using System;

public class ApRecoverMaster : DataMasterBase
{
    public ApRecoverMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.AP_RECOVER);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ApRecoverEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ApRecoverEntity>(obj);
}

