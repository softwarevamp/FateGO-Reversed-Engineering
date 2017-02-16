using System;
using System.Collections.Generic;

public class UserProfileMaster : DataMasterBase
{
    public UserProfileMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_PROFILE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserProfileEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserProfileEntity>(obj);

    public OtherUserGameEntity[] GetOtherUserList()
    {
        OtherUserGameMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<OtherUserGameMaster>(DataNameKind.Kind.OTHER_USER_GAME);
        List<OtherUserGameEntity> list = new List<OtherUserGameEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserProfileEntity entity = base.list[i] as UserProfileEntity;
            if (entity != null)
            {
                list.Add(master.getEntityFromId<OtherUserGameEntity>(entity.userId));
            }
        }
        return list.ToArray();
    }

    public int GetOtherUserSum()
    {
        int num = 0;
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            if (base.list[i] is UserProfileEntity)
            {
                num++;
            }
        }
        return num;
    }
}

