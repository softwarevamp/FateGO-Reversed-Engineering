using System;

public class UserSuperBossMaster : DataMasterBase
{
    public UserSuperBossMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_SUPER_BOSS);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserSuperBossEntity[1]);
        }
    }

    public UserSuperBossEntity getEntityFromId(long userId, int eventId, int superBossId)
    {
        string[] textArray1 = new string[] { userId.ToString(), ":", eventId.ToString(), ":", superBossId.ToString() };
        string key = string.Concat(textArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as UserSuperBossEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserSuperBossEntity>(obj);

    public long getTotalDamagePoint(int eventId)
    {
        long num = 0L;
        long userId = NetworkManager.UserId;
        foreach (UserSuperBossEntity entity in base.list)
        {
            if (((entity != null) && (entity.userId == userId)) && (eventId == entity.eventId))
            {
                num += entity.damage;
            }
        }
        return Math.Min(num, BalanceConfig.UserSuperBossDamagePointMax);
    }
}

