using System;

public class GachaImageMaster : DataMasterBase
{
    public GachaImageMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.GACHA_IMAGE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new GachaImageEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<GachaImageEntity>(obj);
}

