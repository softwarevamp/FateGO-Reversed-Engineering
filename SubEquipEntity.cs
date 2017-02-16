using System;

public class SubEquipEntity : DataEntityBase
{
    public int abilityId;
    public int abilityLv;
    public int condLv;
    public string detail;
    public int id;
    public int imageId;
    public string name;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

