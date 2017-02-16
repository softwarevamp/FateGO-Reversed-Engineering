using System;

public class EventUpValSetupInfo
{
    public EventDetailEntity eventDetailEntity;
    public int eventId;
    public int questId;
    public int questPhase;
    public QuestPhaseEntity questPhaseEntity;

    public EventUpValSetupInfo(int questId, int questPhase)
    {
        this.questId = questId;
        this.questPhase = questPhase;
        this.eventId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestGroupMaster>(DataNameKind.Kind.QUEST_GROUP).GetEventId(questId);
        this.questPhaseEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST_PHASE).getEntityFromId<QuestPhaseEntity>(questId, questPhase);
        if (this.eventId > 0)
        {
            this.eventDetailEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventDetailMaster>(DataNameKind.Kind.EVENT_DETAIL).getEntityFromId(this.eventId);
        }
    }

    public bool IsBonusSkill() => 
        ((this.eventDetailEntity != null) && this.eventDetailEntity.isBonusSkill);

    public bool IsUpVal(int[] questIndividualitieList) => 
        (((((this.questPhaseEntity != null) && (this.questPhaseEntity.individuality != null)) && ((this.questPhaseEntity.individuality.Length > 0) && (questIndividualitieList != null))) && (questIndividualitieList.Length > 0)) && Individuality.CheckIndividualities(questIndividualitieList, this.questPhaseEntity.individuality));
}

