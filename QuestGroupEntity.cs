using System;

public class QuestGroupEntity : DataEntityBase
{
    public int groupId;
    public int questId;
    public int type;

    public override string getPrimarykey()
    {
        string[] textArray1 = new string[] { string.Empty, this.questId.ToString(), ":", this.type.ToString(), ":", this.groupId.ToString() };
        return string.Concat(textArray1);
    }
}

