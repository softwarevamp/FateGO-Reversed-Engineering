using System;

public class NpcServantFollowerMaster : DataMasterBase
{
    public NpcServantFollowerMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.NPC_SERVANT_FOLLOWER);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new NpcServantFollowerEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<NpcServantFollowerEntity>(obj);
}

