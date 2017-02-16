using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
    public bool animateSmoothly;
    public Arrangement arrangement;
    public float cellHeight = 200f;
    public float cellWidth = 200f;
    public bool hideInactive;
    public bool keepWithinPanel;
    public int maxPerLine;
    protected bool mInitDone;
    protected UIPanel mPanel;
    protected bool mReposition;
    public Comparison<Transform> onCustomSort;
    public OnReposition onReposition;
    public UIWidget.Pivot pivot;
    [SerializeField, HideInInspector]
    private bool sorted;
    public Sorting sorting;

    public void AddChild(Transform trans)
    {
        this.AddChild(trans, true);
    }

    public void AddChild(Transform trans, bool sort)
    {
        if (trans != null)
        {
            trans.parent = base.transform;
            this.ResetPosition(this.GetChildList());
        }
    }

    public void ConstrainWithinPanel()
    {
        if (this.mPanel != null)
        {
            this.mPanel.ConstrainTargetToBounds(base.transform, true);
            UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
            if (component != null)
            {
                component.UpdateScrollbars(true);
            }
        }
    }

    public Transform GetChild(int index)
    {
        List<Transform> childList = this.GetChildList();
        return ((index >= childList.Count) ? null : childList[index]);
    }

    public List<Transform> GetChildList()
    {
        Transform transform = base.transform;
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!this.hideInactive || ((child != null) && NGUITools.GetActive(child.gameObject)))
            {
                list.Add(child);
            }
        }
        if ((this.sorting != Sorting.None) && (this.arrangement != Arrangement.CellSnap))
        {
            if (this.sorting == Sorting.Alphabetic)
            {
                list.Sort(new Comparison<Transform>(UIGrid.SortByName));
                return list;
            }
            if (this.sorting == Sorting.Horizontal)
            {
                list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
                return list;
            }
            if (this.sorting == Sorting.Vertical)
            {
                list.Sort(new Comparison<Transform>(UIGrid.SortVertical));
                return list;
            }
            if (this.onCustomSort != null)
            {
                list.Sort(this.onCustomSort);
                return list;
            }
            this.Sort(list);
        }
        return list;
    }

    public int GetIndex(Transform trans) => 
        this.GetChildList().IndexOf(trans);

    protected virtual void Init()
    {
        this.mInitDone = true;
        this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying && NGUITools.GetActive(this))
        {
            this.Reposition();
        }
    }

    public bool RemoveChild(Transform t)
    {
        List<Transform> childList = this.GetChildList();
        if (childList.Remove(t))
        {
            this.ResetPosition(childList);
            return true;
        }
        return false;
    }

    [ContextMenu("Execute")]
    public virtual void Reposition()
    {
        if ((Application.isPlaying && !this.mInitDone) && NGUITools.GetActive(base.gameObject))
        {
            this.Init();
        }
        if (this.sorted)
        {
            this.sorted = false;
            if (this.sorting == Sorting.None)
            {
                this.sorting = Sorting.Alphabetic;
            }
            NGUITools.SetDirty(this);
        }
        List<Transform> childList = this.GetChildList();
        this.ResetPosition(childList);
        if (this.keepWithinPanel)
        {
            this.ConstrainWithinPanel();
        }
        if (this.onReposition != null)
        {
            this.onReposition();
        }
    }

    protected virtual void ResetPosition(List<Transform> list)
    {
        this.mReposition = false;
        int b = 0;
        int num2 = 0;
        int a = 0;
        int num4 = 0;
        Transform transform = base.transform;
        int num5 = 0;
        int count = list.Count;
        while (num5 < count)
        {
            Transform transform2 = list[num5];
            Vector3 localPosition = transform2.localPosition;
            float z = localPosition.z;
            if (this.arrangement == Arrangement.CellSnap)
            {
                if (this.cellWidth > 0f)
                {
                    localPosition.x = Mathf.Round(localPosition.x / this.cellWidth) * this.cellWidth;
                }
                if (this.cellHeight > 0f)
                {
                    localPosition.y = Mathf.Round(localPosition.y / this.cellHeight) * this.cellHeight;
                }
            }
            else
            {
                localPosition = (this.arrangement != Arrangement.Horizontal) ? new Vector3(this.cellWidth * num2, -this.cellHeight * b, z) : new Vector3(this.cellWidth * b, -this.cellHeight * num2, z);
            }
            if (this.animateSmoothly && Application.isPlaying)
            {
                SpringPosition position = SpringPosition.Begin(transform2.gameObject, localPosition, 15f);
                position.updateScrollView = true;
                position.ignoreTimeScale = true;
            }
            else
            {
                transform2.localPosition = localPosition;
            }
            a = Mathf.Max(a, b);
            num4 = Mathf.Max(num4, num2);
            if ((++b >= this.maxPerLine) && (this.maxPerLine > 0))
            {
                b = 0;
                num2++;
            }
            num5++;
        }
        if (this.pivot != UIWidget.Pivot.TopLeft)
        {
            float num8;
            float num9;
            Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
            if (this.arrangement == Arrangement.Horizontal)
            {
                num8 = Mathf.Lerp(0f, a * this.cellWidth, pivotOffset.x);
                num9 = Mathf.Lerp(-num4 * this.cellHeight, 0f, pivotOffset.y);
            }
            else
            {
                num8 = Mathf.Lerp(0f, num4 * this.cellWidth, pivotOffset.x);
                num9 = Mathf.Lerp(-a * this.cellHeight, 0f, pivotOffset.y);
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                SpringPosition component = child.GetComponent<SpringPosition>();
                if (component != null)
                {
                    component.target.x -= num8;
                    component.target.y -= num9;
                }
                else
                {
                    Vector3 vector3 = child.localPosition;
                    vector3.x -= num8;
                    vector3.y -= num9;
                    child.localPosition = vector3;
                }
            }
        }
    }

    protected virtual void Sort(List<Transform> list)
    {
    }

    public static int SortByName(Transform a, Transform b) => 
        string.Compare(a.name, b.name);

    public static int SortHorizontal(Transform a, Transform b) => 
        a.localPosition.x.CompareTo(b.localPosition.x);

    public static int SortVertical(Transform a, Transform b) => 
        b.localPosition.y.CompareTo(a.localPosition.y);

    protected virtual void Start()
    {
        if (!this.mInitDone)
        {
            this.Init();
        }
        bool animateSmoothly = this.animateSmoothly;
        this.animateSmoothly = false;
        this.Reposition();
        this.animateSmoothly = animateSmoothly;
        base.enabled = false;
    }

    protected virtual void Update()
    {
        this.Reposition();
        base.enabled = false;
    }

    public bool repositionNow
    {
        set
        {
            if (value)
            {
                this.mReposition = true;
                base.enabled = true;
            }
        }
    }

    public enum Arrangement
    {
        Horizontal,
        Vertical,
        CellSnap
    }

    public delegate void OnReposition();

    public enum Sorting
    {
        None,
        Alphabetic,
        Horizontal,
        Vertical,
        Custom
    }
}

