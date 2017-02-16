using System;

public class FunctionMaster : DataMasterBase
{
    public FunctionMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.FUNCTION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new FunctionEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<FunctionEntity>(obj);
}

