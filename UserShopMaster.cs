using System;

public class UserShopMaster : DataMasterBase
{
    public UserShopMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_SHOP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserShopEntity[1]);
        }
    }

    public UserShopEntity getEntityFromId(long userId, int shopId)
    {
        object[] objArray1 = new object[] { string.Empty, userId, ":", shopId };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as UserShopEntity);
        }
        return new UserShopEntity(userId, shopId);
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserShopEntity>(obj);
}

