using System;

public class UserServantLearderMaster : DataMasterBase
{
    public UserServantLearderMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_SERVANT_LEADER);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserServantLearderEntity[1]);
        }
    }

    public UserServantLearderEntity getEntityFromId(int classId)
    {
        string key = string.Empty + classId;
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as UserServantLearderEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserServantLearderEntity>(obj);
}

