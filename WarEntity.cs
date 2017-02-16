using System;

public class WarEntity : DataEntityBase
{
    public string age;
    public string bgmId;
    public static readonly int CALDEAGATE_ID = 0x270f;
    public int eventId;
    public int headerImageId;
    public int id;
    public int lastQuestId;
    public string longName;
    public int mapImageH;
    public int mapImageId;
    public int mapImageW;
    public string name;
    public int priority;
    public string scriptId;
    public int status;

    public WarEntity()
    {
    }

    public WarEntity(WarEntity cSrc)
    {
        this.id = cSrc.id;
        this.age = cSrc.age;
        this.name = cSrc.name;
        this.mapImageId = cSrc.mapImageId;
        this.mapImageW = cSrc.mapImageW;
        this.mapImageH = cSrc.mapImageH;
        this.headerImageId = cSrc.headerImageId;
        this.priority = cSrc.priority;
        this.bgmId = cSrc.bgmId;
        this.scriptId = cSrc.scriptId;
        this.eventId = cSrc.eventId;
        this.lastQuestId = cSrc.lastQuestId;
        this.status = cSrc.status;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int getStatus() => 
        this.status;

    public int getWarId() => 
        this.id;

    public bool IsEvent() => 
        (this.eventId > 0);
}

