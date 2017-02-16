using System;
using System.Runtime.InteropServices;

public class SupportServantEquipServantItem
{
    protected ServantEntity equipServantEntity;
    protected long equipTargetId;
    protected UserServantEntity equipUserSvtEntity;
    protected IconLabelInfo iconLabelInfo = new IconLabelInfo();
    protected long oldEquipTargetId;
    protected ServantEntity servantEntity;
    protected UserServantEntity userServantEntity;

    public SupportServantEquipServantItem(UserServantEntity entity, long equipSvtId)
    {
        this.userServantEntity = entity;
        if (this.userServantEntity != null)
        {
            this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userServantEntity.svtId);
            this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.userServantEntity.lv, this.userServantEntity.getLevelMax(), 0, false, false);
        }
        else
        {
            this.servantEntity = null;
        }
        this.oldEquipTargetId = equipSvtId;
        this.SetEquipTarget(equipSvtId);
    }

    public bool GetNpInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId)
    {
        if (this.userServantEntity != null)
        {
            return this.userServantEntity.getTreasureDeviceInfo(out tdId, out tdLv, out tdMaxLv, out tdRank, out tdMaxRank, out tdName, out tdExplanation, out tdGuageCount, out tdCardId);
        }
        tdId = 0;
        tdLv = 0;
        tdMaxLv = 0;
        tdRank = 0;
        tdMaxRank = 0;
        tdName = string.Empty;
        tdExplanation = string.Empty;
        tdGuageCount = 0;
        tdCardId = 0;
        return false;
    }

    public int GetTreasureDeviceLevelIcon()
    {
        if (this.userServantEntity != null)
        {
            return this.userServantEntity.getTreasureDeviceLevelIcon();
        }
        return 0;
    }

    public void SetEquipTarget(long equipUserSvtId)
    {
        this.equipTargetId = equipUserSvtId;
        this.equipUserSvtEntity = null;
        this.equipServantEntity = null;
        if (equipUserSvtId > 0L)
        {
            this.equipUserSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(equipUserSvtId);
            if (this.equipUserSvtEntity.svtId > 0)
            {
                this.equipServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.equipUserSvtEntity.svtId);
            }
        }
    }

    public int AdjustAtk
    {
        get
        {
            if (this.userServantEntity != null)
            {
                return this.userServantEntity.adjustAtk;
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
            return 0;
        }
    }

    public int Atk
    {
        get
        {
            if (this.userServantEntity != null)
            {
                return this.userServantEntity.atk;
            }
            return 0;
        }
    }

    public int EquipAtk
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.atk;
            }
            return 0;
        }
    }

    public int EquipHp
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.hp;
            }
            return 0;
        }
    }

    public int EquipLimitCount
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.limitCount;
            }
            return 0;
        }
    }

    public int EquipLimitCountMax
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipServantEntity.limitMax;
            }
            return 0;
        }
    }

    public UserServantEntity EquipUserServant =>
        this.equipUserSvtEntity;

    public long EquipUserSvtId
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.id;
            }
            return 0L;
        }
    }

    public int Hp
    {
        get
        {
            if (this.userServantEntity != null)
            {
                return this.userServantEntity.hp;
            }
            return 0;
        }
    }

    public IconLabelInfo IconInfo =>
        this.iconLabelInfo;

    public bool IsEquip =>
        (this.equipUserSvtEntity != null);

    public string NameText
    {
        get
        {
            if (this.servantEntity != null)
            {
                return this.servantEntity.name;
            }
            return "error";
        }
    }

    public long OldEquipUserSvtId =>
        this.oldEquipTargetId;

    public UserServantEntity UserServant =>
        this.userServantEntity;
}

