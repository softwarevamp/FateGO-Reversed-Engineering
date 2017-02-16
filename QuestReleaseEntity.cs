using System;

public class QuestReleaseEntity : DataEntityBase
{
    public int closedMessageId;
    public int imagePriority;
    public int questId;
    public int targetId;
    public int type;
    public int value;

    public int getClosedMessageId() => 
        this.closedMessageId;

    public int getImagePriority() => 
        this.imagePriority;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.questId, ":", this.type, ":", this.targetId };
        return string.Concat(objArray1);
    }

    public int getQuestId() => 
        this.questId;

    public int getTargetId() => 
        this.targetId;

    public int getType() => 
        this.type;

    public int getValue() => 
        this.value;
}

