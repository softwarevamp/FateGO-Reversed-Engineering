using System;

public class AreaEntity : DataEntityBase
{
    public int id;
    public string name;
    public int worldId;
    public int x;
    public int y;

    public int getAreaId() => 
        this.id;

    public int getAreaPosX() => 
        this.x;

    public int getAreaPosY() => 
        this.y;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int getWorldId() => 
        this.worldId;
}

