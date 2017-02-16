using System;

public class ClassRelationMaster : DataMasterBase
{
    public ClassRelationMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.CLASS_RELATION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ClassRelationEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ClassRelationEntity>(obj);

    public static float getRate(int atk, int def)
    {
        ClassRelationMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ClassRelationMaster>(DataNameKind.Kind.CLASS_RELATION);
        if (master == null)
        {
            return 1f;
        }
        ClassRelationEntity entity = master.getEntityFromId<ClassRelationEntity>(atk, def);
        return ((entity != null) ? entity.getRate() : 1f);
    }
}

