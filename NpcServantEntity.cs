using System;

public class NpcServantEntity : DataEntityBase
{
    public int actPriority;
    public long aiId;
    public long atk;
    public int chargeTurn;
    public int criticalRate;
    public int deathRate;
    public int displayType;
    public long[] dropIds;
    public long hp;
    public int hpGaugeType;
    public long id;
    public long[] individuality;
    public int limitCount;
    public int lv;
    public int maxActNum;
    public string name;
    public int npcSvtType;
    public long[] passiveSkill;
    public long skillId1;
    public long skillId2;
    public long skillId3;
    public int skillLv1;
    public int skillLv2;
    public int skillLv3;
    public int starRate;
    public long svtId;
    public int tdRate;
    public long treasureDeviceId;
    public int treasureDeviceLv;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

