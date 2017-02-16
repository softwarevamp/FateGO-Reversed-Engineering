using System;
using UnityEngine;

public class CheckCombineEnalbleControl : MonoBehaviour
{
    private CombineEnableData combineEnalbeInfo;
    private ServantEntity servantEntity;
    private UserServantEntity[] userSvtEntityList;
    private UserServantMaster usrSvtMaster;

    public void checkCombineEnableNum()
    {
        this.usrSvtMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        this.userSvtEntityList = this.usrSvtMaster.getOrganizationList();
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        for (int i = 0; i < this.userSvtEntityList.Length; i++)
        {
            UserServantEntity usrData = this.userSvtEntityList[i];
            this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(usrData.svtId);
            if (this.checkLimitUpEnalbe(usrData))
            {
                num++;
            }
            if (this.checkSkillUpEnalbe(usrData))
            {
                num2++;
            }
            if (this.checkNpUpEnalbe(usrData))
            {
                num3++;
            }
        }
        this.combineEnalbeInfo = new CombineEnableData();
        this.combineEnalbeInfo.limitUpEnableNum = num;
        this.combineEnalbeInfo.skillUpEnableNum = num2;
        this.combineEnalbeInfo.npUpEnableNum = num3;
    }

    private bool checkLimitUpEnalbe(UserServantEntity usrData)
    {
        bool flag = false;
        bool flag2 = usrData.isLimitCountMax();
        int limitCount = usrData.limitCount;
        if ((!usrData.IsHeroine() && !flag2) && usrData.isLevelMax())
        {
            int combineLimitId = this.servantEntity.combineLimitId;
            CombineLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CombineLimitMaster>(DataNameKind.Kind.COMBINE_LIMIT).getEntityFromId<CombineLimitEntity>(combineLimitId, limitCount);
            int[] itemIds = entity.itemIds;
            int[] itemNums = entity.itemNums;
            UserItemMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM);
            for (int i = 0; i < itemIds.Length; i++)
            {
                int num4 = itemIds[i];
                int num5 = itemNums[i];
                long[] args = new long[] { usrData.userId, (long) num4 };
                UserItemEntity entity2 = master2.getEntityFromId<UserItemEntity>(args);
                if (entity2 != null)
                {
                    if (num4 == entity2.itemId)
                    {
                        if (num5 > entity2.num)
                        {
                            return false;
                        }
                        flag = true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return flag;
    }

    private bool checkNpUpEnalbe(UserServantEntity usrData)
    {
        bool flag = false;
        if (!usrData.IsHeroine())
        {
            int num;
            int num2;
            int num3;
            int num4;
            int num5;
            string str;
            string str2;
            int num6;
            int num7;
            usrData.getTreasureDeviceInfo(out num, out num2, out num3, out num4, out num5, out str, out str2, out num6, out num7);
            if (num > 0)
            {
                int maxLv = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(num).maxLv;
                if (num2 < maxLv)
                {
                    UserServantEntity[] entityArray = this.usrSvtMaster.getSameServantList(usrData);
                    if ((entityArray != null) && (entityArray.Length > 0))
                    {
                        flag = true;
                    }
                }
            }
        }
        return flag;
    }

    private bool checkSkillUpEnalbe(UserServantEntity usrData)
    {
        bool flag = false;
        int[] numArray = usrData.getSkillIdList();
        int[] numArray2 = usrData.getSkillLevelList();
        for (int i = 0; i < numArray.Length; i++)
        {
            int id = numArray[i];
            int num3 = numArray2[i];
            if (id > 0)
            {
                int maxLv = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(id).maxLv;
                if (num3 < maxLv)
                {
                    CombineSkillEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.COMBINE_SKILL).getEntityFromId<CombineSkillEntity>(this.servantEntity.combineSkillId, num3);
                    if (entity2 != null)
                    {
                        int[] itemIds = entity2.itemIds;
                        int[] itemNums = entity2.itemNums;
                        UserItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM);
                        for (int j = 0; j < itemIds.Length; j++)
                        {
                            int num6 = itemIds[j];
                            int num7 = itemNums[j];
                            long[] args = new long[] { usrData.userId, (long) num6 };
                            UserItemEntity entity3 = master.getEntityFromId<UserItemEntity>(args);
                            if (entity3 != null)
                            {
                                if (num6 != entity3.itemId)
                                {
                                    continue;
                                }
                                if (num7 <= entity3.num)
                                {
                                    flag = true;
                                    continue;
                                }
                                flag = false;
                            }
                            else
                            {
                                flag = false;
                            }
                            break;
                        }
                        if (flag)
                        {
                            return flag;
                        }
                    }
                }
            }
        }
        return flag;
    }

    public CombineEnableData getCombineEnableNumInfo() => 
        this.combineEnalbeInfo;
}

