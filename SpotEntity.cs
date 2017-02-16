using System;

public class SpotEntity : DataEntityBase
{
    public int activeCondType;
    public int activeTargetId;
    public static readonly int CALDEAGATE_ID = 0xf423f;
    public int dispCondType1;
    public int dispCondType2;
    public int dispTargetId1;
    public int dispTargetId2;
    public int id;
    public int imageId;
    public int imageOfsX;
    public int imageOfsY;
    public string name;
    public int nameOfsX;
    public int nameOfsY;
    public int questOfsX;
    public int questOfsY;
    public int warId;
    public int x;
    public int y;

    public SpotEntity()
    {
    }

    public SpotEntity(SpotEntity cSrc)
    {
        this.id = cSrc.id;
        this.warId = cSrc.warId;
        this.name = cSrc.name;
        this.imageId = cSrc.imageId;
        this.x = cSrc.x;
        this.y = cSrc.y;
        this.nameOfsX = cSrc.nameOfsX;
        this.nameOfsY = cSrc.nameOfsY;
        this.dispCondType1 = cSrc.dispCondType1;
        this.dispTargetId1 = cSrc.dispTargetId1;
        this.dispCondType2 = cSrc.dispCondType2;
        this.dispTargetId2 = cSrc.dispTargetId2;
        this.activeCondType = cSrc.activeCondType;
        this.activeTargetId = cSrc.activeTargetId;
    }

    public int getActiveCondType() => 
        this.activeCondType;

    public int getActiveTargetId() => 
        this.activeTargetId;

    public int getDispCondType1() => 
        this.dispCondType1;

    public int getDispCondType2() => 
        this.dispCondType2;

    public int getDispTargetId1() => 
        this.dispTargetId1;

    public int getDispTargetId2() => 
        this.dispTargetId2;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int getSpotId() => 
        this.id;

    public int getWarId() => 
        this.warId;

    public enum enSpotCondType
    {
        INVALID = 4,
        MISSION_ACHIEVE = 5,
        NONE = 1,
        NOT_QUEST_CLEAR = 3,
        QUEST_CLEAR = 2
    }
}

