using System;
using UnityEngine;

public class EventMissionItemListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UISprite addRangeSprite;
    protected UIAtlas baseAtlas;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;
    protected string baseSpriteName;
    [SerializeField]
    protected GameObject compInfo;
    [SerializeField]
    protected GameObject completedImg;
    [SerializeField]
    protected UISlider expBar;
    [SerializeField]
    protected UIIconLabel iconLabel;
    private bool isDispTime;
    [SerializeField]
    protected ItemIconComponent itemIcon;
    [SerializeField]
    protected GameObject lockImgInfo;
    [SerializeField]
    protected UILabel missionCondLb;
    [SerializeField]
    protected UILabel missionNoLb;
    [SerializeField]
    protected UILabel missionProgressLb;
    [SerializeField]
    protected NewIconComponent newIcon;
    private long oldTime;
    [SerializeField]
    protected UISprite rangeSprite;
    [SerializeField]
    protected GameObject receiveImg;
    [SerializeField]
    protected UILabel resTimeLb;
    [SerializeField]
    protected GameObject rewardGetInfo;
    [SerializeField]
    protected GameObject rewardImg;
    [SerializeField]
    protected GameObject rewardStatusInfo;
    public static readonly int TIME_UPDATE_ITVL_SEC = 60;
    [SerializeField]
    protected GameObject timeOverInfo;
    [SerializeField]
    protected UILabel timeOverLb;

    protected void Awake()
    {
        if (this.baseSprite != null)
        {
            this.baseAtlas = this.baseSprite.atlas;
            this.baseSpriteName = this.baseSprite.spriteName;
        }
    }

    private static string GetRestTimeText(long end_time) => 
        (LocalizationManager.Get("TIME_REST_QUEST") + LocalizationManager.GetRestTime(end_time));

    public void SetInput(EventMissionItemListViewItem item)
    {
        if (item != null)
        {
            if (this.baseButton != null)
            {
                string str;
                float num;
                this.baseButton.SetColliderEnable(true, true);
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                this.setRewardStatusImg(item.CurrentStatus);
                this.missionCondLb.text = item.CondMsg;
                if (item.GetProgInfo(out str, out num))
                {
                    this.missionProgressLb.text = str;
                    this.expBar.value = num;
                }
                if (this.newIcon != null)
                {
                    if (item.IsNew && this.isDispTime)
                    {
                        this.newIcon.Set();
                    }
                    else
                    {
                        this.newIcon.Clear();
                    }
                }
            }
        }
        else
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(false);
            }
            if (this.addRangeSprite != null)
            {
                this.addRangeSprite.gameObject.SetActive(false);
            }
        }
    }

    public void SetItem(EventMissionItemListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(false);
            }
            if (this.addRangeSprite != null)
            {
                this.addRangeSprite.gameObject.SetActive(false);
            }
        }
        else
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(mode == DispMode.INVISIBLE);
            }
            if (this.addRangeSprite != null)
            {
                this.addRangeSprite.gameObject.SetActive(item.IsTerminationSpace);
            }
            if (mode != DispMode.INVISIBLE)
            {
                string str;
                float num;
                this.rewardStatusInfo.SetActive(true);
                this.rewardImg.SetActive(false);
                this.receiveImg.SetActive(false);
                this.rewardGetInfo.SetActive(false);
                this.compInfo.SetActive(false);
                this.completedImg.gameObject.SetActive(false);
                this.timeOverInfo.SetActive(false);
                this.lockImgInfo.SetActive(false);
                this.iconLabel.Set(IconLabelInfo.IconKind.ID, item.DispNo, 0, 0, 0L, false, false);
                this.missionCondLb.text = item.CondMsg;
                if (item.eventRewardType == RewardType.Type.GIFT)
                {
                    this.itemIcon.SetGift(item.Type, item.RewardObjId, -1);
                }
                if (item.eventRewardType == RewardType.Type.SET)
                {
                    this.itemIcon.SetItemImage((ImageItem.Id) item.SetExtraIconId);
                }
                this.setRewardStatusImg(item.CurrentStatus);
                if (item.GetProgInfo(out str, out num))
                {
                    this.missionProgressLb.text = str;
                    this.expBar.value = num;
                }
                long num2 = NetworkManager.getTime();
                bool flag = false;
                this.isDispTime = (num2 >= item.EventMissionEntity.startedAt) && (num2 < item.EventMissionEntity.endedAt);
                if (this.isDispTime)
                {
                    string restTimeText = GetRestTimeText(item.EventMissionEntity.endedAt);
                    this.resTimeLb.text = restTimeText;
                    this.oldTime = 0L;
                }
                else if ((item.CurrentStatus != EventMissionItemListViewItem.ProgStatus.CLEAR) && (item.CurrentStatus != EventMissionItemListViewItem.ProgStatus.ACHIVE))
                {
                    flag = true;
                    item.IsEndMission = true;
                }
                this.timeOverInfo.SetActive(flag);
                this.resTimeLb.gameObject.SetActive(this.isDispTime);
                if (this.newIcon != null)
                {
                    if (item.IsNew && this.isDispTime)
                    {
                        this.newIcon.Set();
                    }
                    else
                    {
                        this.newIcon.Clear();
                    }
                }
                if (((item.CurrentStatus == EventMissionItemListViewItem.ProgStatus.LOCK) || (item.CurrentStatus == EventMissionItemListViewItem.ProgStatus.NOSTART)) && this.isDispTime)
                {
                    this.lockImgInfo.SetActive(true);
                }
                if (this.baseButton != null)
                {
                    this.baseButton.SetColliderEnable(true, true);
                    this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                }
            }
        }
    }

    private void setRewardStatusImg(EventMissionItemListViewItem.ProgStatus status)
    {
        switch (status)
        {
            case EventMissionItemListViewItem.ProgStatus.PROGRESS:
                this.rewardImg.SetActive(true);
                this.receiveImg.SetActive(false);
                this.rewardImg.SetActive(true);
                this.lockImgInfo.SetActive(false);
                break;

            case EventMissionItemListViewItem.ProgStatus.CLEAR:
                this.receiveImg.SetActive(true);
                this.rewardImg.SetActive(false);
                this.compInfo.SetActive(true);
                this.completedImg.gameObject.SetActive(true);
                this.lockImgInfo.SetActive(false);
                break;

            case EventMissionItemListViewItem.ProgStatus.ACHIVE:
                this.rewardStatusInfo.SetActive(false);
                this.rewardGetInfo.SetActive(true);
                this.compInfo.SetActive(true);
                this.completedImg.gameObject.SetActive(true);
                break;

            case EventMissionItemListViewItem.ProgStatus.END:
                this.rewardStatusInfo.SetActive(false);
                this.rewardGetInfo.SetActive(false);
                break;

            default:
                this.rewardImg.SetActive(true);
                this.receiveImg.SetActive(false);
                this.rewardImg.SetActive(true);
                break;
        }
    }

    public void UpdateItem(EventMissionItemListViewItem item, DispMode mode)
    {
        if (((item != null) && (item.EventMissionEntity != null)) && (mode != DispMode.INVISIBLE))
        {
            EventMissionEntity eventMissionEntity = item.EventMissionEntity;
            if (this.resTimeLb.gameObject.activeSelf)
            {
                long num = NetworkManager.getTime();
                long num2 = num - this.oldTime;
                if (num2 >= TIME_UPDATE_ITVL_SEC)
                {
                    this.resTimeLb.text = GetRestTimeText(eventMissionEntity.endedAt);
                    this.oldTime = num;
                }
            }
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

