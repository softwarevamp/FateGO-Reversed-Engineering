using System;

public class UserEventMissionCondDetailMaster : DataMasterBase
{
    public UserEventMissionCondDetailMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_EVENT_MISSION_COND_DETAIL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserEventMissionCondDetailEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserEventMissionCondDetailEntity>(obj);
}

