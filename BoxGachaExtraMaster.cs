using System;

public class BoxGachaExtraMaster : DataMasterBase
{
    public BoxGachaExtraMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BOX_GACHA_EXTRA);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BoxGachaExtraEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BoxGachaExtraEntity>(obj);
}

