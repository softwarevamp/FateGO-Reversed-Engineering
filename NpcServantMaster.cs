using System;

public class NpcServantMaster : DataMasterBase
{
    public NpcServantMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.NPC_SERVANT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new NpcServantEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<NpcServantEntity>(obj);
}

