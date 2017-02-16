using System;

public class BuffMaster : DataMasterBase
{
    public BuffMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BUFF);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BuffEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BuffEntity>(obj);
}

