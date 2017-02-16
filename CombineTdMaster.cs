using System;

public class CombineTdMaster : DataMasterBase
{
    public CombineTdMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.COMBINE_TREASUREDEVICE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new CombineTdEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<CombineTdEntity>(obj);
}

