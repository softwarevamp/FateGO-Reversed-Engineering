using System;

public class NpcDeckEntity : DataEntityBase
{
    public string enemyScript;
    public long hp;
    public long id;
    public int lv;
    public string name;
    public long npcSvtId;
    public int num;
    public int probability;
    public int roleType;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.npcSvtId, ":", this.num };
        return string.Concat(objArray1);
    }
}

