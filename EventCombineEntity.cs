using System;

public class EventCombineEntity : DataEntityBase
{
    public int calcType;
    public int id;
    public int target;
    public int value;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.target };
        return string.Concat(objArray1);
    }

    public enum CalcType
    {
        ADDITION = 1,
        MULTIPLICATION = 2
    }
}

