using System;

public class UserGachaMaster : DataMasterBase
{
    public UserGachaMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_GACHA);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserGachaEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserGachaEntity>(obj);
}

