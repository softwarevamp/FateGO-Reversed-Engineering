using System;

public class NpcServantFollowerEntity : DataEntityBase
{
    public int atk;
    public int hideFlag;
    public int hp;
    public long id;
    public int limitCount;
    public int lv;
    public string name;
    public string nameFollower;
    public int skillId1;
    public int skillId2;
    public int skillId3;
    public int skillLv1;
    public int skillLv2;
    public int skillLv3;
    public int svtId;
    public int treasureDeviceId;
    public int treasureDeviceLv;

    public FollowerInfo getFollowerInfo(long followerId)
    {
        FollowerInfo info = new FollowerInfo();
        ServantLeaderInfo info2 = new ServantLeaderInfo {
            userId = followerId,
            userSvtId = 0L,
            svtId = this.svtId,
            limitCount = this.limitCount,
            lv = this.lv,
            exp = 0,
            hp = this.hp,
            atk = this.atk,
            skillId1 = this.skillId1,
            skillId2 = this.skillId2,
            skillId3 = this.skillId3,
            skillLv1 = this.skillLv1,
            skillLv2 = this.skillLv2,
            skillLv3 = this.skillLv3,
            treasureDeviceId = this.treasureDeviceId,
            treasureDeviceLv = this.treasureDeviceLv,
            updatedAt = 0L,
            hideFlag = this.hideFlag
        };
        info.userId = followerId;
        info.type = 3;
        info.userName = this.GetFollowerName();
        info.userLv = 0;
        info.userSvtLeaderHash = new ServantLeaderInfo[] { info2 };
        return info;
    }

    public string GetFollowerName()
    {
        if (this.nameFollower != "NONE")
        {
            return this.nameFollower;
        }
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        return ((entity == null) ? string.Empty : entity.name);
    }

    public string GetName()
    {
        if (this.name != "NONE")
        {
            return this.name;
        }
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        return ((entity == null) ? string.Empty : entity.name);
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public static bool IsHideSupport(int hideFlag) => 
        ((hideFlag & 1) != 0);

    public int imageLimitCount =>
        ImageLimitCount.GetImageLimitCount(this.svtId, this.limitCount);

    public enum FlagKind
    {
        HIDE_SUPPORT
    }

    public enum HideField
    {
        HIDE_SUPPORT = 1
    }
}

