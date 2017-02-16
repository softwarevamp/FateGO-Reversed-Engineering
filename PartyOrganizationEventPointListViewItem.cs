using System;

public class PartyOrganizationEventPointListViewItem : ListViewItem
{
    protected bool isFollower;
    protected EventMargeItemUpValInfo margeItemInfo;
    protected string titleName;

    public PartyOrganizationEventPointListViewItem(int index, EventMargeItemUpValInfo margeItemInfo) : base(index)
    {
        this.margeItemInfo = margeItemInfo;
    }

    public PartyOrganizationEventPointListViewItem(int index, string titleName, bool isFollower) : base(index)
    {
        this.titleName = titleName;
        this.isFollower = isFollower;
    }

    ~PartyOrganizationEventPointListViewItem()
    {
    }

    public string GetDataString()
    {
        if (this.margeItemInfo != null)
        {
            string eventUpString = this.margeItemInfo.GetEventUpString();
            if (!string.IsNullOrEmpty(eventUpString))
            {
                return string.Format(this.margeItemInfo.GetColorString() + this.margeItemInfo.GetEventUpString() + LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_POINT_OFFSET"), eventUpString, this.margeItemInfo.GetTargetString());
            }
        }
        return string.Empty;
    }

    public string GetTitleString()
    {
        if (this.titleName != null)
        {
            return string.Format(LocalizationManager.Get(!this.isFollower ? "PARTY_ORGANIZATION_EVENT_POINT_MEMBER" : "PARTY_ORGANIZATION_EVENT_POINT_MEMBER_FOLLOWER"), this.titleName);
        }
        if (this.margeItemInfo != null)
        {
            return string.Format(LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_POINT_OFFSET") + this.margeItemInfo.GetColorString() + this.margeItemInfo.GetNameTitleString(), this.margeItemInfo.GetItemName(), this.margeItemInfo.GetServantName(), !this.margeItemInfo.isOtherUp ? this.margeItemInfo.GetTargetString() : string.Empty);
        }
        return string.Empty;
    }
}

