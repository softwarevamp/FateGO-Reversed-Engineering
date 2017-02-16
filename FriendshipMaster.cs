using System;
using System.Runtime.InteropServices;

public class FriendshipMaster : DataMasterBase
{
    public FriendshipMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.FRIENDSHIP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new FriendshipEntity[1]);
        }
    }

    public int getFriendShipRank(int friendshipId, int friendship, int start_friendshipRank)
    {
        int num = this.getRankMax(friendshipId);
        int rank = start_friendshipRank;
        for (int i = start_friendshipRank; i <= num; i++)
        {
            FriendshipEntity entity = base.getEntityFromId<FriendshipEntity>(friendshipId, i);
            if (entity == null)
            {
                return rank;
            }
            if (friendship <= entity.friendship)
            {
                return entity.rank;
            }
            rank = entity.rank;
        }
        return rank;
    }

    public bool GetFriendshipRank(int friendshipId, int friendship, out int rank, out int max, out int late, out float fraction)
    {
        int num = 0;
        int num2 = 0;
        rank = -1;
        max = 0;
        late = 0;
        while (true)
        {
            object[] objArray1 = new object[] { string.Empty, friendshipId, ":", (int) max };
            string key = string.Concat(objArray1);
            if (!base.lookup.ContainsKey(key))
            {
                break;
            }
            FriendshipEntity entity = base.lookup[key] as FriendshipEntity;
            if (friendship >= num2)
            {
                rank = max;
                if (entity.friendship > 0)
                {
                    num = num2;
                    num2 = entity.friendship;
                }
            }
            if (entity.friendship <= 0)
            {
                goto Label_00C1;
            }
            max++;
        }
        if (max > 0)
        {
            max--;
        }
    Label_00C1:
        late = num2 - friendship;
        int num3 = num2 - num;
        if (num3 > 0)
        {
            fraction = ((float) (friendship - num)) / ((float) num3);
        }
        else
        {
            fraction = 0f;
        }
        return (rank > 0);
    }

    public int GetFriendshipRankMax(int id)
    {
        object[] objArray1;
        int num = 0;
    Label_0002:
        objArray1 = new object[] { string.Empty, id, ":", num };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            num++;
            goto Label_0002;
        }
        return ((num <= 0) ? 0 : (num - 1));
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<FriendshipEntity>(obj);

    public int getRankMax(int id)
    {
        int num = 0;
        while (true)
        {
            object[] objArray1 = new object[] { string.Empty, id, ":", num };
            string key = string.Concat(objArray1);
            if (!base.lookup.ContainsKey(key))
            {
                return (num - 1);
            }
            num++;
        }
    }
}

