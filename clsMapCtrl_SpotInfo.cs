using System;
using System.Collections.Generic;

public class clsMapCtrl_SpotInfo
{
    private List<clsMapCtrl_QuestInfo> mcQuestInfos = new List<clsMapCtrl_QuestInfo>();
    private enDispType meDispType;
    private enTouchType meTouchType;
    private int miQuestcount;
    private int mSpotId;
    private SpotMaster mSpotMaster;

    public clsMapCtrl_SpotInfo()
    {
        this.mcQuestInfos.Clear();
    }

    private SpotMaster GetSpotMaster()
    {
        if (this.mSpotMaster == null)
        {
            this.mSpotMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SpotMaster>(DataNameKind.Kind.SPOT);
        }
        return this.mSpotMaster;
    }

    public bool IsNextDisp()
    {
        if (this.mfGetDispType() == enDispType.Normal)
        {
            foreach (clsMapCtrl_QuestInfo info in this.mfGetChildListsP())
            {
                if (((info.mfGetQuestType() == QuestEntity.enType.MAIN) && (info.mfGetDispType() == clsMapCtrl_QuestInfo.enDispType.Normal)) && !SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(info.mfGetQuestID(), false))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public clsMapCtrl_QuestInfo mfAddChild(int quest_id)
    {
        clsMapCtrl_QuestInfo item = new clsMapCtrl_QuestInfo();
        item.mfSetMine(quest_id);
        this.mcQuestInfos.Add(item);
        return item;
    }

    public List<clsMapCtrl_QuestInfo> mfGetChildListsP()
    {
        if (this.mcQuestInfos != null)
        {
            return this.mcQuestInfos;
        }
        return null;
    }

    public enDispType mfGetDispType() => 
        this.meDispType;

    public SpotEntity mfGetMine() => 
        this.GetSpotMaster().getEntityFromId<SpotEntity>(this.mSpotId);

    public int mfGetQuestcount() => 
        this.miQuestcount;

    public int mfGetSpotID() => 
        this.mSpotId;

    public enTouchType mfGetTouchType() => 
        this.meTouchType;

    public int mfGetWarID() => 
        this.mfGetMine().getWarId();

    public void mfReset()
    {
        if (this.mcQuestInfos != null)
        {
            for (int i = 0; i < this.mcQuestInfos.Count; i++)
            {
                this.mcQuestInfos[i].mfReset();
            }
            this.mcQuestInfos.Clear();
        }
    }

    public void mfSetDispType(enDispType eDispType)
    {
        this.meDispType = eDispType;
    }

    public void mfSetMine(int spot_id)
    {
        this.mSpotId = spot_id;
    }

    public void mfSetQuestcount(int iQuestcount)
    {
        this.miQuestcount = iQuestcount;
    }

    public void mfSetTouchType(enTouchType eTouchType)
    {
        this.meTouchType = eTouchType;
    }

    public enum enDispType
    {
        None,
        Normal,
        Glay
    }

    public enum enTouchType
    {
        Disable,
        Enable
    }
}

