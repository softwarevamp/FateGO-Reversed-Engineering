using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class UserServantCollectionMaster : DataMasterBase
{
    public UserServantCollectionMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_SERVANT_COLLECTION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserServantCollectionEntity[1]);
        }
    }

    public void continueDeviceServantComment()
    {
        ServantCommentMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT);
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserServantCollectionEntity entity = base.list[i] as UserServantCollectionEntity;
            if (entity != null)
            {
                int[] newList = master.GetNewList(entity.svtId);
                if (newList.Length > 0)
                {
                    ServantCommentManager.SetOpen(entity.svtId, newList);
                }
            }
        }
    }

    public void continueDeviceUserServantCollection()
    {
        List<int> list = new List<int>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserServantCollectionEntity entity = base.list[i] as UserServantCollectionEntity;
            if ((entity != null) && (entity.status == 2))
            {
                list.Add(entity.svtId);
            }
        }
        UserServantCollectionManager.SetOld(list.ToArray());
    }

    public UserServantCollectionEntity[] getCollectionList(out int getSum, out int findSum, bool isEquip)
    {
        long userId = NetworkManager.UserId;
        int[] collectionList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).GetCollectionList(isEquip);
        int length = collectionList.Length;
        List<UserServantCollectionEntity> list = new List<UserServantCollectionEntity>();
        getSum = 0;
        findSum = 0;
        for (int i = 0; i < length; i++)
        {
            int svtId = collectionList[i];
            object[] objArray1 = new object[] { string.Empty, userId, ":", svtId };
            string key = string.Concat(objArray1);
            if (base.lookup.ContainsKey(key))
            {
                UserServantCollectionEntity item = base.lookup[key] as UserServantCollectionEntity;
                if (item.status == 2)
                {
                    getSum++;
                    findSum++;
                }
                else if (item.status == 1)
                {
                    findSum++;
                }
                list.Add(item);
            }
            else
            {
                list.Add(new UserServantCollectionEntity(userId, svtId));
            }
        }
        return list.ToArray();
    }

    public UserServantCollectionEntity getEntityFromId(long userId, int svtId)
    {
        object[] objArray1 = new object[] { string.Empty, userId, ":", svtId };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as UserServantCollectionEntity);
        }
        return new UserServantCollectionEntity(userId, svtId);
    }

    public UserServantCollectionEntity[] getList(CollectionStatus.Kind kind)
    {
        List<UserServantCollectionEntity> list = new List<UserServantCollectionEntity>();
        long userId = NetworkManager.UserId;
        int[] collectionList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).GetCollectionList();
        int length = collectionList.Length;
        for (int i = 0; i < length; i++)
        {
            object[] objArray1 = new object[] { string.Empty, userId, ":", collectionList[i] };
            string key = string.Concat(objArray1);
            if (base.lookup.ContainsKey(key))
            {
                UserServantCollectionEntity item = base.lookup[key] as UserServantCollectionEntity;
                if (item.status == kind)
                {
                    list.Add(item);
                }
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserServantCollectionEntity>(obj);

    public int[] GetNewList()
    {
        List<int> list = new List<int>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserServantCollectionEntity entity = base.list[i] as UserServantCollectionEntity;
            if ((entity != null) && entity.IsNew())
            {
                list.Add(entity.svtId);
            }
        }
        return list.ToArray();
    }

    public bool IsNew(int svtId)
    {
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserServantCollectionEntity entity = base.list[i] as UserServantCollectionEntity;
            if ((entity != null) && (entity.svtId == svtId))
            {
                return entity.IsNew();
            }
        }
        return false;
    }
}

