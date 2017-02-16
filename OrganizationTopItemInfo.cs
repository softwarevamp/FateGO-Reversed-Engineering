using System;

public class OrganizationTopItemInfo
{
    public string eventData;
    public string imageName;
    public int kind;
    public string textCode;

    public OrganizationTopItemInfo(int kind, string textCode, string imageName, string eventData)
    {
        this.kind = kind;
        this.textCode = textCode;
        this.imageName = imageName;
        this.eventData = eventData;
    }
}

