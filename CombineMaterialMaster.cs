using System;

public class CombineMaterialMaster : DataMasterBase
{
    public CombineMaterialMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.COMBINE_MATERIAL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new CombineMaterialEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<CombineMaterialEntity>(obj);
}

