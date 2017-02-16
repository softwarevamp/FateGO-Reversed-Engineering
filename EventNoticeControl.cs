using System;
using UnityEngine;

public class EventNoticeControl : MonoBehaviour
{
    public UILabel eventDetalLb;
    private EventInfoData eventInfo;
    public UILabel eventRemainLb;

    public void setCombineEventData(EventInfoData data)
    {
        this.eventInfo = data;
        this.setEventInfo();
    }

    private void setEventDetail()
    {
        string name = string.Empty;
        switch (this.eventInfo.type)
        {
            case 1:
                name = this.eventInfo.name;
                break;

            case 2:
                name = this.eventInfo.name;
                break;

            case 4:
                name = this.eventInfo.name;
                break;

            case 5:
                name = this.eventInfo.name;
                break;

            case 6:
                name = this.eventInfo.name;
                break;

            case 8:
                name = this.eventInfo.name;
                break;

            case 10:
                name = this.eventInfo.name;
                break;

            case 0x10:
                name = this.eventInfo.name;
                break;

            case 0x11:
                name = this.eventInfo.name;
                break;

            case 0x12:
                name = this.eventInfo.name;
                break;

            case 0x13:
                name = this.eventInfo.name;
                break;
        }
        this.eventDetalLb.text = name;
    }

    public void setEventInfo()
    {
        this.setEventDetail();
        this.setEventRemain();
    }

    private void setEventRemain()
    {
        long num2 = this.eventInfo.endAt - NetworkManager.getTime();
        string str = string.Empty;
        this.eventRemainLb.effectColor = Color.black;
        if (num2 > 0L)
        {
            int num3 = (int) (num2 / 0x15180L);
            str = string.Format(LocalizationManager.Get("EVENT_DAY_TXT"), num3);
            if (num3 <= 0)
            {
                num3 = (int) (num2 / 0xe10L);
                str = string.Format(LocalizationManager.Get("EVENT_TIME_TXT"), num3);
                if (num3 <= 0)
                {
                    num3 = (int) (num2 / 60L);
                    str = string.Format(LocalizationManager.Get("EVENT_MIN_TXT"), num3);
                    this.eventRemainLb.effectColor = Color.white;
                }
            }
        }
        this.eventRemainLb.text = str;
    }
}

