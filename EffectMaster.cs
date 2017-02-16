using System;

public class EffectMaster : DataMasterBase
{
    public EffectMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EFFECT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EffectEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EffectEntity>(obj);
}

