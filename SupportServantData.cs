using System;

public class SupportServantData
{
    private string[] className = new string[] { "SUPPORT_KIND_0", "SUPPORT_KIND_1", "SUPPORT_KIND_2", "SUPPORT_KIND_3", "SUPPORT_KIND_4", "SUPPORT_KIND_5", "SUPPORT_KIND_6", "SUPPORT_KIND_7" };
    protected long[] equipIdList = new long[BalanceConfig.SupportDeckMax];
    protected bool isFriendInfo;
    protected bool isSelectServant;
    protected ServantStatusDialog.Kind kind;
    protected long[] oldEquipIdList = new long[BalanceConfig.SupportDeckMax];
    protected long[] oldServantIdList = new long[BalanceConfig.SupportDeckMax];
    protected long[] servantIdList = new long[BalanceConfig.SupportDeckMax];
    protected UserServantLearderEntity[] userServantLearderEntity = new UserServantLearderEntity[BalanceConfig.SupportDeckMax];

    public string getClassName(int classPos) => 
        LocalizationManager.Get(this.className[classPos]);

    public long getEquip(int classPos) => 
        this.equipIdList[classPos];

    public long[] GetEquipList() => 
        this.equipIdList;

    public int getEquipSum()
    {
        int num = 0;
        for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
        {
            if (this.equipIdList[i] != 0)
            {
                num++;
            }
        }
        return num;
    }

    public long getMember(int classPos)
    {
        if (this.userServantLearderEntity[classPos] == null)
        {
            return -1L;
        }
        return this.userServantLearderEntity[classPos].userSvtId;
    }

    public long getOldEquip(int classPos) => 
        this.oldEquipIdList[classPos];

    public long getOldServant(int classPos) => 
        this.oldServantIdList[classPos];

    public long getServant(int classPos) => 
        this.servantIdList[classPos];

    public long[] GetServantList() => 
        this.servantIdList;

    public int getServantSum()
    {
        int num = 0;
        for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
        {
            if (this.servantIdList[i] != 0)
            {
                num++;
            }
        }
        return num;
    }

    public UserServantLearderEntity getUserServantLearderEntity(int classPos) => 
        this.userServantLearderEntity[classPos];

    public void Init()
    {
        for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
        {
            this.servantIdList[i] = 0L;
            this.equipIdList[i] = 0L;
            this.oldServantIdList[i] = 0L;
            this.userServantLearderEntity[i] = new UserServantLearderEntity();
        }
        UserServantLearderMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantLearderMaster>(DataNameKind.Kind.USER_SERVANT_LEADER);
        UserServantMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        foreach (UserServantLearderEntity entity in master.getEntityList())
        {
            if (entity.userSvtId != 0)
            {
                this.userServantLearderEntity[entity.classId].setUserServantEntity(master2.getEntityFromId<UserServantEntity>(entity.userSvtId), entity.classId);
            }
            this.servantIdList[entity.classId] = this.oldServantIdList[entity.classId] = entity.userSvtId;
            this.equipIdList[entity.classId] = this.oldEquipIdList[entity.classId] = entity.getEquipServantId();
        }
        this.kind = ServantStatusDialog.Kind.PARTY;
        this.isFriendInfo = false;
        this.isSelectServant = false;
    }

    public void Init(FollowerInfo followerInfo, FriendStatus.Kind friendKind, bool isSelectServant)
    {
        for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
        {
            this.userServantLearderEntity[i] = new UserServantLearderEntity();
            int index = followerInfo.getIndexForSupport(i);
            ServantLeaderInfo info = followerInfo.getServantLeaderInfo(index);
            if ((info != null) && (info.svtId != 0))
            {
                this.userServantLearderEntity[i].setUserServantEntity(info, i);
                this.servantIdList[i] = this.oldServantIdList[i] = followerInfo.getSvtId(index);
            }
            else
            {
                this.servantIdList[i] = this.oldServantIdList[i] = 0L;
            }
            int num3 = followerInfo.getEquipSvtId(index);
            if (num3 != 0)
            {
                this.userServantLearderEntity[i].equipTarget1 = followerInfo.getEquipTarget1(index);
                this.equipIdList[i] = this.oldEquipIdList[i] = num3;
            }
            else
            {
                this.equipIdList[i] = this.oldEquipIdList[i] = 0L;
            }
        }
        if (friendKind == FriendStatus.Kind.FRIEND)
        {
            this.kind = ServantStatusDialog.Kind.FRIEND;
        }
        else
        {
            this.kind = ServantStatusDialog.Kind.FOLLOWER;
        }
        this.isFriendInfo = true;
        this.isSelectServant = isSelectServant;
    }

