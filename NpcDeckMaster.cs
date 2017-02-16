using System;

public class NpcDeckMaster : DataMasterBase
{
    public NpcDeckMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.NPC_DECK);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new NpcDeckEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<NpcDeckEntity>(obj);
}

