using System;
using UnityEngine;

public class PartyListViewIndicator : ListViewIndicator
{
    protected EventDetailEntity eventDetailEntity;
    protected int eventDropItemDispNum;
    protected EventPartyMargeUpValInfo eventMargeUpValInfo;
    [SerializeField]
    protected UILabel indexLabel;
    [SerializeField]
    protected UISprite indexSprite;
    protected PartyListViewItem.MenuKind kind;
    [SerializeField]
    protected GameObject leftObject;
    protected ListViewManager manager;
    [SerializeField]
    protected GameObject pageBaseObject;
    protected int pageIndex;
    protected int pageMax;
    [SerializeField]
    protected UISprite[] pageSpriteList;
    [SerializeField]
    protected PartyEventPointIndicator partyEventPointIndicator;
    [SerializeField]
    protected UICommonButton partyStartButton;
    [SerializeField]
    protected GameObject rightObject;
    [SerializeField]
    protected UILabel selectCostLabel;
    [SerializeField]
    protected UILabel selectNameLabel;
    protected EventUpValSetupInfo setupInfo;
    [SerializeField]
    protected UICommonButton[] tabPartyButtonList;
    [SerializeField]
    protected UILabel[] tabPartyLabelList;
    [SerializeField]
    protected UISprite[] tabPartySpriteList;

    public void DrawPartyInfo(PartyListViewItem partyItem)
    {
        if (partyItem == null)
        {
            this.selectCostLabel.text = string.Empty;
            this.eventMargeUpValInfo = null;
            this.partyEventPointIndicator.SetTotalDropItemList(null);
        }
        else
        {
            EventUpValInfo[] infoArray;
            if (partyItem.Cost <= partyItem.MaxCost)
            {
                this.selectCostLabel.text = string.Empty + partyItem.Cost;
            }
            else
            {
                this.selectCostLabel.text = "[FF2020]" + partyItem.Cost;
            }
            this.selectNameLabel.text = string.Format(LocalizationManager.Get("PARTY_ORGANIZATION_NAME"), partyItem.DeckName);
            this.eventMargeUpValInfo = new EventPartyMargeUpValInfo(partyItem.GetSvtIdList(), partyItem.GetSvtNameList(), partyItem.GetIsFollowerList());
            if ((this.eventDetailEntity != null) && partyItem.GetEventUpVal(out infoArray))
            {
                for (int i = 0; i < infoArray.Length; i++)
                {
                    EventUpValInfo info = infoArray[i];
                    if (info != null)
                    {
                        this.eventMargeUpValInfo.Add(info.GetDropItemList(i));
                    }
                }
            }
            this.partyEventPointIndicator.SetTotalDropItemList(this.eventMargeUpValInfo);
        }
    }

    public void OnClickLeft()
    {
        if ((this.manager != null) && (this.pageIndex > 0))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.manager.MoveCenterItem(this.pageIndex - 1, true);
        }
    }

    public void OnClickRight()
    {
        if ((this.manager != null) && ((this.pageIndex >= 0) && (this.pageIndex < (this.pageMax - 1))))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.manager.MoveCenterItem(this.pageIndex + 1, true);
        }
    }

    public override void OnModifyCenterItem(ListViewManager manager, ListViewItem item, bool isTop, bool isBottom, bool isLeft, bool isRight)
    {
        this.manager = manager;
        PartyListViewItem partyItem = item as PartyListViewItem;
        this.leftObject.SetActive(isLeft);
        this.rightObject.SetActive(isRight);
        this.SetPageIndex((item == null) ? -1 : item.Index);
        this.DrawPartyInfo(partyItem);
    }

    public override void OnModifyPosition(ListViewManager manager, ListViewItem item)
    {
    }

    public void SetEventId(EventUpValSetupInfo setupInfo)
    {
        this.setupInfo = setupInfo;
        if ((setupInfo != null) && (setupInfo.eventId > 0))
        {
            this.eventDetailEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventDetailMaster>(DataNameKind.Kind.EVENT_DETAIL).getEntityFromId(setupInfo.eventId);
        }
        else
        {
            this.eventDetailEntity = null;
        }
        this.eventMargeUpValInfo = null;
        this.partyEventPointIndicator.SetTotalDropItemList(null);
    }

    public override void SetIndexMax(int max)
    {
        this.SetPageMax(max);
        this.leftObject.SetActive(false);
        this.rightObject.SetActive(false);
    }

    public void SetKind(PartyListViewItem.MenuKind kind)
    {
        this.kind = kind;
    }

    public void SetPageIndex(int index)
    {
        this.pageIndex = index;
        if (this.pageBaseObject != null)
        {
            for (int i = 0; i < this.pageMax; i++)
            {
                this.pageSpriteList[i].spriteName = (i != index) ? "img_slider_off" : "img_slider_on";
            }
        }
        this.indexLabel.text = string.Empty + (index + 1);
        this.indexSprite.spriteName = "party_txt_" + (index + 1);
        this.indexSprite.MakePixelPerfect();
    }

    public void SetPageMax(int max)
    {
        this.pageMax = (max <= this.pageSpriteList.Length) ? max : this.pageSpriteList.Length;
        if (this.pageMax <= 1)
        {
            this.pageMax = 0;
        }
        if (this.pageBaseObject != null)
        {
            for (int i = 0; i < this.pageSpriteList.Length; i++)
            {
                this.pageSpriteList[i].spriteName = (i >= this.pageMax) ? null : "img_slider_off";
            }
            Vector3 localPosition = this.pageBaseObject.transform.localPosition;
            localPosition.x = -10 * (this.pageMax - 1);
            this.pageBaseObject.transform.localPosition = localPosition;
        }
    }
}

