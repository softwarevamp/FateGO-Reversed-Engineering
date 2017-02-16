using System;

public class UserBoxGachaMaster : DataMasterBase
{
    public UserBoxGachaMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_BOX_GACHA);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserBoxGachaEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserBoxGachaEntity>(obj);
}

