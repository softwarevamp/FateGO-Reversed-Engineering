using System;
using UnityEngine;

[AddComponentMenu("Sample/TestListView/SampleCardUIDragDropListViewDrop")]
public class SampleCardUIDragDropListViewDrop : UIDragDropListViewItem
{
    protected SampleCardListViewDropObject dropObject;

    protected void CardDragDumpEnd()
    {
        ListViewDropObject mListViewObject = base.mListViewObject as ListViewDropObject;
        SampleCardListViewDropObject obj3 = mListViewObject as SampleCardListViewDropObject;
        SampleCardListViewItem item = obj3.GetItem();
        SampleCardListViewObject viewObject = item.ViewObject as SampleCardListViewObject;
        item.IsDeck = false;
        if (viewObject != null)
        {
            viewObject.Init(SampleCardListViewObject.InitMode.INPUT);
        }
        base.OnDragDropRelease(null);
        obj3.Init(SampleCardListViewDropObject.InitMode.INVISIBLE);
        obj3.SetItem(null);
    }

    protected void CardDragDumpStart()
    {
        TweenColor.Begin(base.dragObject, 0.2f, Color.clear).method = UITweener.Method.EaseInOut;
        Vector3 pos = base.dragObject.transform.localPosition + ((Vector3) (Vector3.down * 50f));
        TweenPosition position = TweenPosition.Begin(base.dragObject, 0.2f, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "CardDragDumpEnd";
    }

    protected void CardDragReturnEnd()
    {
        ListViewDropObject mListViewObject = base.mListViewObject as ListViewDropObject;
        SampleCardListViewDropObject obj3 = mListViewObject as SampleCardListViewDropObject;
        base.OnDragDropRelease(null);
        obj3.Init(SampleCardListViewDropObject.InitMode.INPUT);
    }

    protected void CardDragReturnStart()
    {
        ListViewDropObject mListViewObject = base.mListViewObject as ListViewDropObject;
        SampleCardListViewDropObject obj3 = mListViewObject as SampleCardListViewDropObject;
        Vector3 pos = base.dragObject.transform.parent.InverseTransformPoint(obj3.transform.position);
        TweenPosition position = TweenPosition.Begin(base.dragObject, 0.2f, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "CardDragReturnEnd";
    }

    protected void CardDragSwapEnd()
    {
        ListViewDropObject mListViewObject = base.mListViewObject as ListViewDropObject;
        SampleCardListViewDropObject obj3 = mListViewObject as SampleCardListViewDropObject;
        base.OnDragDropRelease(null);
        obj3.Init(SampleCardListViewDropObject.InitMode.INPUT);
        this.dropObject.Init(SampleCardListViewDropObject.InitMode.DRAG_DELETE);
        this.dropObject.Init(SampleCardListViewDropObject.InitMode.INPUT);
        this.dropObject = null;
    }

    protected void CardDragSwapMove()
    {
        ListViewDropObject mListViewObject = base.mListViewObject as ListViewDropObject;
        SampleCardListViewDropObject obj3 = mListViewObject as SampleCardListViewDropObject;
        SampleCardListViewItem item = obj3.GetItem();
        SampleCardListViewItem item2 = this.dropObject.GetItem();
        obj3.SetItem(item2);
        obj3.Init(SampleCardListViewDropObject.InitMode.VALID);
        item.IsDeck = true;
        this.dropObject.SetItem(item);
        this.dropObject.Init(SampleCardListViewDropObject.InitMode.VALID);
        base.Invoke("CardDragSwapEnd", 0.05f);
    }

    protected void CardDragSwapStart(SampleCardListViewDropObject ddo)
    {
        this.dropObject = ddo;
        ListViewDropObject mListViewObject = base.mListViewObject as ListViewDropObject;
        SampleCardListViewDropObject obj3 = mListViewObject as SampleCardListViewDropObject;
        Vector3 pos = base.dragObject.transform.parent.InverseTransformPoint(this.dropObject.transform.position);
        TweenPosition.Begin(base.dragObject, 0.2f, pos).method = UITweener.Method.EaseInOut;
        this.dropObject.Init(SampleCardListViewDropObject.InitMode.DRAG_MOVE, new System.Action(this.CardDragSwapMove), 0f, obj3.transform.position);
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        Debug.Log("DragDrop surface " + surface);
        ListViewDropObject mListViewObject = base.mListViewObject as ListViewDropObject;
        SampleCardListViewDropObject obj3 = mListViewObject as SampleCardListViewDropObject;
        SampleCardListViewItem item = obj3.GetItem();
        SampleCardListViewManager manager = obj3.Manager as SampleCardListViewManager;
        manager.ItemDragEnd();
        if ((surface != null) && (item != null))
        {
            SampleCardUIDragDropListViewSurface component = surface.GetComponent<SampleCardUIDragDropListViewSurface>();
            if (component != null)
            {
                ListViewDropInfo info = new ListViewDropInfo(base.gameObject, surface);
                if (manager.IsDropDropSurface(info))
                {
                    SampleCardListViewDropObject dropObject = component.DropObject;
                    if (dropObject != null)
                    {
                        SampleCardListViewItem item2 = dropObject.GetItem();
                        if (item2 == null)
                        {
                            base.OnDragDropRelease(surface);
                            obj3.Init(SampleCardListViewDropObject.InitMode.INVISIBLE);
                            obj3.SetItem(null);
                            item.IsDeck = true;
                            dropObject.SetItem(item);
                            dropObject.Init(SampleCardListViewDropObject.InitMode.INPUT);
                            return;
                        }
                        if (item2 != item)
                        {
                            this.CardDragSwapStart(dropObject);
                            return;
                        }
                    }
                    else
                    {
                        this.CardDragDumpStart();
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
        ListViewDropObject mListViewObject = base.mListViewObject as ListViewDropObject;
        SampleCardListViewDropObject obj3 = mListViewObject as SampleCardListViewDropObject;
        obj3.Init(SampleCardListViewDropObject.InitMode.INVISIBLE);
        obj3.Manager.ItemDragStart();
    }
}

