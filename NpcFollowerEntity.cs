using System;

public class NpcFollowerEntity : DataEntityBase
{
    public long id;
    public long leaderSvtId;
    public int questId;
    public int questPhase;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.questId, ":", this.questPhase };
        return string.Concat(objArray1);
    }
}

