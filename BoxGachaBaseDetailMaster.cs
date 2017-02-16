using System;

public class BoxGachaBaseDetailMaster : DataMasterBase
{
    public BoxGachaBaseDetailMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BOX_GACHA_BASE_DETAIL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BoxGachaBaseDetailEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BoxGachaBaseDetailEntity>(obj);
}

