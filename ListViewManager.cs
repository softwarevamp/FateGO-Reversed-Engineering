using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/ListViewManager")]
public class ListViewManager : MonoBehaviour
{
    protected ListViewItem bottomItem;
    protected System.Action callbackAfterScroll;
    protected ListViewItem centerItem;
    [SerializeField]
    protected Vector2 clipOffset = Vector2.zero;
    [SerializeField]
    protected Vector2 clipRange = Vector2.zero;
    [SerializeField]
    protected GameObject dragParentObject;
    [SerializeField]
    protected GameObject dropDragPrefab;
    [SerializeField]
    protected List<UIDragDropListViewSurface> dropList;
    [SerializeField]
    protected List<ListViewDropObject> dropObjectList;
    [SerializeField]
    protected GameObject emptyMessageBase;
    [SerializeField]
    protected UILabel emptyMessageLabel;
    protected ListViewItem horizontalItem;
    [SerializeField]
    protected ListViewIndicator indicator;
    protected bool isAllDisp;
    protected bool isInput = true;
    [SerializeField]
    protected bool isLoop;
    protected bool isScrollRefresh;
    protected List<ListViewItem> itemList;
    protected List<ListViewItem> itemSortList;
    protected ListViewItem leftItem;
    [SerializeField]
    protected GameObject listDragPrefab;
    protected List<GameObject> objectList = new List<GameObject>();
    protected Stack<GameObject> objectStock = new Stack<GameObject>();
    protected Vector3 oldScrollPosition;
    protected ListViewItem rightItem;
    [SerializeField]
    protected UIScrollBar scrollBar;
    [SerializeField]
    protected UIScrollView scrollView;
    [SerializeField]
    protected ListViewItemSeed seed;
    protected ListViewSort sort = new ListViewSort(ListViewSort.SortKind.LEVEL, true);
    [SerializeField]
    protected UICommonButton sortKindButton;
    [SerializeField]
    protected UILabel sortKindLabel;
    [SerializeField]
    protected UICommonButton sortOrderButton;
    [SerializeField]
    protected UISprite sortOrderSprite;
    protected int terminalIndex;
    protected ListViewItem topItem;
    protected ListViewItem verticalItem;

    public void BackLoopItem()
    {
        if (((this.isLoop && (this.itemSortList != null)) && (this.itemSortList.Count == 2)) && (this.centerItem != null))
        {
            int num2 = (this.centerItem.SortIndex != 0) ? 0 : 1;
            ListViewItem item = this.itemSortList[num2];
            int index = this.centerItem.LoopIndex - 1;
            if (item.LoopIndex != index)
            {
                item.SetLoopIndex(index);
                item.BasePosition = this.seed.GetLocalPosition(index);
                if (item.ViewObject != null)
                {
                    item.ViewObject.SetTransform(item.BasePosition);
                }
            }
        }
    }

    public void CenterLoopItem()
    {
        if (((this.isLoop && (this.itemSortList != null)) && (this.itemSortList.Count > 2)) && (this.centerItem != null))
        {
            int sortIndex = this.centerItem.SortIndex;
            int loopIndex = this.centerItem.LoopIndex;
            int count = this.itemSortList.Count;
            int num4 = count / 2;
            int num5 = sortIndex;
            for (int i = 0; i < num4; i++)
            {
                num5++;
                loopIndex++;
                if (num5 >= count)
                {
                    num5 = 0;
                }
                ListViewItem item = this.itemSortList[num5];
                if (item.LoopIndex != loopIndex)
                {
                    item.SetLoopIndex(loopIndex);
                    item.BasePosition = this.seed.GetLocalPosition(loopIndex);
                    if (item.ViewObject != null)
                    {
                        item.ViewObject.SetTransform(item.BasePosition);
                    }
                }
                item.IsTermination = this.isAllDisp;
            }
            ListViewItem item2 = this.itemSortList[num5];
            item2.IsTermination = true;
            int num7 = sortIndex;
            loopIndex = this.centerItem.LoopIndex;
            while (true)
            {
                int num8 = num7;
                num7--;
                loopIndex--;
                if (num7 < 0)
                {
                    num7 = count - 1;
                }
                if (num7 == num5)
                {
                    ListViewItem item3 = this.itemSortList[num8];
                    item3.IsTermination = true;
                    this.terminalIndex = num8;
                    break;
                }
                ListViewItem item4 = this.itemSortList[num7];
                if (item4.LoopIndex != loopIndex)
                {
                    item4.SetLoopIndex(loopIndex);
                    item4.BasePosition = this.seed.GetLocalPosition(loopIndex);
                    if (item4.ViewObject != null)
                    {
                        item4.ViewObject.SetTransform(item4.BasePosition);
                    }
                }
                item4.IsTermination = this.isAllDisp;
            }
            this.ClippingItems(false, false);
        }
    }

