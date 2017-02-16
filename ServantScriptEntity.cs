using System;

public class ServantScriptEntity : DataEntityBase
{
    public int faceX;
    public int faceY;
    public int limitCount;
    public int offsetX;
    public int offsetXMyroom;
    public int offsetY;
    public int offsetYMyroom;
    public int svtId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.svtId, ":", this.limitCount };
        return string.Concat(objArray1);
    }
}

