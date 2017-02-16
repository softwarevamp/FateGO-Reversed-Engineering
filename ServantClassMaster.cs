using System;

public class ServantClassMaster : DataMasterBase
{
    public ServantClassMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_CLASS);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantClassEntity[1]);
        }
    }

    public static float getClassAtk(int classId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(classId).getAttackRate();

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantClassEntity>(obj);
}

