using System;
using UnityEngine;

public class BattleResultBondsIconComponent : MonoBehaviour
{
    public UILabel atlabel;
    public ServantFaceIconComponent faceIcon;
    private int friendship;
    private int friendshipId;
    private int friendshipRank;
    public FriendshipGauge gaugeComponent;
    private bool isHeroine;
    private bool isUse;
    public GameObject levelUpObject;
    private int maxLimitCount;
    public UISprite maxSprite;
    private int nextFriendShipRank;
    public GameObject root;
    public GameObject rootNot;
    private int svtId;
    private int svtLimit;
    private int svtLv;
    private long userId;

    public bool changeGauge(float val)
    {
        bool flag;
        bool flag2;
        bool flag3;
        if (!this.isUse)
        {
            return false;
        }
        int num = this.gaugeComponent.changeGauge(val, out flag, out flag2, out flag3);
        if (flag)
        {
            this.maxSprite.gameObject.SetActive(true);
            if (this.atlabel != null)
            {
                this.atlabel.gameObject.SetActive(false);
            }
        }
        else if (this.atlabel != null)
        {
            this.maxSprite.gameObject.SetActive(false);
            this.atlabel.gameObject.SetActive(true);
            if (this.isHeroine)
            {
                this.atlabel.text = "ーーー";
            }
            else
            {
                this.atlabel.text = "＋ " + num;
            }
        }
        if (flag2)
        {
            this.levelUpObject.SetActive(true);
            TweenPosition component = this.levelUpObject.GetComponent<TweenPosition>();
            component.tweenFactor = 0f;
            component.Play();
        }
        return flag3;
    }

    public int getLv() => 
        this.svtLv;

    public int getMaxLimitCount() => 
        this.maxLimitCount;

    public int getNextFriendShipRank() => 
        this.nextFriendShipRank;

    public int getPrevFriendShipRank() => 
        this.friendshipRank;

    public int getSvtId() => 
        this.svtId;

    public int getSvtLimitCount() => 
        this.svtLimit;

    public long getUserId() => 
        this.userId;

    public bool isChangeRank()
    {
        if (!this.isUse)
        {
            return false;
        }
        return (this.friendshipRank != this.nextFriendShipRank);
    }

    public void setHeroine()
    {
        this.isHeroine = true;
    }

    public bool setNextServantData(UserServantCollectionEntity userSvtCol)
    {
        if (!this.isUse)
        {
            return false;
        }
        this.gaugeComponent.setNextGaugeData(this.friendshipId, userSvtCol.friendship, userSvtCol.friendshipRank);
        this.nextFriendShipRank = userSvtCol.friendshipRank;
        return this.gaugeComponent.isChange();
    }

    public void setServantData(UserServantCollectionEntity userSvtCol, UserServantEntity usetSvtEnt)
    {
        if ((userSvtCol == null) || (usetSvtEnt == null))
        {
            this.isUse = false;
            this.root.SetActive(false);
            this.rootNot.SetActive(true);
        }
        else
        {
            this.isUse = true;
            this.isHeroine = false;
            this.rootNot.SetActive(false);
            this.root.SetActive(true);
            this.levelUpObject.SetActive(false);
            this.faceIcon.Set(usetSvtEnt, null);
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(userSvtCol.svtId);
            if (entity != null)
            {
                this.userId = userSvtCol.userId;
                this.svtId = userSvtCol.svtId;
                this.svtLimit = usetSvtEnt.limitCount;
                this.svtLv = usetSvtEnt.lv;
                this.friendshipId = entity.friendshipId;
                this.friendship = userSvtCol.friendship;
                this.friendshipRank = userSvtCol.friendshipRank;
                this.maxLimitCount = userSvtCol.maxLimitCount;
            }
            this.gaugeComponent.setGaugeData(this.friendshipId, this.friendship, this.friendshipRank);
        }
    }
}

