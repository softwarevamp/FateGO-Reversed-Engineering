using System;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/ListViewItem")]
public class ListViewItem
{
    protected Vector3 basePosition;
    protected int index;
    protected bool isTermination;
    protected bool isTerminationSpace;
    protected int loopIndex;
    protected int selectNum;
    protected int sortIndex;
    protected long sortValue0;
    protected long sortValue1;
    protected long sortValue1B;
    protected long sortValue2;
    protected long sortValue2B;
    protected ListViewObject viewObject;

    public ListViewItem()
    {
        this.selectNum = -1;
        this.basePosition = new Vector3();
    }

    public ListViewItem(int index)
    {
        this.selectNum = -1;
        this.index = this.sortIndex = this.loopIndex = index;
        this.sortValue0 = this.sortValue1 = this.sortValue2 = 0L;
        this.basePosition = new Vector3();
    }

    ~ListViewItem()
    {
    }

    public void Set(ListViewItem item)
    {
        this.index = item.index;
        this.sortIndex = item.sortIndex;
        this.loopIndex = item.loopIndex;
        this.sortValue0 = item.sortValue0;
        this.sortValue1 = item.sortValue1;
        this.sortValue1B = item.sortValue1B;
        this.sortValue2 = item.sortValue2;
        this.sortValue2B = item.sortValue2B;
        this.basePosition = item.basePosition;
    }

    public void SetLoopIndex(int index)
    {
        this.loopIndex = index;
    }

    public void SetSortIndex(int index)
    {
        this.sortIndex = index;
    }

    public virtual bool SetSortValue(ListViewSort sort)
    {
        this.sortValue0 = 0L;
        this.sortValue1 = this.index;
        this.sortValue1B = 0L;
        this.sortValue2 = 0L;
        this.sortValue2B = 0L;
        this.isTermination = false;
        this.isTerminationSpace = false;
        return true;
    }

    public int SortCompDown(ListViewItem item)
    {
        if (this.sortValue0 != item.sortValue0)
        {
            return ((this.sortValue0 >= item.sortValue0) ? -1 : 1);
        }
        if (this.sortValue1 != item.sortValue1)
        {
            return ((this.sortValue1 >= item.sortValue1) ? -1 : 1);
        }
        if (this.sortValue1B != item.sortValue1B)
        {
            return ((this.sortValue1B >= item.sortValue1B) ? -1 : 1);
        }
        if (this.sortValue2 != item.sortValue2)
        {
            return ((this.sortValue2 >= item.sortValue2) ? -1 : 1);
        }
        if (this.sortValue2B != item.sortValue2B)
        {
            return ((this.sortValue2B >= item.sortValue2B) ? -1 : 1);
        }
        return 0;
    }

    public int SortCompUp(ListViewItem item)
    {
        if (this.sortValue0 != item.sortValue0)
        {
            return ((this.sortValue0 >= item.sortValue0) ? -1 : 1);
        }
        if (this.sortValue1 != item.sortValue1)
        {
            return ((this.sortValue1 <= item.sortValue1) ? -1 : 1);
        }
        if (this.sortValue1B != item.sortValue1B)
        {
            return ((this.sortValue1B <= item.sortValue1B) ? -1 : 1);
        }
        if (this.sortValue2 != item.sortValue2)
        {
            return ((this.sortValue2 >= item.sortValue2) ? -1 : 1);
        }
        if (this.sortValue2B != item.sortValue2B)
        {
            return ((this.sortValue2B >= item.sortValue2B) ? -1 : 1);
        }
        return 0;
    }

    public Vector3 BasePosition
    {
        get => 
            this.basePosition;
        set
        {
            this.basePosition = value;
        }
    }

    public int Index =>
        this.index;

    public bool IsSelect
    {
        get => 
            (this.selectNum >= 0);
        set
        {
            this.selectNum = !value ? -1 : 0;
        }
    }

    public bool IsTermination
    {
        get => 
            this.isTermination;
        set
        {
            this.isTermination = value;
        }
    }

    public bool IsTerminationSpace
    {
        get => 
            this.isTerminationSpace;
        set
        {
            this.isTerminationSpace = value;
        }
    }

    public int LoopIndex =>
        this.loopIndex;

    public int SelectNum
    {
        get => 
            this.selectNum;
        set
        {
            this.selectNum = value;
        }
    }

    public int SortIndex =>
        this.sortIndex;

    public long SortValue0 =>
        this.sortValue0;

    public long SortValue1 =>
        this.sortValue1;

    public long SortValue2 =>
        this.sortValue2;

    public long SortValue2B =>
        this.sortValue2B;

    public ListViewObject ViewObject
    {
        get => 
            this.viewObject;
        set
        {
            this.viewObject = value;
        }
    }
}

