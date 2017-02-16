using System;

public class UserServantLearderEntity : DataEntityBase
{
    public int adjustAtk;
    public int adjustHp;
    public int atk;
    public int classId = 0;
    public int dispLimitCount;
    public EquipTargetInfo equipTarget1 = null;
    public int exceedCount;
    public int exp;
    public int hp;
    public int limitCount;
    public int lv;
    public int skillId1;
    public int skillId2;
    public int skillId3;
    public int skillLv1;
    public int skillLv2;
    public int skillLv3;
    public int svtId = 0;
    public int treasureDeviceId;
    public int treasureDeviceLv;
    public long updatedAt;
    public long userId;
    public long userSvtId = 0L;

    public long getEquipServantId()
    {
        if (this.equipTarget1 == null)
        {
            return 0L;
        }
        return this.equipTarget1.userSvtId;
    }

    public override string getPrimarykey() => 
        (this.userId.ToString() + ":" + this.classId.ToString());

    public ServantLeaderInfo getServantLeaderInfo() => 
        new ServantLeaderInfo { 
            userId = this.userId,
            classId = this.classId,
            userSvtId = this.svtId,
            svtId = this.svtId,
            limitCount = this.limitCount,
            lv = this.lv,
            exp = this.exp,
            hp = this.hp,
            atk = this.atk,
            adjustAtk = this.adjustAtk,
            adjustHp = this.adjustHp,
            equipTarget1 = this.equipTarget1,
            skillId1 = this.skillId1,
            skillId2 = this.skillId2,
            skillId3 = this.skillId3,
            skillLv1 = this.skillLv1,
            skillLv2 = this.skillLv2,
            skillLv3 = this.skillLv3,
            treasureDeviceId = this.treasureDeviceId,
            treasureDeviceLv = this.treasureDeviceLv,
            exceedCount = this.exceedCount
        };

    public void setUserServantEntity(ServantLeaderInfo info, int classPos)
    {
        this.userId = info.userId;
        this.classId = classPos;
        this.userSvtId = info.svtId;
        this.svtId = info.svtId;
        this.limitCount = info.limitCount;
        this.lv = info.lv;
        this.exp = info.exp;
        this.hp = info.hp;
        this.atk = info.atk;
        this.adjustAtk = info.adjustAtk;
        this.adjustHp = info.adjustHp;
        this.equipTarget1 = info.equipTarget1;
        this.skillId1 = info.skillId1;
        this.skillId2 = info.skillId2;
        this.skillId3 = info.skillId3;
        this.skillLv1 = info.skillLv1;
        this.skillLv2 = info.skillLv2;
        this.skillLv3 = info.skillLv3;
        this.treasureDeviceId = info.treasureDeviceId;
        this.treasureDeviceLv = info.treasureDeviceLv;
        this.exceedCount = info.exceedCount;
    }

    public void setUserServantEntity(UserServantEntity entity, int classPos)
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
        this.dispLimitCount = entity.dispLimitCount;
        this.userId = entity.userId;
        this.classId = classPos;
        this.userSvtId = entity.id;
        this.svtId = entity.svtId;
        this.limitCount = entity.limitCount;
        this.lv = entity.lv;
        this.exp = entity.exp;
        this.hp = entity.hp;
        this.atk = entity.atk;
        this.adjustAtk = entity.adjustAtk;
        this.adjustHp = entity.adjustHp;
        this.skillId1 = entity.skillLv1;
        this.skillId2 = entity.skillLv2;
        this.skillId3 = entity.skillLv3;
        entity.getTreasureDeviceInfo(out num, out num2, out num3, out num4, out num5, out str, out str2, out num6, out num7);
        this.treasureDeviceId = num;
        this.treasureDeviceLv = num2;
        this.exceedCount = entity.exceedCount;
    }
}

