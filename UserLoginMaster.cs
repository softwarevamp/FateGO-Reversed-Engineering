using System;

public class UserLoginMaster : DataMasterBase
{
    public UserLoginMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_LOGIN);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserLoginEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserLoginEntity>(obj);
}

