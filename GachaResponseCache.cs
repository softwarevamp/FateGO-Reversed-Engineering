using System;

public class GachaResponseCache : DataMasterBase
{
    public GachaResponseCache()
    {
        base.cachename = "gacha_draw";
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new GachaResponseData[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<GachaResponseData>(obj);
}

