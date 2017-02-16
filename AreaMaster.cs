using System;

public class AreaMaster : DataMasterBase
{
    public AreaMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.AREA);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new AreaEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<AreaEntity>(obj);
}

