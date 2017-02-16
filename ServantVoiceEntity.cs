using System;
using System.Collections.Generic;

public class ServantVoiceEntity : DataEntityBase
{
    public int id;
    public SvtVoiceInfo[] scriptJson;
    public int type;
    public int voicePrefix;

    public List<ServantVoiceData[]> getBattleVoiceList(string labelName) => 
        this.getVoiceList(SvtVoiceType.Type.BATTLE, labelName);

    public List<ServantVoiceData[]> getCntStopVoiceList() => 
        this.getVoiceList(SvtVoiceType.Type.GROETH, VoiceCondType.Type.COUNT_STOP);

    public ServantVoiceCond[] getConds(SvtVoiceType.Type type, string labelName)
    {
        if (this.type == type)
        {
            for (int i = 0; i < this.scriptJson.Length; i++)
            {
                SvtVoiceInfo info = this.scriptJson[i];
                if (info.infos != null)
                {
                    for (int j = 0; j < info.infos.Length; j++)
                    {
                        ServantVoiceData data = info.infos[j];
                        if (labelName.Equals(data.id))
                        {
                            return info.conds;
                        }
                    }
                }
            }
        }
        return null;
    }

    public List<ServantVoiceData[]> getEventJoinVoiceList() => 
        this.getVoiceList(SvtVoiceType.Type.EVENT_JOIN);

    public List<ServantVoiceData[]> getEventRewardVoiceList()
    {
        if (this.type != this.type)
        {
            return null;
        }
        List<ServantVoiceData[]> list = new List<ServantVoiceData[]>();
        for (int i = 0; i < this.scriptJson.Length; i++)
        {
            SvtVoiceInfo info = this.scriptJson[i];
            if ((info.conds != null) && (info.conds.Length > 0))
            {
                foreach (ServantVoiceCond cond in info.conds)
                {
                    EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(cond.value);
                    if (entity != null)
                    {
                        if (entity.endedAt <= NetworkManager.getTime())
                        {
                            if (cond.condType == 13)
                            {
                                list.Add(info.infos);
                            }
                        }
                        else if (cond.condType == 14)
                        {
                            list.Add(info.infos);
                        }
                    }
                }
            }
            else
            {
                list.Add(info.infos);
            }
        }
        return list;
    }

    public List<ServantVoiceData[]> getEventRewardVoiceList(string labelName) => 
        this.getVoiceList(SvtVoiceType.Type.EVENT_REWARD, labelName);

    public List<ServantVoiceData[]> getFirstGetVoiceList() => 
        this.getVoiceList(SvtVoiceType.Type.FIRST_GET);

