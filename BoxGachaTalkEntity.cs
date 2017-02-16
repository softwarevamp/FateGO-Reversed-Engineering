using System;

public class BoxGachaTalkEntity : DataEntityBase
{
    public string afterDetail;
    public int afterFace;
    public string beforeDetail;
    public int beforeFace;
    public int id;
    public bool isRare;
    public int no;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.no };
        return string.Concat(objArray1);
    }
}

