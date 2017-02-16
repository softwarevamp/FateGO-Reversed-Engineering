using System;
using System.Runtime.InteropServices;

public class FollowerInfo
{
    public int type;
    public long userId;
    public int userLv;
    public string userName;
    public ServantLeaderInfo[] userSvtLeaderHash;

    public int getAdjustAtk(int index) => 
        this.userSvtLeaderHash[index].adjustAtk;

    public int getAdjustHp(int index) => 
        this.userSvtLeaderHash[index].adjustHp;

    public int getAtk(int index) => 
        this.userSvtLeaderHash[index].atk;

    public int getEquipAtk(int index)
    {
        if (index < 0)
        {
            return 0;
        }
        if (this.userSvtLeaderHash == null)
        {
            return 0;
        }
        if (this.userSvtLeaderHash.Length <= index)
        {
            return 0;
        }
        if (this.userSvtLeaderHash[index] == null)
        {
            return 0;
        }
        if (this.userSvtLeaderHash[index].equipTarget1 == null)
        {
            return 0;
        }
        return this.userSvtLeaderHash[index].equipTarget1.atk;
    }

    public int getEquipHp(int index)
    {
        if (index < 0)
        {
            return 0;
        }
        if (this.userSvtLeaderHash == null)
        {
            return 0;
        }
        if (this.userSvtLeaderHash.Length <= index)
        {
            return 0;
        }
        if (this.userSvtLeaderHash[index] == null)
        {
            return 0;
        }
        if (this.userSvtLeaderHash[index].equipTarget1 == null)
        {
            return 0;
        }
        return this.userSvtLeaderHash[index].equipTarget1.hp;
    }

    public int getEquipLimitCount(int index) => 
        this.userSvtLeaderHash[index].equipTarget1.limitCount;

    public int getEquipSkillId(int index) => 
        this.userSvtLeaderHash[index].equipTarget1.skillId1;

    public int getEquipSkillLv(int index) => 
        this.userSvtLeaderHash[index].equipTarget1.skillLv1;

    public int getEquipSvtId(int index)
    {
        if (index < 0)
        {
            return 0;
        }
        if (this.userSvtLeaderHash == null)
        {
            return 0;
        }
        if (this.userSvtLeaderHash.Length <= index)
        {
            return 0;
        }
        if (this.userSvtLeaderHash[index] == null)
        {
            return 0;
        }
        if (this.userSvtLeaderHash[index].equipTarget1 == null)
        {
            return 0;
        }
        return this.userSvtLeaderHash[index].equipTarget1.svtId;
    }

    public EquipTargetInfo getEquipTarget1(int index)
    {
        if (index < 0)
        {
            return null;
        }
        if (this.userSvtLeaderHash == null)
        {
            return null;
        }
        if (this.userSvtLeaderHash.Length <= index)
        {
            return null;
        }
        if (this.userSvtLeaderHash[index] == null)
        {
            return null;
        }
        return this.userSvtLeaderHash[index].equipTarget1;
    }

    public bool getEventUpVal(EventUpValSetupInfo setupInfo, int index) => 
        this.userSvtLeaderHash[index].getEventUpVal(setupInfo);

    public bool getEventUpVal(out EventUpValInfo eventUpVallInfo, EventUpValSetupInfo setupInfo, int index) => 
        this.userSvtLeaderHash[index].getEventUpVal(out eventUpVallInfo, setupInfo);

    public int getExp(int index) => 
        this.userSvtLeaderHash[index].exp;

    public int getFriendPointUpVal(int index) => 
        this.userSvtLeaderHash[index].getFriendPointUpVal();

    public int getHp(int index) => 
        this.userSvtLeaderHash[index].hp;

