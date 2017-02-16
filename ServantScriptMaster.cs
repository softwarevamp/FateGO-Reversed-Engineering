using System;

public class ServantScriptMaster : DataMasterBase
{
    public ServantScriptMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_SCRIPT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantScriptEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantScriptEntity>(obj);
}

