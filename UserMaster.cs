using System;

public class UserMaster : DataMasterBase
{
    public UserMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserEntity>(obj);
}

