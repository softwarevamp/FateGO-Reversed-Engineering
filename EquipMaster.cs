using System;

public class EquipMaster : DataMasterBase
{
    public EquipMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EQUIP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EquipEntity[1]);
        }
    }

    public static string getEquipName(int equipId)
    {
        EquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(equipId);
        if (entity != null)
        {
            return entity.name;
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EquipEntity>(obj);
}

