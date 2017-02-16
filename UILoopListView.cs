using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/UILoopListView")]
public class UILoopListView : MonoBehaviour
{
    [CompilerGenerated]
    private static Comparison<Transform> <>f__am$cacheD;
    private UICenterOnChild centerChild;
    private int childIdx;
    private bool isFirstTime = true;
    private bool isHorizontal;
    private int itemSize = 100;
    protected int maxIndex;
    private List<Transform> mChildList = new List<Transform>();
    protected int minIndex;
    private UIPanel mPanel;
    private UIScrollView mScroll;
    private Transform mTrans;
    public OnInitializeItem onInitializeItem;
    private List<Transform> realChildList = new List<Transform>();

    public int getChildIdx() => 
        this.childIdx;

    public void initWrapContent()
    {
        this.sortBaseOnMovement();
        this.mScroll.ResetPosition();
        this.mScroll.onDragFinished = (UIScrollView.OnDragNotification) Delegate.Combine(this.mScroll.onDragFinished, new UIScrollView.OnDragNotification(this.OnMove));
        this.wrapContent();
        this.isFirstTime = false;
    }

    private void OnMove()
    {
        this.wrapContent();
    }

    public void resetChildPos()
    {
        int count = this.mChildList.Count;
        Debug.Log("**--** Loop List View resetChildPos imax: " + count);
        for (int i = 0; i < count; i++)
        {
            Transform transform = this.mChildList[i];
            transform.localPosition = !this.isHorizontal ? new Vector3(0f, (float) (-i * this.itemSize), 0f) : new Vector3((float) (i * this.itemSize), 0f, 0f);
        }
    }

    public void setScrollPos(int idx)
    {
        int num = idx;
        Transform transform = this.mChildList[num];
        this.mScroll.MoveRelative(transform.localPosition);
        Debug.Log("!!** setScrollPos : " + idx);
        Debug.Log("!!** setScrollPos mChildList Position: " + transform.localPosition);
    }

    private bool setScrollViewInfo()
    {
        this.mTrans = base.transform;
        this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
        this.mScroll = this.mPanel.GetComponent<UIScrollView>();
        if (this.mScroll != null)
        {
            UIWidget component = this.mTrans.GetChild(0).GetComponent<UIWidget>();
            if (this.mScroll.movement == UIScrollView.Movement.Horizontal)
            {
                this.isHorizontal = true;
                this.itemSize = component.width;
                goto Label_00A6;
            }
            if (this.mScroll.movement == UIScrollView.Movement.Vertical)
            {
                this.isHorizontal = false;
                this.itemSize = component.height;
                goto Label_00A6;
            }
        }
        return false;
    Label_00A6:
        return true;
    }

    private void sortBaseOnMovement()
    {
        if (this.setScrollViewInfo())
        {
            this.mChildList.Clear();
            for (int i = 0; i < this.mTrans.childCount; i++)
            {
                this.mChildList.Add(this.mTrans.GetChild(i));
                this.realChildList.Add(this.mTrans.GetChild(i));
            }
            this.resetChildPos();
            if (<>f__am$cacheD == null)
            {
                <>f__am$cacheD = (a, b) => (int) (a.localPosition.x - b.localPosition.x);
            }
            this.mChildList.Sort(<>f__am$cacheD);
        }
    }

    private void updateItem(Transform item, int index)
    {
        if (this.onInitializeItem != null)
        {
            int realIndex = (this.mScroll.movement != UIScrollView.Movement.Vertical) ? Mathf.RoundToInt(item.localPosition.x / ((float) this.itemSize)) : Mathf.RoundToInt(item.localPosition.y / ((float) this.itemSize));
            this.onInitializeItem(item.gameObject, index, realIndex);
        }
    }

    private void wrapContent()
    {
        float num = (this.itemSize * this.mChildList.Count) * 0.5f;
        Vector3[] worldCorners = this.mPanel.worldCorners;
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = worldCorners[i];
            worldCorners[i] = this.mTrans.InverseTransformPoint(position);
        }
        Vector3 vector2 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
        bool flag = true;
        float num3 = num * 2f;
        if (this.isHorizontal)
        {
            for (int j = 0; j < this.mChildList.Count; j++)
            {
                Transform item = this.mChildList[j];
                float num5 = item.localPosition.x - vector2.x;
                if (num5 < -num)
                {
                    Vector3 localPosition = item.localPosition;
                    localPosition.x += num3;
                    num5 = localPosition.x - vector2.x;
                    int num6 = Mathf.RoundToInt(localPosition.x / ((float) this.itemSize));
                    if ((this.minIndex == this.maxIndex) || ((this.minIndex <= num6) && (num6 <= this.maxIndex)))
                    {
                        item.localPosition = localPosition;
                        this.updateItem(item, j);
                        item.name = j.ToString();
                        this.childIdx = j;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (num5 > num)
                {
                    Vector3 vector4 = item.localPosition;
                    vector4.x -= num3;
                    num5 = vector4.x - vector2.x;
                    int num7 = Mathf.RoundToInt(vector4.x / ((float) this.itemSize));
                    if ((this.minIndex == this.maxIndex) || ((this.minIndex <= num7) && (num7 <= this.maxIndex)))
                    {
                        item.localPosition = vector4;
                        this.updateItem(item, j);
                        item.name = j.ToString();
                        this.childIdx = j;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (this.isFirstTime)
                {
                    this.updateItem(item, j);
                }
            }
        }
        else
        {
            for (int k = 0; k < this.mChildList.Count; k++)
            {
                Transform transform2 = this.mChildList[k];
                float num9 = transform2.localPosition.y - vector2.y;
                if (num9 < -num)
                {
                    Vector3 vector5 = transform2.localPosition;
                    vector5.y += num3;
                    num9 = vector5.y - vector2.y;
                    int num10 = Mathf.RoundToInt(vector5.y / ((float) this.itemSize));
                    if ((this.minIndex == this.maxIndex) || ((this.minIndex <= num10) && (num10 <= this.maxIndex)))
                    {
                        transform2.localPosition = vector5;
                        this.updateItem(transform2, k);
                        transform2.name = num10.ToString();
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (num9 > num)
                {
                    Vector3 vector6 = transform2.localPosition;
                    vector6.y -= num3;
                    num9 = vector6.y - vector2.y;
                    int num11 = Mathf.RoundToInt(vector6.y / ((float) this.itemSize));
                    if ((this.minIndex == this.maxIndex) || ((this.minIndex <= num11) && (num11 <= this.maxIndex)))
                    {
                        transform2.localPosition = vector6;
                        this.updateItem(transform2, k);
                        transform2.name = num11.ToString();
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (this.isFirstTime)
                {
                    this.updateItem(transform2, k);
                }
            }
        }
        this.mScroll.restrictWithinPanel = !flag;
    }

    public delegate void OnInitializeItem(GameObject go, int wrapIndex, int realIndex);
}

