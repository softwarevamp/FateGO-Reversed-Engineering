using System;

public class ServantClassEntity : DataEntityBase
{
    public int attackRate;
    public int attri;
    public int groupType;
    public int iconImageId;
    public int id;
    public int imageId;
    public string name;
    public int priority;

    public float getAttackRate() => 
        (((float) this.attackRate) / 1000f);

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

