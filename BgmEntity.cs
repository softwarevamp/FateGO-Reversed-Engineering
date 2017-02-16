using System;

public class BgmEntity : DataEntityBase
{
    public string fileName;
    public int id;

    public BgmEntity()
    {
    }

    public BgmEntity(BgmEntity cSrc)
    {
        this.id = cSrc.id;
        this.fileName = cSrc.fileName;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

