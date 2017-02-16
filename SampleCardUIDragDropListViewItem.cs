using System;
using UnityEngine;

[AddComponentMenu("Sample/TestListView/SampleCardUIDragDropListViewItem")]
public class SampleCardUIDragDropListViewItem : UIDragDropListViewItem
{
    protected void CardDragReturnEnd()
    {
        SampleCardListViewObject mListViewObject = base.mListViewObject as SampleCardListViewObject;
        base.OnDragDropRelease(null);
        mListViewObject.Init(SampleCardListViewObject.InitMode.INPUT);
    }

    protected void CardDragReturnStart()
    {
        SampleCardListViewObject mListViewObject = base.mListViewObject as SampleCardListViewObject;
        Vector3 pos = base.dragObject.transform.parent.InverseTransformPoint(mListViewObject.transform.position);
        TweenPosition position = TweenPosition.Begin(base.dragObject, 0.2f, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "CardDragReturnEnd";
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        Debug.Log("DragDrop surface " + surface);
        SampleCardListViewObject mListViewObject = base.mListViewObject as SampleCardListViewObject;
        SampleCardListViewItem item = mListViewObject.GetItem();
        SampleCardListViewManager manager = mListViewObject.Manager as SampleCardListViewManager;
        manager.ItemDragEnd();
        if ((surface != null) && (item != null))
        {
            SampleCardUIDragDropListViewSurface component = surface.GetComponent<SampleCardUIDragDropListViewSurface>();
            if (component != null)
            {
                ListViewDropInfo info = new ListViewDropInfo(base.gameObject, surface);
                if (manager.IsItemDropSurface(info))
                {
                    SampleCardListViewDropObject dropObject = component.DropObject;
                    if (dropObject != null)
                    {
                        SampleCardListViewItem item2 = dropObject.GetItem();
                        if (item2 != null)
                        {
                            item2.IsDeck = false;
                            SampleCardListViewObject viewObject = item2.ViewObject as SampleCardListViewObject;
                            if (viewObject != null)
                            {
                                viewObject.Init(SampleCardListViewObject.InitMode.INPUT);
                            }
                        }
                        base.OnDragDropRelease(surface);
                        mListViewObject.Init(SampleCardListViewObject.InitMode.INVALID);
                        item.IsDeck = true;
                        dropObject.SetItem(item);
                        dropObject.Init(SampleCardListViewDropObject.InitMode.INPUT);
                        return;
                    }
                }
            }
        }
        this.CardDragReturnStart();
    }

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        SampleCardListViewObject mListViewObject = base.mListViewObject as SampleCardListViewObject;
        mListViewObject.Init(SampleCardListViewObject.InitMode.INVALID);
        mListViewObject.Manager.ItemDragStart();
    }
}

