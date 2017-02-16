using System;

public class EquipEntity : DataEntityBase
{
    public int condUserLv;
    public string detail;
    public int femaleImageId;
    public int femaleSpellId;
    public int id;
    public int imageId;
    public int maleImageId;
    public int maleSpellId;
    public int maxLv;
    public string name;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