    protected bool ClippingItem(ListViewItem item)
    {
        if (item == null)
        {
            return false;
        }
        Vector3 localPosition = this.scrollView.transform.localPosition;
        Vector2 vector2 = new Vector2(this.clipOffset.x - localPosition.x, this.clipOffset.y - localPosition.y);
        float num = vector2.x - (this.clipRange.x / 2f);
        float num2 = vector2.x + (this.clipRange.x / 2f);
        float num3 = vector2.y + (this.clipRange.y / 2f);
        float num4 = vector2.y - (this.clipRange.y / 2f);
        Vector3 basePosition = item.BasePosition;
        return ((((basePosition.x >= num) && (basePosition.x <= num2)) && (basePosition.y <= num3)) && (basePosition.y >= num4));
    }

    protected bool ClippingItem(ListViewObject obj)
    {
        if (obj == null)
        {
            return false;
        }
        return this.ClippingItem(obj.GetItem());
    }

    protected void ClippingItems(bool isIndicatorUpdate = true, bool isCenterCheck = false)
    {
        ListViewItem centerItem = this.centerItem;
        this.centerItem = null;
        if (this.itemSortList == null)
        {
            if (((centerItem != null) && (this.indicator != null)) && isIndicatorUpdate)
            {
                this.indicator.OnModifyCenterItem(this, null, false, false, false, false);
                this.indicator.OnModifyPosition(this, null);
            }
        }
        else
        {
            Vector3 localPosition = this.scrollView.transform.localPosition;
            Vector2 vector2 = new Vector2(this.clipOffset.x - localPosition.x, this.clipOffset.y - localPosition.y);
            float num = vector2.x - (this.clipRange.x / 2f);
            float num2 = vector2.x + (this.clipRange.x / 2f);
            float num3 = vector2.y + (this.clipRange.y / 2f);
            float num4 = vector2.y - (this.clipRange.y / 2f);
            float num5 = 0f;
            float num6 = 0f;
            float num7 = 0f;
            if (this.isLoop)
            {
                if (this.itemSortList.Count > 2)
                {
                    int num8 = (this.terminalIndex <= 0) ? (this.itemSortList.Count - 1) : (this.terminalIndex - 1);
                    ListViewItem item2 = this.itemSortList[this.terminalIndex];
                    ListViewItem item3 = this.itemSortList[num8];
                    Vector3 basePosition = item2.BasePosition;
                    bool flag = (((basePosition.x >= num) && (basePosition.x <= num2)) && (basePosition.y <= num3)) && (basePosition.y >= num4);
                    Vector3 vector4 = item3.BasePosition;
                    bool flag2 = (((vector4.x >= num) && (vector4.x <= num2)) && (vector4.y <= num3)) && (vector4.y >= num4);
                    if (!flag || !flag2)
                    {
                        if (flag)
                        {
                            int num9 = (num8 <= 0) ? (this.itemSortList.Count - 1) : (num8 - 1);
                            ListViewItem item4 = this.itemSortList[num9];
                            item2.IsTermination = this.isAllDisp;
                            item3.IsTermination = true;
                            item3.SetLoopIndex(item2.LoopIndex - 1);
                            item3.BasePosition = this.seed.GetLocalPosition(item3.LoopIndex);
                            if (item3.ViewObject != null)
                            {
                                item3.ViewObject.SetTransform(item3.BasePosition);
                            }
                            item4.IsTermination = true;
                            this.terminalIndex = num8;
                        }
                        else if (flag2)
                        {
                            int num10 = (this.terminalIndex >= (this.itemSortList.Count - 1)) ? 0 : (this.terminalIndex + 1);
                            ListViewItem item5 = this.itemSortList[num10];
                            item3.IsTermination = this.isAllDisp;
                            item2.IsTermination = true;
                            item2.SetLoopIndex(item3.LoopIndex + 1);
                            item2.BasePosition = this.seed.GetLocalPosition(item2.LoopIndex);
                            if (item2.ViewObject != null)
                            {
                                item2.ViewObject.SetTransform(item2.BasePosition);
                            }
                            item5.IsTermination = true;
                            this.terminalIndex = num10;
                        }
                    }
                }
                else if ((this.itemSortList.Count == 2) && (centerItem != null))
                {
                    int num11 = (centerItem.Index != 0) ? 0 : 1;
                    ListViewItem item6 = this.itemSortList[num11];
                    ListViewItem item7 = centerItem;
                    int index = (item7.LoopIndex >= item6.LoopIndex) ? (item7.LoopIndex + 1) : (item7.LoopIndex - 1);
                    Vector3 vector5 = item6.BasePosition;
                    Vector3 position = this.seed.GetLocalPosition(index);
                    float num13 = (vector5.x <= vector2.x) ? (vector2.x - vector5.x) : (vector5.x - vector2.x);
                    float num14 = (vector5.y <= vector2.y) ? (vector2.y - vector5.y) : (vector5.y - vector2.y);
                    float num15 = 0f;
                    float num16 = (position.x <= vector2.x) ? (vector2.x - position.x) : (position.x - vector2.x);
                    float num17 = (position.y <= vector2.y) ? (vector2.y - position.y) : (position.y - vector2.y);
                    float num18 = 0f;
                    if (this.scrollView.canMoveHorizontally)
                    {
                        num15 += num13;
                        num18 += num16;
                    }
                    if (this.scrollView.canMoveVertically)
                    {
                        num15 += num14;
                        num18 += num17;
                    }
                    if (num18 < num15)
                    {
                        item6.SetLoopIndex(index);
                        item6.BasePosition = position;
                        if (item6.ViewObject != null)
                        {
                            item6.ViewObject.SetTransform(position);
                        }
                    }
                }
            }
            foreach (ListViewItem item8 in this.itemSortList)
            {
                bool flag3;
                Vector3 vector7 = item8.BasePosition;
                if (item8.IsTermination)
                {
                    flag3 = true;
                }
                else
                {
                    flag3 = (((vector7.x >= num) && (vector7.x <= num2)) && (vector7.y <= num3)) && (vector7.y >= num4);
                }
                if (item8.ViewObject == null)
                {
                    if (!flag3 || (this.MakeObject(item8) == null))
                    {
                    }
                }
                else if (!flag3 && this.ReleaseObject(item8.ViewObject))
                {
                }
                if (item8.ViewObject != null)
                {
                    float num19 = (vector7.x <= vector2.x) ? (vector2.x - vector7.x) : (vector7.x - vector2.x);
                    float num20 = (vector7.y <= vector2.y) ? (vector2.y - vector7.y) : (vector7.x - vector2.y);
                    float num21 = 0f;
                    if (this.scrollView.canMoveHorizontally)
                    {
                        num21 += num19;
                    }
                    if (this.scrollView.canMoveVertically)
                    {
                        num21 += num20;
                    }
                    if (item8.Index >= 0)
                    {
                        if (this.centerItem == null)
                        {
                            this.centerItem = item8;
                            this.horizontalItem = item8;
                            this.verticalItem = item8;
                            num5 = num21;
                        }
                        else
                        {
                            if (num21 < num5)
                            {
                                this.centerItem = item8;
                                num5 = num21;
                            }
                            if (num19 < num6)
                            {
                                this.horizontalItem = item8;
                                num6 = num19;
                            }
                            if (num20 < num7)
                            {
                                this.verticalItem = item8;
                                num7 = num19;
                            }
                        }
                    }
                }
            }
            if (this.itemSortList.Count <= 1)
            {
                this.horizontalItem = null;
                this.verticalItem = null;
            }
            else
            {
                if (num6 > 10f)
                {
                    this.horizontalItem = null;
                }
                if (num7 > 10f)
                {
                    this.verticalItem = null;
                }
            }
            if (!isCenterCheck)
            {
                this.centerItem = null;
            }
            if ((this.indicator != null) && isIndicatorUpdate)
            {
                if (centerItem != this.centerItem)
                {
                    if (this.centerItem != null)
                    {
                        this.indicator.OnModifyCenterItem(this, this.centerItem, this.centerItem != this.topItem, this.centerItem != this.bottomItem, this.centerItem != this.leftItem, this.centerItem != this.rightItem);
                    }
                    else
                    {
                        this.indicator.OnModifyCenterItem(this, null, false, false, false, false);
                    }
                }
                this.indicator.OnModifyPosition(this, this.centerItem);
            }
        }
    }

