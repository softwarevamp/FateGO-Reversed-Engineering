using System;

public class UserAccessaryMaster : DataMasterBase
{
    public UserAccessaryMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_ACCESSORY);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserAccessaryEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserAccessaryEntity>(obj);
}

