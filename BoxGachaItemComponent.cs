using System;
using UnityEngine;

public class BoxGachaItemComponent : MonoBehaviour
{
    [SerializeField]
    protected UILabel dataLabel;
    [SerializeField]
    protected ItemIconComponent itemIcon;

    public void Clear()
    {
        this.itemIcon.Clear();
        this.dataLabel.text = string.Empty;
    }

    public void Set(int itemId, int payNum)
    {
        if (itemId > 0)
        {
            this.itemIcon.SetItem(itemId, -1);
            this.dataLabel.text = LocalizationManager.GetNumberFormat(payNum);
        }
        else
        {
            this.Clear();
        }
    }
}

