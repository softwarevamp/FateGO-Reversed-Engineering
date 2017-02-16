using System;

public class BattleBgMaster : DataMasterBase
{
    public readonly int DefaultBgShadowImageId = 1;

    public BattleBgMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BATTLE_BG);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BattleBgEntity[1]);
        }
    }

    public int GetBgShadowImageId(int bgId, int bgType)
    {
        int defaultBgShadowImageId = this.DefaultBgShadowImageId;
        long[] args = new long[] { (long) bgId, (long) bgType };
        if (base.isEntityExistsFromId(args))
        {
            defaultBgShadowImageId = base.getEntityFromId<BattleBgEntity>(bgId, bgType).imageId;
        }
        return defaultBgShadowImageId;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BattleBgEntity>(obj);
}

