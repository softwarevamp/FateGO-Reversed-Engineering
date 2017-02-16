using System;

public class ServantCardMaster : DataMasterBase
{
    public ServantCardMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_CARD);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantCardEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantCardEntity>(obj);
}

