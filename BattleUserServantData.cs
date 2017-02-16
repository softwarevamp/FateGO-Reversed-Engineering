using System;

public class BattleUserServantData : UserServantEntity
{
    public int actPriority;
    public int aiId;
    public int chargeTurn;
    public int criticalRate;
    public int deathRate;
    public int displayType;
    public long equipTargetId1;
    public long[] equipTargetIds;
    public int[] individuality;
    public int maxActNum;
    public int npcSvtClassId;
    public int npcSvtType;
    public int overwriteSvtId;
    public int[] passiveSkill;
    public int recover;
    public int skillId1;
    public int skillId2;
    public int skillId3;
    public int starRate;
    public int tdRate;
    public int treasureDeviceId;
    public int treasureDeviceLv;

    public long[] getBattleEquipTargetList()
    {
        if (this.equipTargetIds != null)
        {
            return this.equipTargetIds;
        }
        long[] numArray = new long[BalanceConfig.SvtEquipMax];
        numArray[0] = this.equipTargetId1;
        return numArray;
    }

    public int[] getBattleSkillIdList()
    {
        int[] numArray = new int[BalanceConfig.SvtSkillListMax];
        numArray[0] = this.skillId1;
        numArray[1] = this.skillId2;
        numArray[2] = this.skillId3;
        return numArray;
    }

    public int getBattleSvtId()
    {
        if ((base.dispLimitCount != 1) && (0 < this.overwriteSvtId))
        {
            return this.overwriteSvtId;
        }
        return base.svtId;
    }

    public int[] getPassiveSkill()
    {
        if (this.passiveSkill != null)
        {
            return this.passiveSkill;
        }
        return new int[0];
    }
}

