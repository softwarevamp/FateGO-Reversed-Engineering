using System;

public class GameIllustrationMaster : DataMasterBase
{
    public GameIllustrationMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.GAME_ILLUSTRATION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new GameIllustrationEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<GameIllustrationEntity>(obj);
}