    public void Init(OtherUserGameEntity otherData, FriendStatus.Kind friendKind, bool isSelectServant)
    {
        for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
        {
            this.userServantLearderEntity[i] = new UserServantLearderEntity();
            ServantLeaderInfo info = otherData.getServantLeaderInfo(i);
            if (info != null)
            {
                this.userServantLearderEntity[i].setUserServantEntity(info, i);
                this.servantIdList[i] = this.oldServantIdList[i] = otherData.getSvtId(i);
            }
            else
            {
                this.servantIdList[i] = this.oldServantIdList[i] = 0L;
            }
            int num2 = otherData.getEquipSvtId(i);
            if (num2 != 0)
            {
                this.userServantLearderEntity[i].equipTarget1 = otherData.getEquipInfo(i);
                this.equipIdList[i] = this.oldEquipIdList[i] = num2;
            }
            else
            {
                this.equipIdList[i] = this.oldEquipIdList[i] = 0L;
            }
        }
        if (friendKind == FriendStatus.Kind.FRIEND)
        {
            this.kind = ServantStatusDialog.Kind.FRIEND;
        }
        else
        {
            this.kind = ServantStatusDialog.Kind.FOLLOWER;
        }
        this.isFriendInfo = true;
        this.isSelectServant = isSelectServant;
    }

    public bool isUseServant(long svtId)
    {
        for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
        {
            if (this.servantIdList[i] == svtId)
            {
                return true;
            }
        }
        return false;
    }

    public void removeEquipData(int classPos)
    {
        if (this.userServantLearderEntity[classPos].equipTarget1 != null)
        {
            this.userServantLearderEntity[classPos].equipTarget1.userSvtId = 0L;
        }
        this.equipIdList[classPos] = 0L;
    }

    public void removeServantData(int classPos)
    {
        this.userServantLearderEntity[classPos].userSvtId = 0L;
        this.servantIdList[classPos] = 0L;
    }

    public void setEquipData(int classPos, long svtId)
    {
        if (this.userServantLearderEntity[classPos].equipTarget1 == null)
        {
            this.userServantLearderEntity[classPos].equipTarget1 = new EquipTargetInfo();
        }
        this.userServantLearderEntity[classPos].equipTarget1.userSvtId = svtId;
        this.equipIdList[classPos] = svtId;
    }

    public void setServantData(int classPos, UserServantEntity entity)
    {
        if (this.userServantLearderEntity[classPos] == null)
        {
            this.userServantLearderEntity[classPos] = new UserServantLearderEntity();
        }
        UserServantLearderMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantLearderMaster>(DataNameKind.Kind.USER_SERVANT_LEADER);
        this.userServantLearderEntity[classPos].setUserServantEntity(entity, classPos);
        this.servantIdList[classPos] = entity.id;
    }

    public string updateCheck(int classPos)
    {
        bool flag = false;
        if (this.servantIdList[classPos] != this.oldServantIdList[classPos])
        {
            flag = true;
        }
        if (this.equipIdList[classPos] != this.oldEquipIdList[classPos])
        {
            flag = true;
        }
        if (flag)
        {
            object[] objArray1 = new object[] { "{\"classId\":", classPos, ",\"userSvtId\":", this.servantIdList[classPos], ",\"userSvtEquipId\":", this.equipIdList[classPos], "}" };
            return string.Concat(objArray1);
        }
        return null;
    }

    public bool IsFriendInfo =>
        this.isFriendInfo;

    public bool IsNoServant
    {
        get
        {
            for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
            {
                if (this.servantIdList[i] > 0L)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool IsSelectServant =>
        this.isSelectServant;

    public ServantStatusDialog.Kind Kind =>
        this.kind;

    public enum ClassPos
    {
        ALL,
        SABER,
        ARCHER,
        LANCER,
        RIDER,
        CASTER,
        ASSASSIN,
        BERSERKER
    }
}

