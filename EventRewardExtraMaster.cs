using System;

public class EventRewardExtraMaster : DataMasterBase
{
    public EventRewardExtraMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_REWARD_EXTRA);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventRewardExtraEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventRewardExtraEntity>(obj);
}