    protected void ClippingTerminationItem()
    {
        if (this.itemSortList != null)
        {
            foreach (ListViewItem item in this.itemSortList)
            {
                if (item.ViewObject == null)
                {
                    if (item.IsTermination)
                    {
                        this.MakeObject(item);
                    }
                }
                else if (!item.IsTermination)
                {
                    this.ReleaseObject(item.ViewObject);
                }
            }
        }
    }

    public void CreateList(int sum)
    {
        this.DestroyList();
        this.itemList = new List<ListViewItem>(sum);
        if (this.scrollView != null)
        {
            UIPanel panel = this.scrollView.panel;
            if (panel != null)
            {
                Vector2 vector = (Vector2) (panel.clipOffset * -1f);
                this.scrollView.transform.localPosition = (Vector3) vector;
                this.scrollView.ResetPosition();
            }
        }
        if (this.dropList != null)
        {
            foreach (UIDragDropListViewSurface surface in this.dropList)
            {
                if (surface != null)
                {
                    surface.DragEnd();
                }
            }
        }
    }

    public void DestroyList()
    {
        this.centerItem = null;
        this.horizontalItem = null;
        this.verticalItem = null;
        this.itemSortList = null;
        if (this.itemList != null)
        {
            foreach (GameObject obj2 in this.objectList)
            {
                UnityEngine.Object.Destroy(obj2);
            }
            this.objectList.Clear();
            while (this.objectStock.Count > 0)
            {
                UnityEngine.Object.Destroy(this.objectStock.Pop());
            }
            this.itemList = null;
        }
        if (this.scrollView != null)
        {
            this.scrollView.DisableSpring();
        }
        if (this.scrollBar != null)
        {
            this.scrollBar.alpha = 0f;
        }
        if (this.emptyMessageBase != null)
        {
            this.emptyMessageBase.SetActive(false);
        }
        if (this.indicator != null)
        {
            this.indicator.OnModifyCenterItem(this, null, false, false, false, false);
            this.indicator.OnModifyPosition(this, null);
        }
        this.SetSortButtonImage();
    }

