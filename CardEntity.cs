using System;

public class CardEntity : DataEntityBase
{
    public int addAtk;
    public int addCritical;
    public int addTdGauge;
    public int adjustAtk;
    public int adjustCritical;
    public int adjustTdGauge;
    public int id;
    public int[] individuality;
    public int num;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.num };
        return string.Concat(objArray1);
    }
}