    public List<ServantVoiceData[]> getHomeVoiceList()
    {
        if (this.type != 1)
        {
            return null;
        }
        List<ServantVoiceData[]> list = new List<ServantVoiceData[]>();
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
        long favoriteUserSvtId = entity.favoriteUserSvtId;
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        UserServantEntity entity2 = master.getEntityFromId<UserServantEntity>(favoriteUserSvtId);
        UserServantEntity[] entityArray = master.getList(entity.userId);
        int num2 = 0;
        UserQuestEntity entity3 = null;
        for (int i = 0; i < this.scriptJson.Length; i++)
        {
            bool flag = false;
            bool flag2 = false;
            SvtVoiceInfo info = this.scriptJson[i];
            int length = info.conds.Length;
            if ((info.conds != null) && (info.conds.Length > 0))
            {
                bool flag3 = false;
                foreach (ServantVoiceCond cond in info.conds)
                {
                    int condType = cond.condType;
                    if (condType == 1)
                    {
                        long birthDay = entity.birthDay;
                        if (birthDay > 0L)
                        {
                            DateTime time = NetworkManager.getLocalDateTime();
                            DateTime time2 = NetworkManager.getDateTime(birthDay);
                            int month = time.Month;
                            int day = time.Day;
                            int num10 = time2.Month;
                            int num11 = time2.Day;
                            flag3 = (month == num10) && (day == num11);
                            Debug.LogError(string.Concat(new object[] { "birthday:", flag3, ":", month, "/", num10, " - ", day, "/", num11 }));
                        }
                        else
                        {
                            flag3 = false;
                        }
                    }
                    if (condType == 2)
                    {
                        EventEntity[] enableEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).GetEnableEntitiyList(GameEventType.TYPE.EVENT_QUEST, false);
                        flag3 = (enableEntitiyList != null) && (enableEntitiyList.Length > 0);
                    }
                    switch (condType)
                    {
                        case 3:
                        {
                            int num12 = entity2.getFriendshipRank();
                            int num13 = cond.value;
                            flag3 = num12 == num13;
                            break;
                        }
                        case 4:
                        {
                            flag3 = false;
                            int num14 = cond.value;
                            for (int j = 0; j < entityArray.Length; j++)
                            {
                                if (num14.Equals(entityArray[j].svtId))
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (condType == 5)
                    {
                        int groupId = cond.value;
                        ServantGroupEntity[] entityArray3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantGroupMaster>(DataNameKind.Kind.SERVANT_GROUP).getEntityListById(groupId);
                        flag3 = false;
                        for (int k = 0; k < entityArray.Length; k++)
                        {
                            int svtId = entityArray[k].svtId;
                            for (int m = 0; m < entityArray3.Length; m++)
                            {
                                if (svtId == entityArray3[m].svtId)
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                            if (flag3)
                            {
                                break;
                            }
                        }
                    }
                    if (condType == 6)
                    {
                        num2 = cond.value;
                        long[] args = new long[] { entity.userId, (long) num2 };
                        entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_QUEST).getEntityFromId<UserQuestEntity>(args);
                        flag2 = true;
                        if ((entity3 != null) && (entity3.getClearNum() > 0))
                        {
                            flag3 = length < 2;
                            flag = true;
                        }
                    }
                    if (condType == 7)
                    {
                        int num20 = cond.value;
                        long[] numArray2 = new long[] { entity.userId, (long) num20 };
                        entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_QUEST).getEntityFromId<UserQuestEntity>(numArray2);
                        if (flag2)
                        {
                            flag3 = flag && ((entity3 == null) || (entity3.getClearNum() <= 0));
                        }
                        else
                        {
                            flag3 = (entity3 == null) || (entity3.getClearNum() <= 0);
                        }
                    }
                    if (condType == 12)
                    {
                        int num21 = cond.value;
                        flag3 = !SingletonTemplate<QuestTree>.Instance.IsWarNew(num21);
                    }
                    if (condType == 9)
                    {
                        int num22 = entity2.getLimitCount();
                        int num23 = cond.value;
                        flag3 = num22 == num23;
                    }
                    if (!flag3)
                    {
                        break;
                    }
                }
                if (flag3)
                {
                    Debug.LogError("add :" + info.infos);
                    list.Add(info.infos);
                }
                continue;
            }
            list.Add(info.infos);
        }
        return list;
    }

    public List<ServantVoiceData[]> getHomeVoiceList(string labelName) => 
        this.getVoiceList(SvtVoiceType.Type.HOME, labelName);

    public List<ServantVoiceData[]> getLevelUpVoiceList() => 
        this.getVoiceList(SvtVoiceType.Type.GROETH, VoiceCondType.Type.LEVEL_UP);

    public List<ServantVoiceData[]> getLimitCntUpVoiceList() => 
        this.getVoiceList(SvtVoiceType.Type.GROETH, VoiceCondType.Type.LIMIT_COUNT_COMMON);

    public List<ServantVoiceData[]> getNpVoiceList(string labelName) => 
        this.getVoiceList(SvtVoiceType.Type.TREASURE_DEVICE, labelName);

    public string getOverwriteName(SvtVoiceType.Type type, string labelName)
    {
        if (this.type == type)
        {
            for (int i = 0; i < this.scriptJson.Length; i++)
            {
                SvtVoiceInfo info = this.scriptJson[i];
                if (info.infos != null)
                {
                    for (int j = 0; j < info.infos.Length; j++)
                    {
                        ServantVoiceData data = info.infos[j];
                        if (labelName.Equals(data.id))
                        {
                            return info.overwriteName;
                        }
                    }
                }
            }
        }
        return null;
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.voicePrefix, ":", this.type };
        return string.Concat(objArray1);
    }

