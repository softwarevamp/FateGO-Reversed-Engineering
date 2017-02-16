using System;

public class ConstantMaster : DataMasterBase
{
    public ConstantMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.CONSTANT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ConstantEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ConstantEntity>(obj);

    public static float getRateValue(string name) => 
        (((float) getValue(name)) / 1000f);

    public static int getValue(string name)
    {
        ConstantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ConstantMaster>(DataNameKind.Kind.CONSTANT).getEntityFromKey<ConstantEntity>(name);
        if (entity != null)
        {
            return entity.value;
        }
        return -1;
    }

    public int GetValue(string name)
    {
        ConstantEntity entity = base.getEntityFromKey<ConstantEntity>(name);
        if (entity != null)
        {
            return entity.value;
        }
        return -1;
    }
}

