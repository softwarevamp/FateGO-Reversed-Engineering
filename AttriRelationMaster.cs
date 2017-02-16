using System;

public class AttriRelationMaster : DataMasterBase
{
    public AttriRelationMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.ATTRI_RELATION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new AttriRelationEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<AttriRelationEntity>(obj);

    public static float getRate(int atk, int def)
    {
        AttriRelationMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<AttriRelationMaster>(DataNameKind.Kind.ATTRI_RELATION);
        if (master == null)
        {
            return 1f;
        }
        AttriRelationEntity entity = master.getEntityFromId<AttriRelationEntity>(atk, def);
        return ((entity != null) ? entity.getRate() : 1f);
    }
}