    public void DispItem(int selectIndex = -1, bool isAllDisp = false, int addEmptyTarminal = -1)
    {
        this.ReleaseObject();
        List<ListViewItem> list = new List<ListViewItem>();
        int count = this.itemList.Count;
        this.topItem = null;
        this.bottomItem = null;
        this.leftItem = null;
        this.rightItem = null;
        this.isAllDisp = isAllDisp;
        if (count > 0)
        {
            if (this.scrollView.canMoveHorizontally)
            {
                this.leftItem = this.itemList[0];
                this.rightItem = this.itemList[0];
            }
            if (this.scrollView.canMoveVertically)
            {
                this.topItem = this.itemList[0];
                this.bottomItem = this.itemList[0];
            }
            for (int i = 0; i < count; i++)
            {
                ListViewItem item = this.itemList[i];
                item.SetSortIndex(i);
                item.IsTermination = isAllDisp;
                list.Add(item);
                if (this.topItem != null)
                {
                    if (item.BasePosition.y > this.topItem.BasePosition.y)
                    {
                        this.topItem = item;
                    }
                    else if (item.BasePosition.y < this.bottomItem.BasePosition.y)
                    {
                        this.bottomItem = item;
                    }
                }
                if (this.rightItem != null)
                {
                    if (item.BasePosition.x > this.rightItem.BasePosition.x)
                    {
                        this.rightItem = item;
                    }
                    else if (item.BasePosition.x < this.leftItem.BasePosition.x)
                    {
                        this.leftItem = item;
                    }
                }
            }
            list[0].IsTermination = true;
            list[count - 1].IsTermination = true;
            if (count >= addEmptyTarminal)
            {
                list[count - 1].IsTerminationSpace = true;
            }
        }
        this.itemSortList = list;
        this.terminalIndex = 0;
        if (this.emptyMessageBase != null)
        {
            this.emptyMessageBase.SetActive(count <= 0);
        }
        if (this.indicator != null)
        {
            this.indicator.SetIndexMax(count);
        }
        if (this.scrollView != null)
        {
            this.scrollView.ResetPosition();
        }
        this.ClippingTerminationItem();
        this.ClippingItems(true, false);
        if (this.scrollView != null)
        {
            this.scrollView.DisableSpring();
            if (selectIndex >= 0)
            {
                this.ClippingTerminationItem();
                if (!this.MoveCenterItem(selectIndex, false))
                {
                    this.scrollView.ResetPosition();
                }
            }
            else
            {
                this.scrollView.ResetPosition();
            }
        }
    }

