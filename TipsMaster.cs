using System;

public class TipsMaster : DataMasterBase
{
    public TipsMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.TIPS);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new TipsEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<TipsEntity>(obj);

    public int getSum() => 
        base.list.Count;
}

