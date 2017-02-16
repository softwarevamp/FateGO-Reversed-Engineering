using System;

public class UserContinueMaster : DataMasterBase
{
    public UserContinueMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_CONTINUE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserContinueEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserContinueEntity>(obj);
}