    public void DragMaskEnd()
    {
        UIDragDropListViewBackMask component = this.GetDragRoot().GetComponent<UIDragDropListViewBackMask>();
        if (component != null)
        {
            component.DragEnd();
        }
    }

    public void DragMaskStart()
    {
        UIDragDropListViewBackMask component = this.GetDragRoot().GetComponent<UIDragDropListViewBackMask>();
        if (component != null)
        {
            component.DragStart();
        }
        if (this.scrollView != null)
        {
            this.scrollView.DisableSpring();
        }
    }

    public void EndScrollAnim()
    {
        if (this.callbackAfterScroll != null)
        {
            System.Action callbackAfterScroll = this.callbackAfterScroll;
            this.callbackAfterScroll = null;
            callbackAfterScroll();
        }
    }

    public void FowardLoopItem()
    {
        if (((this.isLoop && (this.itemSortList != null)) && (this.itemSortList.Count == 2)) && (this.centerItem != null))
        {
            int num2 = (this.centerItem.SortIndex != 0) ? 0 : 1;
            ListViewItem item = this.itemSortList[num2];
            int index = this.centerItem.LoopIndex + 1;
            if (item.LoopIndex != index)
            {
                item.SetLoopIndex(index);
                item.BasePosition = this.seed.GetLocalPosition(index);
                if (item.ViewObject != null)
                {
                    item.ViewObject.SetTransform(item.BasePosition);
                }
            }
        }
    }

    public bool GetCanScrollList(out bool isTop, out bool isBottom, out bool isLeft, out bool isRight)
    {
        isTop = false;
        isBottom = false;
        isLeft = false;
        isRight = false;
        if (this.scrollView != null)
        {
            UIPanel panel = this.scrollView.panel;
            if (panel != null)
            {
                Vector4 finalClipRegion = panel.finalClipRegion;
                Bounds bounds = this.scrollView.bounds;
                float num = (finalClipRegion.z != 0f) ? (finalClipRegion.z * 0.5f) : ((float) Screen.width);
                float num2 = (finalClipRegion.w != 0f) ? (finalClipRegion.w * 0.5f) : ((float) Screen.height);
                if (this.scrollView.canMoveHorizontally)
                {
                    if (bounds.min.x < (finalClipRegion.x - num))
                    {
                        isLeft = true;
                    }
                    if (bounds.max.x > (finalClipRegion.x + num))
                    {
                        isRight = true;
                    }
                }
                if (this.scrollView.canMoveVertically)
                {
                    if (bounds.min.y < (finalClipRegion.y - num2))
                    {
                        isTop = true;
                    }
                    if (bounds.max.y > (finalClipRegion.y + num2))
                    {
                        isBottom = true;
                    }
                }
                return true;
            }
        }
        return false;
    }

    public int GetCenterIndex() => 
        ((this.centerItem == null) ? -1 : this.centerItem.Index);

