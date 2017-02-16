using System;
using System.Collections.Generic;

public class NpcFollowerMaster : DataMasterBase
{
    public NpcFollowerMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.NPC_FOLLOWER);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new NpcFollowerEntity[1]);
        }
    }

    public FollowerInfo GetFollower(int questId, int questPhase, long followerId)
    {
        long[] args = new long[] { followerId, (long) questId, (long) questPhase };
        NpcFollowerEntity entity = base.getEntityFromId<NpcFollowerEntity>(args);
        if (entity != null)
        {
            NpcServantFollowerEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.NPC_SERVANT_FOLLOWER).getEntityFromId<NpcServantFollowerEntity>(entity.leaderSvtId);
            if (entity2 != null)
            {
                return entity2.getFollowerInfo(entity.id);
            }
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<NpcFollowerEntity>(obj);

    public NpcFollowerEntity[] GetQuestEntitiyList(int questId, int questPhase)
    {
        List<NpcFollowerEntity> list = new List<NpcFollowerEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            NpcFollowerEntity item = base.list[i] as NpcFollowerEntity;
            if (((item != null) && (item.questId == questId)) && (item.questPhase == questPhase))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public FollowerInfo[] GetQuestFollowerList(int questId, int questPhase)
    {
        NpcServantFollowerMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<NpcServantFollowerMaster>(DataNameKind.Kind.NPC_SERVANT_FOLLOWER);
        List<FollowerInfo> list = new List<FollowerInfo>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            NpcFollowerEntity entity = base.list[i] as NpcFollowerEntity;
            if (((entity != null) && (entity.questId == questId)) && (entity.questPhase == questPhase))
            {
                NpcServantFollowerEntity entity2 = master.getEntityFromId<NpcServantFollowerEntity>(entity.leaderSvtId);
                if (entity2 != null)
                {
                    list.Add(entity2.getFollowerInfo(entity.id));
                }
            }
        }
        return list.ToArray();
    }
}

