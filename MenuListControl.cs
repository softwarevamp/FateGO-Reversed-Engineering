using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuListControl : MonoBehaviour
{
    public CombineInitData combineData;
    private List<EventInfoData> combineEventList;
    public UIGrid menuListGrid;
    public UIScrollView menuListScrollView;
    private List<EventNoticeControl> noticeList;
    private List<SetNoticeNumControl> setNoticeNumList = new List<SetNoticeNumControl>();

    private void checkEventNotice()
    {
        EventEntity[] entityArray = this.combineData.getCombineEventList();
        this.combineEventList = new List<EventInfoData>();
        if (entityArray != null)
        {
            for (int i = 0; i < entityArray.Length; i++)
            {
                EventEntity eventEnt = entityArray[i];
                EventCampaignEntity combineEventEnt = this.combineData.getCombineEventData(eventEnt.id);
                if (combineEventEnt != null)
                {
                    Debug.Log("***!!! Event Combine Data : " + combineEventEnt.eventId);
                    if (combineEventEnt.target == 2)
                    {
                        EventInfoData data = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[0].setCombineEventData(data);
                        this.noticeList[0].gameObject.SetActive(true);
                        this.combineEventList.Add(data);
                    }
                    if (combineEventEnt.target == 1)
                    {
                        EventInfoData data2 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[0].setCombineEventData(data2);
                        this.noticeList[0].gameObject.SetActive(true);
                        this.combineEventList.Add(data2);
                    }
                    if (combineEventEnt.target == 4)
                    {
                        EventInfoData data3 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[0].setCombineEventData(data3);
                        this.noticeList[0].gameObject.SetActive(true);
                        this.combineEventList.Add(data3);
                    }
                    if (combineEventEnt.target == 5)
                    {
                        EventInfoData data4 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[0].setCombineEventData(data4);
                        this.noticeList[0].gameObject.SetActive(true);
                        this.combineEventList.Add(data4);
                    }
                    if (combineEventEnt.target == 6)
                    {
                        EventInfoData data5 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[1].setCombineEventData(data5);
                        this.noticeList[1].gameObject.SetActive(true);
                        this.combineEventList.Add(data5);
                    }
                    if (combineEventEnt.target == 8)
                    {
                        EventInfoData data6 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[2].setCombineEventData(data6);
                        this.noticeList[2].gameObject.SetActive(true);
                        this.combineEventList.Add(data6);
                    }
                    if (combineEventEnt.target == 10)
                    {
                        EventInfoData data7 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[3].setCombineEventData(data7);
                        this.noticeList[3].gameObject.SetActive(true);
                        this.combineEventList.Add(data7);
                    }
                    if (combineEventEnt.target == 0x10)
                    {
                        EventInfoData data8 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[4].setCombineEventData(data8);
                        this.noticeList[4].gameObject.SetActive(true);
                        this.combineEventList.Add(data8);
                    }
                    if (combineEventEnt.target == 0x11)
                    {
                        EventInfoData data9 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[4].setCombineEventData(data9);
                        this.noticeList[4].gameObject.SetActive(true);
                        this.combineEventList.Add(data9);
                    }
                    if (combineEventEnt.target == 0x12)
                    {
                        EventInfoData data10 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[4].setCombineEventData(data10);
                        this.noticeList[4].gameObject.SetActive(true);
                        this.combineEventList.Add(data10);
                    }
                    if (combineEventEnt.target == 0x13)
                    {
                        EventInfoData data11 = this.setEventInfo(combineEventEnt, eventEnt);
                        this.noticeList[4].setCombineEventData(data11);
                        this.noticeList[4].gameObject.SetActive(true);
                        this.combineEventList.Add(data11);
                    }
                }
            }
        }
    }

    public List<EventInfoData> getCombineEventList() => 
        this.combineEventList;

    public void resetEventNotice()
    {
        if ((this.noticeList != null) && (this.noticeList.Count > 0))
        {
            for (int i = 0; i < this.noticeList.Count; i++)
            {
                EventNoticeControl control = this.noticeList[i];
                if (control.gameObject.activeSelf)
                {
                    control.setEventInfo();
                }
            }
        }
    }

    public void resetScrollView()
    {
        this.menuListScrollView.SetDragAmount(0f, 0f, false);
    }

    public void setBannerIcon(UISprite targetSprite, EventEntity eventEntity)
    {
        AtlasManager.SetBannerIcon(targetSprite, eventEntity);
        targetSprite.gameObject.SetActive(true);
    }

    public void setCombineEnableNum(CombineEnableData enableData)
    {
        this.setNoticeNumList[1].setNoticeNum(enableData.limitUpEnableNum);
        this.setNoticeNumList[2].setNoticeNum(enableData.skillUpEnableNum);
        this.setNoticeNumList[3].setNoticeNum(enableData.npUpEnableNum);
    }

    private EventInfoData setEventInfo(EventCampaignEntity combineEventEnt, EventEntity eventEnt)
    {
        EventInfoData data = new EventInfoData {
            eventId = eventEnt.id,
            type = combineEventEnt.target,
            iconId = eventEnt.iconId,
            name = eventEnt.name,
            detail = eventEnt.detail
        };
        float num = ((float) combineEventEnt.value) / 1000f;
        data.value = num;
        data.startAt = eventEnt.startedAt;
        data.endAt = eventEnt.endedAt;
        data.eventEntity = eventEnt;
        return data;
    }

    public void setMenuEventNotice()
    {
        base.gameObject.SetActive(true);
        int childCount = this.menuListGrid.transform.childCount;
        this.noticeList = new List<EventNoticeControl>();
        for (int i = 0; i < (childCount - 1); i++)
        {
            foreach (EventNoticeControl control in this.menuListGrid.transform.GetChild(i).GetComponentsInChildren<EventNoticeControl>(true))
            {
                control.gameObject.SetActive(false);
                this.noticeList.Add(control);
            }
            SetNoticeNumControl componentInChildren = this.menuListGrid.transform.GetChild(i).GetComponentInChildren<SetNoticeNumControl>();
            this.setNoticeNumList.Add(componentInChildren);
        }
        this.checkEventNotice();
    }

    private void Update()
    {
        this.resetEventNotice();
    }
}

