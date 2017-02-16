using System;

public class BoxGachaHistoryMaster : DataMasterBase
{
    public BoxGachaHistoryMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BOX_GACHA_HISTORY);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BoxGachaHistoryEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BoxGachaHistoryEntity>(obj);
}

