using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class VoiceMaster : DataMasterBase
{
    public VoiceMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.VOICE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new VoiceEntity[1]);
        }
    }

    public VoiceEntity[] getEnableEntity(out bool[] isCanPlayList, out string[] overwriteNameList, int svtId, int limitCount)
    {
        ServantVoiceMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE);
        ServantLimitAddMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num = master2.getVoiceId(svtId, limitCount);
        int num2 = master2.getVoicePrefix(svtId, limitCount);
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, svtId);
        ServantVoiceEntity[] entityArray = master.getEntity(svtId, limitCount);
        long num3 = NetworkManager.getTime();
        List<VoiceEntity> list = new List<VoiceEntity>();
        List<bool> list2 = new List<bool>();
        List<string> list3 = new List<string>();
        foreach (DataEntityBase base2 in base.list)
        {
            bool flag2;
            ServantVoiceCond cond;
            ServantVoiceCond[] condArray2;
            int num10;
            VoiceEntity item = base2 as VoiceEntity;
            ServantVoiceEntity entity3 = entityArray[item.svtVoiceType];
            if (entity3 == null)
            {
                continue;
            }
            object[] objArray1 = new object[] { string.Empty, num2, "_", item.id };
            string labelName = string.Concat(objArray1);
            List<ServantVoiceData[]> list4 = entity3.getVoiceList((SvtVoiceType.Type) item.svtVoiceType, labelName);
            if (((list4 == null) || (list4.Count <= 0)) || (list4[0] == null))
            {
                continue;
            }
            string str2 = entity3.getOverwriteName((SvtVoiceType.Type) item.svtVoiceType, labelName);
            ServantVoiceCond[] condArray = entity3.getConds((SvtVoiceType.Type) item.svtVoiceType, labelName);
            bool flag = true;
            CondType.Kind condType = (CondType.Kind) item.condType;
            switch (condType)
            {
                case CondType.Kind.NONE:
                    goto Label_03E6;

                case CondType.Kind.SVT_LEVEL:
                    flag = entity.maxLv >= item.condValue;
                    goto Label_03E6;

                case CondType.Kind.SVT_LIMIT:
                    flag = entity.maxLimitCount >= item.condValue;
                    goto Label_03E6;

                case CondType.Kind.SVT_GET:
                    flag = entity.IsGet();
                    goto Label_03E6;

                case CondType.Kind.SVT_FRIENDSHIP:
                    flag = entity.friendshipRank >= item.condValue;
                    goto Label_03E6;

                default:
                {
                    long birthDay;
                    switch (condType)
                    {
                        case CondType.Kind.FLAG:
                            flag = entity.IsPlayed(item.condValue);
                            goto Label_03E6;

                        case CondType.Kind.SVT_COUNT_STOP:
                            flag = entity.IsLimitCountMax();
                            goto Label_03E6;

                        case CondType.Kind.BIRTH_DAY:
                            birthDay = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>().birthDay;
                            if (birthDay <= 0L)
                            {
                                goto Label_02A2;
                            }
                            if ((BalanceConfig.VtReleaseAt <= 0L) || (entity.createdAt >= BalanceConfig.VtReleaseAt))
                            {
                                break;
                            }
                            goto Label_03E6;

                        case CondType.Kind.EVENT_END:
                            flag2 = false;
                            condArray2 = condArray;
                            num10 = 0;
                            goto Label_03CA;

                        case CondType.Kind.SVT_EVENT_JOIN:
                            flag = entity.IsFinded();
                            goto Label_03E6;

                        default:
                            goto Label_03E6;
                    }
                    DateTime time = NetworkManager.getLocalDateTime(entity.createdAt);
                    DateTime time2 = NetworkManager.getDateTime(birthDay);
                    DateTime dateTime = new DateTime(time.Year, time2.Month, time2.Day);
                    DateTime time4 = new DateTime(time.Year + 1, time2.Month, time2.Day);
                    long num5 = NetworkManager.getLocalTime();
                    long createdAt = entity.createdAt;
                    long num7 = NetworkManager.getTime(dateTime);
                    long num8 = num7 + 0x15180L;
                    long num9 = NetworkManager.getTime(time4);
                    if (((createdAt <= num8) && (num7 <= num5)) || (num9 <= num5))
                    {
                        goto Label_03E6;
                    }
                    break;
                }
            }
        Label_02A2:
            flag = false;
            goto Label_03E6;
        Label_0331:
            cond = condArray2[num10];
            VoiceCondType.Type type = (VoiceCondType.Type) cond.condType;
            if (((type == VoiceCondType.Type.EVENT_END) || (type == VoiceCondType.Type.EVENT_NOEND)) && (cond.value > 0))
            {
                EventEntity entity5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId(cond.value);
                if (entity5 != null)
                {
                    if (num3 < entity5.startedAt)
                    {
                        flag2 = true;
                    }
                    else if (type == VoiceCondType.Type.EVENT_END)
                    {
                        flag = num3 >= entity5.endedAt;
                    }
                    goto Label_03D5;
                }
            }
            num10++;
        Label_03CA:
            if (num10 < condArray2.Length)
            {
                goto Label_0331;
            }
        Label_03D5:
            if (flag2)
            {
                continue;
            }
        Label_03E6:
            list.Add(item);
            list2.Add(flag);
            list3.Add(str2);
        }
        isCanPlayList = list2.ToArray();
        overwriteNameList = list3.ToArray();
        return list.ToArray();
    }

    public VoiceEntity getEntityFromId(string id)
    {
        string key = string.Empty + id;
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as VoiceEntity);
        }
        return null;
    }

    public int getFlagRequestNumber(int svtId, string labelName, bool isUpdate = true)
    {
        if (!string.IsNullOrEmpty(labelName))
        {
            foreach (DataEntityBase base2 in base.list)
            {
                VoiceEntity entity = base2 as VoiceEntity;
                string str = "_" + entity.id;
                if (labelName.EndsWith(str))
                {
                    if (entity.condType != 0x11)
                    {
                        break;
                    }
                    UserServantCollectionEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, svtId);
                    if (!(!isUpdate ? !entity2.IsPlayed(entity.condValue) : entity2.SetPlayed(entity.condValue)))
                    {
                        break;
                    }
                    return entity.condValue;
                }
            }
        }
        return 0;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<VoiceEntity>(obj);
}

