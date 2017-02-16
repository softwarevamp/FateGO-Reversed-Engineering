using System;

public class ServantRarityMaster : DataMasterBase
{
    public ServantRarityMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_RARITY);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantRarityEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantRarityEntity>(obj);
}

