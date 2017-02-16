using System;

public class ServantLimitAddEntity : DataEntityBase
{
    public int battleCharaId;
    public int battleCharaLimitCount;
    public int battleCharaOffsetX;
    public int battleCharaOffsetY;
    public int limitCount;
    public int svtId;
    public int svtVoiceId;
    public int voicePrefix;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.svtId, ":", this.limitCount };
        return string.Concat(objArray1);
    }
}

