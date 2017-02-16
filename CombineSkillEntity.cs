using System;

public class CombineSkillEntity : DataEntityBase
{
    public int id;
    public int[] itemIds;
    public int[] itemNums;
    public int qp;
    public int skillLv;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.skillLv };
        return string.Concat(objArray1);
    }
}

