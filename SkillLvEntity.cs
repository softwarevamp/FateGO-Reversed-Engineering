using System;
using System.Runtime.CompilerServices;

public class SkillLvEntity : DataEntityBase
{
    [CompilerGenerated]
    private static Converter<string, int> <>f__am$cache7;
    public int chargeTurn;
    public int[] funcId;
    public int lv;
    public int priority;
    public int skillDetailId;
    public int skillId;
    public string[] vals;

    public DataVals[] getDataValsList()
    {
        if (this.vals != null)
        {
            DataVals[] valsArray = new DataVals[this.vals.Length];
            for (int i = 0; i < this.vals.Length; i++)
            {
                valsArray[i] = new DataVals(this.vals[i]);
            }
            return valsArray;
        }
        return new DataVals[] { new DataVals(string.Empty) };
    }

    public string getDetail()
    {
        SkillDetailEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL_DETAIL).getEntityFromId<SkillDetailEntity>(this.skillDetailId);
        return ((entity == null) ? LocalizationManager.GetUnknownName() : entity.detail);
    }

    public string getDetail(int lv)
    {
        string str = (lv <= 0) ? string.Empty : string.Format(LocalizationManager.Get("LEVEL_DETAIL_INFO"), lv);
        return string.Format(this.getDetail(), str);
    }

    public FunctionEntity GetEventPointFunc(int eventId)
    {
        if (eventId > 0)
        {
            if (this.funcId == null)
            {
                return null;
            }
            FunctionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionMaster>(DataNameKind.Kind.FUNCTION);
            int[][] numArray = null;
            for (int i = 0; i < this.funcId.Length; i++)
            {
                FunctionEntity entity = master.getEntityFromId<FunctionEntity>(this.funcId[i]);
                if ((entity != null) && (entity.funcType == 0x6b))
                {
                    if (numArray == null)
                    {
                        numArray = this.getValues();
                        if (numArray == null)
                        {
                            continue;
                        }
                    }
                    if (numArray.Length > i)
                    {
                        int[] numArray2 = numArray[i];
                        if (((numArray2 != null) && (numArray2.Length >= 4)) && (eventId == numArray2[3]))
                        {
                            return entity;
                        }
                    }
                }
            }
        }
        return null;
    }

    public bool getEventUpVal(ref EventUpValInfo eventUpVallInfo)
    {
        if (eventUpVallInfo.setupInfo == null)
        {
            return false;
        }
        if (this.funcId == null)
        {
            return false;
        }
        FunctionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionMaster>(DataNameKind.Kind.FUNCTION);
        bool flag = false;
        int[][] numArray = null;
        int eventId = eventUpVallInfo.setupInfo.eventId;
        for (int i = 0; i < this.funcId.Length; i++)
        {
            FunctionEntity funcEntity = master.getEntityFromId<FunctionEntity>(this.funcId[i]);
            if (funcEntity != null)
            {
                if ((((funcEntity.funcType == 0x6b) || (funcEntity.funcType == 0x6c)) || ((funcEntity.funcType == 0x69) || (funcEntity.funcType == 0x6a))) || ((funcEntity.funcType == 0x72) || (funcEntity.funcType == 0x73)))
                {
                    if (numArray == null)
                    {
                        numArray = this.getValues();
                        if (numArray == null)
                        {
                            continue;
                        }
                    }
                    if (numArray.Length > i)
                    {
                        int[] numArray2 = numArray[i];
                        if (((numArray2 != null) && (numArray2.Length >= 4)) && ((eventId == numArray2[3]) && ((numArray2[1] == 0) || (numArray2[2] != 0))))
                        {
                            flag = true;
                            EventDropUpValInfo item = null;
                            if ((funcEntity.funcType == 0x6b) || (funcEntity.funcType == 0x6c))
                            {
                                item = new EventDropUpValInfo(funcEntity);
                            }
                            else
                            {
                                item = new EventDropUpValInfo(funcEntity, numArray2[0]);
                            }
                            if (numArray2[1] == 1)
                            {
                                item.SetAddCount(numArray2[2], eventUpVallInfo.equipSvtId > 0);
                            }
                            else if (numArray2[1] == 2)
                            {
                                item.SetRateCount(numArray2[2], eventUpVallInfo.equipSvtId > 0);
                            }
                            eventUpVallInfo.dropList.Add(item);
                        }
                    }
                }
                else if (funcEntity.funcType == 0x71)
                {
                    if (numArray == null)
                    {
                        numArray = this.getValues();
                        if (numArray == null)
                        {
                            continue;
                        }
                    }
                    if (numArray.Length > i)
                    {
                        int[] numArray3 = numArray[i];
                        if (((numArray3 != null) && (numArray3.Length >= 3)) && ((eventId == numArray3[2]) && ((numArray3[0] == 0) || (numArray3[1] != 0))))
                        {
                            flag = true;
                            foreach (int num3 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(eventUpVallInfo.svtId).individuality)
                            {
                                EventDropUpValInfo info2 = new EventDropUpValInfo(funcEntity, num3);
                                if (numArray3[0] == 1)
                                {
                                    info2.SetAddCount(numArray3[1], eventUpVallInfo.equipSvtId > 0);
                                }
                                else if (numArray3[0] == 2)
                                {
                                    info2.SetRateCount(numArray3[1], eventUpVallInfo.equipSvtId > 0);
                                }
                                eventUpVallInfo.dropList.Add(info2);
                            }
                        }
                    }
                }
                else if ((funcEntity.funcType == 1) || (funcEntity.funcType == 0x10))
                {
                    if (numArray == null)
                    {
                        numArray = this.getValues();
                        if (numArray == null)
                        {
                            continue;
                        }
                    }
                    if (numArray.Length > i)
                    {
                        int[] numArray5 = numArray[i];
                        if (((numArray5 != null) && (numArray5.Length >= 4)) && (eventUpVallInfo.setupInfo.IsUpVal(funcEntity.questTvals) && (numArray5[3] != 0)))
                        {
                            EventDropUpValInfo info3 = new EventDropUpValInfo(funcEntity);
                            info3.SetRateCount(numArray5[3], eventUpVallInfo.equipSvtId > 0);
                            eventUpVallInfo.dropList.Add(info3);
                            return true;
                        }
                    }
                }
            }
        }
        return flag;
    }

    public bool getEventUpVal(int wearersSvtId, EventUpValSetupInfo setupInfo)
    {
        if (setupInfo != null)
        {
            if (this.funcId == null)
            {
                return false;
            }
            ItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM);
            FunctionMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionMaster>(DataNameKind.Kind.FUNCTION);
            int[][] numArray = null;
            for (int i = 0; i < this.funcId.Length; i++)
            {
                FunctionEntity entity = master2.getEntityFromId<FunctionEntity>(this.funcId[i]);
                if (entity != null)
                {
                    if ((((entity.funcType == 0x6b) || (entity.funcType == 0x6c)) || ((entity.funcType == 0x69) || (entity.funcType == 0x6a))) || ((entity.funcType == 0x72) || (entity.funcType == 0x73)))
                    {
                        if (numArray == null)
                        {
                            numArray = this.getValues();
                            if (numArray == null)
                            {
                                continue;
                            }
                        }
                        if (numArray.Length > i)
                        {
                            int[] numArray2 = numArray[i];
                            if (((numArray2 != null) && (numArray2.Length >= 4)) && ((setupInfo.eventId == numArray2[3]) && ((numArray2[1] == 0) || (numArray2[2] != 0))))
                            {
                                return true;
                            }
                        }
                    }
                    else if (entity.funcType == 0x71)
                    {
                        if (wearersSvtId > 0)
                        {
                            if (numArray == null)
                            {
                                numArray = this.getValues();
                                if (numArray == null)
                                {
                                    continue;
                                }
                            }
                            if (numArray.Length > i)
                            {
                                int[] numArray3 = numArray[i];
                                if (((numArray3 != null) && (numArray3.Length >= 3)) && ((setupInfo.eventId == numArray3[2]) && (numArray3[1] != 0)))
                                {
                                    ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(wearersSvtId);
                                    if (master.GetIndividualityList(entity2.individuality).Length > 0)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else if ((entity.funcType == 1) || (entity.funcType == 0x10))
                    {
                        if (numArray == null)
                        {
                            numArray = this.getValues();
                            if (numArray == null)
                            {
                                continue;
                            }
                        }
                        if (numArray.Length > i)
                        {
                            int[] numArray4 = numArray[i];
                            if (((numArray4 != null) && (numArray4.Length >= 4)) && (setupInfo.IsUpVal(entity.questTvals) && (numArray4[3] != 0)))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public int getFriendPointUpVal()
    {
        if (this.funcId != null)
        {
            FunctionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionMaster>(DataNameKind.Kind.FUNCTION);
            for (int i = 0; i < this.funcId.Length; i++)
            {
                FunctionEntity entity = master.getEntityFromId<FunctionEntity>(this.funcId[i]);
                if ((entity != null) && (entity.funcType == 0x68))
                {
                    int[][] numArray = this.getValues();
                    if (numArray == null)
                    {
                        return 0;
                    }
                    if (numArray.Length <= i)
                    {
                        return 0;
                    }
                    int[] numArray2 = numArray[i];
                    if (numArray2 == null)
                    {
                        return 0;
                    }
                    if (numArray2.Length <= 0)
                    {
                        return 0;
                    }
                    return numArray2[0];
                }
            }
        }
        return 0;
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.skillId, ":", this.lv };
        return string.Concat(objArray1);
    }

    public int[][] getValues()
    {
        if (this.vals != null)
        {
            int[][] numArray = new int[this.vals.Length][];
            for (int i = 0; i < this.vals.Length; i++)
            {
                char[] separator = new char[] { ',' };
                string[] array = this.vals[i].Replace("[", string.Empty).Replace("]", string.Empty).Split(separator);
                if (<>f__am$cache7 == null)
                {
                    <>f__am$cache7 = delegate (string input) {
                        int num;
                        if (int.TryParse(input, out num))
                        {
                            return num;
                        }
                        return 0;
                    };
                }
                numArray[i] = Array.ConvertAll<string, int>(array, <>f__am$cache7);
            }
            return numArray;
        }
        return new int[][] { new int[5] };
    }
}

