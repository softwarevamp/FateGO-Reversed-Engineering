using System;

public class UserSubEquipMaster : DataMasterBase
{
    public UserSubEquipMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_SUB_EQUIP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserSubEquipEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserSubEquipEntity>(obj);
}

