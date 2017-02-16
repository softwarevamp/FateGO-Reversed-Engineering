using System;

public class TblUserEntity : DataEntityBase
{
    public int friendPoint;
    public long userId;

    public override string getPrimarykey() => 
        (string.Empty + this.userId);

    public override void printDebug()
    {
        base.printDebug();
        Debug.Log("TblUserEntity:");
    }
}

