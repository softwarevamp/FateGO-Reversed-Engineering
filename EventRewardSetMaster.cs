using System;

public class EventRewardSetMaster : DataMasterBase
{
    public EventRewardSetMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_REWARD_SET);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventRewardSetEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventRewardSetEntity>(obj);
}

