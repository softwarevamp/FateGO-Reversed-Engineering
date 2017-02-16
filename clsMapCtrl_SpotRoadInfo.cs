using System;

public class clsMapCtrl_SpotRoadInfo
{
    private enDispType meDispType;
    private int mSpotRoadId;
    private SpotRoadMaster mSpotRoadMaster;

    private SpotRoadMaster GetSpotRoadMaster()
    {
        if (this.mSpotRoadMaster == null)
        {
            this.mSpotRoadMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SpotRoadMaster>(DataNameKind.Kind.SPOT_ROAD);
        }
        return this.mSpotRoadMaster;
    }

    public enDispType mfGetDispType() => 
        this.meDispType;

    public SpotRoadEntity mfGetMine() => 
        this.GetSpotRoadMaster().getEntityFromId<SpotRoadEntity>(this.mSpotRoadId);

    public void mfReset()
    {
        this.meDispType = enDispType.None;
    }

    public void mfSetDispType(enDispType eDispType)
    {
        this.meDispType = eDispType;
    }

    public void mfSetMine(int spot_road_id)
    {
        this.mSpotRoadId = spot_road_id;
    }

    public enum enDispType
    {
        None,
        Normal,
        Glay
    }
}

