using System;

public class AccessaryEntity : DataEntityBase
{
    public int abilityId;
    public int adjustAtk;
    public int adjustHp;
    public int condLv;
    public int condSvtId;
    public string detail;
    public int id;
    public int imageId;
    public string name;
    public int rariry;
    public int sellCoin;
    public int skillId;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

