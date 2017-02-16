using System;

public class FollowerListRequest : RequestBase
{
    public override bool checkExpirationDate()
    {
        DataMasterBase base2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_FOLLOWER);
        return (base2.isSingleEntityExists() && base2.getSingleEntity<UserFollowerEntity>().isEnableData());
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);
}

