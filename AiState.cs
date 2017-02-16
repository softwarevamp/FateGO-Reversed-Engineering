using System;

public class AiState
{
    public int actCount;
    public int aiGroupId;
    public int baseTurn;
    public int beforeActId = -1;
    public AiAct.TYPE beforeActType;
    public int firstAiGroupId;

    public void changeThinking(int groupId)
    {
        this.aiGroupId = groupId;
        this.actCount = 0;
    }

    public AiAct.TYPE getBeforeAiActType() => 
        this.beforeActType;

    public SaveData getSaveData() => 
        new SaveData { 
            firstAiGroupId = this.firstAiGroupId,
            aiGroupId = this.aiGroupId,
            baseTurn = this.baseTurn,
            actCount = this.actCount,
            beforeActType = this.beforeActType.getInt(),
            beforeActId = this.beforeActId
        };

    public void Initialize(int groupId)
    {
        this.firstAiGroupId = groupId;
        this.aiGroupId = this.firstAiGroupId;
        this.baseTurn = 0;
        this.actCount = 0;
    }

    public void setBeforeAction(AiAct.TYPE type, int actId)
    {
        this.beforeActType = type;
        this.beforeActId = actId;
    }

    public void setSaveData(SaveData sv)
    {
        this.firstAiGroupId = sv.firstAiGroupId;
        this.aiGroupId = sv.aiGroupId;
        this.baseTurn = sv.baseTurn;
        this.actCount = sv.actCount;
        this.beforeActType = AiAct.getType(sv.beforeActType);
        this.beforeActId = sv.beforeActId;
    }

    public class SaveData
    {
        public int actCount;
        public int aiGroupId;
        public int baseTurn;
        public int beforeActId;
        public int beforeActType;
        public int firstAiGroupId;
    }
}

