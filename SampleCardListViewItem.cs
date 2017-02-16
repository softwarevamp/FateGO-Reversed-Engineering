using System;
using UnityEngine;

[AddComponentMenu("Sample/TestListView/SampleCardListViewItem")]
public class SampleCardListViewItem : ListViewItem
{
    protected int cardId;
    protected bool isDeck;

    public SampleCardListViewItem()
    {
        this.cardId = 0;
    }

    public SampleCardListViewItem(int index) : base(index)
    {
        this.cardId = 0;
    }

    ~SampleCardListViewItem()
    {
    }

    private string ToString() => 
        ("cardId " + this.cardId);

    public int CardId
    {
        get => 
            this.cardId;
        set
        {
            this.cardId = value;
        }
    }

    public bool IsDeck
    {
        get => 
            this.isDeck;
        set
        {
            this.isDeck = value;
        }
    }
}

