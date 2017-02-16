using System;

public class ClosedMessageMaster : DataMasterBase
{
    public ClosedMessageMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.CLOSED_MESSAGE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ClosedMessageEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ClosedMessageEntity>(obj);
}

