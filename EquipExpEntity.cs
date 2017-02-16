using System;

public class EquipExpEntity : DataEntityBase
{
    public int equipId;
    public int exp;
    public int lv;
    public int skillLv1;
    public int skillLv2;
    public int skillLv3;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.equipId, ":", this.lv };
        return string.Concat(objArray1);
    }
}

