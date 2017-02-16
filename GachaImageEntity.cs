using System;

public class GachaImageEntity : DataEntityBase
{
    public int areaId;
    public int gachaId;
    public int imageId;
    public GachaData showSvt;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.gachaId, ":", this.areaId };
        return string.Concat(objArray1);
    }
}

