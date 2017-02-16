using System;

public class StageMaster : DataMasterBase
{
    public StageMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.STAGE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new StageEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<StageEntity>(obj);
}

