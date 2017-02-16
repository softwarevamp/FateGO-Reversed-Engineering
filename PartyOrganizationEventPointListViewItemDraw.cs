using System;
using UnityEngine;

public class PartyOrganizationEventPointListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UILabel dataLabel;
    [SerializeField]
    protected UILabel titleLabel;

    public void SetInput(PartyOrganizationEventPointListViewItem item, bool isInput)
    {
    }

    public void SetItem(PartyOrganizationEventPointListViewItem item, DispMode mode)
    {
        if ((item != null) && (mode != DispMode.INVISIBLE))
        {
            this.titleLabel.text = item.GetTitleString();
            this.dataLabel.text = item.GetDataString();
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

