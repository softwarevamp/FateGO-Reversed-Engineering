using System;

public class UserFormationMaster : DataMasterBase
{
    public UserFormationMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_FORMATION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserFormationEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserFormationEntity>(obj);
}

