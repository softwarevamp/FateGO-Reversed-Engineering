using System;
using System.Collections.Generic;

public class ServantVoiceMaster : DataMasterBase
{
    public ServantVoiceMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_VOICE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantVoiceEntity[1]);
        }
    }

    public List<ServantVoiceData[]> getBattleVoiceList(int svtId, int limitCount, string labelName)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.BATTLE, svtId, limitCount);
        if (entity != null)
        {
            return entity.getBattleVoiceList(labelName);
        }
        return null;
    }

    public List<ServantVoiceData[]> getCntStopVoiceList(int svtId, int limitCount)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.GROETH, svtId, limitCount);
        if (entity != null)
        {
            return entity.getCntStopVoiceList();
        }
        return null;
    }

    public ServantVoiceEntity[] getEntity(int svtId, int limitCount)
    {
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num = master.getVoiceId(svtId, limitCount);
        int num2 = master.getVoicePrefix(svtId, limitCount);
        ServantVoiceEntity[] entityArray = new ServantVoiceEntity[8];
        for (int i = 1; i < 8; i++)
        {
            object[] objArray1 = new object[] { string.Empty, num, ":", num2, ":", i };
            string key = string.Concat(objArray1);
            if (base.lookup.ContainsKey(key))
            {
                entityArray[i] = base.lookup[key] as ServantVoiceEntity;
            }
        }
        return entityArray;
    }

    public ServantVoiceEntity getEntity(SvtVoiceType.Type voceType, int svtId, int limitCount)
    {
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num = master.getVoiceId(svtId, limitCount);
        int num2 = master.getVoicePrefix(svtId, limitCount);
        object[] objArray1 = new object[] { string.Empty, num, ":", num2, ":", (int) voceType };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as ServantVoiceEntity);
        }
        return null;
    }

    public List<ServantVoiceData[]> getEntity(SvtVoiceType.Type voceType, int svtId, int limitCount, string labelName)
    {
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num = master.getVoiceId(svtId, limitCount);
        int num2 = master.getVoicePrefix(svtId, limitCount);
        object[] objArray1 = new object[] { string.Empty, num, ":", num2, ":", (int) voceType };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            ServantVoiceEntity entity = base.lookup[key] as ServantVoiceEntity;
            return entity.getVoiceList(voceType, labelName);
        }
        return null;
    }

    public List<ServantVoiceData[]> getEventJoinVoiceList(int svtId, int limitCount)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.EVENT_JOIN, svtId, limitCount);
        if (entity != null)
        {
            return entity.getEventJoinVoiceList();
        }
        return null;
    }

    public List<ServantVoiceData[]> getEventRewardVoiceList(int svtId, int limitCount)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.EVENT_REWARD, svtId, limitCount);
        if (entity != null)
        {
            return entity.getEventRewardVoiceList();
        }
        return null;
    }

    public List<ServantVoiceData[]> getEventRewardVoiceList(int svtId, int limitCount, string labelName)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.EVENT_REWARD, svtId, limitCount);
        if (entity != null)
        {
            return entity.getEventRewardVoiceList(labelName);
        }
        return null;
    }

    public List<ServantVoiceData[]> getFirstGetVoiceList(int svtId, int limitCount)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.FIRST_GET, svtId, limitCount);
        if (entity != null)
        {
            return entity.getFirstGetVoiceList();
        }
        return null;
    }

    public List<ServantVoiceData[]> getHomeVoiceList(int svtId, int limitCount)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.HOME, svtId, limitCount);
        if (entity != null)
        {
            return entity.getHomeVoiceList();
        }
        return null;
    }

    public List<ServantVoiceData[]> getHomeVoiceList(int svtId, int limitCount, string labelName)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.HOME, svtId, limitCount);
        if (entity != null)
        {
            return entity.getHomeVoiceList(labelName);
        }
        return null;
    }

    public List<ServantVoiceData[]> getLevelUpVoiceList(int svtId, int limitCount)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.GROETH, svtId, limitCount);
        if (entity != null)
        {
            return entity.getLevelUpVoiceList();
        }
        return null;
    }

    public List<ServantVoiceData[]> getLimitCntUpVoiceList(int svtId, int limitCount)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.GROETH, svtId, limitCount);
        if (entity != null)
        {
            return entity.getLimitCntUpVoiceList();
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantVoiceEntity>(obj);

    public List<ServantVoiceData[]> getNpVoiceList(int svtId, int limitCount, string labelName)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.TREASURE_DEVICE, svtId, limitCount);
        if (entity != null)
        {
            return entity.getNpVoiceList(labelName);
        }
        return null;
    }

    public List<ServantVoiceData[]> getRankUpFriendShip(int svtId, int limitCount, int friendShipRank)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.HOME, svtId, limitCount);
        if (entity != null)
        {
            return entity.getRankUpFriendShip(friendShipRank);
        }
        return null;
    }

    public List<ServantVoiceData[]> getSpecificLimitCntUpVoiceList(int svtId, int limitCount)
    {
        ServantVoiceEntity entity = this.getEntity(SvtVoiceType.Type.GROETH, svtId, limitCount);
        if (entity != null)
        {
            return entity.getSpecificLimitCntUpVoiceList(limitCount);
        }
        return null;
    }

    public static bool isOpenByServantFriendShip(int svtId, int limitCnt, int friendShipRank)
    {
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num = master.getVoiceId(svtId, limitCnt);
        int num2 = master.getVoicePrefix(svtId, limitCnt);
        List<ServantVoiceData[]> list = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE).getEntityFromId<ServantVoiceEntity>(num, num2, 1).getRankUpFriendShip(friendShipRank);
        return (0 < list.Count);
    }
}

