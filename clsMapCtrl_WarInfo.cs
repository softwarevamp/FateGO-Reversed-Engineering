using System;
using System.Collections.Generic;

public class clsMapCtrl_WarInfo
{
    private List<clsMapCtrl_MapGimmickInfo> mcMapGimmickInfos;
    private List<clsMapCtrl_SpotInfo> mcSpotInfos = new List<clsMapCtrl_SpotInfo>();
    private List<clsMapCtrl_SpotRoadInfo> mcSpotRoadInfos;
    private enStatus meStatus;
    private int mWarId;
    private WarMaster mWarMaster;

    public clsMapCtrl_WarInfo()
    {
        this.mcSpotInfos.Clear();
        this.mcSpotRoadInfos = new List<clsMapCtrl_SpotRoadInfo>();
        this.mcSpotRoadInfos.Clear();
        this.mcMapGimmickInfos = new List<clsMapCtrl_MapGimmickInfo>();
        this.mcMapGimmickInfos.Clear();
    }

    public int GetEventId() => 
        this.mfGetMine().eventId;

    private WarMaster GetWarMaster()
    {
        if (this.mWarMaster == null)
        {
            this.mWarMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR);
        }
        return this.mWarMaster;
    }

    public clsMapCtrl_SpotInfo mfAddChild(int spot_id)
    {
        clsMapCtrl_SpotInfo item = new clsMapCtrl_SpotInfo();
        item.mfSetMine(spot_id);
        this.mcSpotInfos.Add(item);
        return item;
    }

    public clsMapCtrl_MapGimmickInfo mfAddMapGimmick(int map_gimmick_id)
    {
        clsMapCtrl_MapGimmickInfo item = new clsMapCtrl_MapGimmickInfo();
        item.mfSetMine(map_gimmick_id);
        this.mcMapGimmickInfos.Add(item);
        return item;
    }

    public clsMapCtrl_SpotRoadInfo mfAddSpotRoad(int spot_road_id)
    {
        clsMapCtrl_SpotRoadInfo item = new clsMapCtrl_SpotRoadInfo();
        item.mfSetMine(spot_road_id);
        this.mcSpotRoadInfos.Add(item);
        return item;
    }

    public List<clsMapCtrl_MapGimmickInfo> mfGetMapGimmickListsP()
    {
        if (this.mcMapGimmickInfos != null)
        {
            return this.mcMapGimmickInfos;
        }
        return null;
    }

    public WarEntity mfGetMine() => 
        this.GetWarMaster().getEntityFromId<WarEntity>(this.mWarId);

    public List<clsMapCtrl_SpotInfo> mfGetSpotListsP()
    {
        if (this.mcSpotInfos != null)
        {
            return this.mcSpotInfos;
        }
        return null;
    }

    public List<clsMapCtrl_SpotRoadInfo> mfGetSpotRoadListsP()
    {
        if (this.mcSpotRoadInfos != null)
        {
            return this.mcSpotRoadInfos;
        }
        return null;
    }

    public enStatus mfGetStatus() => 
        this.meStatus;

    public int mfGetWarID() => 
        this.mWarId;

    public void mfReset()
    {
        if (this.mcSpotInfos != null)
        {
            for (int i = 0; i < this.mcSpotInfos.Count; i++)
            {
                this.mcSpotInfos[i].mfReset();
            }
            this.mcSpotInfos.Clear();
        }
        if (this.mcSpotRoadInfos != null)
        {
            for (int j = 0; j < this.mcSpotRoadInfos.Count; j++)
            {
                this.mcSpotRoadInfos[j].mfReset();
            }
            this.mcSpotRoadInfos.Clear();
        }
        if (this.mcMapGimmickInfos != null)
        {
            for (int k = 0; k < this.mcMapGimmickInfos.Count; k++)
            {
                this.mcMapGimmickInfos[k].mfReset();
            }
            this.mcMapGimmickInfos.Clear();
        }
    }

    public void mfSetMine(int war_id)
    {
        this.mWarId = war_id;
    }

    public void mfSetStatus(enStatus eStatus)
    {
        this.meStatus = eStatus;
    }

    public enum enStatus
    {
        None,
        Normal,
        New,
        Clear,
        Complete
    }
}

