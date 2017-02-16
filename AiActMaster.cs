using System;

public class AiActMaster : DataMasterBase
{
    public AiActMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.AI_ACT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new AiActEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<AiActEntity>(obj);
}

