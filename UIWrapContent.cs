using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class UIWrapContent : MonoBehaviour
{
    public bool cullContent = true;
    public int itemSize = 100;
    public int maxIndex;
    private List<Transform> mChildren = new List<Transform>();
    private bool mFirstTime = true;
    private bool mHorizontal;
    public int minIndex;
    private UIPanel mPanel;
    private UIScrollView mScroll;
    private Transform mTrans;
    public OnInitializeItem onInitializeItem;

    protected bool CacheScrollView()
    {
        this.mTrans = base.transform;
        this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
        this.mScroll = this.mPanel.GetComponent<UIScrollView>();
        if (this.mScroll != null)
        {
            if (this.mScroll.movement == UIScrollView.Movement.Horizontal)
            {
                this.mHorizontal = true;
                goto Label_007C;
            }
            if (this.mScroll.movement == UIScrollView.Movement.Vertical)
            {
                this.mHorizontal = false;
                goto Label_007C;
            }
        }
        return false;
    Label_007C:
        return true;
    }

    protected virtual void OnMove(UIPanel panel)
    {
        this.WrapContent();
    }

    private void OnValidate()
    {
        if (this.maxIndex < this.minIndex)
        {
            this.maxIndex = this.minIndex;
        }
        if (this.minIndex > this.maxIndex)
        {
            this.maxIndex = this.minIndex;
        }
    }

    private void ResetChildPositions()
    {
        int index = 0;
        int count = this.mChildren.Count;
        while (index < count)
        {
            Transform item = this.mChildren[index];
            item.localPosition = !this.mHorizontal ? new Vector3(0f, (float) (-index * this.itemSize), 0f) : new Vector3((float) (index * this.itemSize), 0f, 0f);
            this.UpdateItem(item, index);
            index++;
        }
    }

    public void resetScroll()
    {
        this.mScroll.ResetPosition();
    }

    public void setScrollPos(int idx)
    {
        Transform transform = this.mChildren[idx];
        this.mScroll.transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        this.mPanel.clipOffset = new Vector2(transform.localPosition.x, 0f);
        Debug.Log("!!** setScrollPos : " + idx);
        Debug.Log("!!** setScrollPos mChildList Position: " + transform.localPosition);
    }

    [ContextMenu("Sort Alphabetically")]
    public void SortAlphabetically()
    {
        if (this.CacheScrollView())
        {
            this.mChildren.Clear();
            for (int i = 0; i < this.mTrans.childCount; i++)
            {
                this.mChildren.Add(this.mTrans.GetChild(i));
            }
            this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortByName));
            this.ResetChildPositions();
        }
    }

    [ContextMenu("Sort Based on Scroll Movement")]
    public void SortBasedOnScrollMovement()
    {
        if (this.CacheScrollView())
        {
            this.mChildren.Clear();
            for (int i = 0; i < this.mTrans.childCount; i++)
            {
                this.mChildren.Add(this.mTrans.GetChild(i));
            }
            if (this.mHorizontal)
            {
                this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
            }
            else
            {
                this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortVertical));
            }
            this.ResetChildPositions();
        }
    }

    protected virtual void Start()
    {
        this.SortBasedOnScrollMovement();
        this.WrapContent();
        if (this.mScroll != null)
        {
            this.mScroll.GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved(this.OnMove);
        }
        this.mFirstTime = false;
    }

    protected virtual void UpdateItem(Transform item, int index)
    {
        if (this.onInitializeItem != null)
        {
            int realIndex = (this.mScroll.movement != UIScrollView.Movement.Vertical) ? Mathf.RoundToInt(item.localPosition.x / ((float) this.itemSize)) : Mathf.RoundToInt(item.localPosition.y / ((float) this.itemSize));
            this.onInitializeItem(item.gameObject, index, realIndex);
        }
    }

    public void WrapContent()
    {
        float num = (this.itemSize * this.mChildren.Count) * 0.5f;
        Vector3[] worldCorners = this.mPanel.worldCorners;
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = worldCorners[i];
            worldCorners[i] = this.mTrans.InverseTransformPoint(position);
        }
        Vector3 vector2 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
        bool flag = true;
        float num3 = num * 2f;
        if (this.mHorizontal)
        {
            float num4 = worldCorners[0].x - this.itemSize;
            float num5 = worldCorners[2].x + this.itemSize;
            int index = 0;
            int count = this.mChildren.Count;
            while (index < count)
            {
                Transform item = this.mChildren[index];
                float num8 = item.localPosition.x - vector2.x;
                if (num8 < -num)
                {
                    Vector3 localPosition = item.localPosition;
                    localPosition.x += num3;
                    num8 = localPosition.x - vector2.x;
                    int num9 = Mathf.RoundToInt(localPosition.x / ((float) this.itemSize));
                    if ((this.minIndex == this.maxIndex) || ((this.minIndex <= num9) && (num9 <= this.maxIndex)))
                    {
                        item.localPosition = localPosition;
                        this.UpdateItem(item, index);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (num8 > num)
                {
                    Vector3 vector4 = item.localPosition;
                    vector4.x -= num3;
                    num8 = vector4.x - vector2.x;
                    int num10 = Mathf.RoundToInt(vector4.x / ((float) this.itemSize));
                    if ((this.minIndex == this.maxIndex) || ((this.minIndex <= num10) && (num10 <= this.maxIndex)))
                    {
                        item.localPosition = vector4;
                        this.UpdateItem(item, index);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (this.mFirstTime)
                {
                    this.UpdateItem(item, index);
                }
                if (this.cullContent)
                {
                    num8 += this.mPanel.clipOffset.x - this.mTrans.localPosition.x;
                    if (!UICamera.IsPressed(item.gameObject))
                    {
                        NGUITools.SetActive(item.gameObject, (num8 > num4) && (num8 < num5), false);
                    }
                }
                index++;
            }
        }
        else
        {
            float num11 = worldCorners[0].y - this.itemSize;
            float num12 = worldCorners[2].y + this.itemSize;
            int num13 = 0;
            int num14 = this.mChildren.Count;
            while (num13 < num14)
            {
                Transform transform2 = this.mChildren[num13];
                float num15 = transform2.localPosition.y - vector2.y;
                if (num15 < -num)
                {
                    Vector3 vector5 = transform2.localPosition;
                    vector5.y += num3;
                    num15 = vector5.y - vector2.y;
                    int num16 = Mathf.RoundToInt(vector5.y / ((float) this.itemSize));
                    if ((this.minIndex == this.maxIndex) || ((this.minIndex <= num16) && (num16 <= this.maxIndex)))
                    {
                        transform2.localPosition = vector5;
                        this.UpdateItem(transform2, num13);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (num15 > num)
                {
                    Vector3 vector6 = transform2.localPosition;
                    vector6.y -= num3;
                    num15 = vector6.y - vector2.y;
                    int num17 = Mathf.RoundToInt(vector6.y / ((float) this.itemSize));
                    if ((this.minIndex == this.maxIndex) || ((this.minIndex <= num17) && (num17 <= this.maxIndex)))
                    {
                        transform2.localPosition = vector6;
                        this.UpdateItem(transform2, num13);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (this.mFirstTime)
                {
                    this.UpdateItem(transform2, num13);
                }
                if (this.cullContent)
                {
                    num15 += this.mPanel.clipOffset.y - this.mTrans.localPosition.y;
                    if (!UICamera.IsPressed(transform2.gameObject))
                    {
                        NGUITools.SetActive(transform2.gameObject, (num15 > num11) && (num15 < num12), false);
                    }
                }
                num13++;
            }
        }
        this.mScroll.restrictWithinPanel = !flag;
    }

    public delegate void OnInitializeItem(GameObject go, int wrapIndex, int realIndex);
}

