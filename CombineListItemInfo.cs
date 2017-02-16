using System;

public class CombineListItemInfo
{
    public string eventData;
    public int kind;
    public string spriteName;
    public int type;

    public CombineListItemInfo(int kind, int type, string spriteName, string eventData)
    {
        this.kind = kind;
        this.type = type;
        this.spriteName = spriteName;
        this.eventData = eventData;
    }
}

