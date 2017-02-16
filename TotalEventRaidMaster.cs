using System;

public class TotalEventRaidMaster : DataMasterBase
{
    public TotalEventRaidMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.TOTAL_EVENT_RAID);
        if (DataMasterBase._never)
        {
            Debug.Log(new TotalEventRaidEntity[1].ToString());
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<TotalEventRaidEntity>(obj);
}

