using System;

public class UserTermMaster : DataMasterBase
{
    public UserTermMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_TERM);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserTermEntity[1]);
        }
    }

    public UserTermEntity getEntityFromId(string id)
    {
        string key = string.Empty + id;
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as UserTermEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserTermEntity>(obj);
}

