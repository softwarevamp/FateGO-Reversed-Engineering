using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class UserServantCollectionEntity : DataEntityBase
{
    public long createdAt;
    public int friendship;
    public int friendshipRank;
    public int maxAtk;
    public int maxHp;
    public int maxLimitCount;
    public int maxLv;
    public int skillLv1;
    public int skillLv2;
    public int skillLv3;
    public int status;
    public int svtId;
    public int treasureDeviceLv1;
    public int treasureDeviceLv2;
    public int treasureDeviceLv3;
    public long userId;
    public long voicePlayed;

    public UserServantCollectionEntity()
    {
    }

    public UserServantCollectionEntity(UserServantCollectionEntity e)
    {
        this.userId = e.userId;
        this.svtId = e.svtId;
        this.status = e.status;
        this.maxLv = e.maxLv;
        this.maxHp = e.maxHp;
        this.maxAtk = e.maxAtk;
        this.maxLimitCount = e.maxLimitCount;
        this.skillLv1 = e.skillLv1;
        this.skillLv2 = e.skillLv2;
        this.skillLv3 = e.skillLv3;
        this.treasureDeviceLv1 = e.treasureDeviceLv1;
        this.treasureDeviceLv2 = e.treasureDeviceLv2;
        this.treasureDeviceLv3 = e.treasureDeviceLv3;
        this.friendship = e.friendship;
        this.friendshipRank = e.friendshipRank;
        this.voicePlayed = e.voicePlayed;
        this.createdAt = e.createdAt;
    }

    public UserServantCollectionEntity(long userId, int svtId)
    {
        this.userId = userId;
        this.svtId = svtId;
        this.status = 0;
        this.skillLv1 = 1;
        this.skillLv2 = 1;
        this.skillLv3 = 1;
        this.treasureDeviceLv1 = 1;
        this.treasureDeviceLv2 = 1;
        this.treasureDeviceLv3 = 1;
        this.friendshipRank = 0;
    }

    public int getCardImageLimitCount(bool isReal = true) => 
        ImageLimitCount.GetCardImageLimitCount(this.svtId, this.maxLimitCount, true, isReal);

    public bool getCollectionStatus(out int lv, out int hp, out int atk)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        if (this.status == 2)
        {
            ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, entity.limitMax);
            lv = entity2.lvMax;
            atk = entity2.atkMax;
            hp = entity2.hpMax;
            return true;
        }
        ServantLimitEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, 0);
        lv = 1;
        atk = entity3.atkBase;
        hp = entity3.hpBase;
        return false;
    }

    public int getFriendShipRank() => 
        this.friendshipRank;

    public bool getFriendShipRankInfo(out int rank, out int maxRank)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        if ((entity != null) && entity.IsServant)
        {
            FriendshipMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FriendshipMaster>(DataNameKind.Kind.FRIENDSHIP);
            rank = this.friendshipRank;
            maxRank = master.GetFriendshipRankMax(entity.friendshipId);
            return true;
        }
        rank = -1;
        maxRank = 0;
        return false;
    }

    public int getImageLimitCount() => 
        ImageLimitCount.GetImageLimitCount(this.svtId, this.maxLimitCount);

    public int getLevelMax()
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, entity.limitMax).lvMax;
    }

    public int getLimitCountMax() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).limitMax;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.svtId };
        return string.Concat(objArray1);
    }

    public void getSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList)
    {
        bool[] flagArray;
        this.getSkillInfo(out idList, out lvList, out chargeList, out titleList, out explanationList, out flagArray, -1);
    }

    public void getSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList, out bool[] tdIsUseList, int beforeClearQuestId = -1)
    {
        bool isServantEquip = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId).IsServantEquip;
        idList = new int[BalanceConfig.SvtSkillListMax];
        lvList = this.getSkillLevelList();
        chargeList = new int[BalanceConfig.SvtSkillListMax];
        titleList = new string[BalanceConfig.SvtSkillListMax];
        explanationList = new string[BalanceConfig.SvtSkillListMax];
        tdIsUseList = new bool[BalanceConfig.SvtSkillListMax];
        ServantSkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL);
        for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
        {
            ServantSkillEntity entity2;
            int priority = 1;
            tdIsUseList[i] = false;
        Label_0082:
            entity2 = master.getEntityFromId(this.svtId, i + 1, priority);
            if (entity2 != null)
            {
                if (entity2.isUse(this.userId, this.maxLv, this.maxLimitCount, beforeClearQuestId))
                {
                    idList[i] = entity2.getSkillId();
                    entity2.getEffectExplanation(out chargeList[i], out titleList[i], out explanationList[i], lvList[i], isServantEquip);
                    tdIsUseList[i] = true;
                    goto Label_0147;
                }
                if (idList[i] == 0)
                {
                    idList[i] = entity2.getSkillId();
                    lvList[i] = -1;
                    entity2.getAcquisitionMethodExplanation(out titleList[i], out explanationList[i]);
                }
            }
            else if (idList[i] == 0)
            {
                lvList[i] = -1;
            }
            continue;
        Label_0147:
            priority++;
            goto Label_0082;
        }
    }

    public int[] getSkillLevelList()
    {
        int[] numArray = new int[BalanceConfig.SvtSkillListMax];
        numArray[0] = this.skillLv1;
        numArray[1] = this.skillLv2;
        numArray[2] = this.skillLv3;
        return numArray;
    }

    public int getSvtId() => 
        this.svtId;

    public bool getTreasureDeviceInfo(out int tdId, out int tdLv, out int tdMaxLv, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId, int idx, int beforeClearQuestId = -1)
    {
        tdId = 0;
        tdLv = 0;
        tdMaxLv = 0;
        tdName = string.Empty;
        tdExplanation = string.Empty;
        tdGuageCount = 0;
        tdCardId = 0;
        switch (idx)
        {
            case 1:
                tdLv = this.treasureDeviceLv1;
                break;

            case 2:
                tdLv = this.treasureDeviceLv2;
                break;

            case 3:
                tdLv = this.treasureDeviceLv3;
                break;

            default:
                return false;
        }
        ServantTreasureDvcMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantTreasureDvcMaster>(DataNameKind.Kind.SERVANT_TREASUREDEVICE);
        int friendshipRank = this.friendshipRank;
        bool flag = false;
        foreach (ServantTreasureDvcEntity entity in master.GetEntityListFromIdNum(this.svtId, idx))
        {
            if ((entity == null) || !entity.isUse(this.userId, this.maxLv, friendshipRank, beforeClearQuestId))
            {
                return flag;
            }
            tdId = entity.treasureDeviceId;
            flag = entity.getEffectExplanation(out tdName, out tdExplanation, out tdMaxLv, out tdGuageCount, out tdCardId, tdLv);
        }
        return flag;
    }

    public bool getTreasureDeviceInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId, int idx, int beforeClearQuestId = -1)
    {
        tdId = 0;
        tdLv = 0;
        tdMaxLv = 0;
        tdRank = 0;
        tdMaxRank = 0;
        tdName = string.Empty;
        tdExplanation = string.Empty;
        tdGuageCount = 0;
        tdCardId = 0;
        switch (idx)
        {
            case 1:
                tdLv = this.treasureDeviceLv1;
                break;

            case 2:
                tdLv = this.treasureDeviceLv2;
                break;

            case 3:
                tdLv = this.treasureDeviceLv3;
                break;

            default:
                return false;
        }
        ServantTreasureDvcMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantTreasureDvcMaster>(DataNameKind.Kind.SERVANT_TREASUREDEVICE);
        int friendshipRank = this.friendshipRank;
        bool flag = false;
        ServantTreasureDvcEntity[] entityListFromIdNum = master.GetEntityListFromIdNum(this.svtId, idx);
        tdMaxRank = entityListFromIdNum.Length;
        foreach (ServantTreasureDvcEntity entity in entityListFromIdNum)
        {
            if ((entity == null) || !entity.isUse(this.userId, this.maxLv, friendshipRank, beforeClearQuestId))
            {
                return flag;
            }
            tdId = entity.treasureDeviceId;
            flag = entity.getEffectExplanation(out tdName, out tdExplanation, out tdMaxLv, out tdGuageCount, out tdCardId, tdLv);
            if (flag)
            {
                tdRank++;
            }
        }
        return flag;
    }

    public int[] getTreasureDeviceSeqIdList(int idx)
    {
        List<int> list = new List<int>();
        ServantTreasureDvcMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<ServantTreasureDvcMaster>(DataNameKind.Kind.SERVANT_TREASUREDEVICE);
        TreasureDvcMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcMaster>(DataNameKind.Kind.TREASUREDEVICE);
        int friendshipRank = this.friendshipRank;
        foreach (ServantTreasureDvcEntity entity in master.GetEntityListFromIdNum(this.svtId, idx))
        {
            if (entity == null)
            {
                break;
            }
            TreasureDvcEntity entity2 = master2.getEntityFromId<TreasureDvcEntity>(entity.treasureDeviceId);
            int item = (entity2 == null) ? entity.treasureDeviceId : entity2.seqId;
            foreach (int num4 in list)
            {
                if (num4 == item)
                {
                    item = 0;
                    break;
                }
            }
            if (item > 0)
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public long getUserId() => 
        this.userId;

    public bool IsFinded() => 
        (this.status != 0);

    public bool IsGet() => 
        (this.status == 2);

    public bool IsLevelMax() => 
        (this.getLevelMax() == this.maxLv);

    public bool IsLimitCountMax() => 
        (this.getLimitCountMax() == this.maxLimitCount);

    public bool IsNew()
    {
        if (NetworkManager.UserId != this.userId)
        {
            return false;
        }
        if (this.status != 2)
        {
            return false;
        }
        return (UserServantCollectionManager.IsNew(this.svtId) || (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).GetNewList(this.svtId).Length > 0));
    }

    public bool IsPlayed(int flagNum) => 
        ((this.voicePlayed & (((int) 1) << flagNum)) != 0L);

    public void SetOld()
    {
        if ((NetworkManager.UserId == this.userId) && (this.status == 2))
        {
            UserServantCollectionManager.SetOld(this.svtId);
        }
    }

    public bool SetPlayed(int flagNum)
    {
        long num = ((int) 1) << flagNum;
        if ((this.voicePlayed & num) == 0)
        {
            this.voicePlayed |= num;
            return true;
        }
        return false;
    }
}