    public List<ServantVoiceData[]> getRankUpFriendShip(int friendShipRank) => 
        this.getVoiceList(SvtVoiceType.Type.HOME, VoiceCondType.Type.FRIENDSHIP, friendShipRank);

    public List<ServantVoiceData[]> getSpecificLimitCntUpVoiceList(int limitCount) => 
        this.getVoiceList(SvtVoiceType.Type.GROETH, VoiceCondType.Type.LIMIT_COUNT, limitCount);

    public string getVoiceAssetName() => 
        ("ChrVoice_" + this.id);

    public List<ServantVoiceData[]> getVoiceList(SvtVoiceType.Type type)
    {
        if (this.type != type)
        {
            return null;
        }
        List<ServantVoiceData[]> list = new List<ServantVoiceData[]>();
        for (int i = 0; i < this.scriptJson.Length; i++)
        {
            list.Add(this.scriptJson[i].infos);
        }
        return list;
    }

    public List<ServantVoiceData[]> getVoiceList(SvtVoiceType.Type type, string labelName)
    {
        if (this.type != type)
        {
            return null;
        }
        List<ServantVoiceData[]> list = new List<ServantVoiceData[]>();
        if ((labelName.EndsWith("_B050") || labelName.EndsWith("_B060")) || labelName.EndsWith("_B070"))
        {
            int index = 0;
            if (labelName.EndsWith("_B060"))
            {
                index = 1;
            }
            else if (labelName.EndsWith("_B070"))
            {
                index = 2;
            }
            if (index < this.scriptJson.Length)
            {
                SvtVoiceInfo info = this.scriptJson[index];
                if ((info.infos != null) && (info.infos.Length > 0))
                {
                    list.Add(info.infos);
                }
            }
            return list;
        }
        for (int i = 0; i < this.scriptJson.Length; i++)
        {
            SvtVoiceInfo info2 = this.scriptJson[i];
            if (info2.infos != null)
            {
                for (int j = 0; j < info2.infos.Length; j++)
                {
                    ServantVoiceData data = info2.infos[j];
                    if (labelName.Equals(data.id))
                    {
                        list.Add(info2.infos);
                        return list;
                    }
                }
            }
        }
        return list;
    }

    public List<ServantVoiceData[]> getVoiceList(SvtVoiceType.Type type, VoiceCondType.Type condType)
    {
        if (this.type != type)
        {
            return null;
        }
        List<ServantVoiceData[]> list = new List<ServantVoiceData[]>();
        for (int i = 0; i < this.scriptJson.Length; i++)
        {
            SvtVoiceInfo info = this.scriptJson[i];
            if (info.conds[0].condType == condType)
            {
                list.Add(info.infos);
            }
        }
        return list;
    }

    public List<ServantVoiceData[]> getVoiceList(SvtVoiceType.Type type, VoiceCondType.Type condType, int condValue)
    {
        if (this.type != type)
        {
            return null;
        }
        List<ServantVoiceData[]> list = new List<ServantVoiceData[]>();
        for (int i = 0; i < this.scriptJson.Length; i++)
        {
            SvtVoiceInfo info = this.scriptJson[i];
            if (info.conds != null)
            {
                for (int j = 0; j < info.conds.Length; j++)
                {
                    if (info.conds[j].condType == condType)
                    {
                        if ((condType == VoiceCondType.Type.LIMIT_COUNT) || (condType == VoiceCondType.Type.FRIENDSHIP))
                        {
                            if (info.conds[j].value == condValue)
                            {
                                list.Add(info.infos);
                            }
                        }
                        else
                        {
                            list.Add(info.infos);
                        }
                    }
                }
            }
        }
        return list;
    }
}

