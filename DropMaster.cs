using System;

public class DropMaster : DataMasterBase
{
    public DropMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.DROP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new DropEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<DropEntity>(obj);
}

