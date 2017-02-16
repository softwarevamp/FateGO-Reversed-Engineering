using System;

public class CvMaster : DataMasterBase
{
    public CvMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.CV);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new CvEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<CvEntity>(obj);
}

