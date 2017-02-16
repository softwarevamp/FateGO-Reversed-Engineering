using System;

public class UserFollowerEntity : DataEntityBase
{
    public long expireAt;
    public FollowerInfo[] followerInfo;
    public long userId;

    public FollowerInfo getFollowerInfo(long followerId)
    {
        if (this.followerInfo != null)
        {
            for (int i = 0; i < this.followerInfo.Length; i++)
            {
                if (this.followerInfo[i].userId == followerId)
                {
                    return this.followerInfo[i];
                }
            }
        }
        return null;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.userId);

    public bool isEnableData() => 
        (NetworkManager.getTime() <= this.expireAt);
}

