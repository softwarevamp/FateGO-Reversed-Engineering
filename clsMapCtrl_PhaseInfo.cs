using System;

public class clsMapCtrl_PhaseInfo
{
    private int mPhase;
    private int mQuestId;
    private QuestPhaseMaster mQuestPhaseMaster;

    private QuestPhaseMaster GetQuestPhaseMaster()
    {
        if (this.mQuestPhaseMaster == null)
        {
            this.mQuestPhaseMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestPhaseMaster>(DataNameKind.Kind.QUEST_PHASE);
        }
        return this.mQuestPhaseMaster;
    }

    public QuestPhaseEntity mfGetMine() => 
        this.GetQuestPhaseMaster().getEntityFromId<QuestPhaseEntity>(this.mQuestId, this.mPhase);

    public void mfReset()
    {
    }

    public void mfSetMine(int quest_id, int phase)
    {
        this.mQuestId = quest_id;
        this.mPhase = phase;
    }
}