    public ListViewItem GetCenterItem() => 
        this.centerItem;

    public virtual GameObject GetDragRoot()
    {
        if (this.dragParentObject != null)
        {
            return this.dragParentObject;
        }
        if (UIDragDropRoot.root != null)
        {
            return UIDragDropRoot.root.gameObject;
        }
        return this.seed.Parent;
    }

    public ListViewItem GetItem(int index)
    {
        if (((this.itemList != null) && (index >= 0)) && (index < this.itemList.Count))
        {
            return this.itemList[index];
        }
        return null;
    }

    public Vector2 getPitch() => 
        this.seed.arrangementPich;

    public virtual void ItemDragEnd()
    {
        foreach (UIDragDropListViewSurface surface in this.dropList)
        {
            if (surface != null)
            {
                surface.DragEnd();
            }
        }
    }

    public virtual void ItemDragStart()
    {
        foreach (UIDragDropListViewSurface surface in this.dropList)
        {
            if (surface != null)
            {
                surface.DragStart();
            }
        }
    }

    public void JumpItem(int index)
    {
        if (this.MoveCenterItem(index, false))
        {
            if (this.seed.arrangement == ListViewItemSeed.Arrangement.Horizontal)
            {
                this.scrollView.RestrictWithinBounds(true, true, false);
            }
            else
            {
                this.scrollView.RestrictWithinBounds(true, false, true);
            }
            this.ClippingItems(false, false);
            if (this.centerItem == null)
            {
                Debug.LogError("4444444444");
                this.centerItem = this.GetItem(index);
                if (this.indicator != null)
                {
                    this.indicator.OnModifyCenterItem(this, this.centerItem, this.centerItem != this.topItem, this.centerItem != this.bottomItem, this.centerItem != this.leftItem, this.centerItem != this.rightItem);
                    this.indicator.OnModifyPosition(this, this.centerItem);
                }
            }
        }
    }

    protected ListViewObject MakeObject(ListViewItem item)
    {
        GameObject obj2;
        if (this.objectStock.Count > 0)
        {
            obj2 = this.objectStock.Pop();
        }
        else
        {
            obj2 = UnityEngine.Object.Instantiate<GameObject>(this.seed.Prefab);
        }
        if (obj2 != null)
        {
            ListViewObject component = obj2.GetComponent<ListViewObject>();
            if (component != null)
            {
                component.SetManager(this);
                component.SetItem(item, this.seed);
                component.SetDragPrefab(this.listDragPrefab);
                this.SetObjectItem(component, item);
                this.objectList.Add(obj2);
                return component;
            }
        }
        return null;
    }

    public bool MoveCenterItem(int index, bool isAnimation = true)
    {
        ListViewItem item = this.GetItem(index);
        if (item == null)
        {
            return false;
        }
        this.CenterLoopItem();
        ListViewObject viewObject = item.ViewObject;
        return viewObject?.MoveCenter(isAnimation);
    }

    public bool MoveTopItem(int index, bool isAnimation = true)
    {
        ListViewItem item = this.GetItem(index);
        if (item == null)
        {
            return false;
        }
        ListViewObject viewObject = item.ViewObject;
        return viewObject?.MoveTop(isAnimation);
    }

    protected void OnEnable()
    {
        this.isScrollRefresh = true;
    }

