using System;

public class CombineLimitMaster : DataMasterBase
{
    public CombineLimitMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.COMBINE_LIMIT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new CombineLimitEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<CombineLimitEntity>(obj);
}