    public int getIndex(int classId)
    {
        if (this.userSvtLeaderHash != null)
        {
            if ((classId == 0) || (this.type == 3))
            {
                if (this.userSvtLeaderHash.Length > 0)
                {
                    return 0;
                }
            }
            else
            {
                ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
                ServantClassMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantClassMaster>(DataNameKind.Kind.SERVANT_CLASS);
                int length = this.userSvtLeaderHash.Length;
                for (int i = 1; i < length; i++)
                {
                    if (this.userSvtLeaderHash[i].svtId > 0)
                    {
                        ServantEntity entity = master.getEntityFromId<ServantEntity>(this.userSvtLeaderHash[i].svtId);
                        if (master2.getEntityFromId<ServantClassEntity>(entity.classId).attri == classId)
                        {
                            return i;
                        }
                    }
                }
            }
        }
        return -1;
    }

    public int getIndexForSupport(int classId)
    {
        if (this.userSvtLeaderHash != null)
        {
            int length = this.userSvtLeaderHash.Length;
            for (int i = 0; i < length; i++)
            {
                if (this.userSvtLeaderHash[i].classId == classId)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public int getLimitCount(int index) => 
        this.userSvtLeaderHash[index].limitCount;

    public int getLv(int index)
    {
        if (index < 0)
        {
            return 0;
        }
        if (this.userSvtLeaderHash == null)
        {
            return 0;
        }
        if (this.userSvtLeaderHash.Length <= index)
        {
            return 0;
        }
        if (this.userSvtLeaderHash[index] == null)
        {
            return 0;
        }
        return this.userSvtLeaderHash[index].lv;
    }

    public int getMaxLv(int index)
    {
        if (index < 0)
        {
            return 0;
        }
        if (this.userSvtLeaderHash == null)
        {
            return 0;
        }
        if (this.userSvtLeaderHash.Length <= index)
        {
            return 0;
        }
        if (this.userSvtLeaderHash[index] == null)
        {
            return 0;
        }
        if (this.type == 3)
        {
            return 0;
        }
        return this.userSvtLeaderHash[index].getLevelMax();
    }

    public ServantLeaderInfo getServantLeaderInfo(int index)
    {
        if (index < 0)
        {
            return null;
        }
        if (this.userSvtLeaderHash == null)
        {
            return null;
        }
        if (this.userSvtLeaderHash.Length <= index)
        {
            return null;
        }
        return this.userSvtLeaderHash[index];
    }

    public int[] getSkillIdList(int index) => 
        this.userSvtLeaderHash[index].getSkillIdList();

    public void getSkillInfo(out int[] idList, out int[] lvList, out int[] chargeList, out string[] titleList, out string[] explanationList, int index)
    {
        this.userSvtLeaderHash[index].getSkillInfo(out idList, out lvList, out chargeList, out titleList, out explanationList);
    }

    public int[] getSkillLevelList(int index) => 
        this.userSvtLeaderHash[index].getSkillLevelList();

    public int getSvtId(int index)
    {
        if (index < 0)
        {
            return 0;
        }
        if (this.userSvtLeaderHash == null)
        {
            return 0;
        }
        if (this.userSvtLeaderHash.Length <= index)
        {
            return 0;
        }
        if (this.userSvtLeaderHash[index] == null)
        {
            return 0;
        }
        return this.userSvtLeaderHash[index].svtId;
    }

    public bool getTreasureDeviceInfo(out int tdLv, out int tdMaxLv, int index) => 
        this.userSvtLeaderHash[index].getTreasureDeviceInfo(out tdLv, out tdMaxLv);

    public bool getTreasureDeviceInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId, int index) => 
        this.userSvtLeaderHash[index].getTreasureDeviceInfo(out tdId, out tdLv, out tdMaxLv, out tdRank, out tdMaxRank, out tdName, out tdExplanation, out tdGuageCount, out tdCardId);

    public int getTreasureDeviceLevelIcon(int index) => 
        this.userSvtLeaderHash[index].getTreasureDeviceLevelIcon();

    public long getUpdatedAt()
    {
        if (this.userSvtLeaderHash == null)
        {
            return 0L;
        }
        if (this.userSvtLeaderHash.Length <= 0)
        {
            return 0L;
        }
        if (this.userSvtLeaderHash[0] == null)
        {
            return 0L;
        }
        return this.userSvtLeaderHash[0].updatedAt;
    }
}

