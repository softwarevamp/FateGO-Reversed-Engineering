using System;

public class ServantCardEntity : DataEntityBase
{
    public int attackType;
    public int cardId;
    public int[] grandDamage;
    public int motion;
    public int[] normalDamage;
    public int[] singleDamage;
    public int svtId;
    public int[] trinityDamage;
    public int[] unisonDamage;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.svtId, ":", this.cardId };
        return string.Concat(objArray1);
    }

    public override void printDebug()
    {
        base.printDebug();
    }
}

