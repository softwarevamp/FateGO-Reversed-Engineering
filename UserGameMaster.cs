using System;

public class UserGameMaster : DataMasterBase
{
    public UserGameMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_GAME);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserGameEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserGameEntity>(obj);

    public static UserGameEntity getSelfUserGame() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserGameMaster>(DataNameKind.Kind.USER_GAME).getEntityFromId<UserGameEntity>(NetworkManager.UserId);
}

