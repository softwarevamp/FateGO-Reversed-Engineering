using System;

public class ServantLimitMaster : DataMasterBase
{
    public ServantLimitMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_LIMIT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantLimitEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantLimitEntity>(obj);
}

