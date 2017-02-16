using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class UserPresentBoxMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<UserPresentBoxEntity> <>f__am$cache0;

    public UserPresentBoxMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_PRESENT_BOX);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserPresentBoxEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserPresentBoxEntity>(obj);

    public UserPresentBoxEntity[] getVaildList(long userId)
    {
        int count = base.list.Count;
        List<UserPresentBoxEntity> list = new List<UserPresentBoxEntity>();
        for (int i = 0; i < count; i++)
        {
            UserPresentBoxEntity item = base.list[i] as UserPresentBoxEntity;
            if ((item != null) && (item.receiveUserId == userId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public UserPresentBoxEntity[] getVaildList(long userId, long[] presentIdList)
    {
        int count = base.list.Count;
        int length = presentIdList.Length;
        List<UserPresentBoxEntity> list = new List<UserPresentBoxEntity>();
        for (int i = 0; i < count; i++)
        {
            UserPresentBoxEntity item = base.list[i] as UserPresentBoxEntity;
            if (((item != null) && (item.receiveUserId == userId)) && !item.IsExpired())
            {
                long presentId = item.presentId;
                for (int j = 0; j < length; j++)
                {
                    if (presentIdList[j] == presentId)
                    {
                        list.Add(item);
                        break;
                    }
                }
            }
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => (int) (b.createdAt - a.createdAt);
        }
        list.Sort(<>f__am$cache0);
        return list.ToArray();
    }
}

