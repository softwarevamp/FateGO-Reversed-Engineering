using System;

public class EquipSkillEntity : DataEntityBase
{
    public int condLv;
    public int equipId;
    public int num;
    public int skillId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.equipId, ":", this.num };
        return string.Concat(objArray1);
    }

    public int getSkillId() => 
        this.skillId;

    public bool isUse(int lv)
    {
        if (this.condLv > lv)
        {
            return false;
        }
        return true;
    }
}

