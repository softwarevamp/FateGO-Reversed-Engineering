using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class clsMapCtrl_QuestInfo
{
    private int mApCalcVal;
    public AreaBoardInfo mAreaBoardInfo;
    private List<clsMapCtrl_PhaseInfo> mcPhaseInfos;
    private enDispType meDispType;
    private long mEndTime;
    private enTouchType meTouchType;
    private int miPhaseCnt;
    private int miPhaseCount;
    private int miQuestPhase;
    public clsMapCtrl_SpotInfo mMapCtrl_SpotInfo;
    public clsMapCtrl_WarInfo mMapCtrl_WarInfo;
    private int mQuestId;
    private QuestMaster mQuestMaster;
    private int mQuestReleaseClosedID;
    private int mQuestReleaseTargetID;
    private CondType.Kind mQuestReleaseType;
    private int mQuestReleaseValue;
    private List<QuestReleaseEntity> mReleaseNgListP;
    private List<QuestReleaseEntity> mReleaseOkListP = new List<QuestReleaseEntity>();
    private bool mtIsNew;
    private int mWarID;
    private List<int> sameGroupQuestIds;

    public clsMapCtrl_QuestInfo()
    {
        this.mReleaseOkListP.Clear();
        this.mReleaseNgListP = new List<QuestReleaseEntity>();
        this.mReleaseNgListP.Clear();
        this.mcPhaseInfos = new List<clsMapCtrl_PhaseInfo>();
        this.mcPhaseInfos.Clear();
        this.miPhaseCnt = 0;
    }

    public void AddSameGroupQuestIds(int[] questIds)
    {
        if (questIds != null)
        {
            int length = questIds.Length;
            if (length > 0)
            {
                if (this.sameGroupQuestIds == null)
                {
                    this.sameGroupQuestIds = new List<int>();
                }
                for (int i = 0; i < length; i++)
                {
                    int item = questIds[i];
                    if ((item != this.mQuestId) && !this.sameGroupQuestIds.Contains(item))
                    {
                        this.sameGroupQuestIds.Add(item);
                    }
                }
            }
        }
    }

    public int GetApCalcVal() => 
        this.mApCalcVal;

    public long GetEndTime() => 
        this.mEndTime;

    private QuestMaster GetQuestMaster()
    {
        if (this.mQuestMaster == null)
        {
            this.mQuestMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST);
        }
        return this.mQuestMaster;
    }

    public List<int> GetSameGroupQuestIds() => 
        this.sameGroupQuestIds;

    public bool IsClear() => 
        CondType.IsQuestClear(this.mQuestId, -1, false);

    public clsMapCtrl_PhaseInfo mfAddChild(int quest_id, int phase)
    {
        clsMapCtrl_PhaseInfo item = new clsMapCtrl_PhaseInfo();
        item.mfSetMine(quest_id, phase);
        this.mcPhaseInfos.Add(item);
        this.miPhaseCnt++;
        return item;
    }

    public List<clsMapCtrl_PhaseInfo> mfGetChildListsP()
    {
        if (this.mcPhaseInfos != null)
        {
            return this.mcPhaseInfos;
        }
        return null;
    }

    public enDispType mfGetDispType() => 
        this.meDispType;

    public QuestEntity mfGetMine() => 
        this.GetQuestMaster().getEntityFromId<QuestEntity>(this.mQuestId);

    public int mfGetPhaseMax() => 
        this.mcPhaseInfos.Count;

    public int mfGetQuestID() => 
        this.mQuestId;

    public int mfGetQuestPhase() => 
        this.miQuestPhase;

    public QuestEntity.enType mfGetQuestType() => 
        ((QuestEntity.enType) this.mfGetMine().getQuestType());

    public List<QuestReleaseEntity> mfGetReleaseNgListP()
    {
        if (this.mReleaseNgListP != null)
        {
            return this.mReleaseNgListP;
        }
        return null;
    }

    public List<QuestReleaseEntity> mfGetReleaseOkListP()
    {
        if (this.mReleaseOkListP != null)
        {
            return this.mReleaseOkListP;
        }
        return null;
    }

    public int mfGetSpotID() => 
        this.mfGetMine().getSpotId();

    public enTouchType mfGetTouchType() => 
        this.meTouchType;

    public int mfGetWarID() => 
        this.mWarID;

    public bool mfIsNew() => 
        this.mtIsNew;

    public void mfReset()
    {
        if (this.mReleaseOkListP != null)
        {
            this.mReleaseOkListP.Clear();
        }
        if (this.mReleaseNgListP != null)
        {
            this.mReleaseNgListP.Clear();
        }
        if (this.mcPhaseInfos != null)
        {
            for (int i = 0; i < this.mcPhaseInfos.Count; i++)
            {
                this.mcPhaseInfos[i].mfReset();
            }
            this.mcPhaseInfos.Clear();
            this.miPhaseCnt = 0;
        }
    }

    public void mfSetDispType(enDispType eDispType, CondType.Kind quest_release_type = 0, int quest_release_target_id = 0, int quest_release_value = 0, int quest_release_closed_id = 0)
    {
        this.meDispType = eDispType;
        this.mQuestReleaseType = quest_release_type;
        this.mQuestReleaseTargetID = quest_release_target_id;
        this.mQuestReleaseValue = quest_release_value;
        this.mQuestReleaseClosedID = quest_release_closed_id;
    }

    public void mfSetIsNew(bool tIsNew)
    {
        this.mtIsNew = tIsNew;
    }

    public void mfSetMine(int quest_id)
    {
        this.mQuestId = quest_id;
    }

    public void mfSetQuestPhase(int iQuestPhase)
    {
        this.miQuestPhase = iQuestPhase;
    }

    public void mfSetTouchType(enTouchType eTouchType)
    {
        this.meTouchType = eTouchType;
    }

    public void mfSetWarID(int war_id)
    {
        this.mWarID = war_id;
    }

    public void SetApCalcVal(int val)
    {
        this.mApCalcVal = val;
    }

    public void SetEndTime(long val)
    {
        this.mEndTime = val;
    }

    public int groupId { get; set; }

    public int QuestReleaseClosedID =>
        this.mQuestReleaseClosedID;

    public int QuestReleaseTargetID =>
        this.mQuestReleaseTargetID;

    public CondType.Kind QuestReleaseType =>
        this.mQuestReleaseType;

    public int QuestReleaseValue =>
        this.mQuestReleaseValue;

    public enum enDispType
    {
        None,
        Normal,
        Closed
    }

    public enum enTouchType
    {
        Disable,
        Enable
    }
}

