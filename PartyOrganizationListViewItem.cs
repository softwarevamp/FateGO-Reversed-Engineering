using System;
using System.Runtime.InteropServices;

public class PartyOrganizationListViewItem : ListViewItem
{
    protected int classId;
    protected long[] equipIdList;
    protected ServantEntity equipServantEntity;
    protected UserServantEntity equipUserServantEntity;
    protected int followerClassId;
    protected int followerIndex;
    protected FollowerInfo followerInfo;
    protected IconLabelInfo iconLabelInfo;
    protected bool isFollower;
    protected int rarity;
    protected ServantEntity servantEntity;
    protected EventUpValSetupInfo setupInfo;
    protected UserServantEntity userServantEntity;

    protected PartyOrganizationListViewItem(int index) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
    }

    public PartyOrganizationListViewItem(int index, bool isFollower, EventUpValSetupInfo setupInfo) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.userServantEntity = null;
        this.followerInfo = null;
        this.isFollower = isFollower;
        this.followerClassId = 0;
        this.followerIndex = 0;
        this.setupInfo = setupInfo;
        this.servantEntity = null;
        this.classId = 0;
        this.rarity = 0;
        this.SetEquipStatus(null);
        this.iconLabelInfo.Clear();
    }

    public PartyOrganizationListViewItem(int index, FollowerInfo follower, int followerClassId, EventUpValSetupInfo setupInfo) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.userServantEntity = null;
        this.followerInfo = follower;
        this.isFollower = true;
        this.followerClassId = followerClassId;
        this.followerIndex = this.followerInfo.getIndex(followerClassId);
        this.setupInfo = setupInfo;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.followerInfo.getSvtId(this.followerIndex));
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.servantEntity.id, this.followerInfo.getLimitCount(this.followerIndex));
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        this.SetEquipStatus(null);
        this.iconLabelInfo.Clear();
    }

    public PartyOrganizationListViewItem(int index, UserServantEntity userServantEntity, long[] equipIdList, EventUpValSetupInfo setupInfo) : base(index)
    {
        this.iconLabelInfo = new IconLabelInfo();
        this.userServantEntity = userServantEntity;
        this.followerInfo = null;
        this.isFollower = false;
        this.followerClassId = 0;
        this.followerIndex = 0;
        this.setupInfo = setupInfo;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userServantEntity.svtId);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userServantEntity.svtId, this.userServantEntity.limitCount);
        this.classId = this.servantEntity.classId;
        this.rarity = entity.rarity;
        this.SetEquipStatus(equipIdList);
        this.iconLabelInfo.Clear();
    }

    public void ClearFollower()
    {
        this.followerInfo = null;
    }

    public PartyOrganizationListViewItem Clone()
    {
        PartyOrganizationListViewItem item = new PartyOrganizationListViewItem(base.index);
        item.Set(this);
        return item;
    }

    public bool CompMember(PartyOrganizationListViewItem item)
    {
        if (this.IsFollower != item.IsFollower)
        {
            return false;
        }
        if (!this.IsFollower)
        {
            if (this.userServantEntity != item.userServantEntity)
            {
                return false;
            }
            if ((this.equipIdList == null) && (item.equipIdList == null))
            {
                return true;
            }
            if ((this.equipIdList != null) && (item.equipIdList != null))
            {
                for (int i = 0; i < BalanceConfig.SvtEquipMax; i++)
                {
                    if (this.equipIdList[i] != item.equipIdList[i])
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void Empty()
    {
        this.userServantEntity = null;
        this.followerInfo = null;
        this.followerClassId = 0;
        this.followerIndex = 0;
        this.equipIdList = null;
        this.servantEntity = null;
        this.classId = 0;
        this.rarity = 0;
        this.SetEquipStatus(null);
        this.iconLabelInfo.Clear();
    }

    ~PartyOrganizationListViewItem()
    {
    }

    public string GetAssetName()
    {
        if (this.servantEntity != null)
        {
            return UINarrowFigureRender.GetAssetName(this.servantEntity.id);
        }
        return null;
    }

    public int[] GetCommandCardList()
    {
        if ((this.servantEntity == null) || (this.servantEntity.cardIds == null))
        {
            return null;
        }
        int[] numArray = new int[3];
        for (int i = 0; i < this.servantEntity.cardIds.Length; i++)
        {
            int num2 = this.servantEntity.cardIds[i];
            if ((num2 > 0) && (num2 <= 3))
            {
                numArray[num2 - 1]++;
            }
        }
        return numArray;
    }

    public long[] GetEquipList() => 
        ((this.equipIdList == null) ? new long[BalanceConfig.SvtEquipMax] : ((long[]) this.equipIdList.Clone()));

    public bool GetEventUpVal(out EventUpValInfo eventUpValInfo)
    {
        if (this.setupInfo != null)
        {
            if (this.userServantEntity != null)
            {
                return this.userServantEntity.getEventUpVal(out eventUpValInfo, this.setupInfo, this.GetEquipList());
            }
            if (this.followerInfo != null)
            {
                return this.followerInfo.getEventUpVal(out eventUpValInfo, this.setupInfo, this.followerIndex);
            }
        }
        eventUpValInfo = null;
        return false;
    }

    public int GetFriendPointUpVal()
    {
        if (this.userServantEntity != null)
        {
            return this.userServantEntity.getFriendPointUpVal(this.GetEquipList());
        }
        if (this.followerInfo != null)
        {
            return this.followerInfo.getFriendPointUpVal(this.followerIndex);
        }
        return 0;
    }

    public bool IsEventUpVal()
    {
        if (this.setupInfo != null)
        {
            if (this.userServantEntity != null)
            {
                return this.userServantEntity.getEventUpVal(this.setupInfo, this.GetEquipList());
            }
            if (this.followerInfo != null)
            {
                return this.followerInfo.getEventUpVal(this.setupInfo, this.followerIndex);
            }
        }
        return false;
    }

    public void Modify()
    {
        if (this.userServantEntity != null)
        {
            long id = this.userServantEntity.id;
            this.userServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id);
        }
    }

    public void Modify(PartyServantListViewItem item)
    {
        this.userServantEntity = item.UserServant;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userServantEntity.svtId);
        this.classId = item.ClassId;
        this.rarity = item.Rarity;
        this.SetEquipStatus(this.equipIdList);
        this.iconLabelInfo.Set(item.IconInfo);
    }

    public void Set(PartyOrganizationListViewItem item)
    {
        base.Set(item);
        this.userServantEntity = item.userServantEntity;
        this.followerInfo = item.followerInfo;
        this.isFollower = item.isFollower;
        this.followerClassId = item.followerClassId;
        this.followerIndex = item.followerIndex;
        this.setupInfo = item.setupInfo;
        this.servantEntity = item.servantEntity;
        this.classId = item.classId;
        this.rarity = item.rarity;
        this.SetEquipStatus(item.equipIdList);
        this.iconLabelInfo.Set(item.iconLabelInfo);
    }

    public void Set(PartyServantListViewItem item)
    {
        base.Set(item);
        this.userServantEntity = item.UserServant;
        this.followerInfo = null;
        this.followerClassId = 0;
        this.followerIndex = 0;
        this.setupInfo = null;
        this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userServantEntity.svtId);
        this.classId = item.ClassId;
        this.rarity = item.Rarity;
        this.SetEquipStatus(this.equipIdList);
    }

    protected void SetEquipStatus(long[] equipIdList = null)
    {
        this.equipUserServantEntity = null;
        this.equipServantEntity = null;
        if (this.isFollower)
        {
            this.equipIdList = null;
            if ((this.followerInfo != null) && (this.followerInfo.getEquipSvtId(this.followerIndex) > 0))
            {
                this.equipServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.followerInfo.getEquipSvtId(this.followerIndex));
            }
        }
        else if (this.userServantEntity != null)
        {
            if ((equipIdList == null) || (this.equipIdList != equipIdList))
            {
                this.equipIdList = (equipIdList == null) ? new long[BalanceConfig.SvtEquipMax] : ((long[]) equipIdList.Clone());
            }
            if (this.equipIdList[0] > 0L)
            {
                this.equipUserServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.equipIdList[0]);
                if ((this.equipUserServantEntity != null) && (this.equipUserServantEntity.svtId > 0))
                {
                    this.equipServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.equipUserServantEntity.svtId);
                }
            }
        }
        else
        {
            this.equipIdList = null;
        }
    }

    public void SetEquipUserServantId(long userSvtId)
    {
        if (this.equipIdList != null)
        {
            this.equipIdList[0] = userSvtId;
            this.SetEquipStatus(this.equipIdList);
        }
    }

    public void Swap(PartyOrganizationListViewItem item)
    {
        UserServantEntity userServantEntity = this.userServantEntity;
        FollowerInfo followerInfo = this.followerInfo;
        bool isFollower = this.isFollower;
        int followerClassId = this.followerClassId;
        int followerIndex = this.followerIndex;
        EventUpValSetupInfo setupInfo = item.setupInfo;
        long[] equipIdList = this.equipIdList;
        ServantEntity servantEntity = this.servantEntity;
        int classId = this.classId;
        int rarity = this.rarity;
        IconLabelInfo iconLabelInfo = this.iconLabelInfo;
        this.userServantEntity = item.userServantEntity;
        this.followerInfo = item.followerInfo;
        this.isFollower = item.isFollower;
        this.followerClassId = item.followerClassId;
        this.followerIndex = item.followerIndex;
        this.setupInfo = item.setupInfo;
        this.servantEntity = item.servantEntity;
        this.classId = item.ClassId;
        this.rarity = item.Rarity;
        this.SetEquipStatus(item.equipIdList);
        this.iconLabelInfo = item.iconLabelInfo;
        item.userServantEntity = userServantEntity;
        item.followerInfo = followerInfo;
        item.isFollower = isFollower;
        item.followerClassId = followerClassId;
        item.followerIndex = followerIndex;
        item.setupInfo = setupInfo;
        item.servantEntity = servantEntity;
        item.classId = classId;
        item.rarity = rarity;
        item.SetEquipStatus(equipIdList);
        item.iconLabelInfo = iconLabelInfo;
    }

    public int AdjustAtk
    {
        get
        {
            if (this.userServantEntity != null)
            {
                return this.userServantEntity.adjustAtk;
            }
            if (this.followerInfo != null)
            {
                return this.followerInfo.getAdjustAtk(this.followerIndex);
            }
            return 0;
        }
    }

    public int AdjustHp
    {
        get
        {
            if (this.userServantEntity != null)
            {
                return this.userServantEntity.adjustHp;
            }
            if (this.followerInfo != null)
            {
                return this.followerInfo.getAdjustHp(this.followerIndex);
            }
            return 0;
        }
    }

    public int ClassId =>
        this.classId;

    public int EquipCost
    {
        get
        {
            if (!this.isFollower && ((this.servantEntity != null) && (this.equipServantEntity != null)))
            {
                return this.equipServantEntity.cost;
            }
            return -1;
        }
    }

    public int EquipLimitCount
    {
        get
        {
            if (this.equipUserServantEntity != null)
            {
                return this.equipUserServantEntity.limitCount;
            }
            if (this.followerInfo != null)
            {
                return this.followerInfo.getEquipLimitCount(this.followerIndex);
            }
            return 0;
        }
    }

    public int EquipLimitCountMax =>
        ((this.equipServantEntity == null) ? 0 : this.equipServantEntity.limitMax);

    public int EquipSvtId =>
        ((this.equipServantEntity == null) ? 0 : this.equipServantEntity.id);

    public EquipTargetInfo EquipTarget1
    {
        get
        {
            if (this.followerInfo != null)
            {
                return this.followerInfo.getEquipTarget1(this.followerIndex);
            }
            return null;
        }
    }

    public long EquipUserSvtId =>
        ((this.equipUserServantEntity == null) ? 0L : this.equipUserServantEntity.id);

    public FollowerInfo FollowerData =>
        this.followerInfo;

    public bool IsEmpty
    {
        get
        {
            if (this.userServantEntity != null)
            {
                return false;
            }
            if (this.isFollower)
            {
                return false;
            }
            return true;
        }
    }

    public bool IsEventJoin =>
        ((this.userServantEntity != null) && this.userServantEntity.IsEventJoin());

    public bool IsFollower =>
        this.isFollower;

    public int Level
    {
        get
        {
            if (this.userServantEntity != null)
            {
                return this.userServantEntity.lv;
            }
            if (this.followerInfo != null)
            {
                return this.followerInfo.getLv(this.followerIndex);
            }
            return 0;
        }
    }

    public int MainCost
    {
        get
        {
            if (!this.isFollower && (this.servantEntity != null))
            {
                return this.servantEntity.cost;
            }
            return 0;
        }
    }

    public int MargeAtk
    {
        get
        {
            if (this.userServantEntity != null)
            {
                if (this.equipUserServantEntity != null)
                {
                    return (this.userServantEntity.atk + this.equipUserServantEntity.atk);
                }
                return this.userServantEntity.atk;
            }
            if (this.followerInfo == null)
            {
                return 0;
            }
            if (this.equipServantEntity != null)
            {
                return (this.followerInfo.getAtk(this.followerIndex) + this.followerInfo.getEquipAtk(this.followerIndex));
            }
            return this.followerInfo.getAtk(this.followerIndex);
        }
    }

    public int MargeCost
    {
        get
        {
            if (this.isFollower)
            {
                return 0;
            }
            if (this.servantEntity == null)
            {
                return 0;
            }
            if (this.equipServantEntity != null)
            {
                return (this.servantEntity.cost + this.equipServantEntity.cost);
            }
            return this.servantEntity.cost;
        }
    }

    public int MargeHp
    {
        get
        {
            if (this.userServantEntity != null)
            {
                if (this.equipUserServantEntity != null)
                {
                    return (this.userServantEntity.hp + this.equipUserServantEntity.hp);
                }
                return this.userServantEntity.hp;
            }
            if (this.followerInfo == null)
            {
                return 0;
            }
            if (this.equipServantEntity != null)
            {
                return (this.followerInfo.getHp(this.followerIndex) + this.followerInfo.getEquipHp(this.followerIndex));
            }
            return this.followerInfo.getHp(this.followerIndex);
        }
    }

    public int MaxLevel
    {
        get
        {
            if (this.userServantEntity != null)
            {
                return this.userServantEntity.getLevelMax();
            }
            if (this.followerInfo != null)
            {
                return this.followerInfo.getMaxLv(this.followerIndex);
            }
            return 0;
        }
    }

    public int Rarity =>
        this.rarity;

    public ServantEntity Servant =>
        this.servantEntity;

    public ServantLeaderInfo ServantLeader
    {
        get
        {
            if (this.followerInfo != null)
            {
                return this.followerInfo.getServantLeaderInfo(this.followerIndex);
            }
            return null;
        }
    }

    public string ServantName
    {
        get
        {
            if (this.followerInfo != null)
            {
                return this.followerInfo.userName;
            }
            if (this.servantEntity != null)
            {
                return this.servantEntity.name;
            }
            return string.Empty;
        }
    }

    public int SvtId =>
        ((this.servantEntity == null) ? 0 : this.servantEntity.id);

    public string SvtNameText
    {
        get
        {
            if ((this.followerInfo != null) && (this.followerInfo.type == 3))
            {
                return this.followerInfo.userName;
            }
            if (this.servantEntity != null)
            {
                return this.servantEntity.name;
            }
            return "error";
        }
    }

    public UserServantEntity UserServant =>
        this.userServantEntity;
}

