using System;
using System.Collections.Generic;

public class DeckServant
{
    public DeckServantData[] svts;

    public DeckServant()
    {
    }

    public DeckServant(int sum)
    {
        this.svts = new DeckServantData[sum];
        for (int i = 0; i < sum; i++)
        {
            this.svts[i] = new DeckServantData();
            this.svts[i].id = i + 1;
        }
    }

    public DeckServant(int sum, DeckServant deckInfo)
    {
        this.svts = new DeckServantData[sum];
        for (int i = 0; i < sum; i++)
        {
            if ((deckInfo.svts != null) && (i < deckInfo.svts.Length))
            {
                this.svts[i] = deckInfo.svts[i];
            }
            else
            {
                this.svts[i] = new DeckServantData();
                this.svts[i].id = i + 1;
            }
        }
    }

    public void CollectUserSvtId(List<long> collectList)
    {
        if (this.svts != null)
        {
            for (int i = 0; i < this.svts.Length; i++)
            {
                long userSvtId = this.svts[i].userSvtId;
                for (int j = 0; j < collectList.Count; j++)
                {
                    if (collectList[j] == userSvtId)
                    {
                        userSvtId = 0L;
                        break;
                    }
                }
                if (userSvtId > 0L)
                {
                    collectList.Add(userSvtId);
                }
            }
        }
    }

    public void CollectUserSvtId(List<long> svtCollectList, List<long> equipCollectList)
    {
        if (this.svts != null)
        {
            for (int i = 0; i < this.svts.Length; i++)
            {
                long userSvtId = this.svts[i].userSvtId;
                for (int j = 0; j < svtCollectList.Count; j++)
                {
                    if (svtCollectList[j] == userSvtId)
                    {
                        userSvtId = 0L;
                        break;
                    }
                }
                if (userSvtId > 0L)
                {
                    svtCollectList.Add(userSvtId);
                }
                if (this.svts[i].userSvtEquipIds != null)
                {
                    for (int k = 0; k < this.svts[i].userSvtEquipIds.Length; k++)
                    {
                        long item = this.svts[i].userSvtEquipIds[k];
                        for (int m = 0; m < equipCollectList.Count; m++)
                        {
                            if (equipCollectList[m] == item)
                            {
                                item = 0L;
                                break;
                            }
                        }
                        if (item > 0L)
                        {
                            equipCollectList.Add(item);
                        }
                    }
                }
            }
        }
    }

    public long[] GetEquipList(int menber)
    {
        long[] numArray = new long[BalanceConfig.SvtEquipMax];
        menber++;
        for (int i = 0; i < this.svts.Length; i++)
        {
            DeckServantData data = this.svts[i];
            if (data.id == menber)
            {
                if (!data.isFollowerSvt && (data.userSvtId > 0L))
                {
                    long[] userSvtEquipIds = data.userSvtEquipIds;
                    if (userSvtEquipIds == null)
                    {
                        return numArray;
                    }
                    for (int j = 0; j < BalanceConfig.SvtEquipMax; j++)
                    {
                        if (j >= userSvtEquipIds.Length)
                        {
                            return numArray;
                        }
                        numArray[j] = userSvtEquipIds[j];
                    }
                }
                return numArray;
            }
        }
        return numArray;
    }

    public long[] GetEquipList(long userSvtId)
    {
        long[] numArray = new long[BalanceConfig.SvtEquipMax];
        for (int i = 0; i < this.svts.Length; i++)
        {
            DeckServantData data = this.svts[i];
            if (data.userSvtId == userSvtId)
            {
                if (!data.isFollowerSvt)
                {
                    long[] userSvtEquipIds = data.userSvtEquipIds;
                    if (userSvtEquipIds == null)
                    {
                        return numArray;
                    }
                    for (int j = 0; j < BalanceConfig.SvtEquipMax; j++)
                    {
                        if (j >= userSvtEquipIds.Length)
                        {
                            return numArray;
                        }
                        numArray[j] = userSvtEquipIds[j];
                    }
                }
                return numArray;
            }
        }
        return numArray;
    }

    public UserServantEntity GetUserServant(int menber)
    {
        menber++;
        for (int i = 0; i < this.svts.Length; i++)
        {
            DeckServantData data = this.svts[i];
            if (data.id == menber)
            {
                if (!data.isFollowerSvt && (data.userSvtId > 0L))
                {
                    return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(data.userSvtId);
                }
                return null;
            }
        }
        return null;
    }

    public bool IsEquip(long userSvtId)
    {
        if (this.svts != null)
        {
            for (int i = 0; i < this.svts.Length; i++)
            {
                if (this.svts[i].userSvtEquipIds != null)
                {
                    for (int j = 0; j < this.svts[i].userSvtEquipIds.Length; j++)
                    {
                        if (this.svts[i].userSvtEquipIds[j] == userSvtId)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}

