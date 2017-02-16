using System;

public class clsMapCtrl_MapGimmickInfo
{
    private enDispType meDispType;
    private int mMapGimmickId;
    private MapGimmickMaster mMapGimmickMaster;

    private MapGimmickMaster GetMapGimmickMaster()
    {
        if (this.mMapGimmickMaster == null)
        {
            this.mMapGimmickMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<MapGimmickMaster>(DataNameKind.Kind.MAP_GIMMICK);
        }
        return this.mMapGimmickMaster;
    }

    public enDispType mfGetDispType() => 
        this.meDispType;

    public MapGimmickEntity mfGetMine() => 
        this.GetMapGimmickMaster().getEntityFromId<MapGimmickEntity>(this.mMapGimmickId);

    public void mfReset()
    {
        this.meDispType = enDispType.None;
    }

    public void mfSetDispType(enDispType eDispType)
    {
        this.meDispType = eDispType;
    }

    public void mfSetMine(int map_gimmick_id)
    {
        this.mMapGimmickId = map_gimmick_id;
    }

    public enum enDispType
    {
        None,
        Normal
    }
}

