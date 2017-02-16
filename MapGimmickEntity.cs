using System;

public class MapGimmickEntity : DataEntityBase
{
    public int depthOffset;
    public int dispCondType;
    public int dispCondType2;
    public int dispTargetId;
    public int dispTargetId2;
    public long endedAt;
    public int id;
    public int imageId;
    public int scale;
    public long startedAt;
    public int warId;
    public int x;
    public int y;

    public MapGimmickEntity()
    {
    }

    public MapGimmickEntity(MapGimmickEntity cSrc)
    {
        this.id = cSrc.id;
        this.warId = cSrc.warId;
        this.imageId = cSrc.imageId;
        this.x = cSrc.x;
        this.y = cSrc.y;
        this.depthOffset = cSrc.depthOffset;
        this.dispCondType = cSrc.dispCondType;
        this.dispTargetId = cSrc.dispTargetId;
        this.dispCondType2 = cSrc.dispCondType2;
        this.dispTargetId2 = cSrc.dispTargetId2;
        this.startedAt = cSrc.startedAt;
        this.endedAt = cSrc.endedAt;
        this.scale = cSrc.scale;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public bool IsEnableTime(long time) => 
        ((time >= this.startedAt) && (time < this.endedAt));
}

