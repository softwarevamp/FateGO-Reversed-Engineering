using System;

public class BattleMaster : DataMasterBase
{
    public BattleMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BATTLE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BattleEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BattleEntity>(obj);
}

