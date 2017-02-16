using System;

public class GameIllustrationEntity : DataEntityBase
{
    public string detial;
    public string labelName;
    public int num;
    public string titleName;
    public int type;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.type, ":", this.num };
        return string.Concat(objArray1);
    }
}

