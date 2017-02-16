using System;
using UnityEngine;

public class EventItemComponent : MonoBehaviour
{
    [SerializeField]
    protected GameObject baseObject;
    [SerializeField]
    protected UISprite baseSp;
    protected int dataItemId;
    [SerializeField]
    protected UILabel dataLabel;
    protected int dataMissionEventId;
    protected int dataPointEventId;
    [SerializeField]
    protected ItemIconComponent itemIcon;
    [SerializeField]
    protected UILabel textLabel;

    public void Clear()
    {
        this.baseObject.SetActive(false);
        this.itemIcon.Clear();
        this.dataLabel.text = string.Empty;
        this.textLabel.text = string.Empty;
    }

    public void Set(int itemId)
    {
        this.dataItemId = itemId;
        if (itemId > 0)
        {
            Debug.Log("!!** !! EventItemComponent ItemId: " + itemId);
            this.baseObject.SetActive(true);
            this.baseSp.gameObject.SetActive(true);
            this.itemIcon.gameObject.SetActive(true);
            this.dataLabel.gameObject.SetActive(true);
            this.textLabel.gameObject.SetActive(false);
            this.itemIcon.SetItem(itemId, -1);
            UserItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, itemId);
            int data = (entity.num >= BalanceConfig.UserEventItemMax) ? BalanceConfig.UserEventItemMax : entity.num;
            this.dataLabel.text = LocalizationManager.GetNumberFormat(data);
        }
        else
        {
            this.Clear();
        }
    }

    public void SetMissionEvent(int eventId)
    {
        this.dataMissionEventId = eventId;
        if (eventId > 0)
        {
            this.baseObject.SetActive(true);
            this.baseSp.gameObject.SetActive(false);
            this.itemIcon.gameObject.SetActive(false);
            this.dataLabel.gameObject.SetActive(false);
            this.textLabel.gameObject.SetActive(true);
            EventMissionEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionMaster>(DataNameKind.Kind.EVENT_MISSION).getEventMissionList(eventId);
            this.textLabel.text = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEventMissionMaster>(DataNameKind.Kind.USER_EVENT_MISSION).getAchiveMissionNum(eventId).ToString() + "/" + entityArray.Length.ToString();
        }
        else
        {
            this.Clear();
        }
    }

    public void SetPointEvent(int eventId)
    {
        this.dataPointEventId = eventId;
        if (eventId > 0)
        {
            this.baseObject.SetActive(true);
            this.baseSp.gameObject.SetActive(true);
            this.itemIcon.gameObject.SetActive(true);
            this.dataLabel.gameObject.SetActive(true);
            this.textLabel.gameObject.SetActive(false);
            this.itemIcon.SetPointEvent(eventId);
            UserEventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEventMaster>(DataNameKind.Kind.USER_EVENT).getEntityFromId(NetworkManager.UserId, eventId);
            int data = (entity.value >= BalanceConfig.UserPointEventMax) ? BalanceConfig.UserPointEventMax : entity.value;
            this.dataLabel.text = LocalizationManager.GetNumberFormat(data);
        }
        else
        {
            this.Clear();
        }
    }

    public void UpdateDisp()
    {
        if (this.dataItemId > 0)
        {
            this.Set(this.dataItemId);
        }
        if (this.dataPointEventId > 0)
        {
            this.SetPointEvent(this.dataPointEventId);
        }
        if (this.dataMissionEventId > 0)
        {
            this.SetMissionEvent(this.dataMissionEventId);
        }
    }
}

