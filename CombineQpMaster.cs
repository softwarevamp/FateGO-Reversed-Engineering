using System;

public class CombineQpMaster : DataMasterBase
{
    public CombineQpMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.COMBINE_QP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new CombineQpEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<CombineQpEntity>(obj);
}