    protected void OnSwipeCenter(GameObject go)
    {
        ListViewObject component = go.GetComponent<ListViewObject>();
        if (((component != null) && (component.transform.parent.gameObject == this.seed.Parent)) && ((this.centerItem != null) && (this.centerItem.ViewObject != component)))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        }
    }

    protected void ReleaseObject()
    {
        foreach (GameObject obj2 in this.objectList)
        {
            ListViewObject component = obj2.GetComponent<ListViewObject>();
            if (component != null)
            {
                component.ReleaseItem();
                this.objectStock.Push(obj2);
            }
        }
        this.objectList.Clear();
    }

    protected bool ReleaseObject(ListViewObject obj)
    {
        if ((obj != null) && obj.ClearItem())
        {
            int index = this.objectList.IndexOf(obj.gameObject);
            if (index >= 0)
            {
                this.objectList.RemoveAt(index);
                this.objectStock.Push(obj.gameObject);
                return true;
            }
        }
        return false;
    }

    public void setCallbackAfterScroll(System.Action callback)
    {
        this.callbackAfterScroll = callback;
    }

    public virtual void SetFilterList(bool[] filterList)
    {
    }

    protected virtual void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        obj.SetInput(this.isInput);
    }

    public virtual void SetSortAscendingOrder(bool isAscendingOrder)
    {
    }

    protected virtual void SetSortButtonImage()
    {
        if (this.sortKindLabel != null)
        {
            this.sortKindLabel.text = this.sort.GetKindButtonText();
        }
        if (this.sortOrderSprite != null)
        {
            if ((this.sort.Kind == ListViewSort.SortKind.LOGIN_ACCESS) || (this.sort.Kind == ListViewSort.SortKind.CREATE))
            {
                this.sortOrderSprite.spriteName = !this.sort.IsAscendingOrder ? "btn_sort_new" : "btn_sort_old";
            }
            else
            {
                this.sortOrderSprite.spriteName = !this.sort.IsAscendingOrder ? "btn_sort_down" : "btn_sort_up";
            }
        }
    }

    public virtual void SetSortKind(ListViewSort.SortKind kind)
    {
    }

    public void SetTopItem(int index)
    {
        if (this.MoveTopItem(index, false))
        {
            if (this.seed.arrangement == ListViewItemSeed.Arrangement.Horizontal)
            {
                this.scrollView.RestrictWithinBounds(true, true, false);
            }
            else
            {
                this.scrollView.RestrictWithinBounds(true, false, true);
            }
            this.ClippingItems(true, false);
        }
    }

    public void SortItem(int selectIndex = -1, bool isAllDisp = false, int addEmptyTarminal = -1)
    {
        this.ReleaseObject();
        this.sort.SetManager(this);
        List<ListViewItem> list = new List<ListViewItem>();
        if (this.itemList != null)
        {
            foreach (ListViewItem item in this.itemList)
            {
                if (item.SetSortValue(this.sort))
                {
                    list.Add(item);
                }
            }
        }
        int count = list.Count;
        if (this.sort.IsAscendingOrder)
        {
            for (int i = 1; i < count; i++)
            {
                for (int j = 0; j < (count - i); j++)
                {
                    ListViewItem item2 = list[j];
                    ListViewItem item3 = list[j + 1];
                    if (item2.SortValue0 < item3.SortValue0)
                    {
                        list[j] = item3;
                        list[j + 1] = item2;
                    }
                    else if (item2.SortValue0 == item3.SortValue0)
                    {
                        if (item2.SortValue1 > item3.SortValue1)
                        {
                            list[j] = item3;
                            list[j + 1] = item2;
                        }
                        else if (item2.SortValue1 == item3.SortValue1)
                        {
                            if (item2.SortValue2 < item3.SortValue2)
                            {
                                list[j] = item3;
                                list[j + 1] = item2;
                            }
                            else if ((item2.SortValue2 == item3.SortValue2) && (item2.SortValue2B > item3.SortValue2B))
                            {
                                list[j] = item3;
                                list[j + 1] = item2;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int k = 1; k < count; k++)
            {
                for (int m = 0; m < (count - k); m++)
                {
                    ListViewItem item4 = list[m];
                    ListViewItem item5 = list[m + 1];
                    if (item4.SortValue0 < item5.SortValue0)
                    {
                        list[m] = item5;
                        list[m + 1] = item4;
                    }
                    else if (item4.SortValue0 == item5.SortValue0)
                    {
                        if (item4.SortValue1 < item5.SortValue1)
                        {
                            list[m] = item5;
                            list[m + 1] = item4;
                        }
                        else if (item4.SortValue1 == item5.SortValue1)
                        {
                            if (item4.SortValue2 < item5.SortValue2)
                            {
                                list[m] = item5;
                                list[m + 1] = item4;
                            }
                            else if ((item4.SortValue2 == item5.SortValue2) && (item4.SortValue2B < item5.SortValue2B))
                            {
                                list[m] = item5;
                                list[m + 1] = item4;
                            }
                        }
                    }
                }
            }
        }
        this.topItem = null;
        this.bottomItem = null;
        this.leftItem = null;
        this.rightItem = null;
        this.isAllDisp = isAllDisp;
        if (count > 0)
        {
            if (this.scrollView.canMoveHorizontally)
            {
                this.leftItem = this.itemList[0];
                this.rightItem = this.itemList[0];
            }
            if (this.scrollView.canMoveVertically)
            {
                this.topItem = this.itemList[0];
                this.bottomItem = this.itemList[0];
            }
            for (int n = 0; n < count; n++)
            {
                ListViewItem item6 = list[n];
                item6.SetSortIndex(n);
                item6.IsTermination = isAllDisp;
                item6.BasePosition = this.seed.GetLocalPosition(n);
                if (this.topItem != null)
                {
                    if (item6.BasePosition.y > this.topItem.BasePosition.y)
                    {
                        this.topItem = item6;
                    }
                    else if (item6.BasePosition.y < this.bottomItem.BasePosition.y)
                    {
                        this.bottomItem = item6;
                    }
                }
                if (this.rightItem != null)
                {
                    if (item6.BasePosition.x > this.rightItem.BasePosition.x)
                    {
                        this.rightItem = item6;
                    }
                    else if (item6.BasePosition.x < this.leftItem.BasePosition.x)
                    {
                        this.leftItem = item6;
                    }
                }
            }
            list[0].IsTermination = true;
            list[count - 1].IsTermination = true;
            if (count >= addEmptyTarminal)
            {
                list[count - 1].IsTerminationSpace = true;
            }
        }
        this.itemSortList = list;
        this.terminalIndex = 0;
        if (this.emptyMessageBase != null)
        {
            this.emptyMessageBase.SetActive(count <= 0);
        }
        if (this.indicator != null)
        {
            this.indicator.SetIndexMax(count);
        }
        if (this.scrollView != null)
        {
            this.scrollView.ResetPosition();
        }
        this.ClippingTerminationItem();
        this.ClippingItems(true, true);
        this.SetSortButtonImage();
        this.sort.SetManager(null);
        if (this.scrollView != null)
        {
            this.scrollView.DisableSpring();
            if (selectIndex >= 0)
            {
                if (!this.MoveCenterItem(selectIndex, false))
                {
                    this.scrollView.ResetPosition();
                }
            }
            else
            {
                this.scrollView.ResetPosition();
            }
        }
    }

    protected void Update()
    {
        if ((this.scrollView != null) && (this.itemSortList != null))
        {
            Vector3 localPosition = this.scrollView.transform.localPosition;
            if (this.isScrollRefresh || !localPosition.Equals(this.oldScrollPosition))
            {
                this.ClippingItems(true, true);
                this.oldScrollPosition = localPosition;
                this.isScrollRefresh = false;
            }
        }
    }

    public int DropObjectSum
    {
        get
        {
            if (this.dropObjectList == null)
            {
                return 0;
            }
            int num = 0;
            foreach (ListViewDropObject obj2 in this.dropObjectList)
            {
                if (obj2 != null)
                {
                    num++;
                }
            }
            return num;
        }
    }

    public bool IsInput
    {
        get => 
            this.isInput;
        set
        {
            this.isInput = value;
            if (this.scrollBar != null)
            {
                this.scrollBar.alpha = this.scrollBar.alpha;
            }
        }
    }

    public bool IsLoop =>
        this.isLoop;

    public int ItemSum
    {
        get
        {
            if (this.itemList != null)
            {
                return this.itemList.Count;
            }
            return 0;
        }
    }

    public List<ListViewObject> ObjectList
    {
        get
        {
            List<ListViewObject> list = new List<ListViewObject>();
            foreach (GameObject obj2 in this.objectList)
            {
                if (obj2 != null)
                {
                    ListViewObject component = obj2.GetComponent<ListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public int ObjectSum
    {
        get
        {
            if (this.itemList == null)
            {
                return 0;
            }
            int num = 0;
            foreach (ListViewItem item in this.itemList)
            {
                if (item.ViewObject != null)
                {
                    num++;
                }
            }
            return num;
        }
    }
}

