using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class FriendshipGauge : MonoBehaviour
{
    private int friendMaxRank = -1;
    private int friendshipId;
    public UISprite[] gaugeIcons;
    private GaugeData nextGauge;
    private GaugeData nowGauge;
    private int prevPoint = -1;
    private int prevRank = -1;
    private FriendshipEntity tmp_friendshipEnt;

    public int changeGauge(float val, out bool max, out bool isLevelUp, out bool isChange)
    {
        max = false;
        isLevelUp = false;
        isChange = false;
        FriendshipMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FriendshipMaster>(DataNameKind.Kind.FRIENDSHIP);
        int friendship = Mathf.FloorToInt(Mathf.Lerp((float) this.nowGauge.friendship, (float) this.nextGauge.friendship, val));
        int friendshipRank = master.getFriendShipRank(this.friendshipId, friendship, this.nowGauge.friendshipRank);
        if (friendshipRank == this.friendMaxRank)
        {
            max = true;
        }
        if (friendshipRank != this.prevRank)
        {
            isLevelUp = true;
            this.prevRank = friendshipRank;
        }
        if (friendship != this.prevPoint)
        {
            isChange = true;
            this.prevPoint = friendship;
        }
        this.changeGaugeData(this.friendshipId, friendship, friendshipRank);
        return (friendship - this.nowGauge.friendship);
    }

    public void changeGaugeData(int friendshipId, int friendship, int friendshipRank)
    {
        FriendshipMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FriendshipMaster>(DataNameKind.Kind.FRIENDSHIP);
        int num = 0;
        for (int i = 0; i < this.gaugeIcons.Length; i++)
        {
            FriendshipEntity entity = master.getEntityFromId<FriendshipEntity>(friendshipId, i);
            if (entity.friendship <= friendship)
            {
                this.gaugeIcons[i].fillAmount = 1f;
            }
            else if ((friendship - num) <= 0)
            {
                this.gaugeIcons[i].fillAmount = 0f;
            }
            else
            {
                float num3 = friendship - num;
                float num4 = entity.friendship - num;
                this.gaugeIcons[i].fillAmount = num3 / num4;
            }
            num = entity.friendship;
        }
    }

    public bool isChange() => 
        (((this.nowGauge != null) && (this.nextGauge != null)) && (this.nowGauge.friendship != this.nextGauge.friendship));

    public void setGaugeData(int friendshipId, int friendship, int friendshipRank)
    {
        this.friendshipId = friendshipId;
        this.nowGauge = new GaugeData();
        this.nowGauge.friendshipRank = friendshipRank;
        this.nowGauge.friendship = friendship;
        this.prevRank = this.nowGauge.friendshipRank;
        this.prevPoint = this.nowGauge.friendship;
        this.changeGaugeData(friendshipId, friendship, friendshipRank);
    }

    public void setNextGaugeData(int friendshipId, int friendship, int friendshipRank)
    {
        this.nextGauge = new GaugeData();
        this.nextGauge.friendshipRank = friendshipRank;
        this.nextGauge.friendship = friendship;
        this.friendMaxRank = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FriendshipMaster>(DataNameKind.Kind.FRIENDSHIP).getRankMax(friendshipId);
    }

    private class GaugeData
    {
        public int friendship;
        public int friendshipRank;
    }
}

