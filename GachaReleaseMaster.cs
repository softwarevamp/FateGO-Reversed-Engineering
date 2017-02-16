using System;

public class GachaReleaseMaster : DataMasterBase
{
    public GachaReleaseMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.GACHA_RELEASE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new GachaReleaseEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<GachaReleaseEntity>(obj);
}

