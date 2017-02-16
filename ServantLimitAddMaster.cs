using System;
using System.Collections.Generic;

public class ServantLimitAddMaster : DataMasterBase
{
    protected ServantMaster servantMaster;

    public ServantLimitAddMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantLimitAddEntity[1]);
        }
    }

    public string getBattleChrId(int svtId, int limitCount)
    {
        int battleCharaId = svtId;
        long[] args = new long[] { (long) svtId, (long) limitCount };
        if (base.isEntityExistsFromId(args))
        {
            battleCharaId = base.getEntityFromId<ServantLimitAddEntity>(svtId, limitCount).battleCharaId;
        }
        return (string.Empty + battleCharaId);
    }

    public int getBattleChrLimitCount(int svtId, int limitCount)
    {
        int battleCharaLimitCount = limitCount;
        long[] args = new long[] { (long) svtId, (long) limitCount };
        if (base.isEntityExistsFromId(args))
        {
            battleCharaLimitCount = base.getEntityFromId<ServantLimitAddEntity>(svtId, limitCount).battleCharaLimitCount;
        }
        return battleCharaLimitCount;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantLimitAddEntity>(obj);

    public int getVoiceId(int svtId, int limitCount)
    {
        int svtVoiceId = 0;
        ServantEntity entity = this.servantMaster.getEntityFromId<ServantEntity>(svtId);
        if ((entity != null) && ((entity.type == 1) || (entity.type == 2)))
        {
            svtVoiceId = svtId;
        }
        long[] args = new long[] { (long) svtId, (long) limitCount };
        if (base.isEntityExistsFromId(args))
        {
            svtVoiceId = base.getEntityFromId<ServantLimitAddEntity>(svtId, limitCount).svtVoiceId;
        }
        return svtVoiceId;
    }

    public int[] getVoiceLimitCountList(int svtId) => 
        this.getVoiceLimitCountList(svtId, BalanceConfig.ServantLimitMax);

    public int[] getVoiceLimitCountList(int svtId, int maxLimitCount)
    {
        List<ServantLimitAddEntity> list = new List<ServantLimitAddEntity>();
        for (int i = 0; i <= maxLimitCount; i++)
        {
            long[] args = new long[] { (long) svtId, (long) i };
            if (base.isEntityExistsFromId(args))
            {
                ServantLimitAddEntity item = base.getEntityFromId<ServantLimitAddEntity>(svtId, i);
                int svtVoiceId = item.svtVoiceId;
                int voicePrefix = item.voicePrefix;
                foreach (ServantLimitAddEntity entity2 in list)
                {
                    if (entity2 != null)
                    {
                        if ((svtVoiceId != entity2.svtVoiceId) || (voicePrefix != entity2.voicePrefix))
                        {
                            continue;
                        }
                        item = null;
                        break;
                    }
                    if ((svtVoiceId == svtId) && (voicePrefix == 0))
                    {
                        item = null;
                        break;
                    }
                }
                if (item != null)
                {
                    list.Add(item);
                }
                continue;
            }
            if (i == 0)
            {
                list.Add(null);
            }
        }
        int[] numArray = new int[list.Count];
        for (int j = 0; j < numArray.Length; j++)
        {
            if (list[j] != null)
            {
                numArray[j] = list[j].limitCount;
            }
            else
            {
                numArray[j] = 0;
            }
        }
        return numArray;
    }

    public int getVoicePrefix(int svtId, int limitCount)
    {
        int voicePrefix = 0;
        long[] args = new long[] { (long) svtId, (long) limitCount };
        if (base.isEntityExistsFromId(args))
        {
            voicePrefix = base.getEntityFromId<ServantLimitAddEntity>(svtId, limitCount).voicePrefix;
        }
        return voicePrefix;
    }

    public override bool preProcess()
    {
        this.servantMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        return true;
    }
}

