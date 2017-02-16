using System;

public class UserQuestMaster : DataMasterBase
{
    public UserQuestMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_QUEST);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserQuestEntity[1]);
        }
    }

    public UserQuestEntity getEntityFromId(long userId, int questId)
    {
        object[] objArray1 = new object[] { string.Empty, userId, ":", questId };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as UserQuestEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserQuestEntity>(obj);
}

