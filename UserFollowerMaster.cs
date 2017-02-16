using System;

public class UserFollowerMaster : DataMasterBase
{
    public UserFollowerMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_FOLLOWER);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserFollowerEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserFollowerEntity>(obj);
}

