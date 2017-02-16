using System;

public class TelopMaster : DataMasterBase
{
    public TelopMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.TELOP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new TelopEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<TelopEntity>(obj);
}

