using System;

public class SpotRoadEntity : DataEntityBase
{
    public int activeCondType;
    public int activeTargetId;
    public int dispCondType;
    public int dispTargetId;
    public int dstSpotId;
    public int id;
    public int imageId;
    public int srcSpotId;
    public int type;
    public int warId;

    public SpotRoadEntity()
    {
    }

    public SpotRoadEntity(SpotRoadEntity cSrc)
    {
        this.id = cSrc.id;
        this.warId = cSrc.warId;
        this.srcSpotId = cSrc.srcSpotId;
        this.dstSpotId = cSrc.dstSpotId;
        this.type = cSrc.type;
        this.imageId = cSrc.imageId;
        this.dispCondType = cSrc.dispCondType;
        this.dispTargetId = cSrc.dispTargetId;
        this.activeCondType = cSrc.activeCondType;
        this.activeTargetId = cSrc.activeTargetId;
    }

    public int getActiveCondType() => 
        this.activeCondType;

    public int getActiveTargetId() => 
        this.activeTargetId;

    public int getDispCondType() => 
        this.dispCondType;

    public int getDispTargetId() => 
        this.dispTargetId;

    public int getDstSpotId() => 
        this.dstSpotId;

    public int getImageId() => 
        this.imageId;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int getSpotRoadId() => 
        this.id;

    public int getSrcSpotId() => 
        this.srcSpotId;

    public int getType() => 
        this.type;

    public int getWarId() => 
        this.warId;
}

