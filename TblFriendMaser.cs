using System;
using System.Collections.Generic;

public class TblFriendMaser : DataMasterBase
{
    public TblFriendMaser()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.TBL_FRIEND);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new TblFriendEntity[1]);
        }
    }

    public int GetFriendSum()
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int num = 0;
        int count = base.list.Count;
        long userId = entity.userId;
        for (int i = 0; i < count; i++)
        {
            TblFriendEntity entity2 = base.list[i] as TblFriendEntity;
            if ((entity2 != null) && (entity2.status == 3))
            {
                if (entity2.userId == userId)
                {
                    num++;
                }
                else if (entity2.friendId == userId)
                {
                    num++;
                }
            }
        }
        return num;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<TblFriendEntity>(obj);

    public OtherUserGameEntity[] GetList(FriendStatus.Kind kind)
    {
        if (kind == FriendStatus.Kind.SEARCH)
        {
            return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserProfileMaster>(DataNameKind.Kind.USER_PROFILE).GetOtherUserList();
        }
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        OtherUserGameMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<OtherUserGameMaster>(DataNameKind.Kind.OTHER_USER_GAME);
        List<OtherUserGameEntity> list = new List<OtherUserGameEntity>();
        int count = base.list.Count;
        long userId = entity.userId;
        if ((kind == FriendStatus.Kind.OFFER) || (kind == FriendStatus.Kind.OFFERED))
        {
            for (int i = 0; i < count; i++)
            {
                TblFriendEntity entity2 = base.list[i] as TblFriendEntity;
                if ((entity2 != null) && ((entity2.status == 1) || (entity2.status == 2)))
                {
                    if (entity2.userId == userId)
                    {
                        if (entity2.status == kind)
                        {
                            OtherUserGameEntity item = master.getEntityFromId<OtherUserGameEntity>(entity2.friendId);
                            if (item != null)
                            {
                                list.Add(item);
                            }
                        }
                    }
                    else if ((entity2.friendId == userId) && (entity2.status != kind))
                    {
                        OtherUserGameEntity entity4 = master.getEntityFromId<OtherUserGameEntity>(entity2.userId);
                        if (entity4 != null)
                        {
                            list.Add(entity4);
                        }
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < count; j++)
            {
                TblFriendEntity entity5 = base.list[j] as TblFriendEntity;
                if ((entity5 != null) && (entity5.status == kind))
                {
                    if (entity5.userId == userId)
                    {
                        OtherUserGameEntity entity6 = master.getEntityFromId<OtherUserGameEntity>(entity5.friendId);
                        if (entity6 != null)
                        {
                            list.Add(entity6);
                        }
                    }
                    else if (entity5.friendId == userId)
                    {
                        OtherUserGameEntity entity7 = master.getEntityFromId<OtherUserGameEntity>(entity5.userId);
                        if (entity7 != null)
                        {
                            list.Add(entity7);
                        }
                    }
                }
            }
        }
        return list.ToArray();
    }

    public int GetSum(FriendStatus.Kind kind)
    {
        if (kind == FriendStatus.Kind.SEARCH)
        {
            return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserProfileMaster>(DataNameKind.Kind.USER_PROFILE).GetOtherUserSum();
        }
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int num = 0;
        int count = base.list.Count;
        long userId = entity.userId;
        if ((kind == FriendStatus.Kind.OFFER) || (kind == FriendStatus.Kind.OFFERED))
        {
            for (int j = 0; j < count; j++)
            {
                TblFriendEntity entity2 = base.list[j] as TblFriendEntity;
                if ((entity2 != null) && ((entity2.status == 1) || (entity2.status == 2)))
                {
                    if (entity2.userId == userId)
                    {
                        if (entity2.status == kind)
                        {
                            num++;
                        }
                    }
                    else if ((entity2.friendId == userId) && (entity2.status != kind))
                    {
                        num++;
                    }
                }
            }
            return num;
        }
        for (int i = 0; i < count; i++)
        {
            TblFriendEntity entity3 = base.list[i] as TblFriendEntity;
            if ((entity3 != null) && (entity3.status == kind))
            {
                if (entity3.userId == userId)
                {
                    num++;
                }
                else if (entity3.friendId == userId)
                {
                    num++;
                }
            }
        }
        return num;
    }
}

