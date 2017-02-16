using System;

public class EquipImageEntity : DataEntityBase
{
    public int id;
    public int offsetX;
    public int offsetXMyroom;
    public int offsetY;
    public int offsetYMyroom;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

