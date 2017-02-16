using System;

public class IllustratorMaster : DataMasterBase
{
    public IllustratorMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.ILLUSTRATOR);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new IllustratorEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<IllustratorEntity>(obj);
}

