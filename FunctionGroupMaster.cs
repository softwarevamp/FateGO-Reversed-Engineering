using System;

public class FunctionGroupMaster : DataMasterBase
{
    public FunctionGroupMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.FUNCTION_GROUP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new FunctionGroupEntity[1]);
        }
    }

    public FunctionGroupEntity getEntityFromId(int funcId)
    {
        string key = string.Empty + funcId;
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as FunctionGroupEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<FunctionGroupEntity>(obj);
}

