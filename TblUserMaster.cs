using System;

public class TblUserMaster : DataMasterBase
{
    public TblUserMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.TBL_USER_GAME);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new TblUserEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<TblUserEntity>(obj);

    public TblUserEntity getUserData(long userId) => 
        base.getEntityFromId<TblUserEntity>(userId);
}

