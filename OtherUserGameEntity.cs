using System;
using System.Runtime.InteropServices;

public class OtherUserGameEntity : DataEntityBase
{
    public string friendCode;
    public long userId;
    public int userLv;
    public string userName;
    public ServantLeaderInfo[] userSvtLeaderHash;

    public int getAdjustAtk(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.adjustAtk;
            }
        }
        return 0;
    }

    public int getAdjustHp(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.adjustHp;
            }
        }
        return 0;
    }

    public int getAtk(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.atk;
            }
        }
        return 0;
    }

    public int getEquipAtk(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if (((info.classId == classPos) && (info.userSvtId != 0)) && (info.equipTarget1 != null))
            {
                return info.equipTarget1.atk;
            }
        }
        return 0;
    }

    public int getEquipExp(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if (((info.classId == classPos) && (info.userSvtId != 0)) && (info.equipTarget1 != null))
            {
                return info.equipTarget1.exp;
            }
        }
        return 0;
    }

    public int getEquipHp(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if (((info.classId == classPos) && (info.userSvtId != 0)) && (info.equipTarget1 != null))
            {
                return info.equipTarget1.hp;
            }
        }
        return 0;
    }

    public EquipTargetInfo getEquipInfo(int classPos = 0)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if (info.classId == classPos)
            {
                return info.equipTarget1;
            }
        }
        return null;
    }

    public int getEquipSvtId(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if (((info.classId == classPos) && (info.userSvtId != 0)) && (info.equipTarget1 != null))
            {
                return info.equipTarget1.svtId;
            }
        }
        return 0;
    }

    public int getHp(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.hp;
            }
        }
        return 0;
    }

    public int getLimitCount(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.limitCount;
            }
        }
        return 0;
    }

    public int getLv(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.lv;
            }
        }
        return 0;
    }

    public int getMaxLv(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.getLevelMax();
            }
        }
        return 0;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.userId);

    public ServantLeaderInfo getServantLeaderInfo(int classPos = 0)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info;
            }
        }
        return null;
    }

    public int[] getSkillIdList(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.getSkillIdList();
            }
        }
        return null;
    }

    public void getSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList, int classPos = 0)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                info.getSkillInfo(out idList, out lvList, out chargeList, out titleList, out explanationList);
                return;
            }
        }
        idList = new int[BalanceConfig.SvtSkillListMax];
        lvList = new int[BalanceConfig.SvtSkillListMax];
        chargeList = new int[BalanceConfig.SvtSkillListMax];
        titleList = new string[BalanceConfig.SvtSkillListMax];
        explanationList = new string[BalanceConfig.SvtSkillListMax];
    }

    public int[] getSkillLevelList(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.getSkillLevelList();
            }
        }
        return null;
    }

    public int getSvtId(int classPos)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.svtId;
            }
        }
        return 0;
    }

    public bool getTreasureDeviceInfo(out int tdLv, out int tdMaxLv, int classPos = 0)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.getTreasureDeviceInfo(out tdLv, out tdMaxLv);
            }
        }
        tdLv = 0;
        tdMaxLv = 0;
        return false;
    }

    public bool getTreasureDeviceInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId, int classPos = 0)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.getTreasureDeviceInfo(out tdId, out tdLv, out tdMaxLv, out tdRank, out tdMaxRank, out tdName, out tdExplanation, out tdGuageCount, out tdCardId);
            }
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

    public int getTreasureDeviceLevelIcon(int classPos = 0)
    {
        foreach (ServantLeaderInfo info in this.userSvtLeaderHash)
        {
            if ((info.classId == classPos) && (info.userSvtId != 0))
            {
                return info.getTreasureDeviceLevelIcon();
            }
        }
        return 0;
    }

    public long getUpdatedAt(int classPos) => 
        this.userSvtLeaderHash[0].updatedAt;
}

