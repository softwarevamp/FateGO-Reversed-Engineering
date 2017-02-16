using System;
using System.Collections.Generic;

public class OtherUserGameMaster : DataMasterBase
{
    public OtherUserGameMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.OTHER_USER_GAME);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new OtherUserGameEntity[1]);
        }
    }

    public void continueDeviceOtherUser()
    {
        List<long> list = new List<long>();
        int count = base.list.Count;
        long userId = NetworkManager.UserId;
        for (int i = 0; i < count; i++)
        {
            OtherUserGameEntity entity = base.list[i] as OtherUserGameEntity;
            if ((entity != null) && (entity.userId == userId))
            {
                list.Add(entity.userId);
            }
        }
        OtherUserNewManager.SetOld(list.ToArray());
    }

    public OtherUserGameEntity[] GetFriendCodeList(string friendCode)
    {
        List<OtherUserGameEntity> list = new List<OtherUserGameEntity>();
        if (friendCode != null)
        {
            int count = base.list.Count;
            for (int i = 0; i < count; i++)
            {
                OtherUserGameEntity item = base.list[i] as OtherUserGameEntity;
                if ((item != null) && friendCode.Equals(item.friendCode))
                {
                    list.Add(item);
                }
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<OtherUserGameEntity>(obj);
}

