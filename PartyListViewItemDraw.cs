using System;
using UnityEngine;

public class PartyListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UIButton baseButton;
    [SerializeField]
    protected PartyOrganizationListViewItemDraw[] itemDrawList = new PartyOrganizationListViewItemDraw[6];
    [SerializeField]
    protected GameObject mainDeckBase;
    [SerializeField]
    protected GameObject warningBase;
    [SerializeField]
    protected UILabel warningMessageLabel;
    [SerializeField]
    protected UILabel warningTitleLabel;

    public void SetInput(PartyListViewItem item, bool isInput)
    {
        if (this.baseButton != null)
        {
            this.baseButton.GetComponent<Collider>().enabled = isInput;
            this.baseButton.SetState(UIButtonColor.State.Normal, true);
        }
        if (item != null)
        {
            for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
            {
                this.itemDrawList[i].SetInput(item.GetMember(i), isInput);
            }
        }
    }

    public void SetItem(PartyListViewItem item, DispMode mode)
    {
        if ((item != null) && (mode != DispMode.INVISIBLE))
        {
            if (mode == DispMode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT)
            {
                for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
                {
                    if (i == 1)
                    {
                        this.itemDrawList[i].SetItem(item.GetMember(i), PartyOrganizationListViewItemDraw.DispMode.INVISIBLE);
                    }
                    else
                    {
                        this.itemDrawList[i].SetItem(item.GetMember(i), PartyOrganizationListViewItemDraw.DispMode.VALID);
                    }
                }
            }
            else
            {
                for (int j = 0; j < BalanceConfig.DeckMemberMax; j++)
                {
                    this.itemDrawList[j].SetItem(item.GetMember(j), (PartyOrganizationListViewItemDraw.DispMode) mode);
                }
            }
            this.warningBase.SetActive(false);
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT,
        ORGANIZATION_GUIDE_DECK_EMPTY_SELECT
    }
}

